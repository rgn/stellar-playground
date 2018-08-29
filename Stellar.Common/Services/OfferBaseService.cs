using Common.Logging;
using System;

namespace Stellar.Common.Services
{
    public abstract class OfferBaseService<T> : IOfferBaseService
    {
        protected ILog logger = LogManager.GetLogger<T>();

        protected const string CFG_OFFER_HUB_NAME = "offer.hub.name";
        protected const string CFG_OFFER_HUB_CONNECTIONSTRING = "offer.hub.connectionstring";

        protected string hubName;
        protected string hubConnectionString;

        protected ISettingsService settingsService;

        public OfferBaseService(ISettingsService settingsService)
        {
            this.settingsService = settingsService;

            hubName = settingsService.GetAppSettings(CFG_OFFER_HUB_NAME);
            hubConnectionString = settingsService.GetAppSettings(CFG_OFFER_HUB_CONNECTIONSTRING);
        }

        public event EventHandler<OfferAddedEventArgs> Added;

        protected void OnOfferAdded(Offer offer)
        {
            Added?.Invoke(this, new OfferAddedEventArgs(offer));
        }
    }      
}
