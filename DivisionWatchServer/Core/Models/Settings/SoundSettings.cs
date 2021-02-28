namespace Core.Models.Settings
{
    public class SoundSettings
    {
        public bool IsMuted { get; set; } = true;
        public int MasterVolume { get; set; } = 50;
        public int UiVolume { get; set; } = 100;
        public int ClockVolume { get; set; } = 100;
        public string ClockSound { get; set; } = string.Empty;
    }
}
