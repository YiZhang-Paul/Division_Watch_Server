using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System;
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

            return items.OrderByDescending(_ => _.Priority.Rank).OrderBy(_ => _.Estimate);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<TaskItem> GetTaskItem(string id)
        {
            return await TaskItemService.GetTaskItem(id).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("empty")]
        public async Task<TaskItem> GetEmptyTaskItem([FromQuery]bool isInterruption = false)
        {
            return await TaskItemService.GetEmptyTaskItem(isInterruption).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddTaskItem([FromBody]TaskItem item)
        {
            try
            {
                return Ok(await TaskItemService.AddTaskItem(item).ConfigureAwait(false));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Route("{id}/children")]
        public async Task<IActionResult> AddChildTaskItem([FromBody]TaskItem item, string id)
        {
            try
            {
                return Ok(await TaskItemService.AddChildTaskItem(id, item).ConfigureAwait(false));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut]
        [Route("convert")]
        public async Task<TaskItem> ConvertToTaskItem([FromBody]TaskItem interruption)
        {
            return await TaskItemService.ConvertToTaskItem(interruption).ConfigureAwait(false);
        }

        [HttpPut]
        [Route("")]
        public async Task<UpdateTaskResult> UpdateTaskItem([FromBody]TaskItem item)
        {
            return await TaskItemService.UpdateTaskItem(item).ConfigureAwait(false);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<DeleteTaskResult> DeleteTaskItem([FromQuery]bool keepChildren, string id)
        {
            return await TaskItemService.DeleteTaskItem(id, keepChildren).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("options")]
        public TaskOptions GetTaskOptions([FromBody]string currentDate)
        {
            return TaskItemService.GetTaskOptions(currentDate);
        }
    }
}
