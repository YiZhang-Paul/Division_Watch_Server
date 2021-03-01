using Core.Configurations;
using Core.Models.TaskItem;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class TaskItemRepository : DatabaseConnector<TaskItem>
    {
        public TaskItemRepository(IOptions<DatabaseConfiguration> configuration) : base(configuration, "TaskItem") { }

        public async Task<IEnumerable<TaskItem>> GetIncompleteTaskItems(int limit)
        {
            var filter = Builders<TaskItem>.Filter.Eq(_ => _.IsCompleted, false);

            return await Collection.Find(filter).Limit(limit).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TaskItem>> GetTaskItemsByCategory(string categoryId)
        {
            var filter = Builders<TaskItem>.Filter.Eq(_ => _.CategoryId, categoryId);

            return await Collection.Find(filter).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TaskItem>> GetChildTaskItems(string parentId)
        {
            var filter = Builders<TaskItem>.Filter.Eq(_ => _.Parent, parentId);

            return await Collection.Find(filter).ToListAsync().ConfigureAwait(false);
        }
    }
}
