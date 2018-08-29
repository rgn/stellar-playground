using System;

namespace Stellar.Common
{
    public class AccountEventArgs : EventArgs
    {
        public Account Account { get; private set; }

        public AccountEventArgs(Account Account)            
        {
            this.Account = Account;
        }
    }
}
