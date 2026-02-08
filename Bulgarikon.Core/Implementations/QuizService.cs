using Bulgarikon.Core.DTOs.QuizDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly BulgarikonDbContext context;

        public QuizService(BulgarikonDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<QuizViewDto>> GetByEraAsync(Guid? eraId)
        {
            IQueryable<Quiz> q = context.Quizzes.AsNoTracking();

            if (eraId.HasValue)
                q = q.Where(x => x.EraId == eraId.Value);

            return await q
                .Include(x => x.Era)
                .OrderBy(x => x.Title)
                .Select(x => new QuizViewDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    EraId = x.EraId,
                    EraName = x.Era.Name
                })
                .ToListAsync();
        }

        public async Task<QuizDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await context.Quizzes.AsNoTracking()
                .Include(x => x.Era)
                .Where(x => x.Id == id)
                .Select(x => new QuizDetailsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    EraId = x.EraId,
                    EraName = x.Era.Name,
                    QuestionsCount = x.Questions.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> CreateAsync(QuizFormDto model)
        {
            var entity = new Quiz
            {
                Id = Guid.NewGuid(),
                Title = model.Title.Trim(),
                EraId = model.EraId
            };

            context.Quizzes.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<QuizFormDto?> GetForEditAsync(Guid id)
        {
            var q = await context.Quizzes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (q == null) return null;

            return new QuizFormDto
            {
                Title = q.Title,
                EraId = q.EraId
            };
        }

        public async Task UpdateAsync(Guid id, QuizFormDto model)
        {
            var q = await context.Quizzes.FirstOrDefaultAsync(x => x.Id == id);
            if (q == null) throw new InvalidOperationException("Quiz not found.");

            q.Title = model.Title.Trim();
            q.EraId = model.EraId;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var q = await context.Quizzes.FirstOrDefaultAsync(x => x.Id == id);
            if (q == null) return;

            context.Quizzes.Remove(q);
            await context.SaveChangesAsync();
        }

        public async Task<QuizTakeDto?> GetForTakeAsync(Guid id)
        {
            var q = await context.Quizzes.AsNoTracking()
                .Include(x => x.Era)
                .Include(x => x.Questions)
                    .ThenInclude(qq => qq.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (q == null) return null;

            return new QuizTakeDto
            {
                QuizId = q.Id,
                Title = q.Title,
                EraName = q.Era.Name,
                Questions = q.Questions
                    .OrderBy(x => x.Text)
                    .Select(qq => new QuizTakeQuestionDto
                    {
                        QuestionId = qq.Id,
                        Text = qq.Text,
                        Answers = qq.Answers
                            .OrderBy(a => a.Text)
                            .Select(a => new QuizTakeAnswerDto
                            {
                                AnswerId = a.Id,
                                Text = a.Text,
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }

        public async Task<QuizResultViewDto?> GetResultAsync(Guid resultId, Guid userId)
        {
            return await context.QuizResults
                .AsNoTracking()
                .Where(r => r.Id == resultId && r.UserId == userId)
                .Select(r => new QuizResultViewDto
                {
                    Id = r.Id,
                    QuizId = r.QuizId,
                    QuizTitle = r.Quiz.Title,
                    EraName = r.Quiz.Era.Name,
                    Score = r.Score,
                    MaxScore = r.Quiz.Questions.Count,
                    DateTaken = r.DateTaken
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<QuizResultListItemDto>> GetMyResultsAsync(Guid userId)
        {
            return await context.QuizResults
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DateTaken)
                .Select(r => new QuizResultListItemDto
                {
                    Id = r.Id,
                    QuizTitle = r.Quiz.Title,
                    Score = r.Score,
                    MaxScore = r.Quiz.Questions.Count,
                    DateTaken = r.DateTaken
                })
                .ToListAsync();
        }

        public async Task<Guid> SubmitAsync(QuizSubmitDto model, Guid userId)
        {
            var quiz = await context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == model.QuizId);

            if (quiz == null)
                throw new InvalidOperationException("Quiz not found.");

            int score = 0;

            foreach (var question in quiz.Questions)
            {
                if (!model.Answers.TryGetValue(question.Id, out var selectedAnswerId))
                    continue;

                var isCorrect = question.Answers
                    .Any(a => a.Id == selectedAnswerId && a.IsCorrect);

                if (isCorrect)
                    score++;
            }

            var result = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quiz.Id,
                UserId = userId,
                Score = score,
                DateTaken = DateTime.UtcNow
            };

            context.QuizResults.Add(result);
            await context.SaveChangesAsync();

            return result.Id;
        }

        public async Task<(int score, int maxScore)> EvaluateAsync(QuizSubmitDto model)
        {
            var quiz = await context.Quizzes
                .AsNoTracking()
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == model.QuizId);

            if (quiz == null)
                throw new InvalidOperationException("Quiz not found.");

            int score = 0;
            int maxScore = quiz.Questions.Count;

            foreach (var question in quiz.Questions)
            {
                if (!model.Answers.TryGetValue(question.Id, out var chosenAnswerId))
                    continue;

                var isCorrect = question.Answers
                    .Any(a => a.Id == chosenAnswerId && a.IsCorrect);

                if (isCorrect)
                    score++;
            }

            return (score, maxScore);
        }

        public async Task<int> GetQuestionsCountAsync(Guid quizId)
        {
            return await context.Questions
                .AsNoTracking()
                .CountAsync(q => q.QuizId == quizId);
        }
    }
}