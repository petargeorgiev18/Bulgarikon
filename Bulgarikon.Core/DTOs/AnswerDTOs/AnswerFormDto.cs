using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Answer;

namespace Bulgarikon.Core.DTOs.AnswerDTOs
{
    public class AnswerFormDto
    {
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = "";

        public bool IsCorrect { get; set; }
    }
}
