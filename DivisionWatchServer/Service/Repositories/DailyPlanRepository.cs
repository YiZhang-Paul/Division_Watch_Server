using Core.Configurations;
using Core.Models.Plans;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class DailyPlanRepository : DatabaseConnector<DailyPlan>
    {
        public DailyPlanRepository(IOptions<DatabaseConfiguration> configuration) : base(configuration, "DailyPlan") { }

        public async override Task Add(DailyPlan plan)
        {
            plan.Date = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day);
            await base.Add(plan).ConfigureAwait(false);
        }

        public async Task<DailyPlan> GetByDate(DateTime date)
        {
            var target = new DateTime(date.Year, date.Month, date.Day);
            var filter = Builders<DailyPlan>.Filter.Eq(_ => _.Date, target);

            return await Collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
