namespace Stellar.Common.Services
{
    public interface IOfferProducerService : IOfferBaseService
    {
        void SendOffer(Offer offer);
    }
}