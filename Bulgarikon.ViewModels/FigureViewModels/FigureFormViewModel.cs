using Bulgarikon.ViewModels.ImageViewModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Figure;

namespace Bulgarikon.ViewModels.FigureViewModels
{
    public class FigureFormViewModel
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
        public List<ImageEditViewModel> Images { get; set; } = new();
        public List<IFormFile>? ImageFiles { get; set; }
    }
}