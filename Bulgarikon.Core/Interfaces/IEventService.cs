using Bulgarikon.Core.DTOs.EventDTOs;

namespace Bulgarikon.Core.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventViewDto>> GetByEraAsync(Guid eraId);
        Task<EventDetailsDto?> GetDetailsAsync(Guid id);
        Task<Guid> CreateAsync(EventFormDto model);
        Task<EventFormDto?> GetForEditAsync(Guid id);
        Task UpdateAsync(Guid id, EventFormDto model);
        Task DeleteAsync(Guid id);
    }
}
