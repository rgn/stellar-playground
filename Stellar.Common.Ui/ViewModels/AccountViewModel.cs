using Caliburn.Micro;
using Stellar.Common.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stellar.Common.Ui.ViewModels
{
    [Export(typeof(AccountViewModel))]
    public class AccountViewModel : Screen
    {
#if DEBUG
        public AccountViewModel()
        {
            this.Account = new Account
            {
                AccountId = "AccountID",
                Balances = new List<Balance>
                {
                    new Balance
                    {
                        AssetCode = "Bal1",
                        AssetIssuer = "AssetIssuer1",
                        AssetType = "AssetType1",
                        BalanceValue = "1",
                        Limit = "100"
                    },
                    new Balance
                    {
                        AssetCode = "Bal2",
                        AssetIssuer = "AssetIssuer2",
                        AssetType = "AssetType2",
                        BalanceValue = "2",
                        Limit = "200"
                    },
                },
                Data = new Dictionary<string, string>
                {
                    { "key1", "data1" },
                    { "key2", "data2" },
                    { "key3", "data3" }
                },
                HomeDomain = "HomeDomain",
                InflationDestination = "InflationDestination"
            };
        }
#endif

        private IStellarService stellarService;
        private bool loadAccount = true;
        private Account account;
        public Account Account
        {
            get
            {
                return account;
            }

            set
            {
                if(account == null || !account.Equals(value))
                {
                    account = value;
                    NotifyOfPropertyChange(() => Account);
                }
            }
        }

        [ImportingConstructor()]
        public AccountViewModel(IStellarService stellarService)
        {
            this.stellarService = stellarService;
            this.stellarService.AccountReceived += StellarService_AccountReceived;

            var accountUpdateTask = new Task(LoadMyAccount);

            accountUpdateTask.Start();
        }

        private void LoadMyAccount()
        {
            while (loadAccount)
            {
                stellarService.GetMyBalance();

                Thread.Sleep(60000); // every minute
            }
        }

        private void StellarService_AccountReceived(object sender, AccountEventArgs e)
        {
            this.Account = e.Account;
        }
    }
}
