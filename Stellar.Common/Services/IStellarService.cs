using System;

namespace Stellar.Common.Services
{
    public interface IStellarService
    {
        event EventHandler<AccountEventArgs> AccountReceived;
        event EventHandler<TransactionEventArgs> TransactionReceived;
        event EventHandler<PaymentEventArgs> PaymentReceived;

        string Server { get; }
        string AccountId { get; }
        string Seed { get; }
        bool UseTestnet { get; }

        void GetMyBalance();
        void MakeTransaction(string targetAccountId, decimal amount, string transactionMessage);
        void ReceiveMyAccountTransactions();
        void ReceiveAccountTransactions();
        void ReceiveMyPayments();
        void ReceivePayments();
    }
}
