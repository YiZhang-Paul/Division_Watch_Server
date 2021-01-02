using Core.Models;
using Service.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TaskItemService
    {
        private TaskItemRepository TaskItemRepository { get; set; }

        public TaskItemService(TaskItemRepository taskItemRepository)
        {
            TaskItemRepository = taskItemRepository;
        }

        public async Task<IEnumerable<TaskItem>> GetTaskItems(int limit)
        {
            try
            {
                return await TaskItemRepository.Get(limit).ConfigureAwait(false);
            }
            catch
            {
                return new List<TaskItem>();
            }
        }

        public async Task<TaskItem> GetTaskItem(string id)
        {
            try
            {
                return await TaskItemRepository.Get(id).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> AddTaskItem(TaskItem item)
        {
            try
            {
                await TaskItemRepository.Add(item).ConfigureAwait(false);

                return item.Id;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<bool> UpdateTaskItem(TaskItem item)
        {
            try
            {
                await TaskItemRepository.Replace(item).ConfigureAwait(false);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTaskItem(string id)
        {
            try
            {
                await TaskItemRepository.Delete(id).ConfigureAwait(false);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
