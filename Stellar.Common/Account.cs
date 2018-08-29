using System;
using System.Collections.Generic;

namespace Stellar.Common
{
    public class Account
    {
        public Guid Id { get; private set; }

        public string AccountId { get; set; }
        public IEnumerable<Balance> Balances { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public string HomeDomain { get; set; }
        public string InflationDestination { get; set; }        

        public Account()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
