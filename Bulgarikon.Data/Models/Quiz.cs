using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Bulgarikon.Common.DataModelsValidation.EntityClassesValidations.Quiz;

namespace Bulgarikon.Data.Models
{
    public class Quiz
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Era))]
        public Guid EraId { get; set; }
        public Era Era { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } 
            = new HashSet<Question>();
    }
}