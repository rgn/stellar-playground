using Caliburn.Micro;
using Stellar.Common.Services;
using System.ComponentModel.Composition;

namespace Stellar.Common.Ui.ViewModels
{
    [Export(typeof(OfferOverviewViewModel))]
    public class PaymentOverviewViewModel : Screen
    {
        IStellarService stellarService;

        public BindableCollection<Payment> Payments
        {
            get; private set;
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

#if DEBUG
        public PaymentOverviewViewModel()
            : this(new DummyStellarService(new DummySettingsService()))
        {
        }
#endif

        [ImportingConstructor()]
        public PaymentOverviewViewModel(IStellarService stellarService)
        {
            this.Header = "Received payments";
            this.stellarService = stellarService;
            this.Payments = new BindableCollection<Payment>();

            this.stellarService.PaymentReceived += StellarService_PaymentReceived;
            this.stellarService.ReceiveMyPayments();
        }

        private void StellarService_PaymentReceived(object sender, PaymentEventArgs e)
        {
            Payments.Add(e.Payment);
        }       
    }
}
