namespace Bulgarikon.ViewModels.QuizViewModels
{
    public class QuizTakeQuestionViewModel
    {
        public Guid QuestionId { get; set; }
        public string Text { get; set; } = "";
        public List<QuizTakeAnswerViewModel> Answers { get; set; } = new();
    }
}