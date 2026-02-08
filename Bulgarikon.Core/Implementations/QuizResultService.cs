using Bulgarikon.Core.DTOs.QuizDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class QuizResultService : IQuizResultService
    {
        private readonly BulgarikonDbContext context;

        public QuizResultService(BulgarikonDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> SaveResultAsync(Guid quizId, Guid userId, int score)
        {
            var entity = new QuizResult
            {
                Id = Guid.NewGuid(),
                QuizId = quizId,
                UserId = userId,
                Score = score,
                DateTaken = DateTime.UtcNow
            };

            context.QuizResults.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<QuizResultViewDto?> GetResultAsync(Guid resultId, Guid userId, bool isAdmin)
        {
            var r = await context.QuizResults.AsNoTracking()
                .Include(x => x.Quiz).ThenInclude(q => q.Era)
                .Include(x => x.Quiz).ThenInclude(q => q.Questions)
                .FirstOrDefaultAsync(x => x.Id == resultId);

            if (r == null) return null;

            if (!isAdmin && r.UserId != userId)
                return null;

            var maxScore = r.Quiz.Questions.Count;

            return new QuizResultViewDto
            {
                Id = r.Id,
                QuizId = r.QuizId,
                QuizTitle = r.Quiz.Title,
                EraName = r.Quiz.Era.Name,
                Score = r.Score,
                MaxScore = maxScore,
                DateTaken = r.DateTaken
            };
        }

        public async Task<IEnumerable<QuizResultListItemDto>> MyResultsAsync(Guid userId)
        {
            return await context.QuizResults.AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.Quiz).ThenInclude(q => q.Questions)
                .OrderByDescending(x => x.DateTaken)
                .Select(x => new QuizResultListItemDto
                {
                    Id = x.Id,
                    QuizTitle = x.Quiz.Title,
                    Score = x.Score,
                    MaxScore = x.Quiz.Questions.Count,
                    DateTaken = x.DateTaken
                })
                .ToListAsync();
        }

        public async Task<int> CountCorrectAsync(Guid quizId, IEnumerable<Guid> chosenAnswerIds)
        {
            var ids = chosenAnswerIds?.ToList() ?? new List<Guid>();
            if (ids.Count == 0) return 0;

            return await context.Answers
                .AsNoTracking()
                .Where(a =>
                    ids.Contains(a.Id) &&
                    a.IsCorrect &&
                    a.Question.QuizId == quizId)
                .CountAsync();
        }
    }
}