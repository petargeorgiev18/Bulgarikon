namespace Bulgarikon.Core.DTOs.ImageDTOs
{
    public class ImageEditDto
    {
        public Guid? Id { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public bool Remove { get; set; }
    }
}
