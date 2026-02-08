namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class QuizTakeAnswerDto
    {
        public Guid AnswerId { get; set; }
        public string Text { get; set; } = "";
    }
}