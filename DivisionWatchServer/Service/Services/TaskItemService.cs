using Core.DTOs;
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
        private const int SkullDuration = 1500000;
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

        public async Task<AddChildResult> AddChildTaskItem(string parentId, TaskItem item)
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
                parent.Estimate = -1;
                item.Parent = parent.Id;
                item.Category ??= parent.Category;
                item.Deadline ??= parent.Deadline;
                item.Estimate = SkullDuration;
                item.Recur = parent.Recur;

                item.Priority ??= new RankItem
                {
                    Rank = (int)Priority.Normal,
                    Name = Enum.GetName(typeof(Priority), Priority.Normal)
                };

                var insertChildTask = TaskItemRepository.Add(item);
                var updateParentTask = TaskItemRepository.Replace(parent);
                await Task.WhenAll(insertChildTask, updateParentTask).ConfigureAwait(false);

                return new AddChildResult { Parent = parent, Child = item };
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

        public async Task<TaskOptions> GetTaskOptions(string currentDate)
        {
            var startDate = DateTime.Parse(currentDate);
            var deadlines = Enumerable.Range(0, 14).Select(_ => startDate.AddDays(_).ToShortDateString());
            var categories = await CategoryRepository.Get().ConfigureAwait(false);
            var maxSkulls = Math.Max(1, 1000 * 60 * 60 * 3 / SkullDuration);
            var estimates = Enumerable.Range(1, maxSkulls).Select(_ => SkullDuration * _);

            return new TaskOptions
            {
                Categories = categories.ToList(),
                Priorities = ToRankItem(typeof(Priority)).ToList(),
                Deadlines = new List<string> { string.Empty }.Concat(deadlines).ToList(),
                Estimates = new List<int> { 600000 }.Concat(estimates).ToList(),
                SkullDuration = SkullDuration
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
