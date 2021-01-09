using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<TaskItem> Subtasks { get; set; } = new List<TaskItem>();
        public bool IsInterruption { get; set; }

        public bool IsCompleted
        {
            get => Subtasks.Any() ? Subtasks.All(_ => _.IsCompleted) : _isCompleted;
            set => _isCompleted = value;
        }

        private bool _isCompleted = false;

        public TaskItem()
        {
            Priority = new RankItem
            {
                Rank = (int)Enums.Priority.Low,
                Name = Enum.GetName(typeof(Enums.Priority), Enums.Priority.Low)
            };
        }
    }
}
