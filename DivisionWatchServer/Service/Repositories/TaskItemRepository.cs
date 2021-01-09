using Core.Configurations;
using Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class TaskItemRepository : DatabaseConnector<TaskItem>
    {
        public TaskItemRepository(IOptions<DatabaseConfiguration> configuration) : base(configuration, "TaskItem") { }

        public async Task<IEnumerable<TaskItem>> GetParentTaskItems(int limit)
        {
            var filter = Builders<TaskItem>.Filter.Eq(_ => _.Parent, null);

            return await Collection.Find(filter).Limit(limit).ToListAsync().ConfigureAwait(false);
        }
    }
}
