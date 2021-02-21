using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Repositories;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/v1/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategoryService CategoryService { get; set; }
        private CategoryRepository CategoryRepository { get; set; }

        public CategoryController(CategoryRepository categoryRepository, CategoryService categoryService)
        {
            CategoryRepository = categoryRepository;
            CategoryService = categoryService;
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
            try
            {
                return Ok(await CategoryService.AddCategory(category).ConfigureAwait(false));
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch
            {
                return BadRequest("Failed to create the category.");
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateCategory([FromBody]Category category)
        {
            try
            {
                return Ok(await CategoryService.UpdateCategory(category).ConfigureAwait(false));
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch
            {
                return BadRequest("Failed to update the category.");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> DeleteCategory([FromQuery]string transfer, string id)
        {
            return await CategoryService.DeleteCategory(id, transfer).ConfigureAwait(false);
        }
    }
}
