using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class FeedbackNotificationService : IFeedbackNotificationService
    {
        private readonly BulgarikonDbContext db;

        public FeedbackNotificationService(BulgarikonDbContext db)
        {
            this.db = db;
        }

        public Task<int> GetAdminNewFeedbackCountAsync()
            => db.Feedbacks.CountAsync(f => !f.IsSeenByAdmin);

        public Task<int> GetUserUnreadRepliesCountAsync(Guid userId)
            => db.Feedbacks.CountAsync(f =>
                f.UserId == userId &&
                f.AdminReply != null &&
                f.RepliedAt != null &&
                !f.IsReplySeenByUser);
    }
}
