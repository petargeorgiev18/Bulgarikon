using Bulgarikon.Core.DTOs.EventDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event, Guid> eventsRepo;
        private readonly BulgarikonDbContext context;

        public EventService(IRepository<Event, Guid> eventsRepo, BulgarikonDbContext context)
        {
            this.eventsRepo = eventsRepo;
            this.context = context;
        }

        public async Task<IEnumerable<EventViewDto>> GetByEraAsync(Guid eraId)
        {
            var list = await context.Events
                .AsNoTracking()
                .Where(e => e.EraId == eraId)
                .Include(e => e.Era)
                .OrderBy(e => e.StartYear ?? int.MaxValue)
                .ThenBy(e => e.EndYear ?? int.MaxValue)
                .ToListAsync();

            return list.Select(e => new EventViewDto
            {
                Id = e.Id,
                Title = e.Title,
                StartYear = e.StartYear,
                EndYear = e.EndYear,
                Location = e.Location,
                EraId = e.EraId,
                EraName = e.Era.Name
            });
        }

        public async Task<EventDetailsDto?> GetDetailsAsync(Guid id)
        {
            var e = await context.Events
                .AsNoTracking()
                .Include(x => x.Era)
                .Include(x => x.EventCivilizations).ThenInclude(ec => ec.Civilization)
                .Include(x => x.EventFigures).ThenInclude(ef => ef.Figure)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null) return null;

            return new EventDetailsDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                StartYear = e.StartYear,
                EndYear = e.EndYear,
                EraId = e.EraId,
                EraName = e.Era.Name,
                Civilizations = e.EventCivilizations
                    .Select(x => new CivilizationChipDto { Id = x.CivilizationId, Name = x.Civilization.Name })
                    .OrderBy(x => x.Name)
                    .ToList(),
                Figures = e.EventFigures
                    .Select(x => new FigureChipDto { Id = x.FigureId, Name = x.Figure.Name })
                    .OrderBy(x => x.Name)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(EventFormDto model)
        {
            NormalizeAndValidateYears(model);

            var entity = new Event
            {
                Id = Guid.NewGuid(),
                Title = model.Title.Trim(),
                Description = model.Description.Trim(),
                Location = model.Location?.Trim(),
                StartYear = model.StartYear,
                EndYear = model.EndYear,
                EraId = model.EraId
            };

            entity.EventCivilizations = model.CivilizationIds
                .Distinct()
                .Select(cid => new EventCivilization { EventId = entity.Id, CivilizationId = cid })
                .ToHashSet();

            entity.EventFigures = model.FigureIds
                .Distinct()
                .Select(fid => new EventFigure { EventId = entity.Id, FigureId = fid })
                .ToHashSet();

            await context.Events.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<EventFormDto?> GetForEditAsync(Guid id)
        {
            var e = await context.Events
                .AsNoTracking()
                .Include(x => x.EventCivilizations)
                .Include(x => x.EventFigures)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null) return null;

            return new EventFormDto
            {
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                StartYear = e.StartYear,
                EndYear = e.EndYear,
                EraId = e.EraId,
                CivilizationIds = e.EventCivilizations.Select(x => x.CivilizationId).ToList(),
                FigureIds = e.EventFigures.Select(x => x.FigureId).ToList()
            };
        }

        public async Task UpdateAsync(Guid id, EventFormDto model)
        {
            NormalizeAndValidateYears(model);

            var e = await context.Events
                .Include(x => x.EventCivilizations)
                .Include(x => x.EventFigures)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null) throw new InvalidOperationException("Event not found.");

            e.Title = model.Title.Trim();
            e.Description = model.Description.Trim();
            e.Location = model.Location?.Trim();
            e.StartYear = model.StartYear;
            e.EndYear = model.EndYear;
            e.EraId = model.EraId;

            if (e.EventCivilizations.Any())
                context.RemoveRange(e.EventCivilizations);

            if (e.EventFigures.Any())
                context.RemoveRange(e.EventFigures);

            e.EventCivilizations = model.CivilizationIds
                .Distinct()
                .Select(cid => new EventCivilization { EventId = e.Id, CivilizationId = cid })
                .ToHashSet();

            e.EventFigures = model.FigureIds
                .Distinct()
                .Select(fid => new EventFigure { EventId = e.Id, FigureId = fid })
                .ToHashSet();

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var e = await context.Events
                .Include(x => x.EventCivilizations)
                .Include(x => x.EventFigures)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null) return;

            if (e.EventCivilizations.Any()) 
                context.RemoveRange(e.EventCivilizations);
            if (e.EventFigures.Any()) 
                context.RemoveRange(e.EventFigures);

            context.Events.Remove(e);
            await context.SaveChangesAsync();
        }

        // Helper method to ensure consistent validation and normalization of years across Create and Update operations
        private static void NormalizeAndValidateYears(EventFormDto m)
        {
            if (!m.StartYear.HasValue && !m.EndYear.HasValue)
                throw new InvalidOperationException("Трябва да има поне една година.");

            if (m.StartYear.HasValue && !m.EndYear.HasValue)
                m.EndYear = m.StartYear;

            if (!m.StartYear.HasValue && m.EndYear.HasValue)
                m.StartYear = m.EndYear;

            if (m.StartYear > m.EndYear)
                throw new InvalidOperationException("Началната година не може да е след крайната.");
        }
    }
}
