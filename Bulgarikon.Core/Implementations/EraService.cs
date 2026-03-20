using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.EraDTOs;
using Bulgarikon.Core.DTOs.ImageDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class EraService : IEraService
    {
        private readonly IRepository<Era, Guid> eraRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly BulgarikonDbContext context;

        public EraService(
            IRepository<Era, Guid> eraRepository,
            BulgarikonDbContext context,
            ICloudinaryService cloudinaryService)
        {
            this.eraRepository = eraRepository;
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<EraViewDto>> GetAllAsync()
        {
            var eras = await eraRepository.Query()
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Include(e => e.Images)
                .ToListAsync();

            return eras.Select(e => new EraViewDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartYear = e.StartYear,
                EndYear = e.EndYear,
                Images = e.Images
                    .Where(i => i.TargetType == ImageTargetType.Era && i.EraId == e.Id)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new ImageViewDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        Caption = i.Caption
                    })
                    .ToList()
            });
        }

        public async Task<EraViewDto?> GetByIdAsync(Guid id)
        {
            var era = await eraRepository.Query()
                .AsNoTracking()
                .Where(e => e.Id == id && !e.IsDeleted)
                .Include(e => e.Images)
                .FirstOrDefaultAsync();

            if (era == null)
                throw new KeyNotFoundException("Era not found.");

            return new EraViewDto
            {
                Id = era.Id,
                Name = era.Name,
                Description = era.Description,
                StartYear = era.StartYear,
                EndYear = era.EndYear,
                Images = era.Images
                    .Where(i => i.TargetType == ImageTargetType.Era && i.EraId == era.Id)
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

        public async Task CreateAsync(EraFormDto model)
        {
            if (model.EndYear < model.StartYear)
                throw new InvalidOperationException("End year cannot be before start year.");

            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Description = model.Description?.Trim(),
                StartYear = model.StartYear,
                EndYear = model.EndYear,
                IsDeleted = false
            };

            await eraRepository.AddAsync(era);

            int sortOrder = 0;

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                foreach (var file in model.ImageFiles.Where(f => f.Length > 0))
                {
                    var uploadResult = await cloudinaryService.UploadImageAsync(file);

                    await context.Images.AddAsync(new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = uploadResult.Url,
                        PublicId = uploadResult.PublicId,
                        Caption = null,
                        SortOrder = sortOrder++,
                        TargetType = ImageTargetType.Era,
                        EraId = era.Id
                    });
                }
            }

            var newImages = model.Images
                .Where(x => !x.Id.HasValue && !x.Remove && !string.IsNullOrWhiteSpace(x.Url))
                .Select(x => new Image
                {
                    Id = Guid.NewGuid(),
                    Url = x.Url.Trim(),
                    PublicId = null,
                    Caption = string.IsNullOrWhiteSpace(x.Caption) ? null : x.Caption.Trim(),
                    SortOrder = sortOrder++,
                    TargetType = ImageTargetType.Era,
                    EraId = era.Id
                })
                .ToList();

            if (newImages.Any())
                await context.Images.AddRangeAsync(newImages);

            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var era = await context.Eras
                .FirstOrDefaultAsync(e => e.Id == id);

            if (era == null)
                throw new KeyNotFoundException("Era not found.");

            if (era.IsDeleted)
                return;

            era.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task EditAsync(Guid id, EraFormDto model)
        {
            if (model.EndYear < model.StartYear)
                throw new InvalidOperationException("End year cannot be before start year.");

            var era = await context.Eras
                .Include(e => e.Images)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (era == null)
                throw new KeyNotFoundException("Era not found.");

            era.Name = model.Name.Trim();
            era.Description = model.Description?.Trim();
            era.StartYear = model.StartYear;
            era.EndYear = model.EndYear;

            var existing = era.Images
                .Where(i => i.TargetType == ImageTargetType.Era && i.EraId == era.Id)
                .OrderBy(i => i.SortOrder)
                .ToList();

            var incoming = model.Images ?? new List<ImageEditDto>();

            var removeIds = incoming
                .Where(x => x.Id.HasValue && x.Remove)
                .Select(x => x.Id!.Value)
                .ToHashSet();

            if (removeIds.Any())
            {
                var toRemove = existing.Where(i => removeIds.Contains(i.Id)).ToList();

                foreach (var image in toRemove)
                {
                    if (!string.IsNullOrWhiteSpace(image.PublicId))
                    {
                        await cloudinaryService.DeleteImageAsync(image.PublicId);
                    }
                }

                if (toRemove.Any())
                    context.Images.RemoveRange(toRemove);
            }

            foreach (var dto in incoming.Where(x => x.Id.HasValue && !x.Remove))
            {
                if (string.IsNullOrWhiteSpace(dto.Url))
                    continue;

                var img = existing.FirstOrDefault(i => i.Id == dto.Id!.Value);
                if (img == null)
                    continue;

                img.Url = dto.Url.Trim();
                img.Caption = string.IsNullOrWhiteSpace(dto.Caption) ? null : dto.Caption.Trim();
            }

            int nextSortOrder = existing.Any() ? existing.Max(i => i.SortOrder) + 1 : 0;

            var toAddFromUrls = incoming
                .Where(x => !x.Id.HasValue && !x.Remove && !string.IsNullOrWhiteSpace(x.Url))
                .Select(x => new Image
                {
                    Id = Guid.NewGuid(),
                    Url = x.Url.Trim(),
                    PublicId = null,
                    Caption = string.IsNullOrWhiteSpace(x.Caption) ? null : x.Caption.Trim(),
                    SortOrder = nextSortOrder++,
                    TargetType = ImageTargetType.Era,
                    EraId = era.Id
                })
                .ToList();

            if (toAddFromUrls.Any())
                await context.Images.AddRangeAsync(toAddFromUrls);

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                foreach (var file in model.ImageFiles.Where(f => f.Length > 0))
                {
                    var uploadResult = await cloudinaryService.UploadImageAsync(file);

                    await context.Images.AddAsync(new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = uploadResult.Url,
                        PublicId = uploadResult.PublicId,
                        Caption = null,
                        SortOrder = nextSortOrder++,
                        TargetType = ImageTargetType.Era,
                        EraId = era.Id
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}