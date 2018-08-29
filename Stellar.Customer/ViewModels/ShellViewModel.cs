using Caliburn.Micro;
using Stellar.Common.Services;
using Stellar.Common.Ui;
using Stellar.Common.Ui.ViewModels;
using System.ComponentModel.Composition;
using System.Linq;

namespace Stellar.Customer.ViewModels
{
    [Export(typeof(IShell))]
    class ShellViewModel : Screen, IShell
    {
        [ImportMany(typeof(FlyoutBaseViewModel))]
        public IObservableCollection<FlyoutBaseViewModel> FlyoutViewModels { get; set; }

        //[Import(typeof(OfferOverviewViewModel))]
        public ExecutableOfferOverviewViewModel ExecutableOfferOverviewViewModel { get; set; }

        //[Import(typeof(PaymentOverviewViewModel))]
        public PaymentOverviewViewModel PaymentOverviewViewModel { get; set; }

        //[Import(typeof(AccountViewModel))]
        public AccountViewModel AccountViewModel { get; set; }

#if DEBUG
        public ShellViewModel()
            : this(new DummyStellarService(new SettingsService()), new DummyOfferConsumerService(new SettingsService()))
        {
        }
#endif

        [ImportingConstructor()]
        public ShellViewModel(IStellarService stellarService, IOfferConsumerService offerConsumerService)
        {
            FlyoutViewModels = new BindableCollection<FlyoutBaseViewModel>();

            ExecutableOfferOverviewViewModel = new ExecutableOfferOverviewViewModel(offerConsumerService, stellarService);
            ExecutableOfferOverviewViewModel.Header = "Latest offers";

            PaymentOverviewViewModel = new PaymentOverviewViewModel(stellarService);
            PaymentOverviewViewModel.Header = "My payments";

            AccountViewModel = new AccountViewModel(stellarService);

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
