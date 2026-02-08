using Bulgarikon.Core.DTOs.QuizDTOs;

namespace Bulgarikon.Core.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizViewDto>> GetByEraAsync(Guid? eraId);
        Task<QuizDetailsDto?> GetDetailsAsync(Guid id);
        Task<Guid> CreateAsync(QuizFormDto model);
        Task<QuizFormDto?> GetForEditAsync(Guid id);
        Task UpdateAsync(Guid id, QuizFormDto model);
        Task DeleteAsync(Guid id);
        Task<QuizTakeDto?> GetForTakeAsync(Guid id);
        Task<Guid> SubmitAsync(QuizSubmitDto model, Guid userId);
        Task<QuizResultViewDto?> GetResultAsync(Guid resultId, Guid userId);
        Task<IEnumerable<QuizResultListItemDto>> GetMyResultsAsync(Guid userId);
        Task<(int score, int maxScore)> EvaluateAsync(QuizSubmitDto model);
        Task<int> GetQuestionsCountAsync(Guid quizId);
    }
}
