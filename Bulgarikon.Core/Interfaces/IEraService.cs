using Bulgarikon.Core.DTOs.EraDTOs;

namespace Bulgarikon.Core.DTOs
{
    public interface IEraService
    {
        Task<IEnumerable<EraViewDto>> GetAllAsync();
        Task<EraViewDto?> GetByIdAsync(Guid id);
        Task CreateAsync(EraFormDto model);
        Task EditAsync(Guid id, EraFormDto model);
        Task Delete(Guid id);
    }
}
