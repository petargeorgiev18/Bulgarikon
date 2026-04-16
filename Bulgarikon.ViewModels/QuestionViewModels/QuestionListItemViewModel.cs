namespace Bulgarikon.ViewModels.QuestionViewModels
{
    public class QuestionListItemViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = "";
        public int AnswersCount { get; set; }
    }
}
