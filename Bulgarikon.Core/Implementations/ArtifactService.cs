using Bulgarikon.Core.DTOs.ArtifactDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Core.Implementations
{
    public class ArtifactService : IArtifactService
    {
        private readonly IRepository<Artifact, Guid> artifacts;
        private readonly BulgarikonDbContext context;
        private readonly ICloudinaryService cloudinaryService;

        public ArtifactService(
            IRepository<Artifact, Guid> artifacts,
            BulgarikonDbContext context,
            ICloudinaryService cloudinaryService)
        {
            this.artifacts = artifacts;
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<ArtifactViewDto>> GetByEraAsync(Guid? eraId, Guid? civilizationId = null)
        {
            IQueryable<Artifact> q = artifacts.Query()
                .Include(a => a.Era)
                .Include(a => a.Civilization)
                .Include(a => a.Images);

            if (eraId.HasValue)
                q = q.Where(a => a.EraId == eraId.Value);

            if (civilizationId.HasValue)
                q = q.Where(a => a.CivilizationId == civilizationId.Value);

            return await q
                .OrderBy(a => a.Year ?? int.MaxValue)
                .ThenBy(a => a.Name)
                .Select(a => new ArtifactViewDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Year = a.Year,
                    Material = a.Material,
                    Location = a.Location,
                    EraId = a.EraId,
                    EraName = a.Era.Name,
                    CivilizationId = a.CivilizationId,
                    CivilizationName = a.Civilization != null ? a.Civilization.Name : null,
                    ImageUrl = a.Images
                        .Where(i => i.TargetType == ImageTargetType.Artifact && i.ArtifactId == a.Id)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.Url)
                        .FirstOrDefault() ?? a.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<ArtifactDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await artifacts.Query()
                .Include(a => a.Era)
                .Include(a => a.Civilization)
                .Include(a => a.Images)
                .Where(a => a.Id == id)
                .Select(a => new ArtifactDetailsDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Material = a.Material,
                    Location = a.Location,
                    Year = a.Year,
                    DiscoveredAt = a.DiscoveredAt,
                    EraId = a.EraId,
                    EraName = a.Era.Name,
                    CivilizationId = a.CivilizationId,
                    CivilizationName = a.Civilization != null ? a.Civilization.Name : null,
                    ImageUrl = a.Images
                        .Where(i => i.TargetType == ImageTargetType.Artifact && i.ArtifactId == a.Id)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.Url)
                        .FirstOrDefault() ?? a.ImageUrl
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> CreateAsync(ArtifactFormDto model)
        {
            Validate(model);

            var entity = new Artifact
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Description = model.Description.Trim(),
                Year = model.Year,
                Material = model.Material.Trim(),
                Location = model.Location.Trim(),
                DiscoveredAt = model.DiscoveredAt,
                EraId = model.EraId,
                CivilizationId = model.CivilizationId
            };

            await artifacts.AddAsync(entity);

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var upload = await cloudinaryService.UploadImageAsync(model.ImageFile);

                await context.Images.AddAsync(new Image
                {
                    Id = Guid.NewGuid(),
                    Url = upload.Url,
                    PublicId = upload.PublicId,
                    Caption = entity.Name,
                    SortOrder = 0,
                    TargetType = ImageTargetType.Artifact,
                    ArtifactId = entity.Id
                });

                entity.ImageUrl = upload.Url;
            }
            else if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                var upload = await cloudinaryService.UploadImageFromUrlAsync(model.ImageUrl.Trim());

                await context.Images.AddAsync(new Image
                {
                    Id = Guid.NewGuid(),
                    Url = upload.Url,
                    PublicId = upload.PublicId,
                    Caption = entity.Name,
                    SortOrder = 0,
                    TargetType = ImageTargetType.Artifact,
                    ArtifactId = entity.Id
                });

                entity.ImageUrl = upload.Url;
            }
            else
            {
                entity.ImageUrl = null;
            }

            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<ArtifactFormDto?> GetForEditAsync(Guid id)
        {
            var a = await artifacts.Query()
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (a == null) return null;

            var mainImageUrl = a.Images
                .Where(i => i.TargetType == ImageTargetType.Artifact && i.ArtifactId == a.Id)
                .OrderBy(i => i.SortOrder)
                .Select(i => i.Url)
                .FirstOrDefault() ?? a.ImageUrl;

            return new ArtifactFormDto
            {
                Name = a.Name,
                Description = a.Description,
                Year = a.Year,
                Material = a.Material,
                Location = a.Location,
                DiscoveredAt = a.DiscoveredAt,
                EraId = a.EraId,
                CivilizationId = a.CivilizationId,
                ImageUrl = mainImageUrl
            };
        }

        public async Task UpdateAsync(Guid id, ArtifactFormDto model)
        {
            Validate(model);

            var a = await context.Artifacts
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (a == null)
                throw new InvalidOperationException("Artifact not found.");

            a.Name = model.Name.Trim();
            a.Description = model.Description.Trim();
            a.Year = model.Year;
            a.Material = model.Material.Trim();
            a.Location = model.Location.Trim();
            a.DiscoveredAt = model.DiscoveredAt;
            a.EraId = model.EraId;
            a.CivilizationId = model.CivilizationId;

            var existingImages = a.Images
                .Where(i => i.TargetType == ImageTargetType.Artifact && i.ArtifactId == a.Id)
                .ToList();

            foreach (var img in existingImages)
            {
                if (!string.IsNullOrWhiteSpace(img.PublicId))
                    await cloudinaryService.DeleteImageAsync(img.PublicId);
            }

            if (existingImages.Any())
                context.Images.RemoveRange(existingImages);

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var upload = await cloudinaryService.UploadImageAsync(model.ImageFile);

                await context.Images.AddAsync(new Image
                {
                    Id = Guid.NewGuid(),
                    Url = upload.Url,
                    PublicId = upload.PublicId,
                    Caption = a.Name,
                    SortOrder = 0,
                    TargetType = ImageTargetType.Artifact,
                    ArtifactId = a.Id
                });

                a.ImageUrl = upload.Url;
            }
            else if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                var upload = await cloudinaryService.UploadImageFromUrlAsync(model.ImageUrl.Trim());

                await context.Images.AddAsync(new Image
                {
                    Id = Guid.NewGuid(),
                    Url = upload.Url,
                    PublicId = upload.PublicId,
                    Caption = a.Name,
                    SortOrder = 0,
                    TargetType = ImageTargetType.Artifact,
                    ArtifactId = a.Id
                });

                a.ImageUrl = upload.Url;
            }
            else
            {
                a.ImageUrl = null;
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var a = await context.Artifacts
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (a == null) return;

            var artifactImages = a.Images
                .Where(i => i.TargetType == ImageTargetType.Artifact && i.ArtifactId == a.Id)
                .ToList();

            foreach (var img in artifactImages)
            {
                if (!string.IsNullOrWhiteSpace(img.PublicId))
                    await cloudinaryService.DeleteImageAsync(img.PublicId);
            }

            if (artifactImages.Any())
                context.Images.RemoveRange(artifactImages);

            artifacts.Delete(a);
            await context.SaveChangesAsync();
        }

        private static void Validate(ArtifactFormDto m)
        {
            if (m.DiscoveredAt.Date > DateTime.Today)
                throw new ValidationException("Датата на откриване не може да е в бъдещето.");
        }
    }
}