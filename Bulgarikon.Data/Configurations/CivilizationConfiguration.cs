using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class CivilizationConfiguration : IEntityTypeConfiguration<Civilization>
    {
        public void Configure(EntityTypeBuilder<Civilization> builder)
        {
            builder.HasData(
                new Civilization
                {
                    Id = SeededIds.Civ_Thracians,
                    Name = "Траки",
                    Description = "Древно население на Балканите, оставило богато културно наследство и значими археологически находки.",
                    Type = CivilizationType.Kingdom,
                    StartYear = -1500,
                    EndYear = 46,
                    ImageUrl = null,
                    EraId = SeededIds.Era_Antiquity
                },
                new Civilization
                {
                    Id = SeededIds.Civ_Bulgars,
                    Name = "Прабългари",
                    Description = "Народ със степен произход, който участва в създаването на българската държава и оформянето на ранната българска народност.",
                    Type = CivilizationType.Kingdom,
                    StartYear = 600,
                    EndYear = 900,
                    ImageUrl = null,
                    EraId = SeededIds.Era_FirstBulgarianEmpire
                },
                new Civilization
                {
                    Id = SeededIds.Civ_Slavs,
                    Name = "Славяни",
                    Description = "Славянските племена се заселват на Балканите и имат ключова роля в етногенезиса и културата на региона.",
                    Type = CivilizationType.Unknown,
                    StartYear = 500,
                    EndYear = 900,
                    ImageUrl = null,
                    EraId = SeededIds.Era_MigrationPeriod
                },
                new Civilization
                {
                    Id = SeededIds.Civ_Byzantines,
                    Name = "Византийска империя",
                    Description = "Източната Римска империя – доминираща сила в Източното Средиземноморие и важен съперник/партньор на България.",
                    Type = CivilizationType.Empire,
                    StartYear = 330,
                    EndYear = 1453,
                    ImageUrl = null,
                    EraId = SeededIds.Era_FirstBulgarianEmpire
                },
                new Civilization
                {
                    Id = SeededIds.Civ_Ottomans,
                    Name = "Османска империя",
                    Description = "Държавата, под чиято власт българските земи остават до Освобождението през 1878 г.",
                    Type = CivilizationType.ForeignPower,
                    StartYear = 1299,
                    EndYear = 1922,
                    ImageUrl = null,
                    EraId = SeededIds.Era_OttomanPeriod
                }
            );
        }
    }
}