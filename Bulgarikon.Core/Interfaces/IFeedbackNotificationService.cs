namespace Bulgarikon.Core.Interfaces
{
    public interface IFeedbackNotificationService
    {
        Task<int> GetAdminNewFeedbackCountAsync();
        Task<int> GetUserUnreadRepliesCountAsync(Guid userId);
    }
}
