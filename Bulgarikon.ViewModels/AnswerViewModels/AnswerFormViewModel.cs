using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Answer;

namespace Bulgarikon.ViewModels.AnswerViewModels
{
    public class AnswerFormViewModel
    {
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = "";

        public bool IsCorrect { get; set; }
    }
}
