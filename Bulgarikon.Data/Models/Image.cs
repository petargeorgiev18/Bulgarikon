using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulgarikon.Data.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string Caption { get; set; } = null!;
        public string TargetType { get; set; } = null!; // e.g., "Question", "Answer", etc. // TO DO: Enum
        public int TargetId { get; set; }
    }

}
