using Bulgarikon.Core.DTOs.FigureDTOs;
using Bulgarikon.Core.DTOs.ImageDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Core.Implementations
{
    public class FigureService : IFigureService
    {
        private readonly IRepository<Figure, Guid> figures;
        private readonly BulgarikonDbContext context;
        private readonly ICloudinaryService cloudinaryService;

        public FigureService(
            IRepository<Figure, Guid> figures,
            BulgarikonDbContext context,
            ICloudinaryService cloudinaryService)
        {
            this.figures = figures;
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<FigureViewDto>> GetByEraAsync(Guid? eraId, Guid? civilizationId = null)
        {
            var q = context.Figures
                .AsNoTracking()
                .Include(f => f.Era)
                .Include(f => f.Civilization)
                .Include(f => f.Images)
                .AsQueryable();

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
                    EraId = f.EraId,
                    EraName = f.Era.Name,
                    CivilizationId = f.CivilizationId,
                    CivilizationName = f.Civilization != null ? f.Civilization.Name : null,
                    Images = f.Images
                        .Where(i => i.TargetType == ImageTargetType.Figure && i.FigureId == f.Id)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => new ImageViewDto
                        {
                            Id = i.Id,
                            Url = i.Url,
                            Caption = i.Caption
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<FigureViewDto?> GetDetailsAsync(Guid id)
        {
            var f = await context.Figures
                .AsNoTracking()
                .Include(x => x.Era)
                .Include(x => x.Civilization)
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (f == null) return null;

            return new FigureViewDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                BirthDate = f.BirthDate,
                DeathDate = f.DeathDate,
                BirthYear = f.BirthYear,
                DeathYear = f.DeathYear,
                EraId = f.EraId,
                EraName = f.Era.Name,
                CivilizationId = f.CivilizationId,
                CivilizationName = f.Civilization != null ? f.Civilization.Name : null,
                Images = f.Images
                    .Where(i => i.TargetType == ImageTargetType.Figure && i.FigureId == f.Id)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new ImageViewDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        Caption = i.Caption
                    })
                    .ToList()
            };
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
                EraId = model.EraId,
                CivilizationId = model.CivilizationId
            };

            await context.Figures.AddAsync(entity);

            int sortOrder = 0;

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                foreach (var file in model.ImageFiles.Where(f => f != null && f.Length > 0))
                {
                    var uploadResult = await cloudinaryService.UploadImageAsync(file);

                    await context.Images.AddAsync(new Image
                    {
                        Id = Guid.NewGuid(),
                        TargetType = ImageTargetType.Figure,
                        FigureId = entity.Id,
                        Url = uploadResult.Url,
                        PublicId = uploadResult.PublicId,
                        Caption = null,
                        SortOrder = sortOrder++
                    });
                }
            }

            var toAdd = (model.Images ?? new List<ImageEditDto>())
                .Where(x => !x.Remove)
                .Select(x => new
                {
                    Url = (x.Url ?? "").Trim(),
                    Caption = x.Caption?.Trim()
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Url))
                .Select(x => new Image
                {
                    Id = Guid.NewGuid(),
                    TargetType = ImageTargetType.Figure,
                    FigureId = entity.Id,
                    Url = x.Url,
                    PublicId = null,
                    Caption = string.IsNullOrWhiteSpace(x.Caption) ? null : x.Caption,
                    SortOrder = sortOrder++
                })
                .ToList();

            if (toAdd.Any())
                await context.Images.AddRangeAsync(toAdd);

            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<FigureFormDto?> GetForEditAsync(Guid id)
        {
            var f = await context.Figures
                .AsNoTracking()
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (f == null) return null;

            return new FigureFormDto
            {
                Name = f.Name,
                Description = f.Description,
                BirthDate = f.BirthDate,
                DeathDate = f.DeathDate,
                BirthYear = f.BirthYear,
                DeathYear = f.DeathYear,
                EraId = f.EraId,
                CivilizationId = f.CivilizationId,
                Images = f.Images
                    .Where(i => i.TargetType == ImageTargetType.Figure && i.FigureId == f.Id)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new ImageEditDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        Caption = i.Caption,
                        Remove = false
                    })
                    .ToList()
            };
        }

        public async Task UpdateAsync(Guid id, FigureFormDto model)
        {
            ValidateFigure(model);

            var f = await context.Figures
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (f == null) throw new InvalidOperationException("Figure not found.");

            f.Name = model.Name.Trim();
            f.Description = model.Description.Trim();
            f.BirthDate = model.BirthDate;
            f.DeathDate = model.DeathDate;
            f.BirthYear = model.BirthYear;
            f.DeathYear = model.DeathYear;
            f.EraId = model.EraId;
            f.CivilizationId = model.CivilizationId;

            var incoming = (model.Images ?? new List<ImageEditDto>())
                .Select(x => new ImageEditDto
                {
                    Id = x.Id,
                    Url = (x.Url ?? "").Trim(),
                    Caption = x.Caption?.Trim(),
                    Remove = x.Remove
                })
                .ToList();

            var existing = f.Images
                .Where(i => i.TargetType == ImageTargetType.Figure && i.FigureId == f.Id)
                .OrderBy(i => i.SortOrder)
                .ToList();

            var removeIds = incoming
                .Where(x => x.Remove && x.Id.HasValue)
                .Select(x => x.Id!.Value)
                .ToHashSet();

            if (removeIds.Any())
            {
                var toRemove = existing.Where(img => removeIds.Contains(img.Id)).ToList();

                foreach (var img in toRemove)
                {
                    if (!string.IsNullOrWhiteSpace(img.PublicId))
                    {
                        await cloudinaryService.DeleteImageAsync(img.PublicId);
                    }
                }

                if (toRemove.Any())
                    context.Images.RemoveRange(toRemove);
            }

            foreach (var u in incoming.Where(x => !x.Remove && x.Id.HasValue && !string.IsNullOrWhiteSpace(x.Url)))
            {
                var dbImg = existing.FirstOrDefault(img => img.Id == u.Id!.Value);
                if (dbImg == null) continue;

                dbImg.Url = u.Url;
                dbImg.Caption = string.IsNullOrWhiteSpace(u.Caption) ? null : u.Caption;
                dbImg.TargetType = ImageTargetType.Figure;
                dbImg.FigureId = f.Id;
            }

            int nextSortOrder = existing.Any() ? existing.Max(i => i.SortOrder) + 1 : 0;

            var adds = incoming
                .Where(x => !x.Remove && !x.Id.HasValue && !string.IsNullOrWhiteSpace(x.Url))
                .ToList();

            if (adds.Any())
            {
                await context.Images.AddRangeAsync(adds.Select(a => new Image
                {
                    Id = Guid.NewGuid(),
                    TargetType = ImageTargetType.Figure,
                    FigureId = f.Id,
                    Url = a.Url,
                    PublicId = null,
                    Caption = string.IsNullOrWhiteSpace(a.Caption) ? null : a.Caption,
                    SortOrder = nextSortOrder++
                }));
            }

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                foreach (var file in model.ImageFiles.Where(file => file != null && file.Length > 0))
                {
                    var uploadResult = await cloudinaryService.UploadImageAsync(file);

                    await context.Images.AddAsync(new Image
                    {
                        Id = Guid.NewGuid(),
                        TargetType = ImageTargetType.Figure,
                        FigureId = f.Id,
                        Url = uploadResult.Url,
                        PublicId = uploadResult.PublicId,
                        Caption = null,
                        SortOrder = nextSortOrder++
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var f = await context.Figures
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (f == null) return;

            var figImages = f.Images
                .Where(i => i.TargetType == ImageTargetType.Figure && i.FigureId == f.Id)
                .ToList();

            foreach (var img in figImages)
            {
                if (!string.IsNullOrWhiteSpace(img.PublicId))
                {
                    await cloudinaryService.DeleteImageAsync(img.PublicId);
                }
            }

            if (figImages.Any())
                context.Images.RemoveRange(figImages);

            context.Figures.Remove(f);
            await context.SaveChangesAsync();
        }

        private static void ValidateFigure(FigureFormDto model)
        {
            bool hasBirth = model.BirthDate.HasValue || model.BirthYear.HasValue;
            bool hasDeath = model.DeathDate.HasValue || model.DeathYear.HasValue;

            if (!hasBirth && !hasDeath)
                throw new ValidationException("Личността трябва да има поне година/дата на раждане или година/дата на смърт.");

            if (model.BirthDate.HasValue && model.BirthYear.HasValue &&
                model.BirthDate.Value.Year != model.BirthYear.Value)
                throw new ValidationException("Годината на раждане не съвпада с датата на раждане.");

            if (model.DeathDate.HasValue && model.DeathYear.HasValue &&
                model.DeathDate.Value.Year != model.DeathYear.Value)
                throw new ValidationException("Годината на смърт не съвпада с датата на смърт.");

            int? birthYear = model.BirthDate?.Year ?? model.BirthYear;
            int? deathYear = model.DeathDate?.Year ?? model.DeathYear;

            if (birthYear.HasValue && deathYear.HasValue && deathYear.Value < birthYear.Value)
                throw new ValidationException("Смъртта не може да е преди раждането.");

            if (model.BirthDate.HasValue && model.DeathDate.HasValue &&
                model.DeathDate.Value.Date < model.BirthDate.Value.Date)
                throw new ValidationException("Датата на смърт не може да е преди датата на раждане.");
        }
    }
}