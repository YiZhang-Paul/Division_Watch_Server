using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/v1/task-item")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private TaskItemService TaskItemService { get; set; }

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<TaskItem>> GetTaskItems([FromQuery]int limit = 0)
        {
            return await TaskItemService.GetTaskItems(limit).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<TaskItem> GetTaskItem(string id)
        {
            return await TaskItemService.GetTaskItem(id).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("")]
        public async Task<string> AddTaskItem([FromBody]TaskItem item)
        {
            return await TaskItemService.AddTaskItem(item).ConfigureAwait(false);
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
    }
}
