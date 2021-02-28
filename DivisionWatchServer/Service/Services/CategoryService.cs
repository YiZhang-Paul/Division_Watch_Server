using Core.Models;
using Service.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CategoryService
    {
        private CategoryRepository CategoryRepository { get; set; }
        private TaskItemRepository TaskItemRepository { get; set; }

        public CategoryService(CategoryRepository categoryRepository, TaskItemRepository taskItemRepository)
        {
            CategoryRepository = categoryRepository;
            TaskItemRepository = taskItemRepository;
        }

        public async Task<Category> AddCategory(Category category)
        {
            await ValidateCategory(category).ConfigureAwait(false);
            await CategoryRepository.Add(category).ConfigureAwait(false);

            return category;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            await ValidateCategory(category).ConfigureAwait(false);
            await CategoryRepository.Replace(category).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> DeleteCategory(string id, string transfer)
        {
            try
            {
                await CategoryRepository.Delete(id).ConfigureAwait(false);
                var items = await TaskItemRepository.GetTaskItemsByCategory(id).ConfigureAwait(false);

                var updated = items.Select(_ =>
                {
                    _.CategoryId = transfer;

                    return _;
                });

                await TaskItemRepository.ReplaceMany(updated).ConfigureAwait(false);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task ValidateCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Must provide a valid name.");
            }

            var existing = await CategoryRepository.GetCategoryByName(category.Name).ConfigureAwait(false);

            if (existing != null && existing.Id != category.Id)
            {
                throw new ArgumentException("Must provide a unique name.");
            }
        }
    }
}
