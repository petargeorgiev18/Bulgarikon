using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Event;

namespace Bulgarikon.Core.DTOs.EventDTOs
{
    public class EventFormDto
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

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
        public Guid EraId { get; set; }

        public List<Guid>? CivilizationIds { get; set; } = new();
        public List<Guid>? FigureIds { get; set; } = new();
    }
}