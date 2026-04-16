namespace Bulgarikon.ViewModels.QuizViewModels
{
    public class QuizResultViewViewModel
    {
        public Guid ResultId { get; set; }
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; } = "";
        public string EraName { get; set; } = "";
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public DateTime DateTaken { get; set; }
    }
}
