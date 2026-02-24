using Bulgarikon.Core.DTOs.FeedbackDTOs;

namespace Bulgarikon.Core.Interfaces
{
    public interface IFeedbackService
    {
        Task CreateAsync(Guid userId, FeedbackCreateDto dto);
        Task<IReadOnlyList<FeedbackListItemDto>> GetAllAsync();
        Task<IReadOnlyList<FeedbackListItemDto>> GetMineAsync(Guid userId);
        Task<FeedbackDetailsDto?> GetDetailsAsync(Guid id);
        Task ReplyAsync(Guid feedbackId, Guid adminUserId, string reply);
        Task<int> GetAdminNewCountAsync();
        Task<int> GetUserUnreadRepliesCountAsync(Guid userId);
        Task MarkAllSeenByAdminAsync();
        Task MarkUserRepliesSeenAsync(Guid userId);
        Task<bool> IsOwnerAsync(Guid feedbackId, Guid userId);
        Task MarkSeenByAdminAsync(Guid feedbackId);
    }
}
