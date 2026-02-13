using Bulgarikon.Data.Models;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasData(
                new Event
                {
                    Id = SeededIds.Event_BattleOngal,
                    Title = "Битката при Онгъла",
                    Description = "През 680 г. хан Аспарух разгромява византийската армия на император Константин IV в укрепения лагер при Онгъла. Победата води до признаването на българската държава през 681 г.",
                    Location = "Онгъла (Северна Добруджа)",
                    StartYear = 680,
                    EndYear = 680,
                    EraId = SeededIds.Era_FirstBulgarianEmpire
                },
                new Event
                {
                    Id = SeededIds.Event_BattleVarbishkiProhod,
                    Title = "Битката във Върбишкия проход",
                    Description = "Сражение, при което хан Крум нанася тежко поражение на византийците след похода на Никифор I.",
                    Location = "Върбишки проход",
                    StartYear = 811,
                    EndYear = 811,
                    EraId = SeededIds.Era_FirstBulgarianEmpire
                },
                new Event
                {
                    Id = SeededIds.Event_FallTurnovo,
                    Title = "Падането на Търново",
                    Description = "Превземането на столицата Търново от османците – ключов момент в падането на Второто българско царство.",
                    Location = "Търново",
                    StartYear = 1393,
                    EndYear = 1393,
                    EraId = SeededIds.Era_SecondBulgarianEmpire
                },
                new Event
                {
                    Id = SeededIds.Event_AprilUprising,
                    Title = "Априлско въстание",
                    Description = "Въстание срещу османската власт, което предизвиква международен отзвук и подкрепя каузата за освобождение.",
                    Location = "Българските земи",
                    StartYear = 1876,
                    EndYear = 1876,
                    EraId = SeededIds.Era_OttomanPeriod
                },
                new Event
                {
                    Id = SeededIds.Event_Union1885,
                    Title = "Съединението на България",
                    Description = "Обединяване на Княжество България и Източна Румелия – ключов акт за националното обединение.",
                    Location = "Пловдив",
                    StartYear = 1885,
                    EndYear = 1885,
                    EraId = SeededIds.Era_OttomanPeriod
                }
            );
        }
    }
}