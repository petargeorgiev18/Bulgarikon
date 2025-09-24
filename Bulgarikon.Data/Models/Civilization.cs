namespace Bulgarikon.Data.Models
{
    public class Civilization
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Civilization() { }
        public ICollection<Artifact> Artifacts { get; set; } = null!;
        public ICollection<Quiz> Quizzes { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string? ImageUrl { get; set; }
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
    }
}