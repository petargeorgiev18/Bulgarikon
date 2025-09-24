using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulgarikon.Data.Models
{
    public class EventCivilization
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Civilization))]
        public int CivilizationId { get; set; }
        public Civilization Civilization { get; set; } = null!;
    }
}