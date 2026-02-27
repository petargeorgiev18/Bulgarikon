using Bulgarikon.Core.DTOs.QuestionDTOs;

namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class QuizReviewViewDto
    {
        public string QuizTitle { get; set; } = null!;
        public int Score { get; set; }
        public int MaxScore { get; set; }

        public List<QuestionReviewDto> Questions { get; set; } = new();
    }
}
