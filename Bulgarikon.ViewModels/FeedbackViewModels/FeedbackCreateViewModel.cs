using System.ComponentModel.DataAnnotations;
using Bulgarikon.Data.Models.Enums;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Feedback;

namespace Bulgarikon.ViewModels.FeedbackViewModels
{
    public class FeedbackCreateViewModel
    {
        [Required]
        [StringLength(SubjectMaxLength, MinimumLength = SubjectMinLength)]
        public string Subject { get; set; } = null!;

        [Required]
        public FeedbackCategory Category { get; set; } = FeedbackCategory.Idea;

        [Required]
        [StringLength(CommentMaxLength, MinimumLength = CommentMinLength)]
        public string Comment { get; set; } = null!;
    }
}
