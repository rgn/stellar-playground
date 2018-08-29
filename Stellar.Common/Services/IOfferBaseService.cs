using System;

namespace Stellar.Common.Services
{
    public interface IOfferBaseService
    {
        event EventHandler<OfferAddedEventArgs> Added;
    }
}