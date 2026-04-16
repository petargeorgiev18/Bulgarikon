using Bulgarikon.Core.DTOs.FigureDTOs;

namespace Bulgarikon.Core.Interfaces
{
    public interface IFigureService
    {
        Task<List<FigureViewDto>> GetByEraAsync(Guid? eraId, Guid? civilizationId);
        Task<FigureViewDto?> GetDetailsAsync(Guid id);
        Task<FigureFormDto?> GetForEditAsync(Guid id);

        Task<Guid> CreateAsync(FigureFormDto model);
        Task UpdateAsync(Guid id, FigureFormDto model);
        Task DeleteAsync(Guid id);
    }
}