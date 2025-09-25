using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Artifact;

namespace Bulgarikon.Data.Models
{
    public class Artifact
    {
        [Key]
        public int Id { get; set; }
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
        public DateTime DiscoveredAt { get; set; }
        [Required]
        [ForeignKey(nameof(Era))]
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Civilization))]
        public int? CivilizationId { get; set; }
        public Civilization Civilization { get; set; } = null!;
        public ICollection<Image> Images { get; set; } 
            = new HashSet<Image>();
    }
}