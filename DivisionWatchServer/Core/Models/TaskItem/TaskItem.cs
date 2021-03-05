using System;
using System.Collections.Generic;

namespace Core.Models.TaskItem
{
    public class TaskItem : DatabaseRecord
    {
        public string Parent { get; set; } = null;
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public RankItem Priority { get; set; }
        public int Estimate { get; set; }
        public DateTime? Deadline { get; set; }
        public string DueTime { get; set; }
        public List<bool> Recur { get; set; } = new List<bool>();
        public List<ChecklistItem> Checklist { get; set; } = new List<ChecklistItem>();
        public List<EstimationResult> EstimationResults { get; set; } = new List<EstimationResult>();
        public bool IsInterruption { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem()
        {
            Priority = new RankItem
            {
                Rank = (int)Enums.Priority.Normal,
                Name = Enum.GetName(typeof(Enums.Priority), Enums.Priority.Normal)
            };
        }
    }
}
