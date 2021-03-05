using Core.Models;
using Core.Models.Plans;
using Service.Repositories;
using System;
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

        public async Task<GoalOptions> GetGoalOptions()
        {
            var settings = (await AppSettingsRepository.Get(1).ConfigureAwait(false)).First();
            var oneDay = 1000 * 60 * 60 * 24;
            var session = settings.SessionSettings.SessionDuration;
            var shortBreak = settings.SessionSettings.ShortBreakDuration;
            var longBreak = settings.SessionSettings.LongBreakDuration;
            var pairDuration = session + shortBreak;
            var seriesDuration = pairDuration * 4 + longBreak;
            var totalSeries = oneDay / seriesDuration;
            var remainingPairs = (oneDay - totalSeries * seriesDuration) / pairDuration;

            return new GoalOptions
            {
                Sessions = new Range<int> { Min = 1, Max = totalSeries * 4 + remainingPairs },
                SessionDuration = session
            };
        }

        public async Task<DailyPlan> GetDailyPlan(DateTime date)
        {
            var plan = await DailyPlanRepository.GetByDate(date).ConfigureAwait(false);

            if (plan != null)
            {
                return plan;
            }

            var options = await GetGoalOptions().ConfigureAwait(false);

            return new DailyPlan(date)
            {
                Goal = new Goal
                {
                    Sessions = Math.Min(12, options.Sessions.Max),
                    SessionDuration = options.SessionDuration
                }
            };
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
