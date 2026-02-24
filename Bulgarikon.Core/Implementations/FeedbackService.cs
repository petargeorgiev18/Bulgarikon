using Bulgarikon.Core.DTOs.FeedbackDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly BulgarikonDbContext context;

        public FeedbackService(BulgarikonDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Guid userId, FeedbackCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var subject = (dto.Subject ?? string.Empty).Trim();
            var comment = (dto.Comment ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("Темата е задължителна.", nameof(dto.Subject));

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Описанието е задължително.", nameof(dto.Comment));

            if (!Enum.IsDefined(typeof(FeedbackCategory), dto.Category))
                dto.Category = FeedbackCategory.Other;

            var feedback = new Feedback
            {
                UserId = userId,
                Subject = subject,
                Category = dto.Category,
                Comment = comment,
                CreatedAt = DateTime.UtcNow,
                IsSeenByAdmin = false,
                IsReplySeenByUser = true
            };

            context.Feedbacks.Add(feedback);
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<FeedbackListItemDto>> GetAllAsync()
        {
            return await context.Feedbacks
                .AsNoTracking()
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new FeedbackListItemDto
                {
                    Id = f.Id,
                    Subject = f.Subject,
                    Category = f.Category,
                    CreatedAt = f.CreatedAt,

                    UserEmail = f.User.Email ?? "(no email)",

                    AdminReply = f.AdminReply,
                    RepliedAt = f.RepliedAt,
                    RepliedByEmail = f.RepliedByUser != null
                        ? (f.RepliedByUser.Email ?? "(no email)")
                        : null,

                    IsSeenByAdmin = f.IsSeenByAdmin,
                    IsReplySeenByUser = f.IsReplySeenByUser
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<FeedbackListItemDto>> GetMineAsync(Guid userId)
        {
            return await context.Feedbacks
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new FeedbackListItemDto
                {
                    Id = f.Id,
                    Subject = f.Subject,
                    Category = f.Category,
                    CreatedAt = f.CreatedAt,
                    UserEmail = string.Empty,
                    AdminReply = f.AdminReply,
                    RepliedAt = f.RepliedAt,
                    RepliedByEmail = f.RepliedByUser != null
                        ? (f.RepliedByUser.Email ?? "(no email)")
                        : null,

                    IsSeenByAdmin = f.IsSeenByAdmin,
                    IsReplySeenByUser = f.IsReplySeenByUser
                })
                .ToListAsync();
        }

        public async Task<FeedbackDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await context.Feedbacks
                .AsNoTracking()
                .Where(f => f.Id == id)
                .Select(f => new FeedbackDetailsDto
                {
                    Id = f.Id,
                    Subject = f.Subject,
                    Category = f.Category,
                    Comment = f.Comment,
                    CreatedAt = f.CreatedAt,

                    UserEmail = f.User.Email ?? "(no email)",

                    AdminReply = f.AdminReply,
                    RepliedAt = f.RepliedAt,
                    RepliedByEmail = f.RepliedByUser != null
                        ? (f.RepliedByUser.Email ?? "(no email)")
                        : null,

                    IsSeenByAdmin = f.IsSeenByAdmin,
                    IsReplySeenByUser = f.IsReplySeenByUser
                })
                .FirstOrDefaultAsync();
        }

        public async Task ReplyAsync(Guid feedbackId, Guid adminUserId, string reply)
        {
            var text = (reply ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Отговорът е задължителен.", nameof(reply));

            var feedback = await context.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId);
            if (feedback == null)
                throw new InvalidOperationException("Feedback не е намерен.");

            feedback.AdminReply = text;
            feedback.RepliedAt = DateTime.UtcNow;
            feedback.RepliedByUserId = adminUserId;
            feedback.IsReplySeenByUser = false;
            feedback.IsSeenByAdmin = true;

            await context.SaveChangesAsync();
        }

        public Task<int> GetAdminNewCountAsync()
            => context.Feedbacks.CountAsync(f => !f.IsSeenByAdmin);

        public Task<int> GetUserUnreadRepliesCountAsync(Guid userId)
            => context.Feedbacks.CountAsync(f =>
                f.UserId == userId &&
                f.AdminReply != null &&
                f.RepliedAt != null &&
                !f.IsReplySeenByUser);

        public async Task MarkAllSeenByAdminAsync()
        {
            await context.Feedbacks
                .Where(f => !f.IsSeenByAdmin)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsSeenByAdmin, true));
        }

        public async Task MarkUserRepliesSeenAsync(Guid userId)
        {
            await context.Feedbacks
                .Where(f =>
                    f.UserId == userId &&
                    f.AdminReply != null &&
                    f.RepliedAt != null &&
                    !f.IsReplySeenByUser)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsReplySeenByUser, true));
        }

        public Task<bool> IsOwnerAsync(Guid feedbackId, Guid userId)
            => context.Feedbacks.AnyAsync(f => f.Id == feedbackId && f.UserId == userId);

        public async Task MarkSeenByAdminAsync(Guid feedbackId)
        {
            await context.Feedbacks
                .Where(f => f.Id == feedbackId && !f.IsSeenByAdmin)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsSeenByAdmin, true));
        }
    }
}