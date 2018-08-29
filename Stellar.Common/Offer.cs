using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stellar.Common
{
    public class Offer
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public Retailer Retailer { get; private set; }

        [JsonProperty]
        public DateTime ValidFrom { get; set; }

        [JsonProperty]
        public DateTime ValidTo { get; set; }

        [JsonProperty]
        public string Product { get; set; }

        [JsonProperty]
        public decimal Price { get; set; }

        public Offer(Retailer Retailer)
        {
            this.Id = Guid.NewGuid();
            this.Retailer = Retailer;
        }

        [JsonConstructor]
        public Offer(Guid Id, Retailer Retailer, DateTime ValidFrom, DateTime ValidTo, string Product, decimal Price)
        {
            this.Id = Id;
            this.Retailer = Retailer;
            this.ValidFrom = ValidFrom;
            this.ValidTo = ValidTo;
            this.Product = Product;
            this.Price = Price;
        }
    }
}
