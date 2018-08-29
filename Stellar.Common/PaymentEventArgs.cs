using System;

namespace Stellar.Common
{
    public class PaymentEventArgs : EventArgs
    {
        public Payment Payment { get; private set; }

        public PaymentEventArgs(Payment Payment)            
        {
            this.Payment = Payment;
        }
    }
}
