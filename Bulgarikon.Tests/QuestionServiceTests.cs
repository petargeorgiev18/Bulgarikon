using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulgarikon.Core.DTOs.AnswerDTOs;
using Bulgarikon.Core.DTOs.QuestionDTOs;
using Bulgarikon.Core.Implementations;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bulgarikon.Tests.Services
{
    [TestFixture]
    public class QuestionServiceTests
    {
        private BulgarikonDbContext db = null!;
        private QuestionService service = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("QuestionServiceTests_" + Guid.NewGuid())
                .Options;

            db = new BulgarikonDbContext(options);
            service = new QuestionService(db);
        }

        [TearDown]
        public async Task TearDown()
        {
            await db.DisposeAsync();
        }

        [Test]
        public async Task GetByQuizAsync_FiltersByQuiz_OrdersByText_AndMapsAnswersCount()
        {
            var quiz1 = Guid.NewGuid();
            var quiz2 = Guid.NewGuid();

            var q1 = new Question { Id = Guid.NewGuid(), QuizId = quiz1, Text = "B" };
            var q2 = new Question { Id = Guid.NewGuid(), QuizId = quiz1, Text = "A" };
            var other = new Question { Id = Guid.NewGuid(), QuizId = quiz2, Text = "X" };

            db.Questions.AddRange(q1, q2, other);

            db.Answers.AddRange(
                new Answer { Id = Guid.NewGuid(), QuestionId = q1.Id, Text = "a1", IsCorrect = false },
                new Answer { Id = Guid.NewGuid(), QuestionId = q1.Id, Text = "a2", IsCorrect = true },
                new Answer { Id = Guid.NewGuid(), QuestionId = q2.Id, Text = "b1", IsCorrect = true },
                new Answer { Id = Guid.NewGuid(), QuestionId = other.Id, Text = "x1", IsCorrect = true }
            );

            await db.SaveChangesAsync();

            var res = (await service.GetByQuizAsync(quiz1)).ToList();

            Assert.That(res.Count, Is.EqualTo(2));
            Assert.That(res[0].Text, Is.EqualTo("A"));
            Assert.That(res[1].Text, Is.EqualTo("B"));
            Assert.That(res[0].AnswersCount, Is.EqualTo(1));
            Assert.That(res[1].AnswersCount, Is.EqualTo(2));
        }

        [Test]
        public async Task CreateAsync_NormalizesTo4Answers_SetsOnlyCorrectByIndex_TrimsText_AndSaves()
        {
            var quizId = Guid.NewGuid();

            var dto = new QuestionFormDto
            {
                QuizId = quizId,
                Text = "  Q?  ",
                CorrectAnswerIndex = 2,
                Answers = new List<AnswerFormDto>
                {
                    new AnswerFormDto { Text = "  A1  ", IsCorrect = true },
                    new AnswerFormDto { Text = "  A2  ", IsCorrect = true },
                    new AnswerFormDto { Text = "  A3  ", IsCorrect = false },
                    new AnswerFormDto { Text = "  A4  ", IsCorrect = false },
                    new AnswerFormDto { Text = "  A5  ", IsCorrect = false }
                }
            };

            var id = await service.CreateAsync(dto);

            var q = await db.Questions.Include(x => x.Answers).FirstAsync(x => x.Id == id);

            Assert.That(q.Text, Is.EqualTo("Q?"));
            Assert.That(q.QuizId, Is.EqualTo(quizId));
            Assert.That(q.Answers.Count, Is.EqualTo(4));
            Assert.That(q.Answers.Any(a => a.Text == "A5"), Is.False);

            Assert.That(q.Answers.Count(a => a.IsCorrect), Is.EqualTo(1));
            var correct = q.Answers.Single(a => a.IsCorrect);
            Assert.That(correct.Text, Is.EqualTo("A3"));
        }

        [Test]
        public void CreateAsync_Throws_WhenLessThan2AnswersProvided()
        {
            var dto = new QuestionFormDto
            {
                QuizId = Guid.NewGuid(),
                Text = "Q",
                CorrectAnswerIndex = 0,
                Answers = new List<AnswerFormDto>
                {
                    new AnswerFormDto { Text = "A1" }
                }
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public void CreateAsync_Throws_WhenAnyAnswerTextIsWhitespace_AfterNormalizationTo4()
        {
            var dto = new QuestionFormDto
            {
                QuizId = Guid.NewGuid(),
                Text = "Q",
                CorrectAnswerIndex = 0,
                Answers = new List<AnswerFormDto>
                {
                    new AnswerFormDto { Text = "A1" },
                    new AnswerFormDto { Text = "   " },
                    new AnswerFormDto { Text = "A3" },
                    new AnswerFormDto { Text = "A4" }
                }
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public void CreateAsync_Throws_WhenAnswersListIsNull_BecauseItCreatesBlankAnswers()
        {
            var dto = new QuestionFormDto
            {
                QuizId = Guid.NewGuid(),
                Text = "Q",
                CorrectAnswerIndex = 0,
                Answers = null
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public async Task GetForEditAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetForEditAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_WhenLessThan4Answers_PadsTo4_AndCorrectIndexPointsToCorrectAnswer()
        {
            var quizId = Guid.NewGuid();
            var qId = Guid.NewGuid();

            db.Questions.Add(new Question { Id = qId, QuizId = quizId, Text = "Q" });

            db.Answers.AddRange(
                new Answer { Id = Guid.NewGuid(), QuestionId = qId, Text = "A1", IsCorrect = false },
                new Answer { Id = Guid.NewGuid(), QuestionId = qId, Text = "A2", IsCorrect = true }
            );

            await db.SaveChangesAsync();

            var res = await service.GetForEditAsync(qId);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Answers.Count, Is.EqualTo(4));
            Assert.That(res.CorrectAnswerIndex, Is.InRange(0, 3));
            Assert.That(res.Answers[res.CorrectAnswerIndex].IsCorrect, Is.True);
        }

        [Test]
        public async Task GetForEditAsync_WhenMoreThan4Answers_TrimsTo4_AndIfNoCorrectAmongFirst4_SetsCorrectIndex0()
        {
            var quizId = Guid.NewGuid();
            var qId = Guid.NewGuid();

            db.Questions.Add(new Question { Id = qId, QuizId = quizId, Text = "Q" });

            // Deterministic IDs so OrderBy(a => a.Id) is predictable:
            // first four are 000...001 - 000...004 (all IsCorrect=false)
            // fifth is 000...005 (IsCorrect=true) but should be trimmed away
            var id1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var id2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var id3 = Guid.Parse("00000000-0000-0000-0000-000000000003");
            var id4 = Guid.Parse("00000000-0000-0000-0000-000000000004");
            var id5 = Guid.Parse("00000000-0000-0000-0000-000000000005");

            db.Answers.AddRange(
                new Answer { Id = id1, QuestionId = qId, Text = "A1", IsCorrect = false },
                new Answer { Id = id2, QuestionId = qId, Text = "A2", IsCorrect = false },
                new Answer { Id = id3, QuestionId = qId, Text = "A3", IsCorrect = false },
                new Answer { Id = id4, QuestionId = qId, Text = "A4", IsCorrect = false },
                new Answer { Id = id5, QuestionId = qId, Text = "A5", IsCorrect = true }
            );

            await db.SaveChangesAsync();

            var res = await service.GetForEditAsync(qId);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Answers.Count, Is.EqualTo(4));
            Assert.That(res.Answers.Any(a => a.Text == "A5"), Is.False);

            Assert.That(res.Answers.Count(a => a.IsCorrect), Is.EqualTo(0));
            Assert.That(res.CorrectAnswerIndex, Is.EqualTo(0));
        }

        [Test]
        public async Task UpdateAsync_UpdatesQuestionText_ReplacesAnswers_NormalizesCorrectIndex_AndTrims()
        {
            await using var conn = new SqliteConnection("DataSource=:memory:");
            await conn.OpenAsync();

            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = OFF;";
                await cmd.ExecuteNonQueryAsync();
            }

            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseSqlite(conn)
                .Options;

            await using var sqliteDb = new BulgarikonDbContext(options);
            await sqliteDb.Database.EnsureCreatedAsync();

            var quizId = Guid.NewGuid();
            var qId = Guid.NewGuid();

            sqliteDb.Questions.Add(new Question { Id = qId, QuizId = quizId, Text = "Old" });
            sqliteDb.Answers.AddRange(
                new Answer { Id = Guid.NewGuid(), QuestionId = qId, Text = "Old1", IsCorrect = true },
                new Answer { Id = Guid.NewGuid(), QuestionId = qId, Text = "Old2", IsCorrect = false },
                new Answer { Id = Guid.NewGuid(), QuestionId = qId, Text = "Old3", IsCorrect = false },
                new Answer { Id = Guid.NewGuid(), QuestionId = qId, Text = "Old4", IsCorrect = false }
            );
            await sqliteDb.SaveChangesAsync();

            sqliteDb.ChangeTracker.Clear();

            var sqliteService = new QuestionService(sqliteDb);

            var dto = new QuestionFormDto
            {
                QuizId = quizId,
                Text = "  New  ",
                CorrectAnswerIndex = 3,
                Answers = new List<AnswerFormDto>
                {
                    new AnswerFormDto { Text = "  A1  ", IsCorrect = true },
                    new AnswerFormDto { Text = "  A2  ", IsCorrect = true },
                    new AnswerFormDto { Text = "  A3  ", IsCorrect = true },
                    new AnswerFormDto { Text = "  A4  ", IsCorrect = true }
                }
            };

            await sqliteService.UpdateAsync(qId, dto);

            var updated = await sqliteDb.Questions.Include(x => x.Answers).FirstAsync(x => x.Id == qId);

            Assert.That(updated.Text, Is.EqualTo("New"));
            Assert.That(updated.Answers.Count, Is.EqualTo(4));
            Assert.That(updated.Answers.Any(a => a.Text.StartsWith("Old")), Is.False);

            Assert.That(updated.Answers.Count(a => a.IsCorrect), Is.EqualTo(1));
            var correct = updated.Answers.Single(a => a.IsCorrect);
            Assert.That(correct.Text, Is.EqualTo("A4"));
        }

        [Test]
        public void UpdateAsync_Throws_WhenNotFound()
        {
            var dto = new QuestionFormDto
            {
                QuizId = Guid.NewGuid(),
                Text = "Q",
                CorrectAnswerIndex = 0,
                Answers = new List<AnswerFormDto>
                {
                    new AnswerFormDto { Text = "A1" },
                    new AnswerFormDto { Text = "A2" },
                    new AnswerFormDto { Text = "A3" },
                    new AnswerFormDto { Text = "A4" }
                }
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public void UpdateAsync_Throws_WhenInvalidAnswersContainWhitespace()
        {
            var quizId = Guid.NewGuid();
            var qId = Guid.NewGuid();

            db.Questions.Add(new Question { Id = qId, QuizId = quizId, Text = "Q" });
            db.Answers.AddRange(
                new Answer { Id = Guid.NewGuid(), QuestionId = qId, Text = "Old1", IsCorrect = true },
                new Answer { Id = Guid.NewGuid(), QuestionId = qId, Text = "Old2", IsCorrect = false }
            );
            db.SaveChanges();

            var dto = new QuestionFormDto
            {
                QuizId = quizId,
                Text = "New",
                CorrectAnswerIndex = 0,
                Answers = new List<AnswerFormDto>
                {
                    new AnswerFormDto { Text = "A1" },
                    new AnswerFormDto { Text = "   " },
                    new AnswerFormDto { Text = "A3" },
                    new AnswerFormDto { Text = "A4" }
                }
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.UpdateAsync(qId, dto));
        }

        [Test]
        public async Task DeleteAsync_DoesNothing_WhenNotFound()
        {
            await service.DeleteAsync(Guid.NewGuid());
            Assert.That(await db.Questions.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteAsync_RemovesQuestion_WhenFound()
        {
            var q = new Question { Id = Guid.NewGuid(), QuizId = Guid.NewGuid(), Text = "Q" };

            db.Questions.Add(q);
            db.Answers.AddRange(
                new Answer { Id = Guid.NewGuid(), QuestionId = q.Id, Text = "A1", IsCorrect = true },
                new Answer { Id = Guid.NewGuid(), QuestionId = q.Id, Text = "A2", IsCorrect = false }
            );
            await db.SaveChangesAsync();

            await service.DeleteAsync(q.Id);

            Assert.That(await db.Questions.CountAsync(), Is.EqualTo(0));
        }
    }
}