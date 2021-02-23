using System.Collections.Generic;

namespace Core.Models.Settings
{
    public class SessionSettingsOptions
    {
        public List<DurationSeries> DurationSeries { get; set; } = new List<DurationSeries>();
        public List<int> DailyLimits { get; set; } = new List<int>();
    }
}
