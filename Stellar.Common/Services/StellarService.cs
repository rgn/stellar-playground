using Common.Logging;
using stellar_dotnet_sdk;
using stellar_dotnet_sdk.responses;
using stellar_dotnet_sdk.responses.operations;
using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Collections.Generic;

namespace Stellar.Common.Services
{
    [Export(typeof(IStellarService))]
    public class StellarService : IStellarService, IDisposable
    {
        private ILog logger = LogManager.GetLogger(typeof(StellarService));

        public const string CFG_STELLAR_ACCOUNT_ID = "stellar.accountid";
        public const string CFG_STELLAR_PRIVATE_SEED = "stellar.seed";
        public const string CFG_STELLAR_SERVER = "stellar.server";
        public const string CFG_STELLAR_TESTNET = "stellar.testnet";

        public event EventHandler<AccountEventArgs> AccountReceived;
        public event EventHandler<TransactionEventArgs> TransactionReceived;
        public event EventHandler<PaymentEventArgs> PaymentReceived;

        private KeyPair keyPair;
        private bool useTestnet;
        private Server server;
        private string hostname;

        protected ISettingsService settingsService;

        public string Server { get => hostname; }
        public string AccountId { get => keyPair.AccountId; }
        public string Seed { get => keyPair.SecretSeed; }
        public bool UseTestnet { get => useTestnet; }

        private bool accountRegistered = false;

        [ImportingConstructor]
        public StellarService(ISettingsService settingsService)
        {
            logger.Trace("Created");

            this.settingsService = settingsService;

            logger.Trace("Load configuration");

            hostname = settingsService.GetAppSettings(CFG_STELLAR_SERVER);

            server = new Server(hostname);

            var shouldUseTestnet = settingsService.GetAppSettings(CFG_STELLAR_TESTNET);            

            if (string.IsNullOrEmpty(shouldUseTestnet) || shouldUseTestnet.Equals("0"))
            {
                useTestnet = false;
            }
            else
            {
                useTestnet = true;

                Network.UseTestNetwork();
            }

            logger.Debug($"UseTestnet: {useTestnet}");

            if (string.IsNullOrEmpty(settingsService.GetAppSettings(CFG_STELLAR_PRIVATE_SEED)))
            {
                logger.Trace("New account required.");

                keyPair = KeyPair.Random();                

                RegisterAccount(keyPair);

                settingsService.AddOrUpdateAppSettings(CFG_STELLAR_ACCOUNT_ID, keyPair.AccountId);
                settingsService.AddOrUpdateAppSettings(CFG_STELLAR_PRIVATE_SEED, keyPair.SecretSeed);
            }
            else
            {
                keyPair = KeyPair.FromSecretSeed(settingsService.GetAppSettings(CFG_STELLAR_PRIVATE_SEED));

                accountRegistered = true;

                logger.Trace($"Using existing account {keyPair.AccountId}.");
            }            
        }

        public void Dispose()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }

        private async void RegisterAccount(KeyPair keyPar)
        {
            logger.Trace($"Registring new account {keyPair.AccountId}.");

            var friendBot = await server.TestNetFriendBot
                    .FundAccount(keyPair)
                    .Execute();

            accountRegistered = true;
        }
       
        public async void GetMyBalance()
        {
            if (!accountRegistered)
                return;

            try
            {
                var account = await server.Accounts.Account(keyPair);

                HandleAccount(server, this, account);
            }
            catch (Exception ex)
            {
                logger.Error("Failed to reteive account.", ex);
            }
        }

        public async void MakeTransaction(string targetAccountId, decimal amount, string transactionMessage)
        {            
            var assetType = new AssetTypeNative();

            var targetKeyPair = KeyPair.FromAccountId(targetAccountId);
            var sourceAccountResponse = await server.Accounts.Account(keyPair);
            var sourceAccount = new stellar_dotnet_sdk.Account(sourceAccountResponse.KeyPair, sourceAccountResponse.SequenceNumber);
            var memo = Memo.Text(transactionMessage);

            var operation = new PaymentOperation.Builder(targetKeyPair, assetType, amount.ToString()).SetSourceAccount(sourceAccount.KeyPair).Build();
            var transaction = new stellar_dotnet_sdk.Transaction.Builder(sourceAccount).AddOperation(operation).AddMemo(memo).Build();

            transaction.Sign(keyPair);
            
            try
            {
                logger.Debug("Starting transaction.");

                await server.SubmitTransaction(transaction);                

                logger.Info("Transaction completed.");
            }
            catch(Exception ex)
            {
                logger.Error("Transaction failed.", ex);
            }
        }

