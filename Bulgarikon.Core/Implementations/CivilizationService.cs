using Bulgarikon.Core.DTOs.CivilizaionDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class CivilizationService : ICivilizationService
    {
        private readonly IRepository<Civilization, Guid> civilizations;

        public CivilizationService(IRepository<Civilization, Guid> civilizations)
        {
            this.civilizations = civilizations;
        }

        public async Task<IEnumerable<CivilizationViewDto>> GetByEraAsync(Guid eraId)
        {
            var list = await civilizations
                .Query()
                .Where(c => c.EraId == eraId)
                .Include(c => c.Era)
                .OrderBy(c => c.StartYear)
                .ToListAsync();

            return list.Select(c => new CivilizationViewDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Type = c.Type,
                StartYear = c.StartYear,
                EndYear = c.EndYear,
                EraId = c.EraId,
                EraName = c.Era?.Name ?? "",
                ImageUrl = c.ImageUrl
            });
        }

        public async Task<CivilizationViewDto?> GetDetailsAsync(Guid id)
        {
            var c = await civilizations
                .Query()
                .Include(x => x.Era)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null) return null;

            return new CivilizationViewDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Type = c.Type,
                StartYear = c.StartYear,
                EndYear = c.EndYear,
                EraId = c.EraId,
                EraName = c.Era?.Name ?? "",
                ImageUrl = c.ImageUrl
            };
        }

        public async Task<Guid> CreateAsync(CivilizationFormDto model)
        {
            var entity = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Description = model.Description.Trim(),
                Type = model.Type,
                StartYear = model.StartYear,
                EndYear = model.EndYear,
                EraId = model.EraId,
                ImageUrl = model.ImageUrl
            };

            await civilizations.AddAsync(entity);
            await civilizations.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<CivilizationFormDto?> GetForEditAsync(Guid id)
        {
            var c = await civilizations.GetByIdAsync(id);
            if (c == null) return null;

            return new CivilizationFormDto
            {
                Name = c.Name,
                Description = c.Description,
                Type = c.Type,
                StartYear = c.StartYear,
                EndYear = c.EndYear,
                EraId = c.EraId,
                ImageUrl = c.ImageUrl
            };
        }

        public async Task UpdateAsync(Guid id, CivilizationFormDto model)
        {
            var c = await civilizations.GetByIdTrackedAsync(id);
            if (c == null) throw new InvalidOperationException("Civilization not found.");

            c.Name = model.Name.Trim();
            c.Description = model.Description.Trim();
            c.Type = model.Type;
            c.StartYear = model.StartYear;
            c.EndYear = model.EndYear;
            c.EraId = model.EraId;
            c.ImageUrl = model.ImageUrl;

            await civilizations.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var c = await civilizations.GetByIdTrackedAsync(id);
            if (c == null) return;

            civilizations.Delete(c);
            await civilizations.SaveChangesAsync();
        }
    }
}