namespace Bulgarikon.ViewModels.ImageViewModels
{
    public class ImageViewViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
    }
}
