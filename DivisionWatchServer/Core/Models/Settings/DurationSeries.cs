namespace Core.Models.Settings
{
    public class DurationSeries
    {
        public int SessionDuration { get; set; }
        public int ShortBreakDuration { get; set; }
        public Range<int> LongBreakRange { get; set; }
    }
}
