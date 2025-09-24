using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Data.Models
{
    public class Era
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public ICollection<Event> Events { get; set; }
            = new HashSet<Event>();
        public ICollection<Civilization> Civilizations { get; set; }
            = new HashSet<Civilization>();
        public ICollection<Figure> Figures { get; set; }
            = new HashSet<Figure>();
        public ICollection<Artifact> Artifacts { get; set; }
            = new HashSet<Artifact>();
        public ICollection<Quiz> Quizzes { get; set; }
            = new HashSet<Quiz>();
    }
}
