using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Channels;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.QuizResult;

namespace Bulgarikon.Data.Models
{
    public class QuizResult
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public BulgarikonUser User { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Quiz))]
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        [Range(ScoreMinValue, ScoreMaxValue)]
        public int Score { get; set; }
        public DateTime DateTaken { get; set; } = DateTime.UtcNow;
    }
}