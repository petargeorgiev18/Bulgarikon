using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.ViewModels.QuizViewModels
{
    public class QuizSubmitViewModel
    {
        [Required]
        public Guid QuizId { get; set; }

        public Dictionary<Guid, Guid> Answers { get; set; } = new();
    }
}
