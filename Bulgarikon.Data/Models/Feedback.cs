using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bulgarikon.Data.Models.Enums;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Feedback;

namespace Bulgarikon.Data.Models
{
    public class Feedback
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public BulgarikonUser User { get; set; } = null!;
        [Required]
        [MaxLength(SubjectMaxLength)]
        public string Subject { get; set; } = null!;
        [Required]
        [MaxLength(CategoryMaxLength)]
        public FeedbackCategory Category { get; set; }
        [Required]
        [MaxLength(CommentMaxLength)]
        public string Comment { get; set; } = null!;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [MaxLength(ReplyMaxLength)]
        public string? AdminReply { get; set; }
        public DateTime? RepliedAt { get; set; }
        [ForeignKey(nameof(RepliedByUser))]
        public Guid? RepliedByUserId { get; set; }
        public BulgarikonUser? RepliedByUser { get; set; }
        public bool IsSeenByAdmin { get; set; } = false;
        public bool IsReplySeenByUser { get; set; } = true;
    }
}
