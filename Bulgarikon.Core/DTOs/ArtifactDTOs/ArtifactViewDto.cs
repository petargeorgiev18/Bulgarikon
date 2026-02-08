namespace Bulgarikon.Core.DTOs.ArtifactDTOs
{
    public class ArtifactViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? Year { get; set; }
        public string Material { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public Guid EraId { get; set; }
        public string EraName { get; set; } = "";

        public Guid? CivilizationId { get; set; }
        public string? CivilizationName { get; set; }
    }
}