using Core.Models;
using System.Collections.Generic;

namespace Core.DTOs
{
    public class DeleteTaskResult
    {
        public TaskItem Parent { get; set; }
        public List<TaskItem> UpdatedChildren { get; set; } = new List<TaskItem>();
        public List<TaskItem> DeletedChildren { get; set; } = new List<TaskItem>();
    }
}
