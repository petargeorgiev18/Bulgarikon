using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Feedback;

namespace Bulgarikon.Core.DTOs.FeedbackDTOs
{
    public class FeedbackReplyDto
    {
        public Guid FeedbackId { get; set; }

        [Required]
        [StringLength(ReplyMaxLength, MinimumLength = 2)]
        public string AdminReply { get; set; } = null!;
    }
}
