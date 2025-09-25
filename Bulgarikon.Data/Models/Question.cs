using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Question;

namespace Bulgarikon.Data.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Quiz))]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public ICollection<Answer> Answers { get; set; }
            = new HashSet<Answer>();
    }
}