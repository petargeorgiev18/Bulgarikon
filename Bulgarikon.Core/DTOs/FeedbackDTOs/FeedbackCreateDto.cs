using System.ComponentModel.DataAnnotations;
using Bulgarikon.Data.Models.Enums;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Feedback;

namespace Bulgarikon.Core.DTOs.FeedbackDTOs
{
    public class FeedbackCreateDto
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
