using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Figure;

namespace Bulgarikon.Data.Models
{
    public class Figure
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(NameFigureMaxLength)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string? ImageUrl { get; set; }
        [Range(BirthYearMinValue, BirthYearMaxValue)]
        public int? BirthYear { get; set; }
        [Range(DeathYearMinValue, DeathYearMaxValue)]
        public int? DeathYear { get; set; }
        [Required]
        [ForeignKey(nameof(Era))]
        public Guid EraId { get; set; }
        public Era Era { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Civilization))]
        public Guid? CivilizationId { get; set; }
        public Civilization? Civilization { get; set; }
        public ICollection<EventFigure> EventFigures { get; set; }
            = new HashSet<EventFigure>();
        public ICollection<Image> Images { get; set; }
            = new HashSet<Image>();
    }
}