using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulgarikon.Data.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Era))]
        public int EraId { get; set; }
        public Era Era { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } = new HashSet<Question>();
    }
}