using Bulgarikon.Data.Models;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class ArtifactConfiguration : IEntityTypeConfiguration<Artifact>
    {
        public void Configure(EntityTypeBuilder<Artifact> builder)
        {
            builder.HasData(
                new Artifact
                {
                    Id = SeededIds.Artifact_GoldMask,
                    Name = "Златна маска (символичен артефакт)",
                    Description = "Ритуална златна маска, свързвана с тракийски погребални практики. Използвана като символ за богатство и статус.",
                    ImageUrl = null,
                    Year = -400,
                    Material = "Злато",
                    Location = "Тракия",
                    DiscoveredAt = new DateTime(2012, 5, 14),
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_Sword,
                    Name = "Меч (ранносредновековен)",
                    Description = "Оръжие от ранното Средновековие. Използвано в бойни сблъсъци и като знак за воинска принадлежност.",
                    ImageUrl = null,
                    Year = 800,
                    Material = "Желязо",
                    Location = "Североизточна България",
                    DiscoveredAt = new DateTime(2008, 9, 2),
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_Inscription,
                    Name = "Каменен надпис",
                    Description = "Каменен надпис, свидетелстващ за административни и културни практики в българските земи през Средновековието.",
                    ImageUrl = null,
                    Year = 900,
                    Material = "Камък",
                    Location = "Плиска",
                    DiscoveredAt = new DateTime(1999, 7, 21),
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_Ring,
                    Name = "Пръстен-печат",
                    Description = "Пръстен-печат, използван за удостоверяване на документи и принадлежност към аристокрацията.",
                    ImageUrl = null,
                    Year = 1230,
                    Material = "Сребро",
                    Location = "Търново",
                    DiscoveredAt = new DateTime(2015, 4, 3),
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = null
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_Pottery,
                    Name = "Керамичен съд",
                    Description = "Керамичен съд от бита. Използван за съхранение на храни и течности, типичен за ежедневието на епохата.",
                    ImageUrl = null,
                    Year = 1700,
                    Material = "Керамика",
                    Location = "Централна България",
                    DiscoveredAt = new DateTime(2003, 10, 18),
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = null
                }
            );
        }
    }
}