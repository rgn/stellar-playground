using Stellar.Common.Services;
using Stellar.Common.Ui.ViewModels;
using System;
using System.ComponentModel.Composition;

namespace Stellar.Customer.ViewModels
{
    [Export(typeof(ExecutableOfferOverviewViewModel))]
    public class ExecutableOfferOverviewViewModel : OfferOverviewViewModel
    {
        Random rand;
        IStellarService stellarService;

#if DEBUG        
        public ExecutableOfferOverviewViewModel()
            : this(new DummyOfferConsumerService(new SettingsService()), new DummyStellarService(new DummySettingsService()))
        {            
        }
#endif

        [ImportingConstructor()]
        public ExecutableOfferOverviewViewModel(IOfferConsumerService offerConsumerService, IStellarService stellarService)
            : base(offerConsumerService)
        {
            rand = new Random();

            this.stellarService = stellarService;            
        }

        public void ExecuteTransaction()
        {
            if(this.SelectedOffer != null)
            {
                var amount = rand.Next(1, 100) * SelectedOffer.Price;
                stellarService.MakeTransaction(SelectedOffer.Retailer.AccountId, amount, $"Thanks for the {SelectedOffer.Product}");
            }
        }
    }
}
