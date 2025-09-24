using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulgarikon.Data.Models.Enums;

namespace Bulgarikon.Data.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public ImageTargetType TargetType { get; set; }
    }
}
