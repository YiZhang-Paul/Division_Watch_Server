using System.Collections.Generic;

namespace Core.Models
{
    public class TaskOptions
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<RankItem> Priorities { get; set; } = new List<RankItem>();
        public List<string> Deadlines { get; set; } = new List<string>();
        public List<int> Estimates { get; set; } = new List<int>();
    }
}
