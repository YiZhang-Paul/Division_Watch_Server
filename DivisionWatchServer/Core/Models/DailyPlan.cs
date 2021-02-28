using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class DailyPlan : DatabaseRecord
    {
        public DateTime Date { get; set; }
        public int Goal { get; set; } = 1;
        public List<string> Planned { get; set; } = new List<string>();
        public List<string> Potential { get; set; } = new List<string>();
    }
}
