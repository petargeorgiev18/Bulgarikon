using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulgarikon.Data.Models
{
    public class QuizResult
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public BulgarikonUser User { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Quiz))]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public int Score { get; set; }
        public DateTime DateTaken { get; set; }
    }
}
