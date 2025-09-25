using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulgarikon.Data.Models.Enums;

namespace Bulgarikon.Data.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public ImageTargetType TargetType { get; set; }
    }
}
