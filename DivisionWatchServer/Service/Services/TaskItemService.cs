using Core.Enums;
using Core.Models;
using Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TaskItemService
    {
        private CategoryRepository CategoryRepository { get; set; }
        private TaskItemRepository TaskItemRepository { get; set; }

        public TaskItemService(CategoryRepository categoryRepository, TaskItemRepository taskItemRepository)
        {
            CategoryRepository = categoryRepository;
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

        public async Task<TaskOptions> GetTaskOptions(TaskOptionsQuery query)
        {
            var startDate = DateTime.Parse(query.CurrentDate);
            var deadLines = Enumerable.Range(0, 14).Select(_ => startDate.AddDays(_).ToShortDateString());
            var categories = await CategoryRepository.Get().ConfigureAwait(false);
            var estimates = Enumerable.Range(1, 8).Select(_ => query.EstimationBase * _);

            return new TaskOptions
            {
                Categories = categories.ToList(),
                Priorities = Enum.GetNames(typeof(Priority)).Select(_ => new RankItem { Rank = (int)Enum.Parse(typeof(Priority), _), Name = _ }).ToList(),
                DeadLines = deadLines.ToList(),
                Estimates = estimates.ToList()
            };
        }
    }
}
