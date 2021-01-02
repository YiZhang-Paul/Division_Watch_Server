using System.Collections.Generic;
using System;

namespace Core.Models
{
    public class Task
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public string Priority { get; set; }
        public int Estimate { get; set; }
        public DateTime Deadline { get; set; }
        public List<Task> Subtask { get; set; }
        public bool IsInterruption { get; set; }
        public bool IsCompleted { get; set; }
    }
}
