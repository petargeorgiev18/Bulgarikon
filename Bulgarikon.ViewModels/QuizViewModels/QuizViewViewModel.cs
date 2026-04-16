namespace Bulgarikon.ViewModels.QuizViewModels
{
    public class QuizViewViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public Guid EraId { get; set; }
        public string EraName { get; set; } = "";
        public int QuestionsCount { get; set; }
    }
}
