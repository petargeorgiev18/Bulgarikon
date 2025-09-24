using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulgarikon.Data.Models
{
    public class EventFigure
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Figure))]
        public int FigureId { get; set; }
        public Figure Figure { get; set; } = null!;
    }
}