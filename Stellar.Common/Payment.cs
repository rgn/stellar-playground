using System;

namespace Stellar.Common
{
    public class Payment
    {
        public Guid Id { get; private set; }

        public string From { get; set; }
        public string To { get; set; }
        public string Amount { get; set; }
        public string AssetCode { get; set; }
        public string AssetIssuer { get; set; }
        public string AssetType { get; set; }
        public string CreatedAt { get; set; }        
        public string PagingToken { get; set; }        
        public string TransactionHash { get; set; }

        public Payment()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
