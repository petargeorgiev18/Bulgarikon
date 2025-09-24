using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulgarikon.Data.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Location { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
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
    }
}