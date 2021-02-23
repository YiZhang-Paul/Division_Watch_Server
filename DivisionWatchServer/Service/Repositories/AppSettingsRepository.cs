using Core.Configurations;
using Core.Models.Settings;
using Microsoft.Extensions.Options;

namespace Service.Repositories
{
    public class AppSettingsRepository : DatabaseConnector<AppSettings>
    {
        public AppSettingsRepository(IOptions<DatabaseConfiguration> configuration) : base(configuration, "AppSettings") { }
    }
}
