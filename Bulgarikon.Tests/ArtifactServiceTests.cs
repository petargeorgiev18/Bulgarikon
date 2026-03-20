using System;
using System.Linq;
using System.Threading.Tasks;
using Bulgarikon.Core.DTOs.ArtifactDTOs;
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
    public class ArtifactServiceTests
    {
        private Mock<IRepository<Artifact, Guid>> repo = null!;
        private Mock<ICloudinaryService> cloudinaryService = null!;
        private BulgarikonDbContext db = null!;
        private ArtifactService service = null!;

        [SetUp]
        public void SetUp()
        {
            repo = new Mock<IRepository<Artifact, Guid>>();
            cloudinaryService = new Mock<ICloudinaryService>();

            db = CreateDb();

            service = new ArtifactService(repo.Object, db, cloudinaryService.Object);

            repo.Setup(r => r.Delete(It.IsAny<Artifact>()))
                .Callback<Artifact>(a => db.Artifacts.Remove(a));
        }

        [TearDown]
        public async Task TearDown()
        {
            await db.DisposeAsync();
        }

        private static BulgarikonDbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("ArtifactServiceTests_" + Guid.NewGuid())
                .Options;

            return new BulgarikonDbContext(options);
        }

        [Test]
        public async Task GetByEraAsync_FiltersByEra_AndOrders()
        {
            var era1 = new Era { Id = Guid.NewGuid(), Name = "Era1", StartYear = 1, EndYear = 2 };
            var era2 = new Era { Id = Guid.NewGuid(), Name = "Era2", StartYear = 1, EndYear = 2 };

            db.Eras.AddRange(era1, era2);

            db.Artifacts.AddRange(
                new Artifact
                {
                    Id = Guid.NewGuid(),
                    Name = "B",
                    Year = 10,
                    EraId = era1.Id,
                    Era = era1,
                    Material = "M",
                    Location = "L",
                    DiscoveredAt = DateTime.UtcNow,
                    Description = "D"
                },
                new Artifact
                {
                    Id = Guid.NewGuid(),
                    Name = "A",
                    Year = 5,
                    EraId = era1.Id,
                    Era = era1,
                    Material = "M",
                    Location = "L",
                    DiscoveredAt = DateTime.UtcNow,
                    Description = "D"
                },
                new Artifact
                {
                    Id = Guid.NewGuid(),
                    Name = "X",
                    Year = 1,
                    EraId = era2.Id,
                    Era = era2,
                    Material = "M",
                    Location = "L",
                    DiscoveredAt = DateTime.UtcNow,
                    Description = "D"
                }
            );

            await db.SaveChangesAsync();

            repo.Setup(r => r.Query()).Returns(db.Artifacts);

            var result = (await service.GetByEraAsync(era1.Id)).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("A"));
            Assert.That(result[1].Name, Is.EqualTo("B"));
        }

        [Test]
        public async Task GetByEraAsync_FiltersByCivilization_WhenProvided()
        {
            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ1 = new Civilization { Id = Guid.NewGuid(), Name = "C1", Description = "D", Type = 0, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };
            var civ2 = new Civilization { Id = Guid.NewGuid(), Name = "C2", Description = "D", Type = 0, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };

            db.AddRange(era, civ1, civ2);

            db.Artifacts.AddRange(
                new Artifact
                {
                    Id = Guid.NewGuid(),
                    Name = "A1",
                    EraId = era.Id,
                    Era = era,
                    CivilizationId = civ1.Id,
                    Civilization = civ1,
                    Material = "M",
                    Location = "L",
                    DiscoveredAt = DateTime.UtcNow,
                    Description = "D"
                },
                new Artifact
                {
                    Id = Guid.NewGuid(),
                    Name = "A2",
                    EraId = era.Id,
                    Era = era,
                    CivilizationId = civ2.Id,
                    Civilization = civ2,
                    Material = "M",
                    Location = "L",
                    DiscoveredAt = DateTime.UtcNow,
                    Description = "D"
                }
            );

            await db.SaveChangesAsync();

            repo.Setup(r => r.Query()).Returns(db.Artifacts);

            var result = (await service.GetByEraAsync(era.Id, civ1.Id)).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("A1"));
            Assert.That(result[0].CivilizationName, Is.EqualTo("C1"));
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotFound()
        {
            repo.Setup(r => r.Query()).Returns(db.Artifacts);

            var result = await service.GetDetailsAsync(Guid.NewGuid());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsDto_WhenFound()
        {
            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };
            var civ = new Civilization { Id = Guid.NewGuid(), Name = "Civ", Description = "D", Type = 0, StartYear = 1, EndYear = 2, EraId = era.Id, Era = era };

            var artifact = new Artifact
            {
                Id = Guid.NewGuid(),
                Name = "Art",
                Description = "Desc",
                Material = "Gold",
                Location = "Sofia",
                DiscoveredAt = new DateTime(2020, 1, 1),
                Year = 100,
                EraId = era.Id,
                Era = era,
                CivilizationId = civ.Id,
                Civilization = civ,
                ImageUrl = "https://img"
            };

            db.AddRange(era, civ, artifact);
            await db.SaveChangesAsync();

            repo.Setup(r => r.Query()).Returns(db.Artifacts);

            var result = await service.GetDetailsAsync(artifact.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Art"));
            Assert.That(result.EraName, Is.EqualTo("Era"));
            Assert.That(result.CivilizationName, Is.EqualTo("Civ"));
            Assert.That(result.ImageUrl, Is.EqualTo("https://img"));
        }

        [Test]
        public async Task GetForEditAsync_ReturnsNull_WhenNotFound()
        {
            repo.Setup(r => r.Query()).Returns(db.Artifacts);

            var result = await service.GetForEditAsync(Guid.NewGuid());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsFormDto_WhenFound()
        {
            var era = new Era { Id = Guid.NewGuid(), Name = "Era", StartYear = 1, EndYear = 2 };

            var artifact = new Artifact
            {
                Id = Guid.NewGuid(),
                Name = "Art",
                Description = "Desc",
                Material = "Gold",
                Location = "Sofia",
                DiscoveredAt = new DateTime(2020, 1, 1),
                Year = 100,
                EraId = era.Id,
                Era = era,
                ImageUrl = "https://img"
            };

            db.AddRange(era, artifact);
            await db.SaveChangesAsync();

            repo.Setup(r => r.Query()).Returns(db.Artifacts);

            var result = await service.GetForEditAsync(artifact.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Art"));
            Assert.That(result.ImageUrl, Is.EqualTo("https://img"));
        }

        [Test]
        public async Task CreateAsync_Trims_AndCallsAddAndSave_WhenUrlIsUsed()
        {
            var dto = new ArtifactFormDto
            {
                Name = "  N  ",
                Description = "  D  ",
                Material = "  Mat ",
                Location = "  Loc ",
                DiscoveredAt = DateTime.Today,
                EraId = Guid.NewGuid(),
                ImageUrl = "  https://img  "
            };

            Artifact? captured = null;

            repo.Setup(r => r.AddAsync(It.IsAny<Artifact>()))
                .Callback<Artifact>(a =>
                {
                    captured = a;
                    db.Artifacts.Add(a);
                })
                .Returns(Task.CompletedTask);

            var id = await service.CreateAsync(dto);

            Assert.That(id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(captured, Is.Not.Null);
            Assert.That(captured!.Name, Is.EqualTo("N"));
            Assert.That(captured.Description, Is.EqualTo("D"));
            Assert.That(captured.Material, Is.EqualTo("Mat"));
            Assert.That(captured.Location, Is.EqualTo("Loc"));
            Assert.That(captured.ImageUrl, Is.EqualTo("https://img"));
        }

        [Test]
        public void CreateAsync_Throws_WhenDiscoveredAtInFuture()
        {
            var dto = new ArtifactFormDto
            {
                Name = "N",
                Description = "D",
                Material = "M",
                Location = "L",
                DiscoveredAt = DateTime.Today.AddDays(1),
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(
                () => service.CreateAsync(dto));
        }

        [Test]
        public async Task UpdateAsync_Throws_WhenNotFound()
        {
            var id = Guid.NewGuid();

            var dto = new ArtifactFormDto
            {
                Name = "N",
                Description = "D",
                Material = "M",
                Location = "L",
                DiscoveredAt = DateTime.Today,
                EraId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await service.UpdateAsync(id, dto));
        }

        [Test]
        public async Task UpdateAsync_UpdatesFields_AndClearsImage_WhenUrlEmptyAndNoFile()
        {
            var id = Guid.NewGuid();

            var existing = new Artifact
            {
                Id = id,
                Name = "Old",
                Description = "Old",
                Material = "Old",
                Location = "Old",
                DiscoveredAt = DateTime.Today,
                EraId = Guid.NewGuid(),
                ImageUrl = "https://old"
            };

            db.Artifacts.Add(existing);
            await db.SaveChangesAsync();

            var dto = new ArtifactFormDto
            {
                Name = "  New ",
                Description = "  ND ",
                Material = "  NM ",
                Location = "  NL ",
                DiscoveredAt = DateTime.Today,
                EraId = Guid.NewGuid(),
                ImageUrl = "   "
            };

            await service.UpdateAsync(id, dto);

            var updated = await db.Artifacts.FirstAsync(x => x.Id == id);

            Assert.That(updated.Name, Is.EqualTo("New"));
            Assert.That(updated.Description, Is.EqualTo("ND"));
            Assert.That(updated.Material, Is.EqualTo("NM"));
            Assert.That(updated.Location, Is.EqualTo("NL"));
            Assert.That(updated.ImageUrl, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_WhenNotFound_DoesNothing()
        {
            var id = Guid.NewGuid();

            await service.DeleteAsync(id);

            Assert.Pass();
        }

        [Test]
        public async Task DeleteAsync_WhenFound_DeletesAndSaves()
        {
            var id = Guid.NewGuid();
            var entity = new Artifact
            {
                Id = id,
                Name = "Art",
                Description = "Desc",
                Material = "Stone",
                Location = "Loc",
                DiscoveredAt = DateTime.Today,
                EraId = Guid.NewGuid()
            };

            db.Artifacts.Add(entity);
            await db.SaveChangesAsync();

            await service.DeleteAsync(id);

            var exists = await db.Artifacts.AnyAsync(x => x.Id == id);
            Assert.That(exists, Is.False);
        }
    }
}