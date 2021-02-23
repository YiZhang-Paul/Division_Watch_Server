using Core.Models.Settings;
using Service.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AppSettingsService
    {
        private AppSettingsRepository AppSettingsRepository { get; set; }

        public AppSettingsService(AppSettingsRepository appSettingsRepository)
        {
            AppSettingsRepository = appSettingsRepository;
        }

        public async Task<SessionSettings> GetSessionSettings()
        {
            return (await GetAppSettings().ConfigureAwait(false)).SessionSettings;
        }

        public async Task<bool> UpdateSessionSettings(SessionSettings settings)
        {
            try
            {
                var appSettings = await GetAppSettings().ConfigureAwait(false);
                appSettings.SessionSettings = settings;
                await AppSettingsRepository.Replace(appSettings).ConfigureAwait(false);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<AppSettings> GetAppSettings()
        {
            return (await AppSettingsRepository.Get(1).ConfigureAwait(false)).First();
        }
    }
}
