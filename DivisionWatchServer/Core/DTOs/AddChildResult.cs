using Core.Models;

namespace Core.DTOs
{
    public class AddChildResult
    {
        public TaskItem Parent { get; set; }
        public TaskItem Child { get; set; }
    }
}
