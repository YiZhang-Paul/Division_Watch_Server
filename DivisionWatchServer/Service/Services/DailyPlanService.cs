using Core.Models;
using Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class DailyPlanService
    {
        private AppSettingsRepository AppSettingsRepository { get; set; }
        private DailyPlanRepository DailyPlanRepository { get; set; }

        public DailyPlanService(AppSettingsRepository appSettingsRepository, DailyPlanRepository dailyPlanRepository)
        {
            AppSettingsRepository = appSettingsRepository;
            DailyPlanRepository = dailyPlanRepository;
        }

        public async Task<IEnumerable<Goal>> GetGoalOptions()
        {
            var settings = (await AppSettingsRepository.Get(1).ConfigureAwait(false)).First();
            var duration = settings.SessionSettings.SessionDuration;
            var sessions = Enumerable.Range(1, 1000 * 60 * 60 * 24 / duration);

            return sessions.Select(_ => new Goal { Sessions = _, SessionDuration = duration });
        }

        public async Task<DailyPlan> GetDailyPlan(DateTime date)
        {
            return await DailyPlanRepository.GetByDate(date).ConfigureAwait(false) ?? new DailyPlan(date);
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
