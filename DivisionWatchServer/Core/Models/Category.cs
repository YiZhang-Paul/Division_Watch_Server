namespace Core.Models
{
    public class Category : DatabaseRecord
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public bool IsEditable { get; set; } = true;
    }
}
