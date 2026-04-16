using Bulgarikon.ViewModels.ImageViewModels;

namespace Bulgarikon.ViewModels.FigureViewModels
{
    public class FigureViewViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        public int? BirthYear { get; set; }
        public int? DeathYear { get; set; }

        public Guid EraId { get; set; }
        public string? EraName { get; set; }

        public Guid? CivilizationId { get; set; }
        public string? CivilizationName { get; set; }
        public List<ImageViewViewModel> Images { get; set; } = new();
    }
}