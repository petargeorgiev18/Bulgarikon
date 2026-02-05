using Bulgarikon.Core.DTOs.FigureDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class FigureService : IFigureService
    {
        private readonly IRepository<Figure, Guid> figures;

        public FigureService(IRepository<Figure, Guid> figures)
        {
            this.figures = figures;
        }

        public async Task<IEnumerable<FigureViewDto>> GetByEraAsync(Guid eraId, Guid? civilizationId = null)
        {
            var q = figures.Query().Where(f => f.EraId == eraId);

            if (civilizationId.HasValue)
                q = q.Where(f => f.CivilizationId == civilizationId.Value);

            return await q
                .OrderBy(f => f.Name)
                .Select(f => new FigureViewDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    BirthDate = f.BirthDate,
                    DeathDate = f.DeathDate,
                    BirthYear = f.BirthYear,
                    DeathYear = f.DeathYear,
                    ImageUrl = f.ImageUrl,
                    EraId = f.EraId,
                    EraName = f.Era.Name,
                    CivilizationId = f.CivilizationId,
                    CivilizationName = f.Civilization != null ? f.Civilization.Name : null
                })
                .ToListAsync();
        }

        public async Task<FigureViewDto?> GetDetailsAsync(Guid id)
        {
            return await figures.Query()
                .Where(f => f.Id == id)
                .Select(f => new FigureViewDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    BirthDate = f.BirthDate,
                    DeathDate = f.DeathDate,
                    BirthYear = f.BirthYear,
                    DeathYear = f.DeathYear,
                    ImageUrl = f.ImageUrl,
                    EraId = f.EraId,
                    EraName = f.Era.Name,
                    CivilizationId = f.CivilizationId,
                    CivilizationName = f.Civilization != null ? f.Civilization.Name : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> CreateAsync(FigureFormDto model)
        {
            var entity = new Figure
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Description = model.Description.Trim(),
                BirthDate = model.BirthDate,
                DeathDate = model.DeathDate,
                BirthYear = model.BirthYear,
                DeathYear = model.DeathYear,
                ImageUrl = model.ImageUrl,
                EraId = model.EraId,
                CivilizationId = model.CivilizationId
            };

            await figures.AddAsync(entity);
            await figures.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<FigureFormDto?> GetForEditAsync(Guid id)
        {
            var f = await figures.GetByIdAsync(id);
            if (f == null) return null;

            return new FigureFormDto
            {
                Name = f.Name,
                Description = f.Description,
                BirthDate = f.BirthDate,
                DeathDate = f.DeathDate,
                BirthYear = f.BirthYear,
                DeathYear = f.DeathYear,
                ImageUrl = f.ImageUrl,
                EraId = f.EraId,
                CivilizationId = f.CivilizationId
            };
        }

        public async Task UpdateAsync(Guid id, FigureFormDto model)
        {
            var f = await figures.GetByIdTrackedAsync(id);
            if (f == null) throw new InvalidOperationException("Figure not found.");

            f.Name = model.Name.Trim();
            f.Description = model.Description.Trim();
            f.BirthDate = model.BirthDate;
            f.DeathDate = model.DeathDate;
            f.BirthYear = model.BirthYear;
            f.DeathYear = model.DeathYear;
            f.ImageUrl = model.ImageUrl;
            f.EraId = model.EraId;
            f.CivilizationId = model.CivilizationId;

            await figures.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var f = await figures.GetByIdTrackedAsync(id);
            if (f == null) return;

            figures.Delete(f);
            await figures.SaveChangesAsync();
        }
    }
}