namespace Bulgarikon.Core.DTOs.EventDTOs
{
    public class EventViewDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;

        public DateTime? Date { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }

        public string? Location { get; set; }

        public Guid EraId { get; set; }
        public string EraName { get; set; } = null!;
    }
}