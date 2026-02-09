using Bulgarikon.Core.DTOs.AnswerDTOs;
using Bulgarikon.Core.DTOs.QuestionDTOs;
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
            NormalizeAnswersFromRadio(model);

            var entity = new Question
            {
                Id = Guid.NewGuid(),
                Text = model.Text.Trim(),
                QuizId = model.QuizId,
                Answers = model.Answers.Select(a => new Answer
                {
                    Id = Guid.NewGuid(),
                    Text = a.Text.Trim(),
                    IsCorrect = a.IsCorrect
                }).ToHashSet()
            };

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

            var answers = q.Answers
                .OrderBy(a => a.Id)
                .Select(a => new AnswerFormDto
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                })
                .ToList();

            while (answers.Count < 4) answers.Add(new AnswerFormDto());
            if (answers.Count > 4) answers = answers.Take(4).ToList();

            var correctIndex = answers.FindIndex(a => a.IsCorrect);
            if (correctIndex < 0) correctIndex = 0;

            return new QuestionFormDto
            {
                QuizId = q.QuizId,
                Text = q.Text,
                Answers = answers,
                CorrectAnswerIndex = correctIndex
            };
        }

        public async Task UpdateAsync(Guid id, QuestionFormDto model)
        {
            NormalizeAnswersFromRadio(model);

            var q = await context.Questions
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (q == null) throw new InvalidOperationException("Question not found.");

            q.Text = model.Text.Trim();

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

        // Helper method to ensure exactly 4 answers and only one correct answer for radio-type questions
        private static void NormalizeAnswersFromRadio(QuestionFormDto model)
        {
            model.Answers = (model.Answers ?? new List<AnswerFormDto>())
                .Take(4)
                .ToList();

            while (model.Answers.Count < 4)
                model.Answers.Add(new AnswerFormDto());

            for (int i = 0; i < model.Answers.Count; i++)
                model.Answers[i].IsCorrect = (i == model.CorrectAnswerIndex);

            ValidateAnswers(model.Answers);
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