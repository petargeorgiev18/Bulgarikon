using Bulgarikon.Core.DTOs.ImageDTOs;
using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Figure;

namespace Bulgarikon.Core.DTOs.FigureDTOs
{
    public class FigureFormDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeathDate { get; set; }

        [Range(BirthYearMinValue, BirthYearMaxValue)]
        public int? BirthYear { get; set; }
        [Range(DeathYearMinValue, DeathYearMaxValue)]
        public int? DeathYear { get; set; }

        [Required]
        public Guid EraId { get; set; }

        public Guid? CivilizationId { get; set; }
        public List<ImageEditDto> Images { get; set; } = new();
    }
}