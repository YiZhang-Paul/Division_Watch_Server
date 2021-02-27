using Core.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/v1/app-settings")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        private AppSettingsService AppSettingsService { get; set; }

        public AppSettingsController(AppSettingsService appSettingsService)
        {
            AppSettingsService = appSettingsService;
        }

        [HttpGet]
        [Route("sound")]
        public async Task<SoundSettings> GetSoundSettings()
        {
            return await AppSettingsService.GetSoundSettings().ConfigureAwait(false);
        }

        [HttpPut]
        [Route("sound")]
        public async Task<bool> UpdateSoundSettings([FromBody]SoundSettings settings)
        {
            return await AppSettingsService.UpdateSoundSettings(settings).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("session")]
        public async Task<SessionSettings> GetSessionSettings()
        {
            return await AppSettingsService.GetSessionSettings().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("session/options")]
        public SessionSettingsOptions GetSessionSettingsOptions()
        {
            return AppSettingsService.GetSessionSettingsOptions();
        }

        [HttpPut]
        [Route("session")]
        public async Task<bool> UpdateSessionSettings([FromBody]SessionSettings settings)
        {
            return await AppSettingsService.UpdateSessionSettings(settings).ConfigureAwait(false);
        }
    }
}
