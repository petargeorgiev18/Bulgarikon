using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulgarikon.Core.DTOs.QuizDTOs;
using Bulgarikon.Core.Implementations;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bulgarikon.Tests.Services
{
    [TestFixture]
    public class QuizResultServiceTests
    {
        private BulgarikonDbContext db = null!;
        private QuizResultService service = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("QuizResultServiceTests_" + Guid.NewGuid())
                .Options;

            db = new BulgarikonDbContext(options);
            service = new QuizResultService(db);
        }

        [TearDown]
        public async Task TearDown()
        {
            await db.DisposeAsync();
        }

        private async Task<(Era era, Quiz quiz, List<Question> questions)> SeedQuizAsync(int questionsCount = 3)
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                StartYear = 1,
                EndYear = 2,
                Description = "D"
            };

            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Title = "Quiz",
                EraId = era.Id,
                Era = era
            };

            var questions = Enumerable.Range(1, questionsCount)
                .Select(i => new Question
                {
                    Id = Guid.NewGuid(),
                    QuizId = quiz.Id,
                    Quiz = quiz,
                    Text = "Q" + i
                })
                .ToList();

            db.Eras.Add(era);
            db.Quizzes.Add(quiz);
            db.Questions.AddRange(questions);

            await db.SaveChangesAsync();
            return (era, quiz, questions);
        }

        [Test]
        public async Task SaveResultAsync_SavesEntity_AndReturnsId()
        {
            var (_, quiz, _) = await SeedQuizAsync(2);
            var userId = Guid.NewGuid();

            var id = await service.SaveResultAsync(quiz.Id, userId, 1);

            Assert.That(id, Is.Not.EqualTo(Guid.Empty));

            var saved = await db.QuizResults.FirstOrDefaultAsync(x => x.Id == id);
            Assert.That(saved, Is.Not.Null);
            Assert.That(saved!.QuizId, Is.EqualTo(quiz.Id));
            Assert.That(saved.UserId, Is.EqualTo(userId));
            Assert.That(saved.Score, Is.EqualTo(1));
            Assert.That(saved.DateTaken, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        public async Task GetResultAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetResultAsync(Guid.NewGuid(), Guid.NewGuid(), isAdmin: false);
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetResultAsync_ReturnsNull_WhenNotAdmin_AndNotOwner()
        {
            var (era, quiz, questions) = await SeedQuizAsync(4);

            var ownerId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var result = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quiz.Id,
                Quiz = quiz,
                UserId = ownerId,
                Score = 2,
                DateTaken = DateTime.UtcNow.AddDays(-1)
            };

            db.QuizResults.Add(result);
            await db.SaveChangesAsync();

            var res = await service.GetResultAsync(result.Id, otherUserId, isAdmin: false);
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetResultAsync_ReturnsDto_WhenOwner()
        {
            var (era, quiz, questions) = await SeedQuizAsync(5);

            var userId = Guid.NewGuid();
            var taken = DateTime.UtcNow.AddHours(-2);

            var result = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quiz.Id,
                Quiz = quiz,
                UserId = userId,
                Score = 3,
                DateTaken = taken
            };

            db.QuizResults.Add(result);
            await db.SaveChangesAsync();

            var res = await service.GetResultAsync(result.Id, userId, isAdmin: false);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Id, Is.EqualTo(result.Id));
            Assert.That(res.QuizId, Is.EqualTo(quiz.Id));
            Assert.That(res.QuizTitle, Is.EqualTo("Quiz"));
            Assert.That(res.EraName, Is.EqualTo("Era"));
            Assert.That(res.Score, Is.EqualTo(3));
            Assert.That(res.MaxScore, Is.EqualTo(5));
            Assert.That(res.DateTaken, Is.EqualTo(taken));
        }

        [Test]
        public async Task GetResultAsync_ReturnsDto_WhenAdmin_EvenIfNotOwner()
        {
            var (_, quiz, _) = await SeedQuizAsync(3);

            var ownerId = Guid.NewGuid();
            var adminId = Guid.NewGuid();

            var result = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quiz.Id,
                Quiz = quiz,
                UserId = ownerId,
                Score = 1,
                DateTaken = DateTime.UtcNow.AddDays(-3)
            };

            db.QuizResults.Add(result);
            await db.SaveChangesAsync();

            var res = await service.GetResultAsync(result.Id, adminId, isAdmin: true);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Id, Is.EqualTo(result.Id));
            Assert.That(res.MaxScore, Is.EqualTo(3));
        }

        [Test]
        public async Task MyResultsAsync_FiltersByUser_OrdersByDateTakenDesc_AndMapsMaxScore()
        {
            var (_, quiz1, questions1) = await SeedQuizAsync(2);
            var (_, quiz2, questions2) = await SeedQuizAsync(4);

            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var r1 = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quiz1.Id,
                Quiz = quiz1,
                UserId = userId,
                Score = 1,
                DateTaken = new DateTime(2024, 01, 01, 10, 0, 0, DateTimeKind.Utc)
            };

            var r2 = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quiz2.Id,
                Quiz = quiz2,
                UserId = userId,
                Score = 3,
                DateTaken = new DateTime(2024, 01, 02, 10, 0, 0, DateTimeKind.Utc)
            };

            var other = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quiz1.Id,
                Quiz = quiz1,
                UserId = otherUserId,
                Score = 2,
                DateTaken = new DateTime(2024, 01, 03, 10, 0, 0, DateTimeKind.Utc)
            };

            db.QuizResults.AddRange(r1, r2, other);
            await db.SaveChangesAsync();

            var res = (await service.MyResultsAsync(userId)).ToList();

            Assert.That(res.Count, Is.EqualTo(2));
            Assert.That(res[0].Id, Is.EqualTo(r2.Id));
            Assert.That(res[1].Id, Is.EqualTo(r1.Id));

            Assert.That(res[0].QuizTitle, Is.EqualTo("Quiz"));
            Assert.That(res[0].Score, Is.EqualTo(3));
            Assert.That(res[0].MaxScore, Is.EqualTo(4));

            Assert.That(res[1].Score, Is.EqualTo(1));
            Assert.That(res[1].MaxScore, Is.EqualTo(2));
        }

        [Test]
        public async Task CountCorrectAsync_Returns0_WhenChosenIdsNullOrEmpty()
        {
            var (_, quiz, _) = await SeedQuizAsync(2);

            var c1 = await service.CountCorrectAsync(quiz.Id, null!);
            var c2 = await service.CountCorrectAsync(quiz.Id, Array.Empty<Guid>());

            Assert.That(c1, Is.EqualTo(0));
            Assert.That(c2, Is.EqualTo(0));
        }

        [Test]
        public async Task CountCorrectAsync_CountsOnlyCorrectChosenAnswers_ForThatQuiz()
        {
            var (_, quiz1, questions1) = await SeedQuizAsync(3);
            var (_, quiz2, questions2) = await SeedQuizAsync(2);

            var q1 = questions1[0];
            var q2 = questions1[1];
            var qOtherQuiz = questions2[0];

            var a1Correct = new Answer { Id = Guid.NewGuid(), QuestionId = q1.Id, Question = q1, Text = "A1", IsCorrect = true };
            var a1Wrong = new Answer { Id = Guid.NewGuid(), QuestionId = q1.Id, Question = q1, Text = "A2", IsCorrect = false };
            var a2Correct = new Answer { Id = Guid.NewGuid(), QuestionId = q2.Id, Question = q2, Text = "B1", IsCorrect = true };
            var otherQuizCorrect = new Answer { Id = Guid.NewGuid(), QuestionId = qOtherQuiz.Id, Question = qOtherQuiz, Text = "X1", IsCorrect = true };

            db.Answers.AddRange(a1Correct, a1Wrong, a2Correct, otherQuizCorrect);
            await db.SaveChangesAsync();

            var chosen = new[]
            {
                a1Correct.Id,
                a1Wrong.Id,
                a2Correct.Id,
                otherQuizCorrect.Id,
                a1Correct.Id
            };

            var count = await service.CountCorrectAsync(quiz1.Id, chosen);

            Assert.That(count, Is.EqualTo(2));
        }
    }
}