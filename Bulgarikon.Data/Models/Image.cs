using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulgarikon.Data.Models.Enums;

namespace Bulgarikon.Data.Models
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public ImageTargetType TargetType { get; set; }
        [ForeignKey(nameof(Figure))]
        public Guid? FigureId { get; set; }
        public Figure Figure { get; set; } = null!;
        [ForeignKey(nameof(Event))]
        public Guid? EventId { get; set; }
        public Event Event { get; set; } = null!;
        [ForeignKey(nameof(Era))]
        public Guid? EraId { get; set; }
        public Era Era { get; set; } = null!;
        [ForeignKey(nameof(Civilization))]
        public Guid? CivilizationId { get; set; }
        public Civilization Civilization { get; set; } = null!;
    }
}
