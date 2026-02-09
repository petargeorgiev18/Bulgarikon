namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class AnswerListItemDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = "";
        public bool IsCorrect { get; set; }
    }
}