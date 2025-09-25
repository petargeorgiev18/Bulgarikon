using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Answer;

namespace Bulgarikon.Data.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;
        [Required]
        public bool IsCorrect { get; set; }
        [Required]
        [ForeignKey(nameof(Question))]  
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
    }
}