        public async void ReceiveMyAccountTransactions()
        {
            logger.Trace($"Get transactions for account {keyPair.AccountId}.");            
            
            await server.Transactions
                .ForAccount(keyPair)
                .Stream((sender, response) => { HandleTransaction(server, sender, response); })
                .Connect();
        }

        public async void ReceiveAccountTransactions()
        {
            logger.Trace($"Get transactions.");

            await server.Transactions
                .ForAccount(keyPair)
                .Stream((sender, response) => { HandleTransaction(server, sender, response); })
                .Connect();
        }

        public async void ReceiveMyPayments()
        {
            logger.Trace($"Get payments for account {keyPair.AccountId}.");

            await server.Payments
                .ForAccount(keyPair)
                .Stream((sender, operationResponse) => { HandleOperationResponse(server, sender, operationResponse); })
                .Connect();
        }

        public async void ReceivePayments()
        {
            logger.Trace($"Get payments.");

            await server.Payments                
                .Stream((sender, operationResponse) => { HandleOperationResponse(server, sender, operationResponse); })
                .Connect();
        }

        protected void HandleAccount(Server server, object sender, AccountResponse accountResponse)
        {
            logger.Trace($"Received account.");

            var balances = new List<Balance>();

            if (accountResponse.Balances != null)
            {
                balances = accountResponse.Balances.Select(x => {
                    return new Balance
                    {
                        AssetType = x.AssetType,
                        AssetCode = x.AssetCode,
                        //AssetIssuer = x.AssetIssuer != null ? x.AssetIssuer.AccountId : "n/a",
                        Limit = x.Limit,
                        BalanceValue = x.BalanceString
                    };
                }).ToList();
            }
            

            var account = new Account
            {
                AccountId = accountResponse.KeyPair.AccountId,
                Balances = balances,
                Data = accountResponse.Data,
                HomeDomain = accountResponse.HomeDomain,
                InflationDestination = accountResponse.InflationDestination
            };

            AccountReceived?.Invoke(this, new AccountEventArgs(account));
        }

        protected void HandleTransaction(Server server, object sender, TransactionResponse transactionResponse)
        {
            var transaction = new Transaction
            {
                CreatedAt = transactionResponse.CreatedAt,
                FeePaid = transactionResponse.FeePaid,
                Hash = transactionResponse.Hash,
                Ledger = transactionResponse.Ledger,
                Message = transactionResponse.MemoStr,
                OperationCount = transactionResponse.OperationCount,
                SourceAccount = transactionResponse.SourceAccount.AccountId,
                SourceAccountSequence = transactionResponse.SourceAccountSequence,
                TargetAccount = keyPair.AccountId
            };

            TransactionReceived?.Invoke(this, new TransactionEventArgs(transaction));            
        }

        protected void HandleOperationResponse(Server server, object sender, OperationResponse operationResponse)
        {
            switch(operationResponse.Type.ToLower())
            {
                case "payment":
                    var paymentOperationResponse = operationResponse as PaymentOperationResponse;
                    OnPaymentOperation(paymentOperationResponse);
                    break;

                default:
                    OnUnknownOperation(operationResponse);
                    break;
            }
        }

        protected void OnPaymentOperation(PaymentOperationResponse paymentOperationResponse)
        {
            logger.Trace($"Received payment: {paymentOperationResponse}");

            var payment = new Payment
            {
                Amount = paymentOperationResponse.Amount,
                AssetCode = paymentOperationResponse.AssetCode,
                AssetIssuer = paymentOperationResponse.AssetIssuer,
                AssetType = paymentOperationResponse.AssetType,
                CreatedAt = paymentOperationResponse.CreatedAt,
                From = paymentOperationResponse.From.AccountId,
                To = paymentOperationResponse.To.AccountId,
                PagingToken = paymentOperationResponse.PagingToken,
                TransactionHash = paymentOperationResponse.TransactionHash
            };

            PaymentReceived?.Invoke(this, new PaymentEventArgs(payment));
        }

        protected void OnUnknownOperation(OperationResponse operationResponse)
        {
            logger.Warn($"Unhandled operation repsonse type `{operationResponse.Type}`.");
        }
    }
}
