namespace Bulgarikon.ViewModels.QuizViewModels
{
    public class QuizTakeViewModel
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; } = "";
        public string EraName { get; set; } = "";
        public List<QuizTakeQuestionViewModel> Questions { get; set; } = new();
    }
}
