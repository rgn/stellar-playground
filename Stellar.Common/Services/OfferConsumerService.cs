using Microsoft.Azure.EventHubs.Processor;
using System;
using System.ComponentModel.Composition;

namespace Stellar.Common.Services
{
    [Export(typeof(IOfferConsumerService))]
    public class OfferConsumerService : OfferBaseService<OfferConsumerService>, IDisposable, IOfferConsumerService
    {
        const string CFG_OFFER_CONSUMER_GROUP = "offer.consumer.group";
        const string CFG_OFFER_STORAGE_CONNECTIONSTRING = "offer.storage.connectionstring";
        const string CFG_OFFER_STORAGE_CONTAINER = "offer.storage.container";

        private EventProcessorHost eventProcessorHost;

        private string consumerGroup;
        private string storageConnectionString;
        private string storageContainer;        

        [ImportingConstructor()]
        public OfferConsumerService(ISettingsService settingsService)
            : base(settingsService)
        {
            this.settingsService = settingsService;

            consumerGroup = settingsService.GetAppSettings(CFG_OFFER_CONSUMER_GROUP);
            storageConnectionString = settingsService.GetAppSettings(CFG_OFFER_STORAGE_CONNECTIONSTRING);
            storageContainer = settingsService.GetAppSettings(CFG_OFFER_STORAGE_CONTAINER);

            eventProcessorHost = new EventProcessorHost(
                hubName,
                consumerGroup,
                hubConnectionString,
                storageConnectionString,
                storageContainer);
        }

        public async void ReceiveOffers()
        {
            var eventProcessorFactory = new Fac(OnOfferAdded);

            await eventProcessorHost.RegisterEventProcessorFactoryAsync(eventProcessorFactory);
            //await eventProcessorHost.RegisterEventProcessorAsync<OfferEventProcessor>();            
        }

        public void Dispose()
        {
            CleanUp();
        }

        private async void CleanUp()
        {
            if (eventProcessorHost != null)
            {
                await eventProcessorHost.UnregisterEventProcessorAsync();
            }
        }
    }

    public class Fac : IEventProcessorFactory
    {
        private Action<Offer> offerHandler;

        public Fac(Action<Offer> offerHandler)
        {
            this.offerHandler = offerHandler;
        }
            
        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new OfferEventProcessor(offerHandler);
        }
    }
}
