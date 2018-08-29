using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stellar.Common.Services
{
    public class DummyOfferConsumerService : OfferBaseService<DummyOfferConsumerService>, IOfferConsumerService
    {
        private bool receiveOffers;

        public DummyOfferConsumerService(ISettingsService settingsService)
            : base(settingsService)
        {
            var x = new Task(GenerateDummyData);

            x.Start();
        }

        void GenerateDummyData()
        {
            for(int i = 0; i < 5; i++)
            {
                var r = new Retailer
                {
                    Name = $"Retailer{i}",
                    AccountId = $"0x{1}00000000000",
                    Latitude = 1 / (i + 1),
                    Longitude = 1 / (i + 1)
                };

                var o = new Offer(r)
                {
                    Product = $"Product {i}",
                    Price = (decimal)i,
                    ValidFrom = DateTime.Today,
                    ValidTo = DateTime.Today.AddDays(i)
                };

                OnOfferAdded(o);
            }
        }

        public void ReceiveOffers()
        {
            var x = new Task(GenerateDummyData);

            x.Start();
        }
    }
}
