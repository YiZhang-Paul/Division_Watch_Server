using Core.Configurations;
using Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class DailyPlanRepository : DatabaseConnector<DailyPlan>
    {
        public DailyPlanRepository(IOptions<DatabaseConfiguration> configuration) : base(configuration, "DailyPlan") { }

        public async Task<DailyPlan> GetByDate(DateTime date)
        {
            var filter = Builders<DailyPlan>.Filter.Eq(_ => _.Date, date);

            return await Collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
