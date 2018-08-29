using System;

namespace Stellar.Common
{
    public class Transaction
    {
        public Guid Id { get; private set; }

        public string CreatedAt { get; set; }
        public decimal FeePaid { get; set; }
        public string Hash { get; set; }
        public long Ledger { get; set; }
        public string Message { get; set; }
        public long OperationCount { get; set; }
        public string SourceAccount { get; set; }
        public long SourceAccountSequence { get; set; }
        public string TargetAccount { get; set; }

        public Transaction()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
