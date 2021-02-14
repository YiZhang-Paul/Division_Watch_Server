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

        public async Task<TaskItem> GetEmptyTaskItem(bool isInterruption)
        {
            var categories = await CategoryRepository.Get().ConfigureAwait(false);

            return new TaskItem
            {
                CategoryId = categories.FirstOrDefault(_ => !_.IsEditable && _.Name == "Default")?.Id,
                Estimate = SkullDuration,
                IsInterruption = isInterruption
            };
        }

        public async Task<TaskItem> AddTaskItem(TaskItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                throw new ArgumentException("Must provide a valid name.");
            }

            try
            {
                await TaskItemRepository.Add(item).ConfigureAwait(false);

                return item;
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
                item.Parent = parent.Id;
                item.CategoryId = string.Empty;
                item.Deadline ??= parent.Deadline;
                item.Estimate = SkullDuration;
                item.Recur = parent.Recur;

                item.Priority ??= new RankItem
                {
                    Rank = (int)Priority.Normal,
                    Name = Enum.GetName(typeof(Priority), Priority.Normal)
                };

                await TaskItemRepository.Add(item).ConfigureAwait(false);
                await UpdateTotalEstimation(parent).ConfigureAwait(false);

                return new AddChildResult { Parent = parent, Child = item };
            }
            catch
            {
                return null;
            }
        }

        public async Task<TaskItem> ConvertToTaskItem(TaskItem interruption)
        {
            interruption.IsInterruption = false;

            return (await UpdateTaskItem(interruption).ConfigureAwait(false))?.Target;
        }

        public async Task<UpdateTaskResult> UpdateTaskItem(TaskItem item)
        {
            try
            {
                var result = new UpdateTaskResult { Target = item };
                await TaskItemRepository.Replace(item).ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(item.Parent))
                {
                    result.Parent = await TaskItemRepository.Get(item.Parent).ConfigureAwait(false);
                    await UpdateTotalEstimation(result.Parent).ConfigureAwait(false);
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DeleteTaskResult> DeleteTaskItem(string id, bool keepChildren)
        {
            try
            {
                var result = new DeleteTaskResult();
                var current = await TaskItemRepository.Get(id).ConfigureAwait(false);

                if (current == null)
                {
                    return null;
                }

                await TaskItemRepository.Delete(id).ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(current.Parent))
                {
                    result.Parent = await TaskItemRepository.Get(current.Parent).ConfigureAwait(false);
                    await UpdateTotalEstimation(result.Parent).ConfigureAwait(false);
                }
                else if (!current.IsInterruption)
                {
                    await ProcessChildTasks(id, keepChildren, result).ConfigureAwait(false);
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        public TaskOptions GetTaskOptions(string currentDate)
        {
            var startDate = DateTime.Parse(currentDate);
            var deadlines = Enumerable.Range(0, 14).Select(_ => startDate.AddDays(_).ToShortDateString());
            var maxSkulls = Math.Max(1, 1000 * 60 * 60 * 3 / SkullDuration);
            var estimates = Enumerable.Range(1, maxSkulls).Select(_ => SkullDuration * _);

            return new TaskOptions
            {
                Priorities = ToRankItem(typeof(Priority)).ToList(),
                Deadlines = new List<string> { string.Empty }.Concat(deadlines).ToList(),
                Estimates = new List<int> { 600000 }.Concat(estimates).ToList(),
                SkullDuration = SkullDuration
            };
        }

        private async Task ProcessChildTasks(string id, bool keepChildren, DeleteTaskResult result)
        {
            var tasks = new List<Task>();

            foreach (var child in await TaskItemRepository.GetChildTaskItems(id).ConfigureAwait(false))
            {
                if (keepChildren)
                {
                    child.Parent = null;
                    result.UpdatedChildren.Add(child);
                    tasks.Add(TaskItemRepository.Replace(child));
                }
                else
                {
                    result.DeletedChildren.Add(child);
                    tasks.Add(TaskItemRepository.Delete(child.Id));
                }
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private IEnumerable<RankItem> ToRankItem(Type type)
        {
            return Enum.GetNames(type).Select(_ => new RankItem
            {
                Rank = (int)Enum.Parse(type, _),
                Name = _
            });
        }

        private async Task UpdateTotalEstimation(TaskItem parent, int minimum = SkullDuration)
        {
            var children = await TaskItemRepository.GetChildTaskItems(parent.Id).ConfigureAwait(false);
            var total = children.Sum(_ => _.Estimate);
            parent.Estimate = total == 0 ? minimum : total;
            await TaskItemRepository.Replace(parent).ConfigureAwait(false);
        }
    }
}
