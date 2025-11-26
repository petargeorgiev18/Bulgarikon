using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulgarikon.Data.Models
{
    public class EventCivilization
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey(nameof(Event))]
        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Civilization))]
        public Guid CivilizationId { get; set; }
        public Civilization Civilization { get; set; } = null!;
    }
}