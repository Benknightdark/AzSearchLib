using AzSearchLib.Models;
using Microsoft.Extensions.Configuration;

namespace AzSearchLib.services
{
    public class AzSearchConfigService
    {
         private IConfiguration _config { get; }
        public AzSearchConfigService(IConfiguration config)
        {
            _config = config;


        }
        public AzSearchConfig GetAzSearchConfig()
        {
            var appConfig = new AzSearchConfig();
            _config.GetSection("AzSearch").Bind(appConfig);
            return appConfig;


        }
    }
}