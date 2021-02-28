using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/v1/daily-plan")]
    [ApiController]
    public class DailyPlanController : ControllerBase
    {
        private DailyPlanService DailyPlanService { get; set; }

        public DailyPlanController(DailyPlanService dailyPlanService)
        {
            DailyPlanService = dailyPlanService;
        }

        [HttpGet]
        [Route("goals")]
        public async Task<IEnumerable<Goal>> GetGoalOptions()
        {
            return await DailyPlanService.GetGoalOptions().ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{date}")]
        public async Task<DailyPlan> GetDailyPlan(DateTime date)
        {
            return await DailyPlanService.GetDailyPlan(date).ConfigureAwait(false);
        }

        [HttpPut]
        [Route("")]
        public async Task<DailyPlan> UpsertDailyPlan([FromBody]DailyPlan plan)
        {
            return await DailyPlanService.UpsertDailyPlan(plan).ConfigureAwait(false);
        }
    }
}
