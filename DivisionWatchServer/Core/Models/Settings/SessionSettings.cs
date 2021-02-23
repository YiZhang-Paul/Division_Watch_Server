namespace Core.Models.Settings
{
    public class SessionSettings
    {
        public int SessionDuration { get; set; }
        public int ShortBreakDuration { get; set; }
        public int LongBreakDuration { get; set; }
        public int DailyLimitSuggestion { get; set; }
    }
}
