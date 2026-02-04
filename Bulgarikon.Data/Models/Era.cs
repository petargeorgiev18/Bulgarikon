using System.ComponentModel.DataAnnotations;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Era;

namespace Bulgarikon.Data.Models
{
    public class Era
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(NameEraMaxLength)]
        public string Name { get; set; } = null!;
        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }
        [Required]
        [Range(StartYearMinValue, StartYearMaxValue)]
        public int StartYear { get; set; }
        [Required]
        [Range(EndYearMinValue, EndYearMaxValue)]
        public int EndYear { get; set; }
        public ICollection<Event> Events { get; set; }
            = new HashSet<Event>();
        public ICollection<Civilization> Civilizations { get; set; }
            = new HashSet<Civilization>();
        public ICollection<Figure> Figures { get; set; }
            = new HashSet<Figure>();
        public ICollection<Artifact> Artifacts { get; set; }
            = new HashSet<Artifact>();
        public ICollection<Quiz> Quizzes { get; set; }
            = new HashSet<Quiz>();
        public ICollection<Image> Images { get; set; }
            = new HashSet<Image>();
    }
}
