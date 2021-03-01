using Core.Models.TaskItem;

namespace Core.DTOs
{
    public class UpdateTaskResult
    {
        public TaskItem Parent { get; set; }
        public TaskItem Target { get; set; }
    }
}
