using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Feedback;

namespace Bulgarikon.ViewModels.FeedbackViewModels
{
    public class FeedbackReplyViewModel
    {
        public Guid FeedbackId { get; set; }

        [Required]
        [StringLength(ReplyMaxLength, MinimumLength = 2)]
        public string AdminReply { get; set; } = null!;
    }
}
