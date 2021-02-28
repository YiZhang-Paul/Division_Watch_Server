using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class DailyPlan : DatabaseRecord
    {
        public DateTime Date { get; set; }
        public Goal Goal { get; set; } = new Goal();
        public List<string> Planned { get; set; } = new List<string>();
        public List<string> Potential { get; set; } = new List<string>();
    }
}
