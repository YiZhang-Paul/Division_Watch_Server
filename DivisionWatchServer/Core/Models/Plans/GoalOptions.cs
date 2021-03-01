namespace Core.Models.Plans
{
    public class GoalOptions
    {
        public Range<int> Sessions { get; set; } = new Range<int>();
        public int SessionDuration { get; set; }
    }
}
