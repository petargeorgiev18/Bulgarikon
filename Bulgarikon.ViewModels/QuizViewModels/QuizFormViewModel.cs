using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Quiz;

namespace Bulgarikon.ViewModels.QuizViewModels
{
    public class QuizFormViewModel
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = "";

        [Required]
        public Guid EraId { get; set; }
    }
}
