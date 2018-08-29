using MahApps.Metro.Controls;
using Stellar.Common.Services;
using System;
using System.ComponentModel.Composition;

namespace Stellar.Common.Ui.ViewModels
{
    [Export(typeof(FlyoutBaseViewModel))]
    public class StellarFlyoutViewModel : FlyoutBaseViewModel
    {
        private IStellarService stellarService;

        private string accountId;
        private string seed;
        private string server;
        private bool isTestnet;        

        public string AccountId { get => accountId; set => accountId = value; }
        public string Seed { get => seed; set => seed = value; }
        public string Server { get => server; set => server = value; }
        public bool IsTestnet { get => isTestnet; set => isTestnet = value; }

        [ImportingConstructor()]
        public StellarFlyoutViewModel(IStellarService stellarService)
        {
            this.stellarService = stellarService;
            this.Header = "Stellar";
            this.Position = Position.Left;
            this.Theme = FlyoutTheme.Accent;

            AccountId = this.stellarService.AccountId;
            Seed = this.stellarService.Seed;
            Server = this.stellarService.Server;
            IsTestnet = this.stellarService.UseTestnet;
        }
    }
}
