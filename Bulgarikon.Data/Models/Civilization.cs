using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bulgarikon.Data.Models.Enums;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Civilization;

namespace Bulgarikon.Data.Models
{
    public class Civilization
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(NameCivilizationMaxLength)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;
        [Required]
        public CivilizationType Type { get; set; }
        [Required]
        [Range(StartYearMinValue, StartYearMaxValue)]
        public int StartYear { get; set; }
        [Required]
        [Range(EndYearMinValue, EndYearMaxValue)]
        public int EndYear { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [ForeignKey(nameof(Era))]
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        public ICollection<Artifact> Artifacts { get; set; } = null!;
        public ICollection<Figure> Figures { get; set; } 
            = new HashSet<Figure>();
        public ICollection<EventCivilization> EventCivilizations { get; set; }
            = new HashSet<EventCivilization>();
    }
}