using System;
using System.Linq;
using System.Threading.Tasks;
using Bulgarikon.Core.DTOs.Common;
using Bulgarikon.Core.DTOs.FigureDTOs;
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
    public class FigureServiceTests
    {
        private BulgarikonDbContext db = null!;
        private Mock<IRepository<Figure, Guid>> figuresRepo = null!;
        private Mock<ICloudinaryService> cloudinaryService = null!;
        private FigureService service = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("FigureServiceTests_" + Guid.NewGuid())
                .Options;

            db = new BulgarikonDbContext(options);

            figuresRepo = new Mock<IRepository<Figure, Guid>>();
            cloudinaryService = new Mock<ICloudinaryService>();

            service = new FigureService(
                figuresRepo.Object,
                db,
                cloudinaryService.Object
            );
        }

        [TearDown]
        public async Task TearDown()
        {
            await db.DisposeAsync();
        }

        private static Era NewEra(string name = "Era")
            => new Era { Id = Guid.NewGuid(), Name = name, StartYear = 1, EndYear = 2 };

        private static Civilization NewCiv(Guid eraId, string name = "Civ")
            => new Civilization
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = "D",
                Type = CivilizationType.Kingdom,
                StartYear = 1,
                EndYear = 2,
                EraId = eraId
            };

        [Test]
        public async Task GetByEraAsync_WhenEraIdNull_ReturnsAll_AndOrdersByName_AndMapsCivilizationNameNull()
        {
            var era1 = NewEra("E1");
            var era2 = NewEra("E2");
            db.Eras.AddRange(era1, era2);

            var civ = NewCiv(era1.Id, "Civ1");
            db.Civilizations.Add(civ);

            db.Figures.AddRange(
                new Figure
                {
                    Id = Guid.NewGuid(),
                    Name = "B",
                    Description = "D",
                    BirthYear = 10,
                    EraId = era1.Id,
                    Era = era1,
                    CivilizationId = null,
                    Civilization = null
                },
                new Figure
                {
                    Id = Guid.NewGuid(),
                    Name = "A",
                    Description = "D",
                    BirthYear = 11,
                    EraId = era2.Id,
                    Era = era2,
                    CivilizationId = civ.Id,
                    Civilization = civ
                }
            );

            await db.SaveChangesAsync();

            var res = (await service.GetByEraAsync(null)).ToList();

            Assert.That(res.Count, Is.EqualTo(2));
            Assert.That(res[0].Name, Is.EqualTo("A"));
            Assert.That(res[1].Name, Is.EqualTo("B"));

            Assert.That(res[0].EraName, Is.EqualTo("E2"));
            Assert.That(res[0].CivilizationName, Is.EqualTo("Civ1"));

            Assert.That(res[1].EraName, Is.EqualTo("E1"));
            Assert.That(res[1].CivilizationName, Is.Null);
        }

        [Test]
        public async Task GetByEraAsync_FiltersByEra_AndCivilization()
        {
            var era1 = NewEra("E1");
            var era2 = NewEra("E2");
            db.Eras.AddRange(era1, era2);

            var civ1 = NewCiv(era1.Id, "C1");
            var civ2 = NewCiv(era1.Id, "C2");
            db.Civilizations.AddRange(civ1, civ2);

            db.Figures.AddRange(
                new Figure { Id = Guid.NewGuid(), Name = "F1", Description = "D", BirthYear = 1, EraId = era1.Id, Era = era1, CivilizationId = civ1.Id, Civilization = civ1 },
                new Figure { Id = Guid.NewGuid(), Name = "F2", Description = "D", BirthYear = 1, EraId = era1.Id, Era = era1, CivilizationId = civ2.Id, Civilization = civ2 },
                new Figure { Id = Guid.NewGuid(), Name = "F3", Description = "D", BirthYear = 1, EraId = era2.Id, Era = era2, CivilizationId = civ1.Id, Civilization = civ1 }
            );

            await db.SaveChangesAsync();

            var res = (await service.GetByEraAsync(era1.Id, civ1.Id)).ToList();

            Assert.That(res.Count, Is.EqualTo(1));
            Assert.That(res[0].Name, Is.EqualTo("F1"));
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetDetailsAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsDto_WithOrderedImages_AndNullCivilizationName()
        {
            var era = NewEra("EraX");
            db.Eras.Add(era);

            var fig = new Figure
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Desc",
                BirthYear = 1,
                EraId = era.Id,
                Era = era,
                CivilizationId = null,
                Civilization = null
            };

            db.Figures.Add(fig);

            var img2 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Figure,
                FigureId = fig.Id,
                Url = "u2",
                Caption = "c2",
                SortOrder = 1
            };
            var img1 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Figure,
                FigureId = fig.Id,
                Url = "u1",
                Caption = "c1",
                SortOrder = 0
            };

            db.Images.AddRange(img2, img1);

            await db.SaveChangesAsync();

            var res = await service.GetDetailsAsync(fig.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Name, Is.EqualTo("Name"));
            Assert.That(res.EraName, Is.EqualTo("EraX"));
            Assert.That(res.CivilizationName, Is.Null);

            Assert.That(res.Images, Is.Not.Null);
            Assert.That(res.Images.Count, Is.EqualTo(2));
            Assert.That(res.Images[0].Url, Is.EqualTo("u1"));
            Assert.That(res.Images[1].Url, Is.EqualTo("u2"));
        }

        [Test]
        public void CreateAsync_Throws_WhenNoBirthAndNoDeath()
        {
            var dto = new FigureFormDto
            {
                Name = "N",
                Description = "D",
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public void CreateAsync_Throws_WhenBirthDateYearMismatchBirthYear()
        {
            var dto = new FigureFormDto
            {
                Name = "N",
                Description = "D",
                EraId = Guid.NewGuid(),
                BirthDate = new DateTime(2001, 1, 1),
                BirthYear = 2002
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public void CreateAsync_Throws_WhenDeathDateYearMismatchDeathYear()
        {
            var dto = new FigureFormDto
            {
                Name = "N",
                Description = "D",
                EraId = Guid.NewGuid(),
                DeathDate = new DateTime(2001, 1, 1),
                DeathYear = 2002
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public void CreateAsync_Throws_WhenDeathYearBeforeBirthYear()
        {
            var dto = new FigureFormDto
            {
                Name = "N",
                Description = "D",
                EraId = Guid.NewGuid(),
                BirthYear = 100,
                DeathYear = 50
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public void CreateAsync_Throws_WhenDeathDateBeforeBirthDate()
        {
            var dto = new FigureFormDto
            {
                Name = "N",
                Description = "D",
                EraId = Guid.NewGuid(),
                BirthDate = new DateTime(2000, 1, 2),
                DeathDate = new DateTime(2000, 1, 1)
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public async Task CreateAsync_NormalizesStrings_AddsFigure_AndAddsOnlyValidImages()
        {
            var era = NewEra("E");
            db.Eras.Add(era);

            var civ = NewCiv(era.Id, "C");
            db.Civilizations.Add(civ);

            await db.SaveChangesAsync();

            var dto = new FigureFormDto
            {
                Name = "  John  ",
                Description = "  Desc  ",
                EraId = era.Id,
                CivilizationId = civ.Id,
                BirthYear = 10,
                Images = new()
                {
                    new ImageEditDto { Url = "  https://img1  ", Caption = "  cap1  ", Remove = false },
                    new ImageEditDto { Url = "   ", Caption = "x", Remove = false },
                    new ImageEditDto { Url = "https://removed", Caption = "x", Remove = true },
                    new ImageEditDto { Url = null, Caption = null, Remove = false }
                }
            };

            cloudinaryService
                .Setup(c => c.UploadImageFromUrlAsync("  https://img1  "))
                .ReturnsAsync(new CloudinaryUploadResultDto { Url = "https://img1", PublicId = "id1" });

            var id = await service.CreateAsync(dto);

            var f = await db.Figures.AsNoTracking().FirstAsync(x => x.Id == id);
            Assert.That(f.Name, Is.EqualTo("John"));
            Assert.That(f.Description, Is.EqualTo("Desc"));
            Assert.That(f.EraId, Is.EqualTo(era.Id));
            Assert.That(f.CivilizationId, Is.EqualTo(civ.Id));

            var imgs = await db.Images.AsNoTracking()
                .Where(i => i.TargetType == ImageTargetType.Figure && i.FigureId == id)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            Assert.That(imgs.Count, Is.EqualTo(1));
            Assert.That(imgs[0].Url, Is.EqualTo("https://img1"));
            Assert.That(imgs[0].Caption, Is.EqualTo("cap1"));
            Assert.That(imgs[0].SortOrder, Is.EqualTo(0));
        }

        [Test]
        public async Task GetForEditAsync_ReturnsNull_WhenNotFound()
        {
            var res = await service.GetForEditAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsDto_WithOrderedImages_AndRemoveFalse()
        {
            var era = NewEra("E");
            db.Eras.Add(era);

            var fig = new Figure
            {
                Id = Guid.NewGuid(),
                Name = "N",
                Description = "D",
                BirthYear = 1,
                EraId = era.Id,
                Era = era
            };
            db.Figures.Add(fig);

            var img2 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Figure,
                FigureId = fig.Id,
                Url = "u2",
                Caption = "c2",
                SortOrder = 1
            };
            var img1 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Figure,
                FigureId = fig.Id,
                Url = "u1",
                Caption = "c1",
                SortOrder = 0
            };
            db.Images.AddRange(img2, img1);

            await db.SaveChangesAsync();

            var res = await service.GetForEditAsync(fig.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Name, Is.EqualTo("N"));
            Assert.That(res.Images.Count, Is.EqualTo(2));
            Assert.That(res.Images[0].Url, Is.EqualTo("u1"));
            Assert.That(res.Images[1].Url, Is.EqualTo("u2"));
            Assert.That(res.Images.All(x => x.Remove == false), Is.True);
        }

        [Test]
        public void UpdateAsync_Throws_WhenInvalidByValidation()
        {
            var dto = new FigureFormDto
            {
                Name = "N",
                Description = "D",
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.UpdateAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public void UpdateAsync_Throws_WhenFigureNotFound()
        {
            var dto = new FigureFormDto
            {
                Name = "N",
                Description = "D",
                EraId = Guid.NewGuid(),
                BirthYear = 1
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public async Task UpdateAsync_Removes_Updates_AddsImages_AndTrimsFields()
        {
            var era = NewEra("E");
            db.Eras.Add(era);

            var civ = NewCiv(era.Id, "C");
            db.Civilizations.Add(civ);

            var fig = new Figure
            {
                Id = Guid.NewGuid(),
                Name = " Old ",
                Description = " OldD ",
                BirthYear = 1,
                EraId = era.Id,
                Era = era,
                CivilizationId = civ.Id,
                Civilization = civ
            };
            db.Figures.Add(fig);

            var removeImg = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Figure,
                FigureId = fig.Id,
                Url = "remove",
                Caption = "r",
                SortOrder = 0
            };
            var updateImg = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Figure,
                FigureId = fig.Id,
                Url = "old",
                Caption = "oldcap",
                SortOrder = 1
            };
            db.Images.AddRange(removeImg, updateImg);

            await db.SaveChangesAsync();

            var dto = new FigureFormDto
            {
                Name = "  NewName  ",
                Description = "  NewDesc  ",
                EraId = era.Id,
                CivilizationId = civ.Id,
                BirthYear = 1,
                Images = new()
                {
                    new ImageEditDto { Id = removeImg.Id, Url = "remove", Caption = "x", Remove = true },
                    new ImageEditDto { Id = updateImg.Id, Url = "  https://updated  ", Caption = "  cap  ", Remove = false },
                    new ImageEditDto { Url = "  https://added  ", Caption = "  addcap  ", Remove = false },
                    new ImageEditDto { Url = "   ", Caption = "x", Remove = false }
                }
            };

            cloudinaryService
                .Setup(c => c.UploadImageFromUrlAsync("https://added"))
                .ReturnsAsync(new CloudinaryUploadResultDto { Url = "https://added", PublicId = "id-added" });

            await service.UpdateAsync(fig.Id, dto);

            var dbFig = await db.Figures.AsNoTracking().FirstAsync(x => x.Id == fig.Id);
            Assert.That(dbFig.Name, Is.EqualTo("NewName"));
            Assert.That(dbFig.Description, Is.EqualTo("NewDesc"));

            var imgs = await db.Images.AsNoTracking()
                .Where(i => i.TargetType == ImageTargetType.Figure && i.FigureId == fig.Id)
                .ToListAsync();

            Assert.That(imgs.Count, Is.EqualTo(2));
            Assert.That(imgs.Any(x => x.Id == removeImg.Id), Is.False);

            var updated = imgs.Single(x => x.Id == updateImg.Id);
            Assert.That(updated.Url, Is.EqualTo("https://updated"));
            Assert.That(updated.Caption, Is.EqualTo("cap"));

            Assert.That(imgs.Any(x => x.Url == "https://added" && x.Caption == "addcap"), Is.True);
        }

        [Test]
        public async Task UpdateAsync_IgnoresUpdate_WhenImageIdNotFoundInExisting()
        {
            var era = NewEra("E");
            db.Eras.Add(era);

            var fig = new Figure
            {
                Id = Guid.NewGuid(),
                Name = "N",
                Description = "D",
                BirthYear = 1,
                EraId = era.Id,
                Era = era
            };
            db.Figures.Add(fig);

            var existing = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Figure,
                FigureId = fig.Id,
                Url = "u",
                Caption = "c",
                SortOrder = 0
            };
            db.Images.Add(existing);

            await db.SaveChangesAsync();

            var dto = new FigureFormDto
            {
                Name = "N",
                Description = "D",
                EraId = era.Id,
                BirthYear = 1,
                Images = new()
                {
                    new ImageEditDto { Id = Guid.NewGuid(), Url = "https://doesnotmatter", Caption = "x", Remove = false }
                }
            };

            await service.UpdateAsync(fig.Id, dto);

            var imgs = await db.Images.AsNoTracking()
                .Where(i => i.TargetType == ImageTargetType.Figure && i.FigureId == fig.Id)
                .ToListAsync();

            Assert.That(imgs.Count, Is.EqualTo(1));
            Assert.That(imgs[0].Url, Is.EqualTo("u"));
            Assert.That(imgs[0].Caption, Is.EqualTo("c"));
        }

        [Test]
        public async Task DeleteAsync_DoesNothing_WhenNotFound()
        {
            await service.DeleteAsync(Guid.NewGuid());
            Assert.That(await db.Figures.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteAsync_RemovesFigure_AndItsFigureImagesOnly()
        {
            var era = NewEra("E");
            db.Eras.Add(era);

            var fig = new Figure
            {
                Id = Guid.NewGuid(),
                Name = "N",
                Description = "D",
                BirthYear = 1,
                EraId = era.Id,
                Era = era
            };
            db.Figures.Add(fig);

            var figImg = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Figure,
                FigureId = fig.Id,
                Url = "u",
                Caption = "c",
                SortOrder = 0,
                PublicId = "figure-public-id"
            };
            var otherTarget = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "keep",
                Caption = "k"
            };
            db.Images.AddRange(figImg, otherTarget);

            await db.SaveChangesAsync();

            await service.DeleteAsync(fig.Id);

            Assert.That(await db.Figures.CountAsync(), Is.EqualTo(0));

            var imgs = await db.Images.AsNoTracking().ToListAsync();
            Assert.That(imgs.Count, Is.EqualTo(1));
            Assert.That(imgs[0].Url, Is.EqualTo("keep"));

            cloudinaryService.Verify(x => x.DeleteImageAsync("figure-public-id"), Times.Once);
        }
    }
}