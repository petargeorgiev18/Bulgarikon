using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulgarikon.Data.Models
{
    public class Figure
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [ForeignKey(nameof(Era))]
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Civilization))]
        public int? CivilizationId { get; set; }
        public Civilization? Civilization { get; set; }
        public ICollection<EventFigure> EventFigures { get; set; }
            = new HashSet<EventFigure>();
    }
}