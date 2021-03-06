using System;
using System.Collections.Generic;

namespace Core.Models.Plans
{
    public class DailyPlan : DatabaseRecord
    {
        public DateTime Date { get; set; }
        public Goal Goal { get; set; } = new Goal();
        public List<string> Planned { get; set; } = new List<string>();
        public List<string> Potential { get; set; } = new List<string>();

        public DailyPlan() { }

        public DailyPlan(DateTime date)
        {
            Date = new DateTime(date.Year, date.Month, date.Day);
        }
    }
}
