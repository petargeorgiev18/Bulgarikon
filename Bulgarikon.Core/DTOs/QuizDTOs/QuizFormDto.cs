using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Quiz;

namespace Bulgarikon.Core.DTOs.QuizDTOs
{
    public class QuizFormDto
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = "";

        [Required]
        public Guid EraId { get; set; }
    }
}
