using System.Collections.Generic;

namespace Core.Models
{
    public class TaskOptions
    {
        public List<RankItem> Priorities { get; set; } = new List<RankItem>();
        public List<int> Estimates { get; set; } = new List<int>();
        public int SkullDuration { get; set; } = 1500000;
    }
}
