namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class QuizViewDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public Guid EraId { get; set; }
        public string EraName { get; set; } = "";
        public int QuestionsCount { get; set; }
    }
}
