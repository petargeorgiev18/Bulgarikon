namespace Bulgarikon.Core.DTOs.EraDTOs
{
    public class EraViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
    }
}