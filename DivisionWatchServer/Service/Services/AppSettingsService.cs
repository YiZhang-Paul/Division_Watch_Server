using Core.Models;
using Core.Models.Settings;
using Service.Repositories;
using System.Collections.Generic;
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

        public async Task<SoundSettings> GetSoundSettings()
        {
            return (await GetAppSettings().ConfigureAwait(false)).SoundSettings;
        }

        public SoundSettingsOptions GetSoundSettingsOptions()
        {
            return new SoundSettingsOptions
            {
                MasterVolume = new Range<int> { Min = 0, Max = 100 },
                UiVolume = new Range<int> { Min = 0, Max = 100 },
                ClockVolume = new Range<int> { Min = 0, Max = 100 }
            };
        }

        public async Task<bool> UpdateSoundSettings(SoundSettings settings)
        {
            try
            {
                var appSettings = await GetAppSettings().ConfigureAwait(false);
                appSettings.SoundSettings = settings;
                await AppSettingsRepository.Replace(appSettings).ConfigureAwait(false);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public SessionSettingsOptions GetSessionSettingsOptions()
        {
            var oneMinute = 1000 * 60;

            return new SessionSettingsOptions
            {
                DurationSeries = new List<DurationSeries>
                {
                    new DurationSeries
                    {
                        SessionDuration = oneMinute * 25,
                        ShortBreakDuration = oneMinute * 5,
                        LongBreakRange = new Range<int> { Min = oneMinute * 15, Max = oneMinute * 30 }
                    },
                    new DurationSeries
                    {
                        SessionDuration = oneMinute * 40,
                        ShortBreakDuration = oneMinute * 12,
                        LongBreakRange = new Range<int> { Min = oneMinute * 23, Max = oneMinute * 41 }
                    },
                    new DurationSeries
                    {
                        SessionDuration = oneMinute * 52,
                        ShortBreakDuration = oneMinute * 17,
                        LongBreakRange = new Range<int> { Min = oneMinute * 30, Max = oneMinute * 50 }
                    }
                },
                DailyLimits = new[] { 2, 4, 6, 8, 10, 12, 14, 16 }.Select(_ => oneMinute * 60 * _).ToList()
            };
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
