using Bulgarikon.Core.DTOs.FigureDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Core.Implementations
{
    public class FigureService : IFigureService
    {
        private readonly IRepository<Figure, Guid> figures;

        public FigureService(IRepository<Figure, Guid> figures)
        {
            this.figures = figures;
        }

        public async Task<IEnumerable<FigureViewDto>> GetByEraAsync(Guid? eraId, Guid? civilizationId = null)
        {
            var q = figures.Query();

            if (eraId.HasValue)
                q = q.Where(f => f.EraId == eraId.Value);

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
            ValidateFigure(model);

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
            ValidateFigure(model);

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

        // Helper method to validate the figure entity record
        private static void ValidateFigure(FigureFormDto model)
        {
            bool hasBirth = model.BirthDate.HasValue || model.BirthYear.HasValue;
            bool hasDeath = model.DeathDate.HasValue || model.DeathYear.HasValue;

            if (!hasBirth && !hasDeath)
                throw new ValidationException("Личността трябва да има поне година/дата на раждане или година/дата на смърт.");

            if (model.BirthDate.HasValue && model.BirthYear.HasValue &&
                model.BirthDate.Value.Year != model.BirthYear.Value)
            {
                throw new ValidationException("Годината на раждане не съвпада с датата на раждане.");
            }

            if (model.DeathDate.HasValue && model.DeathYear.HasValue &&
                model.DeathDate.Value.Year != model.DeathYear.Value)
            {
                throw new ValidationException("Годината на смърт не съвпада с датата на смърт.");
            }

            int? birthYear = model.BirthDate?.Year ?? model.BirthYear;
            int? deathYear = model.DeathDate?.Year ?? model.DeathYear;

            if (birthYear.HasValue && deathYear.HasValue && deathYear.Value < birthYear.Value)
                throw new ValidationException("Смъртта не може да е преди раждането.");

            if (model.BirthDate.HasValue && model.DeathDate.HasValue &&
                model.DeathDate.Value.Date < model.BirthDate.Value.Date)
            {
                throw new ValidationException("Датата на смърт не може да е преди датата на раждане.");
            }
        }
    }
}