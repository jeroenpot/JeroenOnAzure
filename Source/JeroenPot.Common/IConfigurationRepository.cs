using System.Configuration;

namespace JeroenPot.Common
{
    public interface IConfigurationRepository
    {
        string GetAppSetting(string name);
        ConnectionStringSettings GetConnectionString(string name);
    }
}
