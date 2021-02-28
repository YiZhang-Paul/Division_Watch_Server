using Core.Configurations;
using Core.Models;
using Microsoft.Extensions.Options;

namespace Service.Repositories
{
    public class DailyPlanRepository : DatabaseConnector<DailyPlan>
    {
        public DailyPlanRepository(IOptions<DatabaseConfiguration> configuration) : base(configuration, "DailyPlan") { }
    }
}
