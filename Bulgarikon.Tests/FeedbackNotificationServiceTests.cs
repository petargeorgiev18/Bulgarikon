using System;
using System.Threading.Tasks;
using Bulgarikon.Core.Implementations;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bulgarikon.Tests.Services
{
    [TestFixture]
    public class FeedbackNotificationServiceTests
    {
        private BulgarikonDbContext db = null!;
        private FeedbackNotificationService service = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("FeedbackNotificationServiceTests_" + Guid.NewGuid())
                .Options;

            db = new BulgarikonDbContext(options);
            service = new FeedbackNotificationService(db);
        }

        [TearDown]
        public async Task TearDown()
        {
            await db.DisposeAsync();
        }

        [Test]
        public async Task GetAdminNewFeedbackCountAsync_ReturnsOnlyNotSeenByAdmin()
        {
            var userId = Guid.NewGuid();

            db.Feedbacks.AddRange(
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Subject = "S1",
                    Category = Bulgarikon.Data.Models.Enums.FeedbackCategory.Bug,
                    Comment = "C1",
                    CreatedAt = DateTime.UtcNow,
                    IsSeenByAdmin = false,
                    IsReplySeenByUser = true
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Subject = "S2",
                    Category = Bulgarikon.Data.Models.Enums.FeedbackCategory.Idea,
                    Comment = "C2",
                    CreatedAt = DateTime.UtcNow,
                    IsSeenByAdmin = true,
                    IsReplySeenByUser = true
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Subject = "S3",
                    Category = Bulgarikon.Data.Models.Enums.FeedbackCategory.Question,
                    Comment = "C3",
                    CreatedAt = DateTime.UtcNow,
                    IsSeenByAdmin = false,
                    IsReplySeenByUser = true
                }
            );

            await db.SaveChangesAsync();

            var count = await service.GetAdminNewFeedbackCountAsync();
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetUserUnreadRepliesCountAsync_FiltersByUser_AndUnreadReplyRules()
        {
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            db.Feedbacks.AddRange(
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Subject = "Unread1",
                    Category = Bulgarikon.Data.Models.Enums.FeedbackCategory.Other,
                    Comment = "C",
                    CreatedAt = DateTime.UtcNow,
                    AdminReply = "Reply",
                    RepliedAt = DateTime.UtcNow,
                    IsReplySeenByUser = false,
                    IsSeenByAdmin = true
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Subject = "SeenReply",
                    Category = Bulgarikon.Data.Models.Enums.FeedbackCategory.Other,
                    Comment = "C",
                    CreatedAt = DateTime.UtcNow,
                    AdminReply = "Reply",
                    RepliedAt = DateTime.UtcNow,
                    IsReplySeenByUser = true,
                    IsSeenByAdmin = true
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Subject = "NoReplyText",
                    Category = Bulgarikon.Data.Models.Enums.FeedbackCategory.Other,
                    Comment = "C",
                    CreatedAt = DateTime.UtcNow,
                    AdminReply = null,
                    RepliedAt = DateTime.UtcNow,
                    IsReplySeenByUser = false,
                    IsSeenByAdmin = true
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Subject = "NoRepliedAt",
                    Category = Bulgarikon.Data.Models.Enums.FeedbackCategory.Other,
                    Comment = "C",
                    CreatedAt = DateTime.UtcNow,
                    AdminReply = "Reply",
                    RepliedAt = null,
                    IsReplySeenByUser = false,
                    IsSeenByAdmin = true
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    UserId = otherUserId,
                    Subject = "OtherUserUnread",
                    Category = Bulgarikon.Data.Models.Enums.FeedbackCategory.Other,
                    Comment = "C",
                    CreatedAt = DateTime.UtcNow,
                    AdminReply = "Reply",
                    RepliedAt = DateTime.UtcNow,
                    IsReplySeenByUser = false,
                    IsSeenByAdmin = true
                }
            );

            await db.SaveChangesAsync();

            var count = await service.GetUserUnreadRepliesCountAsync(userId);
            Assert.That(count, Is.EqualTo(1));
        }
    }
}