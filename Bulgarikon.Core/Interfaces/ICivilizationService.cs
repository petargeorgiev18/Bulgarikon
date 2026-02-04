using Bulgarikon.Core.DTOs.CivilizaionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulgarikon.Core.Interfaces
{
    public interface ICivilizationService
    {
        Task<IEnumerable<CivilizationViewDto>> GetByEraAsync(Guid eraId);
        Task<CivilizationViewDto?> GetDetailsAsync(Guid id);
        Task<Guid> CreateAsync(CivilizationFormDto model);
        Task<CivilizationFormDto?> GetForEditAsync(Guid id);
        Task UpdateAsync(Guid id, CivilizationFormDto model);
        Task DeleteAsync(Guid id);
    }
}
