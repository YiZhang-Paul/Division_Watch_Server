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

        public async Task<IEnumerable<TaskItem>> GetIncompleteTaskItems(int limit)
        {
            try
            {
                return await TaskItemRepository.GetIncompleteTaskItems(limit).ConfigureAwait(false);
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

        public async Task<TaskItem> AddChildTaskItem(string parentId, TaskItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                throw new ArgumentException("Must provide a valid name.");
            }

            var parent = await TaskItemRepository.Get(parentId).ConfigureAwait(false);

            if (parent == null)
            {
                throw new ArgumentException("Parent task not found.");
            }

            try
            {
                item.Parent = parent.Id;
                item.Category ??= parent.Category;
                item.Deadline ??= parent.Deadline;

                item.Priority ??= new RankItem
                {
                    Rank = (int)Priority.Normal,
                    Name = Enum.GetName(typeof(Priority), Priority.Normal)
                };

                await TaskItemRepository.Add(item).ConfigureAwait(false);

                return item;
            }
            catch
            {
                return null;
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
            var deadlines = Enumerable.Range(0, 14).Select(_ => startDate.AddDays(_).ToShortDateString());
            var categories = await CategoryRepository.Get().ConfigureAwait(false);
            var estimates = Enumerable.Range(1, 8).Select(_ => query.EstimationBase * _);

            return new TaskOptions
            {
                Categories = categories.ToList(),
                Priorities = ToRankItem(typeof(Priority)).ToList(),
                Deadlines = deadlines.ToList(),
                Estimates = estimates.ToList()
            };
        }

        private IEnumerable<RankItem> ToRankItem(Type type)
        {
            return Enum.GetNames(type).Select(_ => new RankItem
            {
                Rank = (int)Enum.Parse(type, _),
                Name = _
            });
        }
    }
}
