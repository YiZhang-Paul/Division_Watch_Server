using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/v1/task-item")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private TaskItemService TaskItemService { get; set; }

        public TaskItemController(TaskItemService taskItemService)
        {
            TaskItemService = taskItemService;
        }

        [HttpGet]
        [Route("incomplete")]
        public async Task<IEnumerable<TaskItem>> GetIncompleteTaskItems([FromQuery]int limit = 0)
        {
            var items = await TaskItemService.GetIncompleteTaskItems(limit).ConfigureAwait(false);

            return items.OrderByDescending(_ => _.Priority.Rank);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<TaskItem> GetTaskItem(string id)
        {
            return await TaskItemService.GetTaskItem(id).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddTaskItem([FromBody]TaskItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                return BadRequest("Must provide a valid name.");
            }

            return Ok(await TaskItemService.AddTaskItem(item).ConfigureAwait(false));
        }

        [HttpPut]
        [Route("")]
        public async Task<bool> UpdateTaskItem([FromBody]TaskItem item)
        {
            return await TaskItemService.UpdateTaskItem(item).ConfigureAwait(false);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> DeleteTaskItem(string id)
        {
            return await TaskItemService.DeleteTaskItem(id).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("options")]
        public async Task<TaskOptions> GetTaskOptions([FromBody]TaskOptionsQuery query)
        {
            return await TaskItemService.GetTaskOptions(query).ConfigureAwait(false);
        }
    }
}
