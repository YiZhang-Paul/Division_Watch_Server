using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/v1/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategoryRepository CategoryRepository { get; set; }

        public CategoryController(CategoryRepository categoryRepository)
        {
            CategoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await CategoryRepository.Get().ConfigureAwait(false);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddCategory([FromBody]Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return BadRequest("Must provide a valid name.");
            }

            try
            {
                await CategoryRepository.Add(category).ConfigureAwait(false);

                return Ok(category.Id);
            }
            catch
            {
                return BadRequest("Failed to create the category.");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> DeleteCategory(string id)
        {
            try
            {
                await CategoryRepository.Delete(id).ConfigureAwait(false);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
