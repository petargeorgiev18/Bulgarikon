namespace Bulgarikon.Data.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public ICollection<Answer> Answers { get; set; }
            = new HashSet<Answer>();
    }
}