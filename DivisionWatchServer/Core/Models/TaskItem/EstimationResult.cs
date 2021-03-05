using System;

namespace Core.Models.TaskItem
{
    public class EstimationResult
    {
        public int Estimated { get; set; }
        public int Actual { get; set; }
        public DateTime EstimationTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}
