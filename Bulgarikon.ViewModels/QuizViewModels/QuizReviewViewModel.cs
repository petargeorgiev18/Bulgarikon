using Bulgarikon.ViewModels.QuestionViewModels;

namespace Bulgarikon.ViewModels.QuizViewModels
{
    public class QuizReviewViewViewModel
    {
        public string QuizTitle { get; set; } = null!;
        public int Score { get; set; }
        public int MaxScore { get; set; }

        public List<QuestionReviewViewModel> Questions { get; set; } = new();
    }
}
