namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class QuizTakeDto
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; } = "";
        public string EraName { get; set; } = "";
        public List<QuizTakeQuestionDto> Questions { get; set; } = new();
    }
}
