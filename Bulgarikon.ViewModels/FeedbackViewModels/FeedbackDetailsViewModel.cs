using Bulgarikon.Data.Models.Enums;

namespace Bulgarikon.ViewModels.FeedbackViewModels
{
    public class FeedbackDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = null!;
        public FeedbackCategory Category { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string UserEmail { get; set; } = null!;
        public string? AdminReply { get; set; }
        public DateTime? RepliedAt { get; set; }
        public string? RepliedByEmail { get; set; }
        public bool IsSeenByAdmin { get; set; }
        public bool IsReplySeenByUser { get; set; }
    }
}
