using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulgarikon.Core.DTOs.Common
{
    public class CloudinaryUploadResultDto
    {
        public string Url { get; set; } = null!;
        public string PublicId { get; set; } = null!;
    }
}
