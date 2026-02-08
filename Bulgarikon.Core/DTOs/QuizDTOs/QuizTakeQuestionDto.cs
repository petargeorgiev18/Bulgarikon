namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class QuizTakeQuestionDto
    {
        public Guid QuestionId { get; set; }
        public string Text { get; set; } = "";
        public List<QuizTakeAnswerDto> Answers { get; set; } = new();
    }
}