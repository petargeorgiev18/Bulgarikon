using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class QuizSubmitDto
    {
        [Required]
        public Guid QuizId { get; set; }

        public Dictionary<Guid, Guid> Answers { get; set; } = new();
    }
}
