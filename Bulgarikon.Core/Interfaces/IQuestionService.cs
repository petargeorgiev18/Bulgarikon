using Bulgarikon.Core.DTOs.QuestionDTOs;

namespace Bulgarikon.Core.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionListItemDto>> GetByQuizAsync(Guid quizId);
        Task<QuestionFormDto?> GetForEditAsync(Guid id);
        Task<Guid> CreateAsync(QuestionFormDto model);
        Task UpdateAsync(Guid id, QuestionFormDto model);
        Task DeleteAsync(Guid id);
    }
}