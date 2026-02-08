using Bulgarikon.Core.DTOs.AnswerDTOs;
using Bulgarikon.Core.DTOs.QuestionDTOs;
using Bulgarikon.Core.DTOs.QuizDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Core.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly BulgarikonDbContext context;

        public QuestionService(BulgarikonDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<QuestionListItemDto>> GetByQuizAsync(Guid quizId)
        {
            return await context.Questions.AsNoTracking()
                .Where(q => q.QuizId == quizId)
                .OrderBy(q => q.Text)
                .Select(q => new QuestionListItemDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    AnswersCount = q.Answers.Count
                })
                .ToListAsync();
        }

        public async Task<Guid> CreateAsync(QuestionFormDto model)
        {
            ValidateAnswers(model.Answers);

            var entity = new Question
            {
                Id = Guid.NewGuid(),
                Text = model.Text.Trim(),
                QuizId = model.QuizId
            };

            entity.Answers = model.Answers
                .Select(a => new Answer
                {
                    Id = Guid.NewGuid(),
                    Text = a.Text.Trim(),
                    IsCorrect = a.IsCorrect,
                    QuestionId = entity.Id
                })
                .ToHashSet();

            context.Questions.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<QuestionFormDto?> GetForEditAsync(Guid id)
        {
            var q = await context.Questions.AsNoTracking()
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (q == null) return null;

            return new QuestionFormDto
            {
                QuizId = q.QuizId,
                Text = q.Text,
                Answers = q.Answers
                    .OrderBy(a => a.Text)
                    .Select(a => new AnswerFormDto
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    })
                    .ToList()
            };
        }

        public async Task UpdateAsync(Guid id, QuestionFormDto model)
        {
            ValidateAnswers(model.Answers);

            var q = await context.Questions
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (q == null) throw new InvalidOperationException("Question not found.");

            q.Text = model.Text.Trim();

            // replace answers (simple & safe)
            context.Answers.RemoveRange(q.Answers);

            q.Answers = model.Answers.Select(a => new Answer
            {
                Id = Guid.NewGuid(),
                Text = a.Text.Trim(),
                IsCorrect = a.IsCorrect,
                QuestionId = q.Id
            }).ToHashSet();

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var q = await context.Questions.FirstOrDefaultAsync(x => x.Id == id);
            if (q == null) return;

            context.Questions.Remove(q);
            await context.SaveChangesAsync();
        }

        private static void ValidateAnswers(List<AnswerFormDto> answers)
        {
            if (answers == null || answers.Count < 2)
                throw new ValidationException("Трябва да има поне 2 отговора.");

            if (answers.Any(a => string.IsNullOrWhiteSpace(a.Text)))
                throw new ValidationException("Всички отговори трябва да имат текст.");

            var correctCount = answers.Count(a => a.IsCorrect);
            if (correctCount != 1)
                throw new ValidationException("Трябва да има точно 1 правилен отговор.");
        }
    }
}