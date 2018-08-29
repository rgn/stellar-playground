namespace Stellar.Common.Services
{
    public interface ISettingsService
    {
        string GetAppSettings(string key);
        void AddOrUpdateAppSettings(string key, string value);
    }
}
