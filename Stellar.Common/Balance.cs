using System;

namespace Stellar.Common
{
    public class Balance
    {
        public Guid Id { get; private set; }
        
        public string AssetType { get; set; }
        public string AssetCode { get; set; }
        public string AssetIssuer { get; set; }        
        public string Limit { get; set; }        
        public string BalanceValue { get; set; }

        public Balance()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
