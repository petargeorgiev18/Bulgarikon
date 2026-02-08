using Bulgarikon.Core.DTOs.QuizDTOs;

namespace Bulgarikon.Core.Interfaces
{
    public interface IQuizResultService
    {
        Task<Guid> SaveResultAsync(Guid quizId, Guid userId, int score);
        Task<QuizResultViewDto?> GetResultAsync(Guid resultId, Guid userId, bool isAdmin);
        Task<IEnumerable<QuizResultListItemDto>> MyResultsAsync(Guid userId);
        Task<int> CountCorrectAsync(Guid quizId, IEnumerable<Guid> chosenAnswerIds);
    }
}