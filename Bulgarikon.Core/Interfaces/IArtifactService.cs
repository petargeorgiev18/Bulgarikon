using Bulgarikon.Core.DTOs.ArtifactDTOs;

namespace Bulgarikon.Core.Interfaces
{
    public interface IArtifactService
    {
        Task<IEnumerable<ArtifactViewDto>> GetByEraAsync(Guid? eraId, Guid? civilizationId = null);
        Task<ArtifactDetailsDto?> GetDetailsAsync(Guid id);

        Task<Guid> CreateAsync(ArtifactFormDto model);
        Task<ArtifactFormDto?> GetForEditAsync(Guid id);
        Task UpdateAsync(Guid id, ArtifactFormDto model);

        Task DeleteAsync(Guid id);
    }
}