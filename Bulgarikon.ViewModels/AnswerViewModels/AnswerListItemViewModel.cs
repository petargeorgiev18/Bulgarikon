namespace Bulgarikon.ViewModels.QuizViewModels
{
    public class AnswerListItemViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = "";
        public bool IsCorrect { get; set; }
    }
}