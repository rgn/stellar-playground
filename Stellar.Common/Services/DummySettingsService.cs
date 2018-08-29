namespace Stellar.Common.Services
{
    public class DummySettingsService : ISettingsService
    {
        public void AddOrUpdateAppSettings(string key, string value)
        {            
        }

        public string GetAppSettings(string key)
        {
            return string.Empty;
        }
    }
}
