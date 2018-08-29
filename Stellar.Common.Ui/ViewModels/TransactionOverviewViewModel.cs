using System;
using Caliburn.Micro;
using Stellar.Common.Services;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Stellar.Common.Ui.ViewModels
{    
    [Export(typeof(OfferOverviewViewModel))]
    public class TransactionOverviewViewModel : Screen
    {
        IStellarService stellarService;

        public BindableCollection<Transaction> Transactions
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
        public TransactionOverviewViewModel()
            : this(new DummyStellarService(new DummySettingsService()))
        {            
            
        }
#endif

        [ImportingConstructor()]
        public TransactionOverviewViewModel(IStellarService stellarService)
        {
            this.Header = "Received transactions";
            this.stellarService = stellarService;
            this.Transactions = new BindableCollection<Transaction>();            

            this.stellarService.TransactionReceived += StellarService_TransactionReceived;
            this.stellarService.ReceiveAccountTransactions();            
        }        

        private void StellarService_TransactionReceived(object sender, TransactionEventArgs e)
        {
            Transactions.Add(e.Transaction);
        }
    }
}
