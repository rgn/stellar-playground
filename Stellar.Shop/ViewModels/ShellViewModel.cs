using System;
using System.Linq;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Stellar.Common.Services;
using Common.Logging;
using Stellar.Common.Ui.ViewModels;
using Stellar.Common.Ui;
using Stellar.Common.Ui.Controls;

namespace Stellar.Shop.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell
    {
        [Import(typeof(ISettingsService))]
        public ISettingsService SettingsService { get; set; }

        [ImportMany(typeof(FlyoutBaseViewModel))]
        public IObservableCollection<FlyoutBaseViewModel> FlyoutViewModels { get; set; }

        //[Import(typeof(OfferOverviewViewModel))]
        public OfferOverviewViewModel OfferOverviewVieModel { get; set; }

        //[Import(typeof(OfferCreateViewModel))]
        public OfferCreateViewModel OfferCreateViewModel { get; set; }

        //[Import(typeof(PaymentOverviewViewModel))]
        public PaymentOverviewViewModel PaymentOverviewViewModel { get; set; }

        //[Import(typeof(AccountViewModel))
        public AccountViewModel AccountViewModel { get; set; }

#if DEBUG
        public ShellViewModel()
            : this(new DummySettingsService(), new DummyOfferProducerService(new DummySettingsService()), new DummyStellarService(new DummySettingsService()))
        {
        }
#endif

        [ImportingConstructor()]
        public ShellViewModel(ISettingsService settingsService, IOfferProducerService offerProducerService, IStellarService stellarService)
        {
            FlyoutViewModels = new BindableCollection<FlyoutBaseViewModel>();

            OfferOverviewVieModel = new OfferOverviewViewModel(offerProducerService);
            OfferCreateViewModel = new OfferCreateViewModel(settingsService, offerProducerService);
            PaymentOverviewViewModel = new PaymentOverviewViewModel(stellarService);
            AccountViewModel = new AccountViewModel(stellarService);
        }

        public void Close()
        {
            this.TryClose();
        }

        public void ToggleShopFlyout()
        {
            ToggleFlyout<RetailerFlyoutViewModel>();
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
