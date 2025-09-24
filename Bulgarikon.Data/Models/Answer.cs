using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulgarikon.Data.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public bool IsCorrect { get; set; }
        [Required]
        [ForeignKey(nameof(Question))]  
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
    }
}