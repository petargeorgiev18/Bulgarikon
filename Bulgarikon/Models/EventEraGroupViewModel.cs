using Bulgarikon.Core.DTOs.EventDTOs;

namespace Bulgarikon.Models
{
    public class EventEraGroupViewModel
    {
        public Guid EraId { get; set; }
        public string EraName { get; set; } = "";
        public int EraStartYear { get; set; }
        public int EraEndYear { get; set; }
        public List<EventViewDto> Events { get; set; } = new();
    }
}
