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
            var q = artifacts.Query();

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
                    ImageUrl = a.ImageUrl,
                    EraId = a.EraId,
                    EraName = a.Era.Name,
                    CivilizationId = a.CivilizationId,
                    CivilizationName = a.Civilization != null ? a.Civilization.Name : null
                })
                .ToListAsync();
        }

        public async Task<ArtifactDetailsDto?> GetDetailsAsync(Guid id)
        {
            return await artifacts.Query()
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
                    ImageUrl = a.ImageUrl,
                    EraId = a.EraId,
                    EraName = a.Era.Name,
                    CivilizationId = a.CivilizationId,
                    CivilizationName = a.Civilization != null ? a.Civilization.Name : null
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
                ImageUrl = model.ImageUrl?.Trim(),
                Year = model.Year,
                Material = model.Material.Trim(),
                Location = model.Location.Trim(),
                DiscoveredAt = model.DiscoveredAt,
                EraId = model.EraId,
                CivilizationId = model.CivilizationId
            };

            await artifacts.AddAsync(entity);
            await artifacts.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<ArtifactFormDto?> GetForEditAsync(Guid id)
        {
            var a = await artifacts.GetByIdAsync(id);
            if (a == null) return null;

            return new ArtifactFormDto
            {
                Name = a.Name,
                Description = a.Description,
                ImageUrl = a.ImageUrl,
                Year = a.Year,
                Material = a.Material,
                Location = a.Location,
                DiscoveredAt = a.DiscoveredAt,
                EraId = a.EraId,
                CivilizationId = a.CivilizationId
            };
        }

        public async Task UpdateAsync(Guid id, ArtifactFormDto model)
        {
            Validate(model);

            var a = await artifacts.GetByIdTrackedAsync(id);
            if (a == null) throw new InvalidOperationException("Artifact not found.");

            a.Name = model.Name.Trim();
            a.Description = model.Description.Trim();
            a.ImageUrl = model.ImageUrl?.Trim();
            a.Year = model.Year;
            a.Material = model.Material.Trim();
            a.Location = model.Location.Trim();
            a.DiscoveredAt = model.DiscoveredAt;
            a.EraId = model.EraId;
            a.CivilizationId = model.CivilizationId;

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
            // 1) DiscoveredAt не може да е в бъдещето (добър UX, ако искаш)
            if (m.DiscoveredAt.Date > DateTime.Today)
                throw new ValidationException("Датата на откриване не може да е в бъдещето.");

            // 2) Year е optional -> не го правим required
            // 3) CivilizationId е optional -> ok
        }
    }
}