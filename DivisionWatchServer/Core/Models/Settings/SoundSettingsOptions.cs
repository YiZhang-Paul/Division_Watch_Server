namespace Core.Models.Settings
{
    public class SoundSettingsOptions
    {
        public Range<int> MasterVolume { get; set; }
        public Range<int> UIVolume { get; set; }
        public Range<int> ClockVolume { get; set; }
    }
}
