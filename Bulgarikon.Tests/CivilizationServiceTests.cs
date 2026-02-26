using System;
using System.Linq;
using System.Threading.Tasks;
using Bulgarikon.Core.DTOs.CivilizaionDTOs;
using Bulgarikon.Core.Implementations;
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
    public class CivilizationServiceTests
    {
        private Mock<IRepository<Civilization, Guid>> civRepo = null!;
        private Mock<IRepository<Image, Guid>> imgRepo = null!;
        private CivilizationService service = null!;

        [SetUp]
        public void SetUp()
        {
            civRepo = new Mock<IRepository<Civilization, Guid>>();
            imgRepo = new Mock<IRepository<Image, Guid>>();
            service = new CivilizationService(civRepo.Object, imgRepo.Object);
        }

        private static BulgarikonDbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("CivilizationServiceTests_" + Guid.NewGuid())
                .Options;

            return new BulgarikonDbContext(options);
        }

        [Test]
        public async Task GetByEraAsync_WhenEraIdProvided_Filters_AndSetsImageUrlFromImages()
        {
            await using var db = CreateDb();

            var era1 = new Era { Id = Guid.NewGuid(), Name = "Era1", StartYear = 1, EndYear = 2 };
            var era2 = new Era { Id = Guid.NewGuid(), Name = "Era2", StartYear = 1, EndYear = 2 };
            db.Eras.AddRange(era1, era2);

            var c1 = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = "C1",
                Description = "D1",
                Type = CivilizationType.Kingdom,
                StartYear = 100,
                EndYear = 200,
                EraId = era1.Id,
                Era = era1
            };

            var c2 = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = "C2",
                Description = "D2",
                Type = CivilizationType.Empire,
                StartYear = 50,
                EndYear = 150,
                EraId = era1.Id,
                Era = era1
            };

            var otherEra = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = "Other",
                Description = "D",
                Type = CivilizationType.Republic,
                StartYear = 1,
                EndYear = 2,
                EraId = era2.Id,
                Era = era2
            };

            db.Civilizations.AddRange(c1, c2, otherEra);

            var img1Id = new Guid("00000000-0000-0000-0000-000000000001");
            var img2Id = new Guid("00000000-0000-0000-0000-000000000002");
            var imgC2Id = new Guid("00000000-0000-0000-0000-000000000003");

            var img1 = new Image
            {
                Id = img1Id,
                TargetType = ImageTargetType.Civilization,
                CivilizationId = c1.Id,
                Url = "https://img/first"
            };

            var img2 = new Image
            {
                Id = img2Id,
                TargetType = ImageTargetType.Civilization,
                CivilizationId = c1.Id,
                Url = "https://img/second"
            };

            var imgForC2 = new Image
            {
                Id = imgC2Id,
                TargetType = ImageTargetType.Civilization,
                CivilizationId = c2.Id,
                Url = "https://img/c2"
            };

            db.Images.AddRange(img1, img2, imgForC2);

            await db.SaveChangesAsync();

            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            var result = (await service.GetByEraAsync(era1.Id)).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("C2"));
            Assert.That(result[1].Name, Is.EqualTo("C1"));

            Assert.That(result[0].EraName, Is.EqualTo("Era1"));
            Assert.That(result[1].EraName, Is.EqualTo("Era1"));

            Assert.That(result[0].ImageUrl, Is.EqualTo("https://img/c2"));
            Assert.That(result[1].ImageUrl, Is.EqualTo("https://img/first"));
        }

        [Test]
        public async Task GetByEraAsync_WhenEraIdNull_ReturnsAllCivilizations()
        {
            await using var db = CreateDb();

            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            db.Eras.Add(era);

            db.Civilizations.AddRange(
                new Civilization { Id = Guid.NewGuid(), Name = "C1", Description = "D", Type = CivilizationType.Kingdom, StartYear = 10, EndYear = 20, EraId = era.Id, Era = era },
                new Civilization { Id = Guid.NewGuid(), Name = "C2", Description = "D", Type = CivilizationType.Empire, StartYear = 5, EndYear = 15, EraId = era.Id, Era = era }
            );

            await db.SaveChangesAsync();

            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            var result = (await service.GetByEraAsync(null)).ToList();
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotFound()
        {
            await using var db = CreateDb();
            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            var res = await service.GetDetailsAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsDto_WithFirstImageUrl()
        {
            await using var db = CreateDb();

            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = "Rome",
                Description = "Desc",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 100,
                EraId = era.Id,
                Era = era
            };

            db.AddRange(era, civ);

            var img = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Civilization,
                CivilizationId = civ.Id,
                Url = "https://img/rome"
            };
            db.Images.Add(img);

            await db.SaveChangesAsync();

            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            var res = await service.GetDetailsAsync(civ.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Name, Is.EqualTo("Rome"));
            Assert.That(res.EraName, Is.EqualTo("Era"));
            Assert.That(res.ImageUrl, Is.EqualTo("https://img/rome"));
        }


        [Test]
        public void CreateAsync_Throws_WhenEndYearBeforeStartYear()
        {
            var dto = new CivilizationFormDto
            {
                Name = "C",
                Description = "D",
                Type = CivilizationType.Kingdom,
                StartYear = 100,
                EndYear = 50,
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public async Task CreateAsync_AddsCivilization_AndAddsImage_WhenImageUrlProvided()
        {
            var dto = new CivilizationFormDto
            {
                Name = "  Rome ",
                Description = "  Desc ",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 2,
                EraId = Guid.NewGuid(),
                ImageUrl = "  https://img  "
            };

            Civilization? capturedCiv = null;
            Image? capturedImg = null;

            civRepo.Setup(r => r.AddAsync(It.IsAny<Civilization>()))
                .Callback<Civilization>(c => capturedCiv = c)
                .Returns(Task.CompletedTask);

            imgRepo.Setup(r => r.AddAsync(It.IsAny<Image>()))
                .Callback<Image>(i => capturedImg = i)
                .Returns(Task.CompletedTask);

            civRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var id = await service.CreateAsync(dto);

            Assert.That(id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(capturedCiv, Is.Not.Null);
            Assert.That(capturedCiv!.Name, Is.EqualTo("Rome"));

            Assert.That(capturedImg, Is.Not.Null);
            Assert.That(capturedImg!.TargetType, Is.EqualTo(ImageTargetType.Civilization));
            Assert.That(capturedImg.Url, Is.EqualTo("https://img"));
            Assert.That(capturedImg.CivilizationId, Is.EqualTo(capturedCiv.Id));

            civRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CreateAsync_DoesNotAddImage_WhenImageUrlWhitespace()
        {
            var dto = new CivilizationFormDto
            {
                Name = "Rome",
                Description = "Desc",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 2,
                EraId = Guid.NewGuid(),
                ImageUrl = "   "
            };

            civRepo.Setup(r => r.AddAsync(It.IsAny<Civilization>())).Returns(Task.CompletedTask);
            civRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await service.CreateAsync(dto);

            imgRepo.Verify(r => r.AddAsync(It.IsAny<Image>()), Times.Never);
        }


        [Test]
        public async Task GetForEditAsync_ReturnsNull_WhenNotFound()
        {
            await using var db = CreateDb();
            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            var res = await service.GetForEditAsync(Guid.NewGuid());
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsFormDto_WithImageUrl()
        {
            await using var db = CreateDb();

            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = "Rome",
                Description = "Desc",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 2,
                EraId = era.Id,
                Era = era
            };

            db.AddRange(era, civ);

            db.Images.Add(new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Civilization,
                CivilizationId = civ.Id,
                Url = "https://img/rome"
            });

            await db.SaveChangesAsync();

            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            var res = await service.GetForEditAsync(civ.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Name, Is.EqualTo("Rome"));
            Assert.That(res.ImageUrl, Is.EqualTo("https://img/rome"));
        }

        [Test]
        public void UpdateAsync_Throws_WhenEndYearBeforeStartYear()
        {
            var dto = new CivilizationFormDto
            {
                Name = "C",
                Description = "D",
                Type = CivilizationType.Kingdom,
                StartYear = 100,
                EndYear = 50,
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => service.UpdateAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public async Task UpdateAsync_Throws_WhenCivilizationNotFound()
        {
            await using var db = CreateDb();
            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            var dto = new CivilizationFormDto
            {
                Name = "C",
                Description = "D",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 2,
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public async Task UpdateAsync_WhenUrlBecomesWhitespace_DeletesExistingImage()
        {
            await using var db = CreateDb();

            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = "Rome",
                Description = "Desc",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 2,
                EraId = era.Id,
                Era = era
            };

            var existingImg = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Civilization,
                CivilizationId = civ.Id,
                Url = "https://old"
            };

            db.AddRange(era, civ, existingImg);
            await db.SaveChangesAsync();

            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            Image? deleted = null;
            imgRepo.Setup(r => r.Delete(It.IsAny<Image>()))
                .Callback<Image>(i => deleted = i);

            civRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dto = new CivilizationFormDto
            {
                Name = "  New ",
                Description = "  ND ",
                Type = CivilizationType.Kingdom,
                StartYear = 1,
                EndYear = 2,
                EraId = era.Id,
                ImageUrl = "   "
            };

            await service.UpdateAsync(civ.Id, dto);

            Assert.That(deleted, Is.Not.Null);
            Assert.That(deleted!.Id, Is.EqualTo(existingImg.Id));

            var dbCiv = await db.Civilizations.FirstAsync(x => x.Id == civ.Id);
            Assert.That(dbCiv.Name, Is.EqualTo("New"));
        }

        [Test]
        public async Task UpdateAsync_WhenNoExistingImage_AndUrlProvided_AddsImage()
        {
            await using var db = CreateDb();

            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = "Rome",
                Description = "Desc",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 2,
                EraId = era.Id,
                Era = era
            };

            db.AddRange(era, civ);
            await db.SaveChangesAsync();

            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            Image? added = null;
            imgRepo.Setup(r => r.AddAsync(It.IsAny<Image>()))
                .Callback<Image>(i => added = i)
                .Returns(Task.CompletedTask);

            civRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dto = new CivilizationFormDto
            {
                Name = "Rome",
                Description = "Desc",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 2,
                EraId = era.Id,
                ImageUrl = " https://new "
            };

            await service.UpdateAsync(civ.Id, dto);

            Assert.That(added, Is.Not.Null);
            Assert.That(added!.Url, Is.EqualTo("https://new"));
            Assert.That(added.TargetType, Is.EqualTo(ImageTargetType.Civilization));
            Assert.That(added.CivilizationId, Is.EqualTo(civ.Id));
        }

        [Test]
        public async Task UpdateAsync_WhenExistingImage_AndUrlProvided_UpdatesExistingImage()
        {
            await using var db = CreateDb();

            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = "Rome",
                Description = "Desc",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 2,
                EraId = era.Id,
                Era = era
            };

            var existingImg = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Civilization,
                CivilizationId = civ.Id,
                Url = "https://old"
            };

            db.AddRange(era, civ, existingImg);
            await db.SaveChangesAsync();

            civRepo.Setup(r => r.Query()).Returns(db.Civilizations);
            imgRepo.Setup(r => r.Query()).Returns(db.Images);

            civRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dto = new CivilizationFormDto
            {
                Name = "Rome",
                Description = "Desc",
                Type = CivilizationType.Empire,
                StartYear = 1,
                EndYear = 2,
                EraId = era.Id,
                ImageUrl = " https://updated "
            };

            await service.UpdateAsync(civ.Id, dto);

            var img = await db.Images.FirstAsync(i => i.CivilizationId == civ.Id && i.TargetType == ImageTargetType.Civilization);
            Assert.That(img.Url, Is.EqualTo("https://updated"));
        }

        [Test]
        public async Task DeleteAsync_WhenCivilizationNotFound_DoesNotDeleteCivilization_ButDeletesImages()
        {
            await using var db = CreateDb();

            var civId = Guid.NewGuid();
            db.Images.Add(new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Civilization,
                CivilizationId = civId,
                Url = "https://img"
            });
            await db.SaveChangesAsync();

            imgRepo.Setup(r => r.Query()).Returns(db.Images);
            civRepo.Setup(r => r.GetByIdTrackedAsync(civId)).ReturnsAsync((Civilization?)null);

            int deleteImagesCount = 0;
            imgRepo.Setup(r => r.Delete(It.IsAny<Image>()))
                .Callback(() => deleteImagesCount++);

            await service.DeleteAsync(civId);

            Assert.That(deleteImagesCount, Is.EqualTo(1));
            civRepo.Verify(r => r.Delete(It.IsAny<Civilization>()), Times.Never);
            civRepo.Verify(r => r.SaveChangesAsync(), Times.Never); 
        }

        [Test]
        public async Task DeleteAsync_WhenFound_DeletesImages_DeletesCivilization_AndSaves()
        {
            await using var db = CreateDb();

            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ = new Civilization
            {
                Id = Guid.NewGuid(),
                Name = "C",
                Description = "D",
                Type = CivilizationType.Kingdom,
                StartYear = 1,
                EndYear = 2,
                EraId = era.Id,
                Era = era
            };

            db.AddRange(era, civ);
            db.Images.AddRange(
                new Image { Id = Guid.NewGuid(), TargetType = ImageTargetType.Civilization, CivilizationId = civ.Id, Url = "https://1" },
                new Image { Id = Guid.NewGuid(), TargetType = ImageTargetType.Civilization, CivilizationId = civ.Id, Url = "https://2" }
            );

            await db.SaveChangesAsync();

            imgRepo.Setup(r => r.Query()).Returns(db.Images);
            civRepo.Setup(r => r.GetByIdTrackedAsync(civ.Id)).ReturnsAsync(civ);

            int deletedImages = 0;
            imgRepo.Setup(r => r.Delete(It.IsAny<Image>()))
                .Callback(() => deletedImages++);

            civRepo.Setup(r => r.Delete(It.IsAny<Civilization>()));
            civRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await service.DeleteAsync(civ.Id);

            Assert.That(deletedImages, Is.EqualTo(2));
            civRepo.Verify(r => r.Delete(civ), Times.Once);
            civRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}