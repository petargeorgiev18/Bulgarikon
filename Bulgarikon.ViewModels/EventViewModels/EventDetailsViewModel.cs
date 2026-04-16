using Bulgarikon.ViewModels.ImageViewModels;

namespace Bulgarikon.ViewModels.EventViewModels
{
    public class EventDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Location { get; set; }

        public int? StartYear { get; set; }
        public int? EndYear { get; set; }

        public Guid EraId { get; set; }
        public string EraName { get; set; } = null!;

        public List<CivilizationChipViewModel> Civilizations { get; set; } = new();
        public List<FigureChipViewModel> Figures { get; set; } = new();

        public List<ImageViewViewModel> Images { get; set; } = new();
    }
}