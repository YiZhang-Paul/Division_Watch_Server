using Core.Models.TaskItem;
using System.Collections.Generic;

namespace Core.DTOs
{
    public class UpdateTasksResult
    {
        public List<TaskItem> Parents { get; set; }
        public List<TaskItem> Targets { get; set; }
    }
}
