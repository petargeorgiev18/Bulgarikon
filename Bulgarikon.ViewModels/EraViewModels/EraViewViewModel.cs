using Bulgarikon.ViewModels.ImageViewModels;

namespace Bulgarikon.ViewModels.EraViewModels
{
    public class EraViewViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public List<ImageViewViewModel> Images { get; set; } = new();
    }
}