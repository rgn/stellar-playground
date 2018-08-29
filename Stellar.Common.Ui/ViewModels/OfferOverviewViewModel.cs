using Caliburn.Micro;
using Stellar.Common.Services;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Threading;

namespace Stellar.Common.Ui.ViewModels
{    
    [Export(typeof(OfferOverviewViewModel))]
    public class OfferOverviewViewModel : Screen
    {
        IOfferProducerService offerProducerService;
        IOfferConsumerService offerConsumerService;

        public BindableCollection<Offer> Offers
        {
            get; private set;
        }

        private Offer selectedOffer;
        public Offer SelectedOffer
        {
            get { return selectedOffer; }
            set
            {
                selectedOffer = value;
                NotifyOfPropertyChange(() => SelectedOffer);
            }
        }

        private string header;
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                if (string.IsNullOrEmpty(header) || !header.Equals(value))
                {
                    header = value;
                    NotifyOfPropertyChange(() => Header);
                }
            }
        }

        public OfferOverviewViewModel(string header)
        {
            this.Header = header;
            this.Offers = new BindableCollection<Offer>();
        }

#if DEBUG
        public OfferOverviewViewModel()
            : this(new DummyOfferConsumerService(new DummySettingsService()))
        {           
        }
#endif

        [ImportingConstructor()]
        public OfferOverviewViewModel(IOfferProducerService offerProducerService)
            : this("My offers")
        {
            this.offerProducerService = offerProducerService;
            this.offerProducerService.Added += OfferService_Added;

            this.Offers = new BindableCollection<Offer>();
        }

        [ImportingConstructor()]
        public OfferOverviewViewModel(IOfferConsumerService offerConsumerService)
            : this("Received Offers")
        {
            this.offerConsumerService = offerConsumerService;
            this.offerConsumerService.Added += OfferService_Added;
            this.offerConsumerService.ReceiveOffers();

            AttemptingDeactivation += TransactionOverviewViewModel_AttemptingDeactivation;
            RemoveOutdateOffers().Start();
        }

        private void OfferService_Added(object sender, OfferAddedEventArgs e)
        {
            this.Offers.Add(e.Offer);
        }

        private bool removeOutdatedOffers = true;

        private void TransactionOverviewViewModel_AttemptingDeactivation(object sender, DeactivationEventArgs e)
        {
            removeOutdatedOffers = true;
        }

        private Task RemoveOutdateOffers()
        {
            return new Task(() => {

                while (removeOutdatedOffers)
                {
                    var outdatedOffers = Offers.Where(x => x.ValidTo < DateTime.Now);

                    for(int i = 0; i < outdatedOffers.Count(); i++)
                    {
                        Offers.Remove(outdatedOffers.ElementAt(i));
                    }

                    Thread.Sleep(500);
                };
            });
        }
    }
}
