namespace Core.Models.Settings
{
    public class SoundSettings
    {
        public bool IsMuted { get; set; }
        public int MasterVolume { get; set; } = 50;
        public int UIVolume { get; set; } = 100;
        public int ClockVolume { get; set; } = 100;
    }
}
