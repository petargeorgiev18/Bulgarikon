using Bulgarikon.Core.DTOs.FigureDTOs;

namespace Bulgarikon.Core.Interfaces
{
    public interface IFigureService
    {
        Task<IEnumerable<FigureViewDto>> GetByEraAsync(Guid? eraId, Guid? civilizationId = null);
        Task<FigureViewDto?> GetDetailsAsync(Guid id);

        Task<Guid> CreateAsync(FigureFormDto model);
        Task<FigureFormDto?> GetForEditAsync(Guid id);
        Task UpdateAsync(Guid id, FigureFormDto model);

        Task DeleteAsync(Guid id);
    }
}