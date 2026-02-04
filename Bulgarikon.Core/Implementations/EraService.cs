using Bulgarikon.Core.DTOs;
using Bulgarikon.Core.DTOs.EraDTOs;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class EraService : IEraService
    {
        private readonly IRepository<Era, Guid> eraRepository;
        public EraService(IRepository<Era, Guid> eraRepository)
        {
            this.eraRepository = eraRepository;
        }

        public async Task<IEnumerable<EraViewDto>> GetAllAsync()
        {
            return await eraRepository.Query()
                .Select(e => new EraViewDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    StartYear = e.StartYear,
                    EndYear = e.EndYear
                })
                .ToListAsync();
        }

        public async Task<EraViewDto?> GetByIdAsync(Guid id)
        {
            Era? era = await eraRepository.GetByIdAsync(id);

            if (era == null)
                throw new KeyNotFoundException("Era not found.");

            return new EraViewDto
            {
                Id = era.Id,
                Name = era.Name,
                Description = era.Description,
                StartYear = era.StartYear,
                EndYear = era.EndYear
            };
        }

        public async Task CreateAsync(EraFormDto model)
        {
            if (model.EndYear < model.StartYear)
                throw new InvalidOperationException("End year cannot be before start year.");

            Era era = new Era
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                StartYear = model.StartYear,
                EndYear = model.EndYear
            };

            await eraRepository.AddAsync(era);
            await eraRepository.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var era = await eraRepository.GetByIdTrackedAsync(id);

            if (era == null)
                throw new KeyNotFoundException("Era not found.");

            eraRepository.Delete(era);
            await eraRepository.SaveChangesAsync();
        }

        public async Task EditAsync(Guid id, EraFormDto model)
        {
            if (model.EndYear < model.StartYear)
                throw new InvalidOperationException("End year cannot be before start year.");

            Era? era = await eraRepository.GetByIdTrackedAsync(id);
            if (era == null)
                throw new KeyNotFoundException("Era not found.");

            era.Name = model.Name;
            era.Description = model.Description;
            era.StartYear = model.StartYear;
            era.EndYear = model.EndYear;

            await eraRepository.SaveChangesAsync();
        }
    }
}