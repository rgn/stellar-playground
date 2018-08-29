using System.Configuration;
using System.ComponentModel.Composition;
using Common.Logging;

namespace Stellar.Common.Services
{
    [Export(typeof(ISettingsService))]    
    public class SettingsService : ISettingsService
    {
        ILog logger = LogManager.GetLogger(typeof(SettingsService));

        [ImportingConstructor()]
        public SettingsService()
        {
            logger.Trace("Created");
        }

        public string GetAppSettings(string key)
        {
            logger.Trace($"Get settings value for key {key}.");

            var value = string.Empty;

            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] != null)
                {
                    value = settings[key].Value;

                    logger.Debug($"Key {key} has value `{value}`.");
                }
                {
                    logger.Warn($"Key {key} was not found.");
                }
            }
            catch (ConfigurationErrorsException ex) 
            {
                logger.Error($"Error reading app settings", ex);
            }

            return value;
        }

        public void AddOrUpdateAppSettings(string key, string value)
        {
            logger.Trace($"Write value for key {key}.");

            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);

                    logger.Debug($"Added value for key {key}.");
                }
                else
                {
                    settings[key].Value = value;

                    logger.Debug($"Updated value for key {key}.");
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                logger.Error($"Error writing app settings", ex);
            }
        }
    }
}
