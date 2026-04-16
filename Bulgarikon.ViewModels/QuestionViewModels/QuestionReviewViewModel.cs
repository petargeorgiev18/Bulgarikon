namespace Bulgarikon.ViewModels.QuestionViewModels
{
    public class QuestionReviewViewModel
    {
        public string QuestionText { get; set; } = null!;

        public string? SelectedAnswer { get; set; }

        public string CorrectAnswer { get; set; } = null!;

        public bool IsCorrect { get; set; }
    }
}
