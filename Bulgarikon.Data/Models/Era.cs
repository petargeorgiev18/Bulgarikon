namespace Bulgarikon.Data.Models
{
    public class Era
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Event> Events { get; set; } = null!;
        public ICollection<Figure> Figures { get; set; } = null!;
        public ICollection<Artifact> Artifacts { get; set; } = null!;
        public ICollection<Quiz> Quizzes { get; set; } = null!;
    }
}
