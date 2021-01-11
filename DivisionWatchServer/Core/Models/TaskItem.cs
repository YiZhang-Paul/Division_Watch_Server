using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class TaskItem : DatabaseRecord
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public RankItem Priority { get; set; }
        public int Estimate { get; set; }
        public string Deadline { get; set; }
        public List<bool> Recur { get; set; } = new List<bool>();
        public List<EstimationResult> EstimationResults { get; set; } = new List<EstimationResult>();
        public string Parent { get; set; } = null;
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
