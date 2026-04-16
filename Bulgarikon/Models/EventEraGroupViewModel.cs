using Bulgarikon.Core.DTOs.EventDTOs;
using Bulgarikon.ViewModels.EventViewModels;

namespace Bulgarikon.Models
{
    public class EventEraGroupViewModel
    {
        public Guid EraId { get; set; }
        public string EraName { get; set; } = "";
        public int EraStartYear { get; set; }
        public int EraEndYear { get; set; }
        public List<EventViewViewModel> Events { get; set; } = new();
    }
}
