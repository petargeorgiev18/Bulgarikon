using Bulgarikon.Core.DTOs.EventDTOs;
using Bulgarikon.Core.DTOs.ImageDTOs;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Core.Implementations
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event, Guid> eventsRepo;
        private readonly BulgarikonDbContext context;
        private readonly ICloudinaryService cloudinaryService;

        public EventService(
            IRepository<Event, Guid> eventsRepo,
            BulgarikonDbContext context,
            ICloudinaryService cloudinaryService)
        {
            this.eventsRepo = eventsRepo;
            this.context = context;
            this.cloudinaryService = cloudinaryService;
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
                .Include(x => x.Images)
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
                Images = e.Images
                    .Where(i => i.TargetType == ImageTargetType.Event && i.EventId == e.Id)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new ImageViewDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        Caption = i.Caption
                    })
                    .ToList(),
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

            entity.EventCivilizations = (model.CivilizationIds ?? new List<Guid>())
                .Where(x => x != Guid.Empty)
                .Distinct()
                .Select(cid => new EventCivilization { EventId = entity.Id, CivilizationId = cid })
                .ToHashSet();

            entity.EventFigures = (model.FigureIds ?? new List<Guid>())
                .Where(x => x != Guid.Empty)
                .Distinct()
                .Select(fid => new EventFigure { EventId = entity.Id, FigureId = fid })
                .ToHashSet();

            await context.Events.AddAsync(entity);

            int sortOrder = 0;

            if (model.ImageFiles != null)
            {
                foreach (var file in model.ImageFiles.Where(f => f != null && f.Length > 0))
                {
                    var uploadResult = await cloudinaryService.UploadImageAsync(file);

                    await context.Images.AddAsync(new Image
                    {
                        Id = Guid.NewGuid(),
                        TargetType = ImageTargetType.Event,
                        Url = uploadResult.Url,
                        PublicId = uploadResult.PublicId,
                        Caption = null,
                        SortOrder = sortOrder++,
                        EventId = entity.Id
                    });
                }
            }

            if (model.Images != null)
            {
                foreach (var img in model.Images.Where(x => !x.Remove && !string.IsNullOrWhiteSpace(x.Url)))
                {
                    var upload = await cloudinaryService.UploadImageFromUrlAsync(img.Url!);

                    await context.Images.AddAsync(new Image
                    {
                        Id = Guid.NewGuid(),
                        TargetType = ImageTargetType.Event,
                        Url = upload.Url,
                        PublicId = upload.PublicId,
                        Caption = img.Caption?.Trim(),
                        SortOrder = sortOrder++,
                        EventId = entity.Id
                    });
                }
            }

            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<EventFormDto?> GetForEditAsync(Guid id)
        {
            var e = await context.Events
                .AsNoTracking()
                .Include(x => x.EventCivilizations)
                .Include(x => x.EventFigures)
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return null;

            return new EventFormDto
            {
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                StartYear = e.StartYear,
                EndYear = e.EndYear,
                EraId = e.EraId,
                CivilizationIds = e.EventCivilizations.Select(x => x.CivilizationId).ToList(),
                FigureIds = e.EventFigures.Select(x => x.FigureId).ToList(),
                Images = e.Images
                    .Where(i => i.TargetType == ImageTargetType.Event && i.EventId == e.Id)
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

        public async Task UpdateAsync(Guid id, EventFormDto model)
        {
            NormalizeAndValidateYears(model);

            var e = await context.Events
                .Include(x => x.EventCivilizations)
                .Include(x => x.EventFigures)
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null) throw new InvalidOperationException();

            e.Title = model.Title.Trim();
            e.Description = model.Description.Trim();
            e.Location = model.Location?.Trim();
            e.StartYear = model.StartYear;
            e.EndYear = model.EndYear;
            e.EraId = model.EraId;

            context.RemoveRange(e.EventCivilizations);
            context.RemoveRange(e.EventFigures);

            e.EventCivilizations = (model.CivilizationIds ?? new List<Guid>())
                .Where(x => x != Guid.Empty)
                .Distinct()
                .Select(cid => new EventCivilization { EventId = e.Id, CivilizationId = cid })
                .ToHashSet();

            e.EventFigures = (model.FigureIds ?? new List<Guid>())
                .Where(x => x != Guid.Empty)
                .Distinct()
                .Select(fid => new EventFigure { EventId = e.Id, FigureId = fid })
                .ToHashSet();

            var existing = e.Images.ToList();

            var removeIds = (model.Images ?? new List<ImageEditDto>())
                .Where(x => x.Remove && x.Id.HasValue)
                .Select(x => x.Id!.Value)
                .ToHashSet();

            var toRemove = existing.Where(x => removeIds.Contains(x.Id)).ToList();

            foreach (var img in toRemove)
            {
                if (!string.IsNullOrWhiteSpace(img.PublicId))
                    await cloudinaryService.DeleteImageAsync(img.PublicId);
            }

            if (toRemove.Any())
                context.Images.RemoveRange(toRemove);

            var updates = (model.Images ?? new List<ImageEditDto>())
                .Where(x => !x.Remove && x.Id.HasValue && !string.IsNullOrWhiteSpace(x.Url));

            foreach (var u in updates)
            {
                var dbImg = existing.FirstOrDefault(x => x.Id == u.Id);
                if (dbImg == null) continue;

                dbImg.Url = u.Url!.Trim();
                dbImg.Caption = string.IsNullOrWhiteSpace(u.Caption) ? null : u.Caption!.Trim();
            }

            int sortOrder = existing.Any() ? existing.Max(x => x.SortOrder) + 1 : 0;

            var adds = (model.Images ?? new List<ImageEditDto>())
                .Where(x => !x.Remove && !x.Id.HasValue && !string.IsNullOrWhiteSpace(x.Url));

            foreach (var a in adds)
            {
                var upload = await cloudinaryService.UploadImageFromUrlAsync(a.Url!);

                await context.Images.AddAsync(new Image
                {
                    Id = Guid.NewGuid(),
                    TargetType = ImageTargetType.Event,
                    Url = upload.Url,
                    PublicId = upload.PublicId,
                    Caption = a.Caption,
                    SortOrder = sortOrder++,
                    EventId = e.Id
                });
            }

            if (model.ImageFiles != null)
            {
                foreach (var file in model.ImageFiles.Where(f => f != null && f.Length > 0))
                {
                    var uploadResult = await cloudinaryService.UploadImageAsync(file);

                    await context.Images.AddAsync(new Image
                    {
                        Id = Guid.NewGuid(),
                        TargetType = ImageTargetType.Event,
                        Url = uploadResult.Url,
                        PublicId = uploadResult.PublicId,
                        Caption = null,
                        SortOrder = sortOrder++,
                        EventId = e.Id
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var e = await context.Events
                .Include(x => x.EventCivilizations)
                .Include(x => x.EventFigures)
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null) return;

            foreach (var img in e.Images)
            {
                if (!string.IsNullOrWhiteSpace(img.PublicId))
                    await cloudinaryService.DeleteImageAsync(img.PublicId);
            }

            context.Images.RemoveRange(e.Images);
            context.RemoveRange(e.EventCivilizations);
            context.RemoveRange(e.EventFigures);
            context.Events.Remove(e);

            await context.SaveChangesAsync();
        }

        public async Task AddCivilizationAsync(Guid eventId, Guid civilizationId)
        {
            if (eventId == Guid.Empty || civilizationId == Guid.Empty) return;

            var exists = await context.EventCivilizations
                .AnyAsync(x => x.EventId == eventId && x.CivilizationId == civilizationId);

            if (!exists)
            {
                context.EventCivilizations.Add(new EventCivilization
                {
                    EventId = eventId,
                    CivilizationId = civilizationId
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task AddFigureAsync(Guid eventId, Guid figureId)
        {
            if (eventId == Guid.Empty || figureId == Guid.Empty) return;

            var exists = await context.EventFigures
                .AnyAsync(x => x.EventId == eventId && x.FigureId == figureId);

            if (!exists)
            {
                context.EventFigures.Add(new EventFigure
                {
                    EventId = eventId,
                    FigureId = figureId
                });

                await context.SaveChangesAsync();
            }
        }

        private static void NormalizeAndValidateYears(EventFormDto m)
        {
            if (!m.StartYear.HasValue && !m.EndYear.HasValue)
                throw new InvalidOperationException();

            if (m.StartYear.HasValue && !m.EndYear.HasValue)
                m.EndYear = m.StartYear;

            if (!m.StartYear.HasValue && m.EndYear.HasValue)
                m.StartYear = m.EndYear;

            if (m.StartYear > m.EndYear)
                throw new InvalidOperationException();
        }
    }
}