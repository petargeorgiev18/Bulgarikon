using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulgarikon.Data.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //public User User { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
