using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bulgarikon.Data.Models.Enums;

namespace Bulgarikon.Data.Models
{
    public class Civilization
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<Artifact> Artifacts { get; set; } = null!;
        public CivilizationType Type { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [ForeignKey(nameof(Era))]
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        public ICollection<Figure> Figures { get; set; } 
            = new HashSet<Figure>();
        public ICollection<EventCivilization> EventCivilizations { get; set; }
            = new HashSet<EventCivilization>();
    }
}