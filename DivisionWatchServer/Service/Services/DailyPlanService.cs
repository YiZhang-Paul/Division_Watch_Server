using Core.Models;
using Service.Repositories;
using System;
using System.Threading.Tasks;

namespace Service.Services
{
    public class DailyPlanService
    {
        private DailyPlanRepository DailyPlanRepository { get; set; }

        public DailyPlanService(DailyPlanRepository dailyPlanRepository)
        {
            DailyPlanRepository = dailyPlanRepository;
        }

        public async Task<DailyPlan> GetDailyPlan(DateTime date)
        {
            var plan = await DailyPlanRepository.GetByDate(date).ConfigureAwait(false);

            return plan == null ? new DailyPlan { Date = date } : plan;
        }

        public async Task<DailyPlan> UpsertDailyPlan(DailyPlan plan)
        {
            var existing = await DailyPlanRepository.GetByDate(plan.Date).ConfigureAwait(false);

            if (existing == null)
            {
                await DailyPlanRepository.Add(plan).ConfigureAwait(false);
            }
            else
            {
                await DailyPlanRepository.Replace(plan).ConfigureAwait(false);
            }

            return plan;
        }
    }
}
