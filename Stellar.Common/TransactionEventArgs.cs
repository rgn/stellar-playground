using System;

namespace Stellar.Common
{
    public class TransactionEventArgs : EventArgs
    {
        public Transaction Transaction { get; private set; }

        public TransactionEventArgs(Transaction Transaction)            
        {
            this.Transaction = Transaction;
        }
    }
}
