namespace Bulgarikon.Core.DTOs.QuestionDTOs
{
    public class QuestionListItemDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = "";
        public int AnswersCount { get; set; }
    }
}
