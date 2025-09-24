using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulgarikon.Data.Models
{
    public class Artifact
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int? Year { get; set; }
        public string Material { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime DiscoveredAt { get; set; }
        [Required]
        [ForeignKey(nameof(Era))]
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Civilization))]
        public int? CivilizationId { get; set; }
        public Civilization Civilization { get; set; } = null!;
        public ICollection<Image> Images { get; set; } 
            = new HashSet<Image>();
    }
}