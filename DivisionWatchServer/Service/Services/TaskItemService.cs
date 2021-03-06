using Core.DTOs;
using Core.Enums;
using Core.Models.TaskItem;
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
        private AppSettingsService AppSettingsService { get; set; }

        public TaskItemService
        (
            CategoryRepository categoryRepository,
            TaskItemRepository taskItemRepository,
            AppSettingsService appSettingsService
        )
        {
            CategoryRepository = categoryRepository;
            TaskItemRepository = taskItemRepository;
            AppSettingsService = appSettingsService;
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
            var settings = await AppSettingsService.GetSessionSettings().ConfigureAwait(false);
            var categories = await CategoryRepository.Get().ConfigureAwait(false);

            return new TaskItem
            {
                CategoryId = categories.FirstOrDefault(_ => !_.IsEditable && _.Name == "Default")?.Id,
                Estimate = settings.SessionDuration,
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
                var settings = await AppSettingsService.GetSessionSettings().ConfigureAwait(false);
                item.Parent = parent.Id;
                item.CategoryId = null;
                item.Deadline ??= parent.Deadline;
                item.Estimate = settings.SessionDuration;
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

        public async Task<TaskItem> ConvertChildToParent(TaskItem child)
        {
            var parent = await TaskItemRepository.Get(child.Parent).ConfigureAwait(false);

            if (parent == null)
            {
                return null;
            }

            child.Parent = null;
            child.CategoryId = parent.CategoryId;
            await TaskItemRepository.Replace(child).ConfigureAwait(false);

            return child;
        }

        public async Task<TaskItem> ConvertInterruptionToTaskItem(TaskItem interruption)
        {
            interruption.IsInterruption = false;
            await TaskItemRepository.Replace(interruption).ConfigureAwait(false);

            return interruption;
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

        public async Task<UpdateTasksResult> UpdateTaskItems(List<TaskItem> items)
        {
            try
            {
                var result = new UpdateTasksResult { Targets = items };
                var parentIds = items.Select(_ => _.Parent).Where(_ => !string.IsNullOrWhiteSpace(_)).Distinct();
                await TaskItemRepository.ReplaceMany(items).ConfigureAwait(false);

                var tasks = parentIds.Select(async _ =>
                {
                    var parent = await TaskItemRepository.Get(_).ConfigureAwait(false);
                    await UpdateTotalEstimation(parent).ConfigureAwait(false);

                    return parent;
                });

                result.Parents = (await Task.WhenAll(tasks).ConfigureAwait(false)).ToList();

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
                    await ProcessChildTasks(current, keepChildren, result).ConfigureAwait(false);
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TaskOptions> GetTaskOptions()
        {
            var settings = await AppSettingsService.GetSessionSettings().ConfigureAwait(false);
            var maxSkulls = Math.Max(1, 1000 * 60 * 60 * 3 / settings.SessionDuration);
            var estimates = Enumerable.Range(1, maxSkulls).Select(_ => settings.SessionDuration * _);

            return new TaskOptions
            {
                Priorities = ToRankItem(typeof(Priority)).ToList(),
                Estimates = new List<int> { 900000 }.Concat(estimates).ToList(),
                SkullDuration = settings.SessionDuration
            };
        }

        private async Task ProcessChildTasks(TaskItem parent, bool keepChildren, DeleteTaskResult result)
        {
            var tasks = await TaskItemRepository.GetChildTaskItems(parent.Id).ConfigureAwait(false);

            if (!keepChildren)
            {
                result.DeletedChildren = tasks.ToList();
                await TaskItemRepository.DeleteMany(result.DeletedChildren).ConfigureAwait(false);

                return;
            }

            foreach (var task in tasks)
            {
                task.Parent = null;
                task.CategoryId = parent.CategoryId;
                result.UpdatedChildren.Add(task);
            }

            await TaskItemRepository.ReplaceMany(result.UpdatedChildren).ConfigureAwait(false);
        }

        private IEnumerable<RankItem> ToRankItem(Type type)
        {
            return Enum.GetNames(type).Select(_ => new RankItem
            {
                Rank = (int)Enum.Parse(type, _),
                Name = _
            });
        }

        private async Task UpdateTotalEstimation(TaskItem parent)
        {
            var settings = await AppSettingsService.GetSessionSettings().ConfigureAwait(false);
            var children = await TaskItemRepository.GetChildTaskItems(parent.Id).ConfigureAwait(false);
            var total = children.Sum(_ => _.Estimate);
            parent.Estimate = total == 0 ? settings.SessionDuration : total;
            await TaskItemRepository.Replace(parent).ConfigureAwait(false);
        }
    }
}
