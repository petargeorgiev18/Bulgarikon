using Bulgarikon.Core.DTOs.EraDTOs;
using Bulgarikon.Core.DTOs.ImageDTOs;
using Bulgarikon.Core.Implementations;
using Bulgarikon.Core.Interfaces;
using Bulgarikon.Data;
using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Bulgarikon.Tests.Services
{
    [TestFixture]
    public class EraServiceTests
    {
        private Mock<IRepository<Era, Guid>> eraRepo = null!;
        private Mock<ICloudinaryService> cloudinaryServiceMock = null!;
        private BulgarikonDbContext db = null!;
        private EraService service = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("EraServiceTests_" + Guid.NewGuid())
                .Options;

            db = new BulgarikonDbContext(options);

            eraRepo = new Mock<IRepository<Era, Guid>>();
            cloudinaryServiceMock = new Mock<ICloudinaryService>();

            eraRepo.Setup(r => r.Query()).Returns(db.Eras);

            eraRepo.Setup(r => r.AddAsync(It.IsAny<Era>()))
                .Callback<Era>(e => db.Eras.Add(e))
                .Returns(Task.CompletedTask);

            service = new EraService(eraRepo.Object, db, cloudinaryServiceMock.Object);
        }

        [TearDown]
        public async Task TearDown()
        {
            await db.DisposeAsync();
        }

        [Test]
        public async Task GetAllAsync_ReturnsOnlyNotDeletedEras_WithOnlyEraImages()
        {
            var era1 = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era1",
                Description = "D1",
                StartYear = 1,
                EndYear = 10,
                IsDeleted = false
            };

            var era2 = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era2",
                Description = "D2",
                StartYear = 11,
                EndYear = 20,
                IsDeleted = false
            };

            var deletedEra = new Era
            {
                Id = Guid.NewGuid(),
                Name = "DeletedEra",
                Description = "Hidden",
                StartYear = 21,
                EndYear = 30,
                IsDeleted = true
            };

            db.Eras.AddRange(era1, era2, deletedEra);

            db.Images.AddRange(
                new Image
                {
                    Id = Guid.NewGuid(),
                    TargetType = ImageTargetType.Era,
                    EraId = era1.Id,
                    Url = "https://era1/1",
                    Caption = "c1",
                    SortOrder = 0
                },
                new Image
                {
                    Id = Guid.NewGuid(),
                    TargetType = ImageTargetType.Era,
                    EraId = era2.Id,
                    Url = "https://era2/1",
                    SortOrder = 0
                },
                new Image
                {
                    Id = Guid.NewGuid(),
                    TargetType = ImageTargetType.Era,
                    EraId = deletedEra.Id,
                    Url = "https://deleted/1",
                    SortOrder = 0
                });

            db.Images.Add(new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Civilization,
                CivilizationId = Guid.NewGuid(),
                Url = "https://civ/1"
            });

            await db.SaveChangesAsync();

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(x => x.Id == deletedEra.Id), Is.False);

            var r1 = result.First(x => x.Id == era1.Id);
            Assert.That(r1.Images.Count, Is.EqualTo(1));
            Assert.That(r1.Images[0].Url, Is.EqualTo("https://era1/1"));
            Assert.That(r1.Images[0].Caption, Is.EqualTo("c1"));

            var r2 = result.First(x => x.Id == era2.Id);
            Assert.That(r2.Images.Count, Is.EqualTo(1));
            Assert.That(r2.Images[0].Url, Is.EqualTo("https://era2/1"));
        }

        [Test]
        public void GetByIdAsync_Throws_WhenNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetByIdAsync(Guid.NewGuid()));
        }

        [Test]
        public void GetByIdAsync_Throws_WhenEraIsSoftDeleted()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Deleted",
                Description = "Desc",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = true
            };

            db.Eras.Add(era);
            db.SaveChanges();

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetByIdAsync(era.Id));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsEra_WithOnlyEraImages()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                Description = "Desc",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = false
            };

            db.Eras.Add(era);

            db.Images.AddRange(
                new Image
                {
                    Id = Guid.NewGuid(),
                    TargetType = ImageTargetType.Era,
                    EraId = era.Id,
                    Url = "https://era/1",
                    Caption = "cap",
                    SortOrder = 0
                },
                new Image
                {
                    Id = Guid.NewGuid(),
                    TargetType = ImageTargetType.Event,
                    EventId = Guid.NewGuid(),
                    Url = "https://event/1"
                });

            await db.SaveChangesAsync();

            var res = await service.GetByIdAsync(era.Id);

            Assert.That(res, Is.Not.Null);
            Assert.That(res!.Name, Is.EqualTo("Era"));
            Assert.That(res.Images.Count, Is.EqualTo(1));
            Assert.That(res.Images[0].Url, Is.EqualTo("https://era/1"));
            Assert.That(res.Images[0].Caption, Is.EqualTo("cap"));
        }

        [Test]
        public void CreateAsync_Throws_WhenEndBeforeStart()
        {
            var dto = new EraFormDto
            {
                Name = "Era",
                Description = "D",
                StartYear = 10,
                EndYear = 1,
                Images = new List<ImageEditDto>()
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));
        }

        [Test]
        public async Task CreateAsync_CreatesEra_TrimsFields_AddsOnlyValidImages()
        {
            var dto = new EraFormDto
            {
                Name = "  Era  ",
                Description = "  Desc  ",
                StartYear = 1,
                EndYear = 2,
                Images = new List<ImageEditDto>
                {
                    new ImageEditDto { Url = "  https://img/1  ", Caption = "  cap1  " },
                    new ImageEditDto { Url = "   ", Caption = "ignored" },
                    new ImageEditDto { Url = null, Caption = "ignored" }
                },
                ImageFiles = null
            };

            await service.CreateAsync(dto);

            var createdEra = await db.Eras.AsNoTracking().FirstOrDefaultAsync();
            Assert.That(createdEra, Is.Not.Null);
            Assert.That(createdEra!.Name, Is.EqualTo("Era"));
            Assert.That(createdEra.Description, Is.EqualTo("Desc"));
            Assert.That(createdEra.StartYear, Is.EqualTo(1));
            Assert.That(createdEra.EndYear, Is.EqualTo(2));
            Assert.That(createdEra.IsDeleted, Is.False);

            var imgs = await db.Images.AsNoTracking()
                .Where(i => i.TargetType == ImageTargetType.Era && i.EraId == createdEra.Id)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            Assert.That(imgs.Count, Is.EqualTo(1));
            Assert.That(imgs[0].Url, Is.EqualTo("https://img/1"));
            Assert.That(imgs[0].Caption, Is.EqualTo("cap1"));
            Assert.That(imgs[0].SortOrder, Is.EqualTo(0));
        }

        [Test]
        public async Task CreateAsync_WhenNoImages_DoesNotAddAnyImages()
        {
            var dto = new EraFormDto
            {
                Name = "Era",
                Description = null,
                StartYear = 1,
                EndYear = 2,
                Images = new List<ImageEditDto>(),
                ImageFiles = null
            };

            await service.CreateAsync(dto);

            var era = await db.Eras.AsNoTracking().FirstAsync();
            var imgs = await db.Images.AsNoTracking()
                .Where(i => i.TargetType == ImageTargetType.Era && i.EraId == era.Id)
                .ToListAsync();

            Assert.That(imgs.Count, Is.EqualTo(0));
        }

        [Test]
        public void Delete_Throws_WhenEraNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() => service.Delete(Guid.NewGuid()));
        }

        [Test]
        public async Task Delete_SoftDeletesEra_AndDoesNotRemoveImages()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = false
            };

            db.Eras.Add(era);

            var eraImg1 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://era/1",
                SortOrder = 0
            };

            var eraImg2 = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://era/2",
                SortOrder = 1
            };

            var otherImg = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Event,
                EventId = Guid.NewGuid(),
                Url = "https://event/1"
            };

            db.Images.AddRange(eraImg1, eraImg2, otherImg);
            await db.SaveChangesAsync();

            await service.Delete(era.Id);

            var dbEra = await db.Eras.AsNoTracking().FirstAsync(e => e.Id == era.Id);
            Assert.That(dbEra.IsDeleted, Is.True);

            var eraImgsStillThere = await db.Images.AsNoTracking()
                .CountAsync(i => i.TargetType == ImageTargetType.Era && i.EraId == era.Id);
            Assert.That(eraImgsStillThere, Is.EqualTo(2));

            var otherStillThere = await db.Images.AsNoTracking()
                .AnyAsync(i => i.Id == otherImg.Id);
            Assert.That(otherStillThere, Is.True);
        }

        [Test]
        public async Task Delete_DoesNotCallCloudinaryDelete_BecauseSoftDeleteDoesNotRemoveImages()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = false
            };

            db.Eras.Add(era);

            var eraImg = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://era/1",
                PublicId = "bulgarikon/test-image",
                SortOrder = 0
            };

            db.Images.Add(eraImg);
            await db.SaveChangesAsync();

            await service.Delete(era.Id);

            cloudinaryServiceMock.Verify(
                x => x.DeleteImageAsync(It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        public async Task Delete_WhenAlreadySoftDeleted_DoesNothing()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = true
            };

            db.Eras.Add(era);
            await db.SaveChangesAsync();

            await service.Delete(era.Id);

            var dbEra = await db.Eras.AsNoTracking().FirstAsync(e => e.Id == era.Id);
            Assert.That(dbEra.IsDeleted, Is.True);
        }

        [Test]
        public void EditAsync_Throws_WhenEndBeforeStart()
        {
            var dto = new EraFormDto
            {
                Name = "Era",
                Description = "D",
                StartYear = 10,
                EndYear = 1
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => service.EditAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public void EditAsync_Throws_WhenEraNotFound()
        {
            var dto = new EraFormDto
            {
                Name = "Era",
                Description = "D",
                StartYear = 1,
                EndYear = 2,
                Images = new List<ImageEditDto>()
            };

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.EditAsync(Guid.NewGuid(), dto));
        }

        [Test]
        public void EditAsync_Throws_WhenEraIsSoftDeleted()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "DeletedEra",
                Description = "D",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = true
            };

            db.Eras.Add(era);
            db.SaveChanges();

            var dto = new EraFormDto
            {
                Name = "Era",
                Description = "D",
                StartYear = 1,
                EndYear = 2,
                Images = new List<ImageEditDto>()
            };

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.EditAsync(era.Id, dto));
        }

        [Test]
        public async Task EditAsync_UpdatesEraFields_AndRemovesMarkedImages()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Old",
                Description = "OldDesc",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = false
            };

            var img1 = new Image
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://old/1",
                Caption = "c1",
                SortOrder = 0
            };

            var img2 = new Image
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://old/2",
                Caption = "c2",
                SortOrder = 1
            };

            db.Eras.Add(era);
            db.Images.AddRange(img1, img2);
            await db.SaveChangesAsync();

            var dto = new EraFormDto
            {
                Name = "  New  ",
                Description = "  NewDesc  ",
                StartYear = 10,
                EndYear = 20,
                Images = new List<ImageEditDto>
                {
                    new ImageEditDto { Id = img1.Id, Remove = true },
                    new ImageEditDto { Id = img2.Id, Url = "https://old/2", Caption = "c2", Remove = false }
                },
                ImageFiles = null
            };

            await service.EditAsync(era.Id, dto);

            var updatedEra = await db.Eras.AsNoTracking().FirstAsync(e => e.Id == era.Id);
            Assert.That(updatedEra.Name, Is.EqualTo("New"));
            Assert.That(updatedEra.Description, Is.EqualTo("NewDesc"));
            Assert.That(updatedEra.StartYear, Is.EqualTo(10));
            Assert.That(updatedEra.EndYear, Is.EqualTo(20));

            var imgs = await db.Images.AsNoTracking()
                .Where(i => i.TargetType == ImageTargetType.Era && i.EraId == era.Id)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            Assert.That(imgs.Count, Is.EqualTo(1));
            Assert.That(imgs[0].Id, Is.EqualTo(img2.Id));
        }

        [Test]
        public async Task EditAsync_RemovingCloudinaryImage_CallsDeleteImageAsync()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = false
            };

            var img = new Image
            {
                Id = Guid.NewGuid(),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://old/1",
                PublicId = "bulgarikon/old-public-id",
                SortOrder = 0
            };

            db.Eras.Add(era);
            db.Images.Add(img);
            await db.SaveChangesAsync();

            var dto = new EraFormDto
            {
                Name = "Era",
                Description = "",
                StartYear = 1,
                EndYear = 2,
                Images = new List<ImageEditDto>
                {
                    new ImageEditDto
                    {
                        Id = img.Id,
                        Remove = true
                    }
                },
                ImageFiles = null
            };

            await service.EditAsync(era.Id, dto);

            cloudinaryServiceMock.Verify(
                x => x.DeleteImageAsync("bulgarikon/old-public-id"),
                Times.Once);
        }

        [Test]
        public async Task EditAsync_UpdatesExistingImage_WhenProvidedIdAndNotRemoved()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = false
            };

            var img = new Image
            {
                Id = new Guid("00000000-0000-0000-0000-000000000010"),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://old",
                Caption = "oldcap",
                SortOrder = 0
            };

            db.Eras.Add(era);
            db.Images.Add(img);
            await db.SaveChangesAsync();

            var dto = new EraFormDto
            {
                Name = "Era",
                Description = null,
                StartYear = 1,
                EndYear = 2,
                Images = new List<ImageEditDto>
                {
                    new ImageEditDto
                    {
                        Id = img.Id,
                        Url = "  https://new  ",
                        Caption = "  newcap  ",
                        Remove = false
                    }
                },
                ImageFiles = null
            };

            await service.EditAsync(era.Id, dto);

            var dbImg = await db.Images.AsNoTracking().FirstAsync(i => i.Id == img.Id);
            Assert.That(dbImg.Url, Is.EqualTo("https://new"));
            Assert.That(dbImg.Caption, Is.EqualTo("newcap"));
            Assert.That(dbImg.TargetType, Is.EqualTo(ImageTargetType.Era));
            Assert.That(dbImg.EraId, Is.EqualTo(era.Id));
        }

        [Test]
        public async Task EditAsync_DoesNotUpdate_WhenUrlIsWhitespace_ForExistingImage()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = false
            };

            var img = new Image
            {
                Id = new Guid("00000000-0000-0000-0000-000000000020"),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://old",
                Caption = "oldcap",
                SortOrder = 0
            };

            db.Eras.Add(era);
            db.Images.Add(img);
            await db.SaveChangesAsync();

            var dto = new EraFormDto
            {
                Name = "Era",
                Description = null,
                StartYear = 1,
                EndYear = 2,
                Images = new List<ImageEditDto>
                {
                    new ImageEditDto
                    {
                        Id = img.Id,
                        Url = "   ",
                        Caption = "newcap",
                        Remove = false
                    }
                },
                ImageFiles = null
            };

            await service.EditAsync(era.Id, dto);

            var dbImg = await db.Images.AsNoTracking().FirstAsync(i => i.Id == img.Id);
            Assert.That(dbImg.Url, Is.EqualTo("https://old"));
            Assert.That(dbImg.Caption, Is.EqualTo("oldcap"));
        }

        [Test]
        public async Task EditAsync_AddsNewImages_WhenNoIdAndUrlProvided()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = false
            };

            db.Eras.Add(era);
            await db.SaveChangesAsync();

            var dto = new EraFormDto
            {
                Name = "Era",
                Description = null,
                StartYear = 1,
                EndYear = 2,
                Images = new List<ImageEditDto>
                {
                    new ImageEditDto { Url = "  https://new/1  ", Caption = "  c1  " },
                    new ImageEditDto { Url = "   " },
                    new ImageEditDto { Url = "https://new/2", Caption = null }
                },
                ImageFiles = null
            };

            await service.EditAsync(era.Id, dto);

            var imgs = await db.Images.AsNoTracking()
                .Where(i => i.TargetType == ImageTargetType.Era && i.EraId == era.Id)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            Assert.That(imgs.Count, Is.EqualTo(2));
            Assert.That(imgs[0].Url, Is.EqualTo("https://new/1"));
            Assert.That(imgs[0].Caption, Is.EqualTo("c1"));
            Assert.That(imgs[0].SortOrder, Is.EqualTo(0));

            Assert.That(imgs[1].Url, Is.EqualTo("https://new/2"));
            Assert.That(imgs[1].Caption, Is.Null);
            Assert.That(imgs[1].SortOrder, Is.EqualTo(1));
        }

        [Test]
        public async Task EditAsync_IgnoresUpdate_WhenIncomingIdNotFoundInExisting()
        {
            var era = new Era
            {
                Id = Guid.NewGuid(),
                Name = "Era",
                StartYear = 1,
                EndYear = 2,
                IsDeleted = false
            };

            var existingImg = new Image
            {
                Id = new Guid("00000000-0000-0000-0000-000000000030"),
                TargetType = ImageTargetType.Era,
                EraId = era.Id,
                Url = "https://old",
                Caption = "old",
                SortOrder = 0
            };

            db.Eras.Add(era);
            db.Images.Add(existingImg);
            await db.SaveChangesAsync();

            var dto = new EraFormDto
            {
                Name = "Era",
                Description = null,
                StartYear = 1,
                EndYear = 2,
                Images = new List<ImageEditDto>
                {
                    new ImageEditDto
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000031"),
                        Url = "https://should-not-apply",
                        Caption = "x",
                        Remove = false
                    }
                },
                ImageFiles = null
            };

            await service.EditAsync(era.Id, dto);

            var dbImg = await db.Images.AsNoTracking().FirstAsync(i => i.Id == existingImg.Id);
            Assert.That(dbImg.Url, Is.EqualTo("https://old"));
            Assert.That(dbImg.Caption, Is.EqualTo("old"));
        }
    }
}