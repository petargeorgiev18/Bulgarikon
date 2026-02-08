using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Artifact;

namespace Bulgarikon.Core.DTOs.ArtifactDTOs
{
    public class ArtifactFormDto
    {
        [Required]
        [MaxLength(NameArtifactMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Range(YearMinValue, YearMaxValue)]
        public int? Year { get; set; }

        [Required]
        [MaxLength(MaterialMaxLength)]
        public string Material { get; set; } = null!;

        [Required]
        [MaxLength(LocationMaxLength)]
        public string Location { get; set; } = null!;

        [Required]
        public DateTime DiscoveredAt { get; set; } = DateTime.Today;

        [Required]
        public Guid EraId { get; set; }

        public Guid? CivilizationId { get; set; }
    }
}