namespace Bulgarikon.Core.DTOs.ImageDTOs
{
    public class ImageViewDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
    }
}
