namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class QuizDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public Guid EraId { get; set; }
        public string EraName { get; set; } = "";
        public int QuestionsCount { get; set; }
    }
}
