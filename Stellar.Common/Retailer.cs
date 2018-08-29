using Newtonsoft.Json;
using System;

namespace Stellar.Common
{
    public class Retailer
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string AccountId { get; set; }

        [JsonProperty]
        public decimal Longitude { get; set; }

        [JsonProperty]
        public decimal Latitude { get; set; }

        public Retailer()
        {
            Id = Guid.NewGuid();
        }

        public Retailer(Guid Id)
        {
            this.Id = Id;
        }

        [JsonConstructor]
        public Retailer(Guid Id, string Name, string AccountId, decimal Longitude, decimal Latitude)
            : this(Id)
        {
            this.Name = Name;
            this.AccountId = AccountId;
            this.Longitude = Longitude;
            this.Latitude = Latitude;
        }
    }    
}
