using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stellar.Common.Services
{
    public class DummyStellarService : IStellarService
    {
        public DummyStellarService(ISettingsService settingsService)
        {
            var gt = new Task(GenerateTransactions);

            gt.Start();

            var gp = new Task(GeneratePayments);

            gp.Start();
        }

        private void GenerateTransactions()
        {
            for (int i = 0; i < 5; i++)
            {
                var t = new Transaction
                {
                    CreatedAt = DateTime.Now.ToString(),
                    FeePaid = i,
                    Hash = $"TransactionHash{i}",
                    Ledger = i,
                    Message = $"TransactionMessage{i}",
                    OperationCount = i,
                    SourceAccount = "SourceAccountId",
                    TargetAccount = "TargetAccountId",
                    SourceAccountSequence = i
                };

                TransactionReceived?.Invoke(this, new TransactionEventArgs(t));
            }
        }

        private void GeneratePayments()
        {
            for (int i = 0; i < 5; i++)
            {
                var p = new Payment
                {
                    Amount = $"{i}",
                    AssetCode = "B1",
                    AssetIssuer = "AssetIssuerId",
                    AssetType = "Native",
                    CreatedAt = DateTime.Now.ToString(),
                    PagingToken = $"{i}",
                    From = $"FromAccountId{i}",
                    To = $"ToAccountId{i}",
                    TransactionHash = $"TransactionHash{i}"
                };

                PaymentReceived?.Invoke(this, new PaymentEventArgs(p));
            }
        }

        public string Server => "Dummy";

        public string AccountId => "MyAccountID";

        public string Seed => "MySeed";

        public bool UseTestnet => true;

        public event EventHandler<AccountEventArgs> AccountReceived;
        public event EventHandler<TransactionEventArgs> TransactionReceived;
        public event EventHandler<PaymentEventArgs> PaymentReceived;

        public async void MakeTransaction(string targetAccountId, decimal amount, string transactionMessage)
        {
            var t = new Transaction
            {
                CreatedAt = DateTime.Now.ToString(),
                FeePaid = 1,
                Hash = "MadeTransactionHash",
                Ledger = 1,
                Message = "MadeTransactionMessage",
                OperationCount = 1,
                SourceAccount = "SourceAccountId",
                TargetAccount = "TargetAccountId",
                SourceAccountSequence = 1
            };

            TransactionReceived?.Invoke(this, new TransactionEventArgs(t));
        }

        public void ReceiveAccountTransactions()
        {
            var t = new Transaction
            {
                CreatedAt = DateTime.Now.ToString(),
                FeePaid = 1,
                Hash = "ReceivedTransactionHash",
                Ledger = 1,
                Message = "ReceivedTransactionMessage",
                OperationCount = 1,
                SourceAccount = "SourceAccountId",
                TargetAccount = "TargetAccountId",
                SourceAccountSequence = 1
            };

            TransactionReceived?.Invoke(this, new TransactionEventArgs(t));
        }

        public void GetMyBalance()
        {
            var account = new Account
            {
                AccountId = "asldfjlsakdjflkdsajfl",
                HomeDomain = "AccountHomeDomain",
                InflationDestination = "InflationDestination",
                Balances = new List<Balance>
                {
                    new Balance
                    {
                        AssetCode = "B1",
                        AssetIssuer = "B1Issuer",
                        AssetType = "B1Type",
                        BalanceValue = "1",
                        Limit = "100"
                    },
                    new Balance
                    {
                        AssetCode = "B2",
                        AssetIssuer = "B2Issuer",
                        AssetType = "B2Type",
                        BalanceValue = "5",
                        Limit = "500"
                    }
                },
                Data = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" },
                    { "key3", "value3" },
                    { "key4", "value4" }
                }
            };
            
            AccountReceived?.Invoke(this, new AccountEventArgs(account));
        }

        public void ReceiveMyAccountTransactions()
        {
            GenerateTransactions();
        }

        public void ReceiveMyPayments()
        {
            GeneratePayments();
        }

        public void ReceivePayments()
        {
            GeneratePayments();
        }
    }
}
