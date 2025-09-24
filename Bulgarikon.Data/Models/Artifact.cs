namespace Bulgarikon.Data.Models
{
    public class Artifact
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Material { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        public ICollection<Civilization> Civilizations { get; set; } = null!;
        public ICollection<Quiz> Quizzes { get; set; } = null!;
    }
}