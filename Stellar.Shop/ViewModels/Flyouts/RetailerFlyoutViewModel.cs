using MahApps.Metro.Controls;
using Stellar.Common;
using Stellar.Common.Services;
using Stellar.Common.Ui.ViewModels;
using System;
using System.ComponentModel.Composition;

namespace Stellar.Shop.ViewModels
{
    [Export(typeof(FlyoutBaseViewModel))]
    public class RetailerFlyoutViewModel : FlyoutBaseViewModel
    {
        private Retailer retailer;

        public Guid Id { get => retailer.Id; }
        public string Name { get => retailer.Name; set => retailer.Name = value; }
        public decimal Longitude { get => this.retailer.Longitude; set => this.retailer.Longitude = value; }
        public decimal Latitude { get => this.retailer.Latitude; set => this.retailer.Latitude = value; }

        ISettingsService settingsService;

        [ImportingConstructor()]
        public RetailerFlyoutViewModel(ISettingsService settingsService)
        {
            this.settingsService = settingsService;

            this.Header = "Shop";
            this.Position = Position.Left;
            this.Theme = FlyoutTheme.Accent;


            this.retailer = this.settingsService.GetRetailer();
        }

        public void SaveRetailer()
        {
            this.settingsService.SaveRetailer(this.retailer);
        }
    }
}
