using Bulgarikon.Core.DTOs.CivilizaionDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Core.Implementations
{
    public class CivilizationService : ICivilizationService
    {
        private readonly IRepository<Civilization, Guid> civilizations;
        private readonly IRepository<Image, Guid> images;

        public CivilizationService(
            IRepository<Civilization, Guid> civilizations,
            IRepository<Image, Guid> images)
        {
            this.civilizations = civilizations;
            this.images = images;
        }

        public async Task<IEnumerable<CivilizationViewDto>> GetByEraAsync(Guid? eraId)
        {
            IQueryable<Civilization> q = civilizations.Query()
                .Include(c => c.Era);

            if (eraId.HasValue)
                q = q.Where(c => c.EraId == eraId.Value);

            var list = await q
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
                    EraName = c.Era != null ? c.Era.Name : ""
                })
                .ToListAsync();

            var ids = list.Select(x => x.Id).ToList();

            var imgMap = await images.Query()
                .Where(i => i.TargetType == ImageTargetType.Civilization
                            && i.CivilizationId.HasValue
                            && ids.Contains(i.CivilizationId.Value))
                .GroupBy(i => i.CivilizationId!.Value)
                .Select(g => new
                {
                    CivId = g.Key,
                    Url = g.OrderBy(x => x.Id).Select(x => x.Url).FirstOrDefault()
                })
                .ToDictionaryAsync(x => x.CivId, x => x.Url);

            foreach (var c in list)
                c.ImageUrl = imgMap.TryGetValue(c.Id, out var url) ? url : null;

            return list;
        }

        public async Task<CivilizationViewDto?> GetDetailsAsync(Guid id)
        {
            var c = await civilizations.Query()
                .Include(x => x.Era)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null) return null;

            var imgUrl = await images.Query()
                .Where(i => i.TargetType == ImageTargetType.Civilization
                            && i.CivilizationId == id)
                .OrderBy(i => i.Id)
                .Select(i => i.Url)
                .FirstOrDefaultAsync();

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
                ImageUrl = imgUrl
            };
        }

        public async Task<Guid> CreateAsync(CivilizationFormDto model)
        {
            if (model.EndYear < model.StartYear)
                throw new ValidationException("Крайната година не може да е преди началната.");

            var entity = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Description = model.Description.Trim(),
                Type = model.Type,
                StartYear = model.StartYear,
                EndYear = model.EndYear,
                EraId = model.EraId
            };

            await civilizations.AddAsync(entity);

            var url = model.ImageUrl?.Trim();
            if (!string.IsNullOrWhiteSpace(url))
            {
                await images.AddAsync(new Image
                {
                    Id = Guid.NewGuid(),
                    Url = url,
                    TargetType = ImageTargetType.Civilization,
                    CivilizationId = entity.Id
                });
            }

            await civilizations.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<CivilizationFormDto?> GetForEditAsync(Guid id)
        {
            var c = await civilizations.Query()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null) return null;

            var imgUrl = await images.Query()
                .Where(i => i.TargetType == ImageTargetType.Civilization
                            && i.CivilizationId == id)
                .OrderBy(i => i.Id)
                .Select(i => i.Url)
                .FirstOrDefaultAsync();

            return new CivilizationFormDto
            {
                Name = c.Name,
                Description = c.Description,
                Type = c.Type,
                StartYear = c.StartYear,
                EndYear = c.EndYear,
                EraId = c.EraId,
                ImageUrl = imgUrl
            };
        }

        public async Task UpdateAsync(Guid id, CivilizationFormDto model)
        {
            if (model.EndYear < model.StartYear)
                throw new ValidationException("Крайната година не може да е преди началната.");

            var c = await civilizations.Query()
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null)
                throw new InvalidOperationException("Civilization not found.");

            c.Name = model.Name.Trim();
            c.Description = model.Description.Trim();
            c.Type = model.Type;
            c.StartYear = model.StartYear;
            c.EndYear = model.EndYear;
            c.EraId = model.EraId;

            var url = model.ImageUrl?.Trim();

            var existingImg = await images.Query()
                .AsTracking()
                .Where(i => i.TargetType == ImageTargetType.Civilization
                            && i.CivilizationId == id)
                .OrderBy(i => i.Id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(url))
            {
                if (existingImg != null)
                    images.Delete(existingImg);
            }
            else
            {
                if (existingImg == null)
                {
                    await images.AddAsync(new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = url,
                        TargetType = ImageTargetType.Civilization,
                        CivilizationId = id
                    });
                }
                else
                {
                    existingImg.Url = url;
                    existingImg.TargetType = ImageTargetType.Civilization;
                    existingImg.CivilizationId = id;
                }
            }

            await civilizations.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var imgs = await images.Query()
                .Where(i => i.TargetType == ImageTargetType.Civilization
                            && i.CivilizationId == id)
                .ToListAsync();

            foreach (var img in imgs)
                images.Delete(img);

            var c = await civilizations.GetByIdTrackedAsync(id);
            if (c == null) return;

            civilizations.Delete(c);
            await civilizations.SaveChangesAsync();
        }
    }
}