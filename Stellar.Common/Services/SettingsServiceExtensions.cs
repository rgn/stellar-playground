using System;

namespace Stellar.Common.Services
{
    public static class SettingsServiceExtensions
    {
        #region Retailer
        public const string CFG_RETAILER_ID = "retailer.id";
        public const string CFG_RETAILER_NAME = "retailer.name";
        public const string CFG_RETAILER_LONGITUDE = "retailer.longitude";
        public const string CFG_RETAILER_LATITUDE = "retailer.latitude";
        #endregion

        public static Retailer GetRetailer(this ISettingsService settingsService)
        {
            Retailer retailer;
            var id = settingsService.GetAppSettings(CFG_RETAILER_ID);
            var accountId = settingsService.GetAppSettings(StellarService.CFG_STELLAR_ACCOUNT_ID);
            var name = settingsService.GetAppSettings(CFG_RETAILER_NAME);
            var latitude = settingsService.GetAppSettings(CFG_RETAILER_LATITUDE);
            var longitude = settingsService.GetAppSettings(CFG_RETAILER_LONGITUDE);

            Guid gId;
            Guid.TryParse(id, out gId);

            decimal decLatitude;
            decimal.TryParse(latitude, out decLatitude);

            decimal decLongitude;
            decimal.TryParse(longitude, out decLongitude);

            if (string.IsNullOrEmpty(id))
            {
                retailer = new Retailer()
                {
                    Name = name,
                    AccountId = accountId,
                    Longitude = decLongitude,
                    Latitude = decLatitude
                };
            }
            else
            {
                retailer = new Retailer(gId)
                {
                    Name = name,
                    AccountId = accountId,
                    Longitude = decLongitude,
                    Latitude = decLatitude
                };                
            }

            return retailer;
        }

        public static void SaveRetailer(this ISettingsService settingsService, Retailer retailer)
        {
            settingsService.AddOrUpdateAppSettings(CFG_RETAILER_ID, retailer.Id.ToString());
            settingsService.AddOrUpdateAppSettings(CFG_RETAILER_NAME, retailer.Name);
            settingsService.AddOrUpdateAppSettings(CFG_RETAILER_LONGITUDE, retailer.Longitude.ToString());
            settingsService.AddOrUpdateAppSettings(CFG_RETAILER_LATITUDE, retailer.Latitude.ToString());
        }        
    }
}
