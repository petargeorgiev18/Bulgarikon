using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Event;

namespace Bulgarikon.Data.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;
        [MaxLength(LocationMaxLength)]
        public string? Location { get; set; }
        [Range(StartYearMinValue, StartYearMaxValue)]
        public int? StartYear { get; set; }
        [Range(EndYearMinValue, EndYearMaxValue)]
        public int? EndYear { get; set; }
        [Required]
        [ForeignKey(nameof(Era))]
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Civilization))]
        public int? CivilizationId { get; set; }
        public Civilization Civilization { get; set; } = null!;
        public ICollection<EventFigure> EventFigures { get; set; }
            = new HashSet<EventFigure>();
        public ICollection<EventCivilization> EventCivilizations { get; set; } 
            = new HashSet<EventCivilization>();
        public ICollection<Image> Images { get; set; }
            = new HashSet<Image>();
    }
}