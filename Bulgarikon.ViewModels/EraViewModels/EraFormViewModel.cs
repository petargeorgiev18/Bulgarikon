using Bulgarikon.ViewModels.ImageViewModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Era;

namespace Bulgarikon.ViewModels.EraViewModels
{
    public class EraFormViewModel
    {
        [Required]
        [MaxLength(NameEraMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(StartYearMinValue, StartYearMaxValue)]
        public int StartYear { get; set; }

        [Required]
        [Range(EndYearMinValue, EndYearMaxValue)]
        public int EndYear { get; set; }

        public List<ImageEditViewModel> Images { get; set; } = new();

        public List<IFormFile>? ImageFiles { get; set; }
    }
}
