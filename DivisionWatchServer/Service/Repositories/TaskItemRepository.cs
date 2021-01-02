using Core.Configurations;
using Core.Models;
using Microsoft.Extensions.Options;

namespace Service.Repositories
{
    public class TaskItemRepository : DatabaseConnector<TaskItem>
    {
        public TaskItemRepository(IOptions<DatabaseConfiguration> configuration) : base(configuration, "TaskItem") { }
    }
}
