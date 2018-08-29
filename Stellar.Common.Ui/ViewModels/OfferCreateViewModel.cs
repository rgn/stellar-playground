using Caliburn.Micro;
using Stellar.Common;
using Stellar.Common.Services;
using System;
using System.ComponentModel.Composition;

namespace Stellar.Common.Ui.ViewModels
{
    [Export(typeof(OfferCreateViewModel))]
    public class OfferCreateViewModel : Screen
    {
        ISettingsService settingsService;
        IOfferProducerService offerService;
        
        private DateTime validFrom;
        private DateTime validTo;
        private string product;
        private decimal price;

        public DateTime ValidFrom
        {
            get
            {
                return validFrom;
            }

            set
            {
                if (!validFrom.Equals(value))
                {
                    validFrom = value;
                    NotifyOfPropertyChange(() => ValidFrom);
                }               
            }
        }

        public DateTime ValidTo
        {
            get
            {
                return validTo;
            }

            set
            {
                if (!validTo.Equals(value))
                {
                    validTo = value;
                    NotifyOfPropertyChange(() => ValidTo);
                }
            }
        }

        public string Product
        {
            get
            {
                return product;
            }

            set {
                if (string.IsNullOrEmpty(product) || !product.Equals(value))
                {
                    product = value;
                    NotifyOfPropertyChange(() => Product);
                }                
            }
        }

        public decimal Price
        {
            get
            {
                return price;
            }

            set
            {
                if (!price.Equals(value))
                {
                    price = value;
                    NotifyOfPropertyChange(() => Price);
                }
            }
        }

        [ImportingConstructor()]
        public OfferCreateViewModel(ISettingsService settingsService, IOfferProducerService offerService)
        {
            this.settingsService = settingsService;
            this.offerService = offerService;
            this.offerService.Added += OfferService_Added;
            
            ResetOffer();
        }

        private void ResetOffer()
        {
            this.ValidFrom = DateTime.Today;
            this.ValidTo = DateTime.Today.AddMinutes(5.0);
            this.Product = string.Empty;
            this.Price = 1;
        }

        public void CreateOffer()
        {
            var retailer = this.settingsService.GetRetailer();           

            var offer = new Offer(retailer) {            
                ValidFrom = ValidFrom,
                ValidTo = ValidTo,
                Product = Product,
                Price = Price
            };
            
            offerService.SendOffer(offer);
        }

        private void OfferService_Added(object sender, OfferAddedEventArgs e)
        {
            ResetOffer();
        }
    }
}
