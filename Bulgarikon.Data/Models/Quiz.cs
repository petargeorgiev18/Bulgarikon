namespace Bulgarikon.Data.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } = new HashSet<Question>();
    }
}