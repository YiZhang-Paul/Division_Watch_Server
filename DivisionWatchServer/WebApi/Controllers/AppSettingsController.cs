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
        [Route("session")]
        public async Task<SessionSettings> GetSessionSettings()
        {
            return await AppSettingsService.GetSessionSettings().ConfigureAwait(false);
        }

        [HttpPut]
        [Route("session")]
        public async Task<bool> GetSessionSettings([FromBody]SessionSettings settings)
        {
            return await AppSettingsService.UpdateSessionSettings(settings).ConfigureAwait(false);
        }
    }
}
