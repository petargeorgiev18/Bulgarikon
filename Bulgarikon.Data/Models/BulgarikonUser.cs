using Microsoft.AspNetCore.Identity;

namespace Bulgarikon.Data.Models
{
    public class BulgarikonUser : IdentityUser<Guid>
    {
        public ICollection<QuizResult> QuizResults { get; set; }
            = new HashSet<QuizResult>();
        public ICollection<Feedback> FeedbacksCreated { get; set; } 
            = new HashSet<Feedback>();
        public ICollection<Feedback> FeedbacksReplied { get; set; }
            = new HashSet<Feedback>();
    }
}
