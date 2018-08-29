using System;

namespace Stellar.Common
{
    public class OfferAddedEventArgs : EventArgs
    {
        public Offer Offer { get; private set; }

        public OfferAddedEventArgs(Offer Offer)
        {
            this.Offer = Offer;
        }
    }
}
