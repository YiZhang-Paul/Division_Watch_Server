namespace Core.Models.Settings
{
    public class AppSettings : DatabaseRecord
    {
        public SoundSettings SoundSettings { get; set; } = new SoundSettings();
        public SessionSettings SessionSettings { get; set; } = new SessionSettings();
    }
}
