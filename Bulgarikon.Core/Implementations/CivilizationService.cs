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
            return await civilizations
                .Query()
                .Where(c => c.EraId == eraId)
                .Include(c => c.Era)
                .OrderBy(c => c.StartYear)
                .Select(c => new CivilizationViewDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type,
                    StartYear = c.StartYear,
                    EndYear = c.EndYear,
                    EraId = c.EraId,
                    EraName = c.Era.Name,
                    ImageUrl = c.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<CivilizationViewDto?> GetDetailsAsync(Guid id)
        {
            return await civilizations
                .Query()
                .Where(c => c.Id == id)
                .Include(c => c.Era)
                .Select(c => new CivilizationViewDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type,
                    StartYear = c.StartYear,
                    EndYear = c.EndYear,
                    EraId = c.EraId,
                    EraName = c.Era.Name,
                    ImageUrl = c.ImageUrl
                })
                .FirstOrDefaultAsync();
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
                ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl) ? null : model.ImageUrl.Trim()
            };

            await civilizations.AddAsync(entity);
            await civilizations.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<CivilizationFormDto?> GetForEditAsync(Guid id)
        {
            return await civilizations
                .Query()
                .Where(c => c.Id == id)
                .Select(c => new CivilizationFormDto
                {
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type,
                    StartYear = c.StartYear,
                    EndYear = c.EndYear,
                    EraId = c.EraId,
                    ImageUrl = c.ImageUrl
                })
                .FirstOrDefaultAsync();
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
            c.ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl) ? null : model.ImageUrl.Trim();

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