using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulgarikon.Core.DTOs.Common;
using Bulgarikon.Core.DTOs.EventDTOs;
using Bulgarikon.Core.DTOs.ImageDTOs;
using Bulgarikon.Core.Implementations;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Bulgarikon.Tests.Services
{
    [TestFixture]
    public class EventServiceTests
    {
        private Mock<IRepository<Event, Guid>> eventsRepo = null!;
        private Mock<ICloudinaryService> cloudinaryService = null!;
        private BulgarikonDbContext db = null!;
        private EventService service = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("EventServiceTests_" + Guid.NewGuid())
                .Options;

            db = new BulgarikonDbContext(options);

            eventsRepo = new Mock<IRepository<Event, Guid>>();
            cloudinaryService = new Mock<ICloudinaryService>();

            service = new EventService(eventsRepo.Object, db, cloudinaryService.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await db.DisposeAsync();
        }

        [Test]
        public async Task GetByEraAsync_FiltersByEra_OrdersByYears_NullsGoLast()
        {
            var era1 = new Era { Id = Guid.NewGuid(), Name = "Era1", StartYear = 1, EndYear = 2 };
            var era2 = new Era { Id = Guid.NewGuid(), Name = "Era2", StartYear = 1, EndYear = 2 };
            db.Eras.AddRange(era1, era2);

            var e1 = new Event
            {
                Id = Guid.NewGuid(),
                Title = "E1",
                Description = "D",
                EraId = era1.Id,
                Era = era1,
                StartYear = 10,
                EndYear = 20
            };
            var e2 = new Event
            {
                Id = Guid.NewGuid(),
                Title = "E2",
                Description = "D",
                EraId = era1.Id,
                Era = era1,
                StartYear = 5,
                EndYear = 50
            };
            var e3NullYears = new Event
            {
                Id = Guid.NewGuid(),
                Title = "E3",
                Description = "D",
                EraId = era1.Id,
                Era = era1,
                StartYear = null,
                EndYear = null
            };
            var otherEra = new Event
            {
                Id = Guid.NewGuid(),
                Title = "Other",
                Description = "D",
                EraId = era2.Id,
                Era = era2,
                StartYear = 1,
                EndYear = 2
            };

            db.Events.AddRange(e1, e2, e3NullYears, otherEra);
            await db.SaveChangesAsync();

            var result = (await service.GetByEraAsync(era1.Id)).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Title, Is.EqualTo("E2"));
            Assert.That(result[1].Title, Is.EqualTo("E1"));
            Assert.That(result[2].Title, Is.EqualTo("E3"));
            Assert.That(result.All(x => x.EraName == "Era1"), Is.True);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetDetailsAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsDto_WithFilteredImages_AndSortedChips()
        {
            var era = new Era { Id = Guid.NewGuid(), Name = "Medieval", StartYear = 1, EndYear = 2 };
            db.Eras.Add(era);

            var civB = new Civilization { Id = Guid.NewGuid(), Name = "Bulgaria", Description = "D", Type = CivilizationType.Kingdom, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };
            var civA = new Civilization { Id = Guid.NewGuid(), Name = "Avar", Description = "D", Type = CivilizationType.Empire, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };

            var figB = new Figure { Id = Guid.NewGuid(), Name = "Boris", Description = "D", EraId = era.Id, Era = era, BirthYear = 1 };
            var figA = new Figure { Id = Guid.NewGuid(), Name = "Asparuh", Description = "D", EraId = era.Id, Era = era, BirthYear = 1 };

            db.Civilizations.AddRange(civB, civA);
            db.Figures.AddRange(figB, figA);

            var ev = new Event
            {
                Id = Guid.NewGuid(),
                Title = "Battle",
                Description = "Big battle",
                Location = "Somewhere",
                StartYear = 811,
                EndYear = 811,
                EraId = era.Id,
                Era = era
            };

            db.Events.Add(ev);

            db.EventCivilizations.AddRange(
                new EventCivilization { Id = Guid.NewGuid(), EventId = ev.Id, CivilizationId = civB.Id, Civilization = civB },
                new EventCivilization { Id = Guid.NewGuid(), EventId = ev.Id, CivilizationId = civA.Id, Civilization = civA }
            );

            db.EventFigures.AddRange(
                new EventFigure { Id = Guid.NewGuid(), EventId = ev.Id, FigureId = figB.Id, Figure = figB },
                new EventFigure { Id = Guid.NewGuid(), EventId = ev.Id, FigureId = figA.Id, Figure = figA }
            );

            var imgEvent1 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Event,
                EventId = ev.Id,
                Url = "https://event/1",
                Caption = "c1",
                SortOrder = 0
            };
            var imgEvent2 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Event,
                EventId = ev.Id,
                Url = "https://event/2",
                Caption = null,
                SortOrder = 1
            };
            var imgOtherType = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://era/1"
            };
            var imgOtherEvent = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Event,
                EventId = Guid.NewGuid(),
                Url = "https://event/other"
            };

            db.Images.AddRange(imgEvent1, imgEvent2, imgOtherType, imgOtherEvent);

            await db.SaveChangesAsync();

            var res = await service.GetDetailsAsync(ev.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Title, Is.EqualTo("Battle"));
            Assert.That(res.EraName, Is.EqualTo("Medieval"));

            Assert.That(res.Images.Count, Is.EqualTo(2));
            Assert.That(res.Images[0].Url, Is.EqualTo("https://event/1"));
            Assert.That(res.Images[1].Url, Is.EqualTo("https://event/2"));

            Assert.That(res.Civilizations.Select(x => x.Name).ToList(), Is.EqualTo(new[] { "Avar", "Bulgaria" }));
            Assert.That(res.Figures.Select(x => x.Name).ToList(), Is.EqualTo(new[] { "Asparuh", "Boris" }));
        }

        [Test]
        public void CreateAsync_Throws_WhenNoYearsProvided()
        {
            var dto = new EventFormDto
            {
                Title = "T",
                Description = "D",
                Location = "L",
                StartYear = null,
                EndYear = null,
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public void CreateAsync_Throws_WhenStartGreaterThanEnd()
        {
            var dto = new EventFormDto
            {
                Title = "T",
                Description = "D",
                StartYear = 200,
                EndYear = 100,
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public async Task CreateAsync_WhenOnlyStartYear_SetsEndYearSameValue_AndTrims_AndDistinctsIds_AndAddsImages()
        {
            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ1 = new Civilization { Id = Guid.NewGuid(), Name = "C1", Description = "D", Type = CivilizationType.Kingdom, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };
            var fig1 = new Figure { Id = Guid.NewGuid(), Name = "F1", Description = "D", EraId = era.Id, Era = era, BirthYear = 1 };

            db.AddRange(era, civ1, fig1);
            await db.SaveChangesAsync();

            var dto = new EventFormDto
            {
                Title = "  Title  ",
                Description = "  Desc  ",
                Location = "  Loc  ",
                StartYear = 123,
                EndYear = null,
                EraId = era.Id,
                CivilizationIds = new List<Guid> { civ1.Id, civ1.Id, Guid.Empty },
                FigureIds = new List<Guid> { fig1.Id, fig1.Id, Guid.Empty },
                Images = new List<ImageEditDto>
                {
                    new ImageEditDto { Url = "  https://img/1  ", Caption = "  c1  ", Remove = false },
                    new ImageEditDto { Url = "   ", Caption = "ignored", Remove = false },
                    new ImageEditDto { Url = "https://img/2", Caption = null, Remove = true },
                    new ImageEditDto { Url = null, Caption = "ignored", Remove = false }
                }
            };

            cloudinaryService
                .Setup(c => c.UploadImageFromUrlAsync("  https://img/1  "))
                .ReturnsAsync(new CloudinaryUploadResultDto { Url = "https://img/1", PublicId = "id1" });

            var id = await service.CreateAsync(dto);

            var created = await db.Events
                .Include(x => x.EventCivilizations)
                .Include(x => x.EventFigures)
                .FirstAsync(x => x.Id == id);

            Assert.That(created.Title, Is.EqualTo("Title"));
            Assert.That(created.Description, Is.EqualTo("Desc"));
            Assert.That(created.Location, Is.EqualTo("Loc"));
            Assert.That(created.StartYear, Is.EqualTo(123));
            Assert.That(created.EndYear, Is.EqualTo(123));

            Assert.That(created.EventCivilizations.Count, Is.EqualTo(1));
            Assert.That(created.EventCivilizations.First().CivilizationId, Is.EqualTo(civ1.Id));

            Assert.That(created.EventFigures.Count, Is.EqualTo(1));
            Assert.That(created.EventFigures.First().FigureId, Is.EqualTo(fig1.Id));

            var imgs = await db.Images.AsNoTracking()
                .Where(i => i.TargetType == ImageTargetType.Event && i.EventId == id)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            Assert.That(imgs.Count, Is.EqualTo(1));
            Assert.That(imgs[0].Url, Is.EqualTo("https://img/1"));
            Assert.That(imgs[0].Caption, Is.EqualTo("c1"));
            Assert.That(imgs[0].SortOrder, Is.EqualTo(0));
        }

        [Test]
        public async Task CreateAsync_WhenOnlyEndYear_SetsStartYearSameValue_AndNullLocationOk()
        {
            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            db.Eras.Add(era);
            await db.SaveChangesAsync();

            var dto = new EventFormDto
            {
                Title = "T",
                Description = "D",
                Location = null,
                StartYear = null,
                EndYear = 777,
                EraId = era.Id,
                CivilizationIds = null,
                FigureIds = null,
                Images = null
            };

            var id = await service.CreateAsync(dto);

            var created = await db.Events.FirstAsync(x => x.Id == id);
            Assert.That(created.StartYear, Is.EqualTo(777));
            Assert.That(created.EndYear, Is.EqualTo(777));
            Assert.That(created.Location, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetForEditAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsDto_WithIdsAndImagesOrderedBySortOrder()
        {
            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ1 = new Civilization { Id = Guid.NewGuid(), Name = "C1", Description = "D", Type = CivilizationType.Kingdom, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };
            var fig1 = new Figure { Id = Guid.NewGuid(), Name = "F1", Description = "D", EraId = era.Id, Era = era, BirthYear = 1 };
            db.AddRange(era, civ1, fig1);

            var ev = new Event
            {
                Id = Guid.NewGuid(),
                Title = "T",
                Description = "D",
                Location = "L",
                StartYear = 1,
                EndYear = 2,
                EraId = era.Id,
                Era = era
            };
            db.Events.Add(ev);

            db.EventCivilizations.Add(new EventCivilization { Id = Guid.NewGuid(), EventId = ev.Id, CivilizationId = civ1.Id });
            db.EventFigures.Add(new EventFigure { Id = Guid.NewGuid(), EventId = ev.Id, FigureId = fig1.Id });

            var img1 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Event,
                EventId = ev.Id,
                Url = "https://img/1",
                Caption = "c1",
                SortOrder = 0
            };
            var img2 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Event,
                EventId = ev.Id,
                Url = "https://img/2",
                Caption = null,
                SortOrder = 1
            };

            db.Images.Add(new Image { Id = Guid.NewGuid(), TargetType = ImageTargetType.Era, EraId = era.Id, Url = "https://era/1" });
            db.Images.AddRange(img2, img1);

            await db.SaveChangesAsync();

            var res = await service.GetForEditAsync(ev.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Title, Is.EqualTo("T"));
            Assert.That(res.CivilizationIds, Is.EquivalentTo(new[] { civ1.Id }));
            Assert.That(res.FigureIds, Is.EquivalentTo(new[] { fig1.Id }));

            Assert.That(res.Images.Count, Is.EqualTo(2));
            Assert.That(res.Images[0].Url, Is.EqualTo("https://img/1"));
            Assert.That(res.Images[1].Url, Is.EqualTo("https://img/2"));
            Assert.That(res.Images.All(x => x.Remove == false), Is.True);
        }

        [Test]
        public void UpdateAsync_Throws_WhenEventNotFound()
        {
            var dto = new EventFormDto
            {
                Title = "T",
                Description = "D",
                StartYear = 1,
                EndYear = 1,
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public void UpdateAsync_Throws_WhenNoYearsProvided()
        {
            var dto = new EventFormDto
            {
                Title = "T",
                Description = "D",
                StartYear = null,
                EndYear = null,
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public async Task UpdateAsync_ReplacesJoinTables_RemovesUpdatesAddsImages_AndNormalizesYears()
        {
            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civOld = new Civilization { Id = Guid.NewGuid(), Name = "OldC", Description = "D", Type = CivilizationType.Kingdom, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };
            var civNew = new Civilization { Id = Guid.NewGuid(), Name = "NewC", Description = "D", Type = CivilizationType.Empire, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };
            var figOld = new Figure { Id = Guid.NewGuid(), Name = "OldF", Description = "D", EraId = era.Id, Era = era, BirthYear = 1 };
            var figNew = new Figure { Id = Guid.NewGuid(), Name = "NewF", Description = "D", EraId = era.Id, Era = era, BirthYear = 1 };

            db.AddRange(era, civOld, civNew, figOld, figNew);

            var ev = new Event
            {
                Id = Guid.NewGuid(),
                Title = "OldT",
                Description = "OldD",
                Location = "OldL",
                StartYear = 10,
                EndYear = 20,
                EraId = era.Id,
                Era = era
            };
            db.Events.Add(ev);

            db.EventCivilizations.Add(new EventCivilization { Id = Guid.NewGuid(), EventId = ev.Id, CivilizationId = civOld.Id });
            db.EventFigures.Add(new EventFigure { Id = Guid.NewGuid(), EventId = ev.Id, FigureId = figOld.Id });

            var imgToRemove = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Event,
                EventId = ev.Id,
                Url = "https://remove",
                Caption = "r",
                SortOrder = 0
            };
            var imgToUpdate = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Event,
                EventId = ev.Id,
                Url = "https://old",
                Caption = "old",
                SortOrder = 1
            };
            var otherType = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://era/1"
            };

            db.Images.AddRange(imgToRemove, imgToUpdate, otherType);

            await db.SaveChangesAsync();

            var dto = new EventFormDto
            {
                Title = "  NewT  ",
                Description = "  NewD  ",
                Location = "  NewL  ",
                StartYear = 999,
                EndYear = null,
                EraId = era.Id,
                CivilizationIds = new List<Guid> { civNew.Id, civNew.Id, Guid.Empty },
                FigureIds = new List<Guid> { figNew.Id, figNew.Id, Guid.Empty },
                Images = new List<ImageEditDto>
                {
                    new ImageEditDto { Id = imgToRemove.Id, Url = imgToRemove.Url, Caption = imgToRemove.Caption, Remove = true },
                    new ImageEditDto { Id = imgToUpdate.Id, Url = "  https://updated  ", Caption = "  cap  ", Remove = false },
                    new ImageEditDto { Url = "  https://added  ", Caption = null, Remove = false },
                    new ImageEditDto { Url = "   ", Caption = "ignored", Remove = false },
                    new ImageEditDto { Id = Guid.NewGuid(), Url = "https://id-not-found", Caption = "x", Remove = false }
                }
            };

            cloudinaryService
                .Setup(c => c.UploadImageFromUrlAsync("  https://added  "))
                .ReturnsAsync(new CloudinaryUploadResultDto { Url = "https://added", PublicId = "id-added" });

            await service.UpdateAsync(ev.Id, dto);

            var updated = await db.Events
                .Include(x => x.EventCivilizations)
                .Include(x => x.EventFigures)
                .FirstAsync(x => x.Id == ev.Id);

            Assert.That(updated.Title, Is.EqualTo("NewT"));
            Assert.That(updated.Description, Is.EqualTo("NewD"));
            Assert.That(updated.Location, Is.EqualTo("NewL"));
            Assert.That(updated.StartYear, Is.EqualTo(999));
            Assert.That(updated.EndYear, Is.EqualTo(999));

            Assert.That(updated.EventCivilizations.Count, Is.EqualTo(1));
            Assert.That(updated.EventCivilizations.First().CivilizationId, Is.EqualTo(civNew.Id));

            Assert.That(updated.EventFigures.Count, Is.EqualTo(1));
            Assert.That(updated.EventFigures.First().FigureId, Is.EqualTo(figNew.Id));

            var eventImgs = await db.Images.AsNoTracking()
                .Where(i => i.TargetType == ImageTargetType.Event && i.EventId == ev.Id)
                .ToListAsync();

            Assert.That(eventImgs.Any(i => i.Id == imgToRemove.Id), Is.False);
            Assert.That(eventImgs.Any(i => i.Id == imgToUpdate.Id && i.Url == "https://updated" && i.Caption == "cap"), Is.True);
            Assert.That(eventImgs.Any(i => i.Url == "https://added"), Is.True);

            Assert.That(await db.Images.AsNoTracking().AnyAsync(i => i.Id == otherType.Id), Is.True);
        }

        [Test]
        public async Task DeleteAsync_WhenNotFound_DoesNothing()
        {
            await service.DeleteAsync(Guid.NewGuid());

            Assert.That(await db.Events.CountAsync(), Is.EqualTo(0));
            Assert.That(await db.Images.CountAsync(), Is.EqualTo(0));
            Assert.That(await db.EventCivilizations.CountAsync(), Is.EqualTo(0));
            Assert.That(await db.EventFigures.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteAsync_WhenFound_RemovesEvent_RemovesJoins_RemovesOnlyEventImages()
        {
            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ = new Civilization { Id = Guid.NewGuid(), Name = "C", Description = "D", Type = CivilizationType.Kingdom, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };
            var fig = new Figure { Id = Guid.NewGuid(), Name = "F", Description = "D", EraId = era.Id, Era = era, BirthYear = 1 };
            db.AddRange(era, civ, fig);

            var ev = new Event
            {
                Id = Guid.NewGuid(),
                Title = "T",
                Description = "D",
                StartYear = 1,
                EndYear = 1,
                EraId = era.Id,
                Era = era
            };
            db.Events.Add(ev);

            db.EventCivilizations.Add(new EventCivilization { Id = Guid.NewGuid(), EventId = ev.Id, CivilizationId = civ.Id });
            db.EventFigures.Add(new EventFigure { Id = Guid.NewGuid(), EventId = ev.Id, FigureId = fig.Id });

            var imgEvent = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Event,
                EventId = ev.Id,
                Url = "https://event/1",
                PublicId = "cloudinary-event-1",
                SortOrder = 0
            };
            var imgOtherType = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://era/1"
            };
            db.Images.AddRange(imgEvent, imgOtherType);

            await db.SaveChangesAsync();

            await service.DeleteAsync(ev.Id);

            Assert.That(await db.Events.AsNoTracking().AnyAsync(x => x.Id == ev.Id), Is.False);
            Assert.That(await db.EventCivilizations.AsNoTracking().AnyAsync(x => x.EventId == ev.Id), Is.False);
            Assert.That(await db.EventFigures.AsNoTracking().AnyAsync(x => x.EventId == ev.Id), Is.False);

            Assert.That(await db.Images.AsNoTracking().AnyAsync(i => i.TargetType == ImageTargetType.Event && i.EventId == ev.Id), Is.False);
            Assert.That(await db.Images.AsNoTracking().AnyAsync(i => i.Id == imgOtherType.Id), Is.True);

            cloudinaryService.Verify(x => x.DeleteImageAsync("cloudinary-event-1"), Times.Once);
        }

        [Test]
        public async Task AddCivilizationAsync_WhenIdsEmpty_ReturnsWithoutSaving()
        {
            await service.AddCivilizationAsync(Guid.Empty, Guid.NewGuid());
            await service.AddCivilizationAsync(Guid.NewGuid(), Guid.Empty);

            Assert.That(await db.EventCivilizations.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task AddCivilizationAsync_WhenNotExists_Adds_ThenSecondCallDoesNothing()
        {
            var eventId = Guid.NewGuid();
            var civId = Guid.NewGuid();

            await service.AddCivilizationAsync(eventId, civId);

            Assert.That(await db.EventCivilizations.CountAsync(), Is.EqualTo(1));

            await service.AddCivilizationAsync(eventId, civId);

            Assert.That(await db.EventCivilizations.CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task AddFigureAsync_WhenIdsEmpty_ReturnsWithoutSaving()
        {
            await service.AddFigureAsync(Guid.Empty, Guid.NewGuid());
            await service.AddFigureAsync(Guid.NewGuid(), Guid.Empty);

            Assert.That(await db.EventFigures.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task AddFigureAsync_WhenNotExists_Adds_ThenSecondCallDoesNothing()
        {
            var eventId = Guid.NewGuid();
            var figId = Guid.NewGuid();

            await service.AddFigureAsync(eventId, figId);

            Assert.That(await db.EventFigures.CountAsync(), Is.EqualTo(1));

            await service.AddFigureAsync(eventId, figId);

            Assert.That(await db.EventFigures.CountAsync(), Is.EqualTo(1));
        }
    }
}