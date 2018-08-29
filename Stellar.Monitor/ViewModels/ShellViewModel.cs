using Caliburn.Micro;
using Stellar.Common.Services;
using Stellar.Common.Ui;
using Stellar.Common.Ui.ViewModels;
using System.ComponentModel.Composition;
using System.Linq;

namespace Stellar.Monitor.ViewModels
{
    [Export(typeof(IShell))]
    class ShellViewModel : Screen, IShell
    {
        [ImportMany(typeof(FlyoutBaseViewModel))]
        public IObservableCollection<FlyoutBaseViewModel> FlyoutViewModels { get; set; }

        //[Import(typeof(OfferOverviewViewModel))]
        public OfferOverviewViewModel OfferOverviewViewModel { get; set; }

        //[Import(typeof(PaymentOverviewViewModel))]
        public PaymentOverviewViewModel PaymentOverviewViewModel { get; set; }

        //[Import(typeof(TransactionOverviewViewModel))]
        public TransactionOverviewViewModel TransactionOverviewViewModel { get; set; }

        //[Import(typeof(MapViewModel))]
        public MapViewModel MapViewModel { get; set; }

#if DEBUG
        public ShellViewModel()
            : this(new DummyStellarService(new DummySettingsService()), new DummyOfferConsumerService(new DummySettingsService()))
        {
        }
#endif

        [ImportingConstructor()]
        public ShellViewModel(IStellarService stellarService, IOfferConsumerService offerConsumerService)
        {
            FlyoutViewModels = new BindableCollection<FlyoutBaseViewModel>();

            OfferOverviewViewModel = new OfferOverviewViewModel(offerConsumerService);
            OfferOverviewViewModel.Header = "Latest offers";

            PaymentOverviewViewModel = new PaymentOverviewViewModel(stellarService);            

            TransactionOverviewViewModel = new TransactionOverviewViewModel(stellarService);
            TransactionOverviewViewModel.Header = "Latest transactions";

            MapViewModel = new MapViewModel();
        }        

        public void ToggleStellarFlyout()
        {
            ToggleFlyout<StellarFlyoutViewModel>();
        }

        private void ToggleFlyout<T>()
        {
            var flyout = this.FlyoutViewModels.Where(x => x.GetType().Equals(typeof(T))).FirstOrDefault();

            if (flyout != null)
            {
                flyout.IsOpen = !flyout.IsOpen;
            }
            else
            {
                //TODO: add logging
            }
        }
    }
}
