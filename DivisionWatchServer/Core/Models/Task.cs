using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class Task
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public Priority Priority { get; set; } = Priority.Low;
        public int Estimate { get; set; }
        public DateTime Deadline { get; set; }
        public List<int> Recur { get; set; } = new List<int>();
        public List<EstimationResult> EstimationResults { get; set; } = new List<EstimationResult>();
        public List<Task> Subtask { get; set; } = new List<Task>();
        public bool IsInterruption { get; set; }

        public bool IsCompleted
        {
            get => Subtask.Any() ? Subtask.All(_ => _.IsCompleted) : _isCompleted;
            set => _isCompleted = value;
        }

        private bool _isCompleted = false;

        public Task(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Must provide a valid name.");
            }

            Name = name;
        }
    }
}
