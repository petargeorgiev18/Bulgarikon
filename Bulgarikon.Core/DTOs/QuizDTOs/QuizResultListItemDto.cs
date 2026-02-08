namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class QuizResultListItemDto
    {
        public Guid Id { get; set; }
        public string QuizTitle { get; set; } = "";
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public DateTime DateTaken { get; set; }
    }
}
