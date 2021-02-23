namespace Core.Models.Settings
{
    public class AppSettings : DatabaseRecord
    {
        public SessionSettings SessionSettings { get; set; } = new SessionSettings();
    }
}
