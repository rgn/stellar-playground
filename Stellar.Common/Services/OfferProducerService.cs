using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.ComponentModel.Composition;
using System.Text;

namespace Stellar.Common.Services
{
    [Export(typeof(IOfferProducerService))]
    public class OfferProducerService : OfferBaseService<OfferProducerService>, IDisposable, IOfferProducerService
    {
        private EventHubClient eventHubClient;        

        [ImportingConstructor()]
        public OfferProducerService(ISettingsService settingsService)
            : base(settingsService)
        {            
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(hubConnectionString)
            {
                EntityPath = hubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            this.logger.Trace("Created.");
        }

        public async void SendOffer(Offer offer)
        {
            this.logger.Debug($"Sending offer {offer.Id}.");
            try
            {
                var strOffer = JsonConvert.SerializeObject(offer, Formatting.Indented);

                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(strOffer)));

                OnOfferAdded(offer);

                this.logger.Info($"Offer {offer.Id} sent.");
            }
            catch (Exception exception)
            {
                this.logger.Error($"{DateTime.Now} > Exception: {exception.Message}");                
            }
        }

        public void Dispose()
        {
            CleanUp();
        }

        private async void CleanUp()
        {
            this.logger.Trace("Clean up.");

            if (eventHubClient != null)
            {
                await eventHubClient.CloseAsync();
            }
        }
    }
}
