using Bulgarikon.Data.Models.Enums;

namespace Bulgarikon.Core.DTOs.FeedbackDTOs
{
    public class FeedbackListItemDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = null!;
        public FeedbackCategory Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserEmail { get; set; } = null!;
        public bool IsReplied => !string.IsNullOrWhiteSpace(AdminReply);
        public string? AdminReply { get; set; }
        public DateTime? RepliedAt { get; set; }
        public string? RepliedByEmail { get; set; }
        public bool IsSeenByAdmin { get; set; }
        public bool IsReplySeenByUser { get; set; }
    }
}
