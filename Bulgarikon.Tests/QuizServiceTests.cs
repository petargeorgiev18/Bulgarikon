using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulgarikon.Core.DTOs.AnswerDTOs;
using Bulgarikon.Core.DTOs.QuestionDTOs;
using Bulgarikon.Core.DTOs.QuizDTOs;
using Bulgarikon.Core.Implementations;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bulgarikon.Tests.Services
{
    [TestFixture]
    public class QuizServiceTests
    {
        private BulgarikonDbContext db = null!;
        private QuizService service = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("QuizServiceTests_" + Guid.NewGuid())
                .Options;

            db = new BulgarikonDbContext(options);
            service = new QuizService(db);
        }

        [TearDown]
        public async Task TearDown()
        {
            await db.DisposeAsync();
        }

        private async Task<Era> SeedEraAsync(string name = "Era")
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = name,
                StartYear = 1,
                EndYear = 2,
                Description = "D"
            };

            db.Eras.Add(era);
            await db.SaveChangesAsync();
            return era;
        }

        private async Task<Quiz> SeedQuizAsync(Era era, string title = "Quiz", int questionsCount = 0)
        {
            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Title = title,
                EraId = era.Id,
                Era = era
            };

            db.Quizzes.Add(quiz);

            if (questionsCount > 0)
            {
                var questions = Enumerable.Range(1, questionsCount)
                    .Select(i => new Question
                    {
                        Id = Guid.NewGuid(),
                        QuizId = quiz.Id,
                        Quiz = quiz,
                        Text = "Q" + i
                    })
                    .ToList();

                db.Questions.AddRange(questions);
            }

            await db.SaveChangesAsync();
            return quiz;
        }

        private async Task SeedQuestionWithAnswersAsync(Quiz quiz, string text, params (string answerText, bool isCorrect)[] answers)
        {
            var q = new Question
            {
                Id = Guid.NewGuid(),
                QuizId = quiz.Id,
                Quiz = quiz,
                Text = text
            };

            db.Questions.Add(q);

            var ans = answers.Select(a => new Answer
            {
                Id = Guid.NewGuid(),
                QuestionId = q.Id,
                Question = q,
                Text = a.answerText,
                IsCorrect = a.isCorrect
            }).ToList();

            db.Answers.AddRange(ans);
            await db.SaveChangesAsync();
        }

        [Test]
        public async Task GetByEraAsync_WhenEraIdProvided_Filters_OrdersByTitle_AndMapsCounts()
        {
            var era1 = await SeedEraAsync("Era1");
            var era2 = await SeedEraAsync("Era2");

            var q1 = await SeedQuizAsync(era1, "B", questionsCount: 2);
            var q2 = await SeedQuizAsync(era1, "A", questionsCount: 1);
            var other = await SeedQuizAsync(era2, "X", questionsCount: 3);

            var res = (await service.GetByEraAsync(era1.Id)).ToList();

            Assert.That(res.Count, Is.EqualTo(2));
            Assert.That(res[0].Title, Is.EqualTo("A"));
            Assert.That(res[1].Title, Is.EqualTo("B"));

            Assert.That(res[0].EraId, Is.EqualTo(era1.Id));
            Assert.That(res[0].EraName, Is.EqualTo("Era1"));
            Assert.That(res[0].QuestionsCount, Is.EqualTo(1));

            Assert.That(res[1].QuestionsCount, Is.EqualTo(2));
        }

        [Test]
        public async Task GetByEraAsync_WhenEraIdNull_ReturnsAllOrderedByTitle()
        {
            var era1 = await SeedEraAsync("Era1");
            var era2 = await SeedEraAsync("Era2");

            await SeedQuizAsync(era1, "B", questionsCount: 0);
            await SeedQuizAsync(era2, "A", questionsCount: 0);
            await SeedQuizAsync(era1, "C", questionsCount: 0);

            var res = (await service.GetByEraAsync(null)).ToList();

            Assert.That(res.Count, Is.EqualTo(3));
            Assert.That(res.Select(x => x.Title).ToList(), Is.EqualTo(new[] { "A", "B", "C" }));
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetDetailsAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsDto_WhenFound()
        {
            var era = await SeedEraAsync("Medieval");
            var quiz = await SeedQuizAsync(era, "QuizTitle", questionsCount: 4);

            var res = await service.GetDetailsAsync(quiz.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Id, Is.EqualTo(quiz.Id));
            Assert.That(res.Title, Is.EqualTo("QuizTitle"));
            Assert.That(res.EraId, Is.EqualTo(era.Id));
            Assert.That(res.EraName, Is.EqualTo("Medieval"));
            Assert.That(res.QuestionsCount, Is.EqualTo(4));
        }

        [Test]
        public async Task CreateAsync_TrimsTitle_Saves_AndReturnsId()
        {
            var era = await SeedEraAsync("Era1");

            var dto = new QuizFormDto
            {
                Title = "  New Quiz  ",
                EraId = era.Id
            };

            var id = await service.CreateAsync(dto);

            Assert.That(id, Is.Not.EqualTo(Guid.Empty));

            var created = await db.Quizzes.FirstOrDefaultAsync(x => x.Id == id);
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Title, Is.EqualTo("New Quiz"));
            Assert.That(created.EraId, Is.EqualTo(era.Id));
        }

        [Test]
        public async Task GetForEditAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetForEditAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsFormDto_WhenFound()
        {
            var era = await SeedEraAsync("Era1");
            var quiz = await SeedQuizAsync(era, "TitleX", questionsCount: 0);

            var res = await service.GetForEditAsync(quiz.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Title, Is.EqualTo("TitleX"));
            Assert.That(res.EraId, Is.EqualTo(era.Id));
        }

        [Test]
        public void UpdateAsync_Throws_WhenNotFound()
        {
            var dto = new QuizFormDto
            {
                Title = "X",
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public async Task UpdateAsync_UpdatesTitleAndEra_Trims_AndSaves()
        {
            var era1 = await SeedEraAsync("Era1");
            var era2 = await SeedEraAsync("Era2");
            var quiz = await SeedQuizAsync(era1, "Old", questionsCount: 0);

            var dto = new QuizFormDto
            {
                Title = "  New  ",
                EraId = era2.Id
            };

            await service.UpdateAsync(quiz.Id, dto);

            var updated = await db.Quizzes.FirstAsync(x => x.Id == quiz.Id);
            Assert.That(updated.Title, Is.EqualTo("New"));
            Assert.That(updated.EraId, Is.EqualTo(era2.Id));
        }

        [Test]
        public async Task DeleteAsync_DoesNothing_WhenNotFound()
        {
            await service.DeleteAsync(Guid.NewGuid());
            Assert.That(await db.Quizzes.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteAsync_RemovesQuiz_WhenFound()
        {
            var era = await SeedEraAsync("Era");
            var quiz = await SeedQuizAsync(era, "ToDelete", questionsCount: 0);

            await service.DeleteAsync(quiz.Id);

            Assert.That(await db.Quizzes.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetForTakeAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetForTakeAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetForTakeAsync_ReturnsQuizTakeDto_OrdersQuestionsByText_AndAnswersByText()
        {
            var era = await SeedEraAsync("Ancient");
            var quiz = await SeedQuizAsync(era, "TakeMe", questionsCount: 0);

            await SeedQuestionWithAnswersAsync(quiz, "B question",
                ("b2", false),
                ("b1", true));

            await SeedQuestionWithAnswersAsync(quiz, "A question",
                ("a2", false),
                ("a1", true));

            var res = await service.GetForTakeAsync(quiz.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.QuizId, Is.EqualTo(quiz.Id));
            Assert.That(res.Title, Is.EqualTo("TakeMe"));
            Assert.That(res.EraName, Is.EqualTo("Ancient"));

            Assert.That(res.Questions.Count, Is.EqualTo(2));
            Assert.That(res.Questions[0].Text, Is.EqualTo("A question"));
            Assert.That(res.Questions[1].Text, Is.EqualTo("B question"));

            Assert.That(res.Questions[0].Answers.Select(a => a.Text).ToList(), Is.EqualTo(new[] { "a1", "a2" }));
            Assert.That(res.Questions[1].Answers.Select(a => a.Text).ToList(), Is.EqualTo(new[] { "b1", "b2" }));
        }

        [Test]
        public async Task GetResultAsync_ReturnsNull_WhenNotFoundOrNotOwner()
        {
            var era = await SeedEraAsync("Era");
            var quiz = await SeedQuizAsync(era, "Quiz", questionsCount: 2);

            var ownerId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var r = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quiz.Id,
                Quiz = quiz,
                UserId = ownerId,
                Score = 1,
                DateTaken = DateTime.UtcNow
            };

            db.QuizResults.Add(r);
            await db.SaveChangesAsync();

            var notFound = await service.GetResultAsync(Guid.NewGuid(), ownerId);
            var notOwner = await service.GetResultAsync(r.Id, otherUserId);

            Assert.That(notFound, Is.Null);
            Assert.That(notOwner, Is.Null);
        }

        [Test]
        public async Task GetResultAsync_ReturnsDto_WhenOwner()
        {
            var era = await SeedEraAsync("EraX");
            var quiz = await SeedQuizAsync(era, "QuizX", questionsCount: 3);

            var userId = Guid.NewGuid();
            var taken = DateTime.UtcNow.AddMinutes(-30);

            var r = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quiz.Id,
                Quiz = quiz,
                UserId = userId,
                Score = 2,
                DateTaken = taken
            };

            db.QuizResults.Add(r);
            await db.SaveChangesAsync();

            var res = await service.GetResultAsync(r.Id, userId);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Id, Is.EqualTo(r.Id));
            Assert.That(res.QuizId, Is.EqualTo(quiz.Id));
            Assert.That(res.QuizTitle, Is.EqualTo("QuizX"));
            Assert.That(res.EraName, Is.EqualTo("EraX"));
            Assert.That(res.Score, Is.EqualTo(2));
            Assert.That(res.MaxScore, Is.EqualTo(3));
            Assert.That(res.DateTaken, Is.EqualTo(taken));
        }

        [Test]
        public async Task GetMyResultsAsync_FiltersByUser_OrdersByDateTakenDesc_AndMapsMaxScore()
        {
            var era = await SeedEraAsync("Era");
            var quiz1 = await SeedQuizAsync(era, "Q1", questionsCount: 2);
            var quiz2 = await SeedQuizAsync(era, "Q2", questionsCount: 4);

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
                QuizId = quiz2.Id,
                Quiz = quiz2,
                UserId = otherUserId,
                Score = 4,
                DateTaken = new DateTime(2024, 01, 03, 10, 0, 0, DateTimeKind.Utc)
            };

            db.QuizResults.AddRange(r1, r2, other);
            await db.SaveChangesAsync();

            var res = (await service.GetMyResultsAsync(userId)).ToList();

            Assert.That(res.Count, Is.EqualTo(2));
            Assert.That(res[0].Id, Is.EqualTo(r2.Id));
            Assert.That(res[1].Id, Is.EqualTo(r1.Id));

            Assert.That(res[0].QuizTitle, Is.EqualTo("Q2"));
            Assert.That(res[0].Score, Is.EqualTo(3));
            Assert.That(res[0].MaxScore, Is.EqualTo(4));

            Assert.That(res[1].QuizTitle, Is.EqualTo("Q1"));
            Assert.That(res[1].MaxScore, Is.EqualTo(2));
        }

        [Test]
        public void SubmitAsync_Throws_WhenQuizNotFound()
        {
            var dto = new QuizSubmitDto
            {
                QuizId = Guid.NewGuid(),
                Answers = new Dictionary<Guid, Guid>()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.SubmitAsync(dto, Guid.NewGuid()));
        }

        [Test]
        public async Task SubmitAsync_ComputesScore_IgnoresMissingAnswers_SavesQuizResult_AndReturnsId()
        {
            var era = await SeedEraAsync("Era");
            var quiz = await SeedQuizAsync(era, "Quiz", questionsCount: 0);

            var q1 = new Question { Id = Guid.NewGuid(), QuizId = quiz.Id, Quiz = quiz, Text = "Q1" };
            var q2 = new Question { Id = Guid.NewGuid(), QuizId = quiz.Id, Quiz = quiz, Text = "Q2" };

            var q1a1 = new Answer { Id = Guid.NewGuid(), QuestionId = q1.Id, Question = q1, Text = "A1", IsCorrect = true };
            var q1a2 = new Answer { Id = Guid.NewGuid(), QuestionId = q1.Id, Question = q1, Text = "A2", IsCorrect = false };

            var q2a1 = new Answer { Id = Guid.NewGuid(), QuestionId = q2.Id, Question = q2, Text = "B1", IsCorrect = false };
            var q2a2 = new Answer { Id = Guid.NewGuid(), QuestionId = q2.Id, Question = q2, Text = "B2", IsCorrect = true };

            db.Questions.AddRange(q1, q2);
            db.Answers.AddRange(q1a1, q1a2, q2a1, q2a2);
            await db.SaveChangesAsync();

            var userId = Guid.NewGuid();

            var submit = new QuizSubmitDto
            {
                QuizId = quiz.Id,
                Answers = new Dictionary<Guid, Guid>
                {
                    [q1.Id] = q1a1.Id
                }
            };

            var resultId = await service.SubmitAsync(submit, userId);

            Assert.That(resultId, Is.Not.EqualTo(Guid.Empty));

            var saved = await db.QuizResults.FirstAsync(x => x.Id == resultId);
            Assert.That(saved.UserId, Is.EqualTo(userId));
            Assert.That(saved.QuizId, Is.EqualTo(quiz.Id));
            Assert.That(saved.Score, Is.EqualTo(1));
        }

        [Test]
        public void EvaluateAsync_Throws_WhenQuizNotFound()
        {
            var dto = new QuizSubmitDto
            {
                QuizId = Guid.NewGuid(),
                Answers = new Dictionary<Guid, Guid>()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.EvaluateAsync(dto));
        }

        [Test]
        public async Task EvaluateAsync_ReturnsScoreAndMaxScore_AndIgnoresMissingAnswers()
        {
            var era = await SeedEraAsync("Era");
            var quiz = await SeedQuizAsync(era, "Quiz", questionsCount: 0);

            var q1 = new Question { Id = Guid.NewGuid(), QuizId = quiz.Id, Quiz = quiz, Text = "Q1" };
            var q2 = new Question { Id = Guid.NewGuid(), QuizId = quiz.Id, Quiz = quiz, Text = "Q2" };

            var q1a1 = new Answer { Id = Guid.NewGuid(), QuestionId = q1.Id, Question = q1, Text = "A1", IsCorrect = true };
            var q1a2 = new Answer { Id = Guid.NewGuid(), QuestionId = q1.Id, Question = q1, Text = "A2", IsCorrect = false };

            var q2a1 = new Answer { Id = Guid.NewGuid(), QuestionId = q2.Id, Question = q2, Text = "B1", IsCorrect = false };
            var q2a2 = new Answer { Id = Guid.NewGuid(), QuestionId = q2.Id, Question = q2, Text = "B2", IsCorrect = true };

            db.Questions.AddRange(q1, q2);
            db.Answers.AddRange(q1a1, q1a2, q2a1, q2a2);
            await db.SaveChangesAsync();

            var dto = new QuizSubmitDto
            {
                QuizId = quiz.Id,
                Answers = new Dictionary<Guid, Guid>
                {
                    [q1.Id] = q1a1.Id,
                    [q2.Id] = q2a1.Id
                }
            };

            var (score, maxScore) = await service.EvaluateAsync(dto);

            Assert.That(maxScore, Is.EqualTo(2));
            Assert.That(score, Is.EqualTo(1));
        }

        [Test]
        public async Task GetQuestionsCountAsync_ReturnsCount_ForQuiz()
        {
            var era = await SeedEraAsync("Era");
            var quiz1 = await SeedQuizAsync(era, "Q1", questionsCount: 3);
            var quiz2 = await SeedQuizAsync(era, "Q2", questionsCount: 1);

            var c1 = await service.GetQuestionsCountAsync(quiz1.Id);
            var c2 = await service.GetQuestionsCountAsync(quiz2.Id);

            Assert.That(c1, Is.EqualTo(3));
            Assert.That(c2, Is.EqualTo(1));
        }
    }
}