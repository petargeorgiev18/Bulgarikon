using Bulgarikon.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Civilization;

namespace Bulgarikon.Core.DTOs.CivilizaionDTOs
{
    public class CivilizationFormDto
    {
        [Required]
        [MaxLength(NameCivilizationMaxLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public CivilizationType Type { get; set; }

        [Required]
        [Range(StartYearMinValue, StartYearMaxValue)]
        public int StartYear { get; set; }

        [Required]
        [Range(EndYearMinValue, EndYearMaxValue)]
        public int EndYear { get; set; }

        [Required]
        public Guid EraId { get; set; }

        public string? ImageUrl { get; set; }
    }
}
