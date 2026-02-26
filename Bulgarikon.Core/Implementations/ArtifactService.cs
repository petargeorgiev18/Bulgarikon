using Bulgarikon.Core.DTOs.ArtifactDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bulgarikon.Core.Implementations
{
    public class ArtifactService : IArtifactService
    {
        private readonly IRepository<Artifact, Guid> artifacts;

        public ArtifactService(IRepository<Artifact, Guid> artifacts)
        {
            this.artifacts = artifacts;
        }

        public async Task<IEnumerable<ArtifactViewDto>> GetByEraAsync(Guid? eraId, Guid? civilizationId = null)
        {
            IQueryable<Artifact> q = artifacts.Query()
                .Include(a => a.Era)
                .Include(a => a.Civilization);

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
                    ImageUrl = a.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<ArtifactDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await artifacts.Query()
                .Include(a => a.Era)
                .Include(a => a.Civilization)
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
                    ImageUrl = a.ImageUrl
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
                CivilizationId = model.CivilizationId,
                ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl)
                    ? null
                    : model.ImageUrl.Trim()
            };

            await artifacts.AddAsync(entity);
            await artifacts.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<ArtifactFormDto?> GetForEditAsync(Guid id)
        {
            var a = await artifacts.Query()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (a == null) return null;

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
                ImageUrl = a.ImageUrl
            };
        }

        public async Task UpdateAsync(Guid id, ArtifactFormDto model)
        {
            Validate(model);

            var a = await artifacts.GetByIdTrackedAsync(id);
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
            a.ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl)
                ? null
                : model.ImageUrl.Trim();

            await artifacts.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var a = await artifacts.GetByIdTrackedAsync(id);
            if (a == null) return;

            artifacts.Delete(a);
            await artifacts.SaveChangesAsync();
        }

        private static void Validate(ArtifactFormDto m)
        {
            if (m.DiscoveredAt.Date > DateTime.Today)
                throw new ValidationException("Датата на откриване не може да е в бъдещето.");
        }
    }
}