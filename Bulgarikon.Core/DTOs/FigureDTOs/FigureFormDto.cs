using System.ComponentModel.DataAnnotations;

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

        public int? BirthYear { get; set; }
        public int? DeathYear { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public Guid EraId { get; set; }

        public Guid? CivilizationId { get; set; }
    }

}