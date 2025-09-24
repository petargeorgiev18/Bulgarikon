using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulgarikon.Data.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public BulgarikonUser User { get; set; } = null!;
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
