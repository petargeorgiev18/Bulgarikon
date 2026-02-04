using Bulgarikon.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulgarikon.Core.DTOs.CivilizaionDTOs
{
    public class CivilizationViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CivilizationType Type { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public Guid EraId { get; set; }
        public string EraName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
