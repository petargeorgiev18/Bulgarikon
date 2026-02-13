using Bulgarikon.Data.Models;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class EraConfiguration : IEntityTypeConfiguration<Era>
    {
        public void Configure(EntityTypeBuilder<Era> builder)
        {
            builder.HasData(
                new Era
                {
                    Id = SeededIds.Era_Antiquity,
                    Name = "Античност",
                    Description = "Античността обхваща най-ранните периоди от човешката история и възхода на древните цивилизации. Характерна е с развитието на земеделие, градове, писменост и ранни форми на държавност.",
                    StartYear = -3000,
                    EndYear = 500
                },
                new Era
                {
                    Id = SeededIds.Era_MigrationPeriod,
                    Name = "Велико преселение на народите",
                    Description = "Период на значителни миграции в Европа (IV–VII век), свързан с разместване на племена и народи и с упадъка на Западната Римска империя.",
                    StartYear = 300,
                    EndYear = 700
                },
                new Era
                {
                    Id = SeededIds.Era_FirstBulgarianEmpire,
                    Name = "Първо българско царство",
                    Description = "Държавата, утвърдена след 681 г., се превръща във водеща сила на Балканите. Периодът е белязан от християнизацията и културния разцвет, особено при цар Симеон Велики.",
                    StartYear = 681,
                    EndYear = 1018
                },
                new Era
                {
                    Id = SeededIds.Era_SecondBulgarianEmpire,
                    Name = "Второ българско царство",
                    Description = "Възстановено през 1185 г., Второто българско царство преживява културен подем и териториално разширение, като значим владетел е цар Иван Асен II. Периодът завършва с османското завоевание.",
                    StartYear = 1185,
                    EndYear = 1396
                },
                new Era
                {
                    Id = SeededIds.Era_OttomanPeriod,
                    Name = "Османско владичество",
                    Description = "Периодът на османска власт над българските земи (края на XIV век – 1878 г.) е време на дълбоки социални и културни промени, но и на съпротива и националноосвободителни движения.",
                    StartYear = 1396,
                    EndYear = 1878
                }
            );
        }
    }
}