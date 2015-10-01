using System.Configuration;

namespace JeroenPot.Common
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        public string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        public ConnectionStringSettings GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name];
        }

        public string GetAzureWebJobsStorageConnection()
        {
            ConnectionStringSettings connectionString = GetConnectionString("AzureWebJobsStorage");
            if (connectionString == null)
            {
                throw new ConfigurationErrorsException("Connectionstring AzureWebJobsStorage not found");
            }

            return connectionString.ConnectionString;
        }
    }
}