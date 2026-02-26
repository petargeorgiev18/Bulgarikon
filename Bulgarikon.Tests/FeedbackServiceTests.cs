using System;
using System.Linq;
using System.Threading.Tasks;
using Bulgarikon.Core.DTOs.FeedbackDTOs;
using Bulgarikon.Core.Implementations;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bulgarikon.Tests.Services
{
    [TestFixture]
    public class FeedbackServiceTests
    {
        private BulgarikonDbContext db = null!;
        private FeedbackService service = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("FeedbackServiceTests_" + Guid.NewGuid())
                .Options;

            db = new BulgarikonDbContext(options);
            service = new FeedbackService(db);
        }

        [TearDown]
        public async Task TearDown()
        {
            await db.DisposeAsync();
        }

        private static BulgarikonUser NewUser(Guid id, string? email = null)
            => new BulgarikonUser { Id = id, UserName = email ?? $"u{id}@x.com", Email = email };

        private async Task SeedFeedbackAsync(
            Guid feedbackId,
            Guid userId,
            string subject,
            FeedbackCategory category,
            string comment,
            DateTime createdAtUtc,
            bool isSeenByAdmin,
            bool isReplySeenByUser,
            string? adminReply = null,
            DateTime? repliedAtUtc = null,
            Guid? repliedByUserId = null)
        {
            var f = new Feedback
            {
                Id = feedbackId,
                UserId = userId,
                Subject = subject,
                Category = category,
                Comment = comment,
                CreatedAt = createdAtUtc,
                IsSeenByAdmin = isSeenByAdmin,
                IsReplySeenByUser = isReplySeenByUser,
                AdminReply = adminReply,
                RepliedAt = repliedAtUtc,
                RepliedByUserId = repliedByUserId
            };

            db.Feedbacks.Add(f);
            await db.SaveChangesAsync();
        }

        private static async Task<(BulgarikonDbContext Db, FeedbackService Service, SqliteConnection Conn)> CreateSqliteDbAsync()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            await conn.OpenAsync();

            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseSqlite(conn)
                .Options;

            var sqliteDb = new BulgarikonDbContext(options);
            await sqliteDb.Database.EnsureCreatedAsync();

            var sqliteService = new FeedbackService(sqliteDb);

            return (sqliteDb, sqliteService, conn);
        }

        [Test]
        public void CreateAsync_Throws_WhenDtoIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateAsync(Guid.NewGuid(), null!));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void CreateAsync_Throws_WhenSubjectMissing(string? subject)
        {
            var dto = new FeedbackCreateDto
            {
                Subject = subject,
                Comment = "ok",
                Category = FeedbackCategory.Bug
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(Guid.NewGuid(), dto));
            Assert.That(ex!.ParamName, Is.EqualTo("Subject"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void CreateAsync_Throws_WhenCommentMissing(string? comment)
        {
            var dto = new FeedbackCreateDto
            {
                Subject = "ok",
                Comment = comment,
                Category = FeedbackCategory.Bug
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(Guid.NewGuid(), dto));
            Assert.That(ex!.ParamName, Is.EqualTo("Comment"));
        }

        [Test]
        public async Task CreateAsync_TrimsFields_SetsDefaults_AndFixesInvalidEnum()
        {
            var userId = Guid.NewGuid();

            var dto = new FeedbackCreateDto
            {
                Subject = "  Subj  ",
                Comment = "  Comm  ",
                Category = (FeedbackCategory)999
            };

            await service.CreateAsync(userId, dto);

            var saved = await db.Feedbacks.AsNoTracking().SingleAsync();
            Assert.That(saved.UserId, Is.EqualTo(userId));
            Assert.That(saved.Subject, Is.EqualTo("Subj"));
            Assert.That(saved.Comment, Is.EqualTo("Comm"));
            Assert.That(saved.Category, Is.EqualTo(FeedbackCategory.Other));
            Assert.That(saved.IsSeenByAdmin, Is.False);
            Assert.That(saved.IsReplySeenByUser, Is.True);
            Assert.That(saved.CreatedAt, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        public async Task GetAllAsync_OrdersByCreatedAtDesc_MapsEmails_AndReplyEmails()
        {
            var u1 = NewUser(Guid.NewGuid(), "user1@x.com");
            var admin = NewUser(Guid.NewGuid(), "admin@x.com");
            db.Users.AddRange(u1, admin);
            await db.SaveChangesAsync();

            var olderId = Guid.NewGuid();
            var newerId = Guid.NewGuid();

            await SeedFeedbackAsync(
                feedbackId: olderId,
                userId: u1.Id,
                subject: "Old",
                category: FeedbackCategory.Idea,
                comment: "C",
                createdAtUtc: DateTime.UtcNow.AddDays(-2),
                isSeenByAdmin: false,
                isReplySeenByUser: true,
                adminReply: "ReplyOld",
                repliedAtUtc: DateTime.UtcNow.AddDays(-1),
                repliedByUserId: admin.Id);

            await SeedFeedbackAsync(
                feedbackId: newerId,
                userId: u1.Id,
                subject: "New",
                category: FeedbackCategory.Bug,
                comment: "C2",
                createdAtUtc: DateTime.UtcNow.AddDays(-1),
                isSeenByAdmin: true,
                isReplySeenByUser: false);

            var list = await service.GetAllAsync();

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0].Id, Is.EqualTo(newerId));
            Assert.That(list[1].Id, Is.EqualTo(olderId));

            Assert.That(list[0].UserEmail, Is.EqualTo("user1@x.com"));
            Assert.That(list[0].RepliedByEmail, Is.Null);

            Assert.That(list[1].UserEmail, Is.EqualTo("user1@x.com"));
            Assert.That(list[1].RepliedByEmail, Is.EqualTo("admin@x.com"));
            Assert.That(list[1].AdminReply, Is.EqualTo("ReplyOld"));
            Assert.That(list[1].RepliedAt, Is.Not.Null);
        }

        [Test]
        public async Task GetAllAsync_UsesFallbackEmails_WhenMissing()
        {
            var u = NewUser(Guid.NewGuid(), null);
            var admin = NewUser(Guid.NewGuid(), null);
            db.Users.AddRange(u, admin);
            await db.SaveChangesAsync();

            await SeedFeedbackAsync(
                feedbackId: Guid.NewGuid(),
                userId: u.Id,
                subject: "S",
                category: FeedbackCategory.Other,
                comment: "C",
                createdAtUtc: DateTime.UtcNow,
                isSeenByAdmin: false,
                isReplySeenByUser: true,
                adminReply: "R",
                repliedAtUtc: DateTime.UtcNow,
                repliedByUserId: admin.Id);

            var list = await service.GetAllAsync();
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0].UserEmail, Is.EqualTo("(no email)"));
            Assert.That(list[0].RepliedByEmail, Is.EqualTo("(no email)"));
        }

        [Test]
        public async Task GetMineAsync_FiltersByUser_OrdersDesc_AndUserEmailIsEmpty()
        {
            var u1 = NewUser(Guid.NewGuid(), "u1@x.com");
            var u2 = NewUser(Guid.NewGuid(), "u2@x.com");
            db.Users.AddRange(u1, u2);
            await db.SaveChangesAsync();

            var mineNew = Guid.NewGuid();
            var mineOld = Guid.NewGuid();

            await SeedFeedbackAsync(
                feedbackId: mineOld,
                userId: u1.Id,
                subject: "MineOld",
                category: FeedbackCategory.Idea,
                comment: "C",
                createdAtUtc: DateTime.UtcNow.AddDays(-2),
                isSeenByAdmin: false,
                isReplySeenByUser: true);

            await SeedFeedbackAsync(
                feedbackId: mineNew,
                userId: u1.Id,
                subject: "MineNew",
                category: FeedbackCategory.Bug,
                comment: "C2",
                createdAtUtc: DateTime.UtcNow.AddDays(-1),
                isSeenByAdmin: true,
                isReplySeenByUser: false);

            await SeedFeedbackAsync(
                feedbackId: Guid.NewGuid(),
                userId: u2.Id,
                subject: "OtherUser",
                category: FeedbackCategory.Other,
                comment: "C3",
                createdAtUtc: DateTime.UtcNow,
                isSeenByAdmin: false,
                isReplySeenByUser: true);

            var list = await service.GetMineAsync(u1.Id);

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0].Id, Is.EqualTo(mineNew));
            Assert.That(list[1].Id, Is.EqualTo(mineOld));
            Assert.That(list[0].UserEmail, Is.EqualTo(string.Empty));
            Assert.That(list[1].UserEmail, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetDetailsAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsDto_MapsEmails_AndReplyInfo()
        {
            var u = NewUser(Guid.NewGuid(), "u@x.com");
            var admin = NewUser(Guid.NewGuid(), "a@x.com");
            db.Users.AddRange(u, admin);
            await db.SaveChangesAsync();

            var fid = Guid.NewGuid();

            await SeedFeedbackAsync(
                feedbackId: fid,
                userId: u.Id,
                subject: "S",
                category: FeedbackCategory.Question,
                comment: "C",
                createdAtUtc: DateTime.UtcNow.AddHours(-5),
                isSeenByAdmin: false,
                isReplySeenByUser: true,
                adminReply: "R",
                repliedAtUtc: DateTime.UtcNow.AddHours(-1),
                repliedByUserId: admin.Id);

            var res = await service.GetDetailsAsync(fid);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Id, Is.EqualTo(fid));
            Assert.That(res.UserEmail, Is.EqualTo("u@x.com"));
            Assert.That(res.RepliedByEmail, Is.EqualTo("a@x.com"));
            Assert.That(res.AdminReply, Is.EqualTo("R"));
            Assert.That(res.RepliedAt, Is.Not.Null);
        }

        [Test]
        public void ReplyAsync_Throws_WhenReplyWhitespace()
        {
            Assert.ThrowsAsync<ArgumentException>(() => service.ReplyAsync(Guid.NewGuid(), Guid.NewGuid(), "   "));
        }

        [Test]
        public void ReplyAsync_Throws_WhenFeedbackNotFound()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => service.ReplyAsync(Guid.NewGuid(), Guid.NewGuid(), "ok"));
        }

        [Test]
        public async Task ReplyAsync_SetsReplyFields_Trims_AndMarksFlags()
        {
            var u = NewUser(Guid.NewGuid(), "u@x.com");
            var admin = NewUser(Guid.NewGuid(), "a@x.com");
            db.Users.AddRange(u, admin);
            await db.SaveChangesAsync();

            var fid = Guid.NewGuid();

            await SeedFeedbackAsync(
                feedbackId: fid,
                userId: u.Id,
                subject: "S",
                category: FeedbackCategory.Other,
                comment: "C",
                createdAtUtc: DateTime.UtcNow.AddDays(-1),
                isSeenByAdmin: false,
                isReplySeenByUser: true);

            await service.ReplyAsync(fid, admin.Id, "  hello  ");

            var f = await db.Feedbacks.FirstAsync(x => x.Id == fid);
            Assert.That(f.AdminReply, Is.EqualTo("hello"));
            Assert.That(f.RepliedAt, Is.Not.Null);
            Assert.That(f.RepliedByUserId, Is.EqualTo(admin.Id));
            Assert.That(f.IsReplySeenByUser, Is.False);
            Assert.That(f.IsSeenByAdmin, Is.True);
        }

        [Test]
        public async Task GetAdminNewCountAsync_ReturnsCountOfNotSeenByAdmin()
        {
            var u = NewUser(Guid.NewGuid(), "u@x.com");
            db.Users.Add(u);
            await db.SaveChangesAsync();

            await SeedFeedbackAsync(Guid.NewGuid(), u.Id, "S1", FeedbackCategory.Other, "C", DateTime.UtcNow, isSeenByAdmin: false, isReplySeenByUser: true);
            await SeedFeedbackAsync(Guid.NewGuid(), u.Id, "S2", FeedbackCategory.Other, "C", DateTime.UtcNow, isSeenByAdmin: true, isReplySeenByUser: true);
            await SeedFeedbackAsync(Guid.NewGuid(), u.Id, "S3", FeedbackCategory.Other, "C", DateTime.UtcNow, isSeenByAdmin: false, isReplySeenByUser: true);

            var count = await service.GetAdminNewCountAsync();
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetUserUnreadRepliesCountAsync_ReturnsOnlyUnreadWithReplyAndRepliedAt()
        {
            var u = NewUser(Guid.NewGuid(), "u@x.com");
            var admin = NewUser(Guid.NewGuid(), "a@x.com");
            db.Users.AddRange(u, admin);
            await db.SaveChangesAsync();

            await SeedFeedbackAsync(
                Guid.NewGuid(), u.Id, "Unread", FeedbackCategory.Other, "C",
                DateTime.UtcNow.AddHours(-2),
                isSeenByAdmin: true,
                isReplySeenByUser: false,
                adminReply: "R",
                repliedAtUtc: DateTime.UtcNow.AddHours(-1),
                repliedByUserId: admin.Id);

            await SeedFeedbackAsync(
                Guid.NewGuid(), u.Id, "Seen", FeedbackCategory.Other, "C",
                DateTime.UtcNow.AddHours(-2),
                isSeenByAdmin: true,
                isReplySeenByUser: true,
                adminReply: "R",
                repliedAtUtc: DateTime.UtcNow.AddHours(-1),
                repliedByUserId: admin.Id);

            await SeedFeedbackAsync(
                Guid.NewGuid(), u.Id, "NoReply", FeedbackCategory.Other, "C",
                DateTime.UtcNow.AddHours(-2),
                isSeenByAdmin: true,
                isReplySeenByUser: false,
                adminReply: null,
                repliedAtUtc: DateTime.UtcNow.AddHours(-1),
                repliedByUserId: admin.Id);

            await SeedFeedbackAsync(
                Guid.NewGuid(), u.Id, "NoRepliedAt", FeedbackCategory.Other, "C",
                DateTime.UtcNow.AddHours(-2),
                isSeenByAdmin: true,
                isReplySeenByUser: false,
                adminReply: "R",
                repliedAtUtc: null,
                repliedByUserId: admin.Id);

            var count = await service.GetUserUnreadRepliesCountAsync(u.Id);
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task MarkAllSeenByAdminAsync_SetsIsSeenByAdminTrue_ForAllUnseen()
        {
            var (sqliteDb, sqliteService, conn) = await CreateSqliteDbAsync();
            try
            {
                var u = NewUser(Guid.NewGuid(), "u@x.com");
                sqliteDb.Users.Add(u);
                await sqliteDb.SaveChangesAsync();

                sqliteDb.Feedbacks.AddRange(
                    new Feedback
                    {
                        Id = Guid.NewGuid(),
                        UserId = u.Id,
                        Subject = "S1",
                        Category = FeedbackCategory.Other,
                        Comment = "C",
                        CreatedAt = DateTime.UtcNow,
                        IsSeenByAdmin = false,
                        IsReplySeenByUser = true
                    },
                    new Feedback
                    {
                        Id = Guid.NewGuid(),
                        UserId = u.Id,
                        Subject = "S2",
                        Category = FeedbackCategory.Other,
                        Comment = "C",
                        CreatedAt = DateTime.UtcNow,
                        IsSeenByAdmin = true,
                        IsReplySeenByUser = true
                    },
                    new Feedback
                    {
                        Id = Guid.NewGuid(),
                        UserId = u.Id,
                        Subject = "S3",
                        Category = FeedbackCategory.Other,
                        Comment = "C",
                        CreatedAt = DateTime.UtcNow,
                        IsSeenByAdmin = false,
                        IsReplySeenByUser = true
                    }
                );
                await sqliteDb.SaveChangesAsync();

                await sqliteService.MarkAllSeenByAdminAsync();

                var all = await sqliteDb.Feedbacks.AsNoTracking().ToListAsync();
                Assert.That(all.All(x => x.IsSeenByAdmin), Is.True);
            }
            finally
            {
                await sqliteDb.DisposeAsync();
                await conn.DisposeAsync();
            }
        }

        [Test]
        public async Task MarkUserRepliesSeenAsync_SetsIsReplySeenByUserTrue_OnlyForUnreadRepliesOfUser()
        {
            var (sqliteDb, sqliteService, conn) = await CreateSqliteDbAsync();
            try
            {
                var u1 = NewUser(Guid.NewGuid(), "u1@x.com");
                var u2 = NewUser(Guid.NewGuid(), "u2@x.com");
                sqliteDb.Users.AddRange(u1, u2);
                await sqliteDb.SaveChangesAsync();

                var unread = Guid.NewGuid();
                var seen = Guid.NewGuid();
                var noReply = Guid.NewGuid();
                var otherUserUnread = Guid.NewGuid();

                sqliteDb.Feedbacks.AddRange(
                    new Feedback
                    {
                        Id = unread,
                        UserId = u1.Id,
                        Subject = "Unread",
                        Category = FeedbackCategory.Other,
                        Comment = "C",
                        CreatedAt = DateTime.UtcNow.AddHours(-2),
                        IsSeenByAdmin = true,
                        IsReplySeenByUser = false,
                        AdminReply = "R",
                        RepliedAt = DateTime.UtcNow.AddHours(-1)
                    },
                    new Feedback
                    {
                        Id = seen,
                        UserId = u1.Id,
                        Subject = "Seen",
                        Category = FeedbackCategory.Other,
                        Comment = "C",
                        CreatedAt = DateTime.UtcNow.AddHours(-2),
                        IsSeenByAdmin = true,
                        IsReplySeenByUser = true,
                        AdminReply = "R",
                        RepliedAt = DateTime.UtcNow.AddHours(-1)
                    },
                    new Feedback
                    {
                        Id = noReply,
                        UserId = u1.Id,
                        Subject = "NoReply",
                        Category = FeedbackCategory.Other,
                        Comment = "C",
                        CreatedAt = DateTime.UtcNow.AddHours(-2),
                        IsSeenByAdmin = true,
                        IsReplySeenByUser = false,
                        AdminReply = null,
                        RepliedAt = DateTime.UtcNow.AddHours(-1)
                    },
                    new Feedback
                    {
                        Id = otherUserUnread,
                        UserId = u2.Id,
                        Subject = "OtherUserUnread",
                        Category = FeedbackCategory.Other,
                        Comment = "C",
                        CreatedAt = DateTime.UtcNow.AddHours(-2),
                        IsSeenByAdmin = true,
                        IsReplySeenByUser = false,
                        AdminReply = "R",
                        RepliedAt = DateTime.UtcNow.AddHours(-1)
                    }
                );

                await sqliteDb.SaveChangesAsync();

                await sqliteService.MarkUserRepliesSeenAsync(u1.Id);

                var all = await sqliteDb.Feedbacks.AsNoTracking().ToListAsync();

                Assert.That(all.Single(x => x.Id == unread).IsReplySeenByUser, Is.True);
                Assert.That(all.Single(x => x.Id == seen).IsReplySeenByUser, Is.True);
                Assert.That(all.Single(x => x.Id == noReply).IsReplySeenByUser, Is.False);
                Assert.That(all.Single(x => x.Id == otherUserUnread).IsReplySeenByUser, Is.False);
            }
            finally
            {
                await sqliteDb.DisposeAsync();
                await conn.DisposeAsync();
            }
        }

        [Test]
        public async Task IsOwnerAsync_ReturnsTrueOnlyForMatchingUser()
        {
            var u1 = NewUser(Guid.NewGuid(), "u1@x.com");
            var u2 = NewUser(Guid.NewGuid(), "u2@x.com");
            db.Users.AddRange(u1, u2);
            await db.SaveChangesAsync();

            var fid = Guid.NewGuid();
            await SeedFeedbackAsync(fid, u1.Id, "S", FeedbackCategory.Other, "C", DateTime.UtcNow, isSeenByAdmin: false, isReplySeenByUser: true);

            var yes = await service.IsOwnerAsync(fid, u1.Id);
            var no = await service.IsOwnerAsync(fid, u2.Id);

            Assert.That(yes, Is.True);
            Assert.That(no, Is.False);
        }

        [Test]
        public async Task MarkSeenByAdminAsync_MarksOnlyIfCurrentlyUnseen()
        {
            var (sqliteDb, sqliteService, conn) = await CreateSqliteDbAsync();
            try
            {
                var u = NewUser(Guid.NewGuid(), "u@x.com");
                sqliteDb.Users.Add(u);
                await sqliteDb.SaveChangesAsync();

                var unseenId = Guid.NewGuid();
                var seenId = Guid.NewGuid();

                sqliteDb.Feedbacks.AddRange(
                    new Feedback
                    {
                        Id = unseenId,
                        UserId = u.Id,
                        Subject = "Unseen",
                        Category = FeedbackCategory.Other,
                        Comment = "C",
                        CreatedAt = DateTime.UtcNow,
                        IsSeenByAdmin = false,
                        IsReplySeenByUser = true
                    },
                    new Feedback
                    {
                        Id = seenId,
                        UserId = u.Id,
                        Subject = "Seen",
                        Category = FeedbackCategory.Other,
                        Comment = "C",
                        CreatedAt = DateTime.UtcNow,
                        IsSeenByAdmin = true,
                        IsReplySeenByUser = true
                    }
                );

                await sqliteDb.SaveChangesAsync();

                await sqliteService.MarkSeenByAdminAsync(unseenId);
                await sqliteService.MarkSeenByAdminAsync(seenId);

                var all = await sqliteDb.Feedbacks.AsNoTracking().ToListAsync();
                Assert.That(all.Single(x => x.Id == unseenId).IsSeenByAdmin, Is.True);
                Assert.That(all.Single(x => x.Id == seenId).IsSeenByAdmin, Is.True);
            }
            finally
            {
                await sqliteDb.DisposeAsync();
                await conn.DisposeAsync();
            }
        }
    }
}