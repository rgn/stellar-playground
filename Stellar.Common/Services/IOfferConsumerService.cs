namespace Stellar.Common.Services
{
    public interface IOfferConsumerService : IOfferBaseService
    {
        void ReceiveOffers();
    }
}