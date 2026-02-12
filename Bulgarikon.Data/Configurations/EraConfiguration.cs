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
                    Name = "Ancient Era",
                    Description = "The Ancient Era encompasses the earliest periods of human history, including the rise of civilizations such as Mesopotamia, Ancient Egypt, and the Indus Valley. It is characterized by the development of writing, agriculture, and early forms of government.",
                    StartYear = -3000,
                    EndYear = 500
                },
                new Era
                {
                    Id = SeededIds.Era_MigrationPeriod,
                    Name = "Migration Period",
                    Description = "The Migration Period, also known as the Barbarian Invasions, was a time of significant population movements in Europe, particularly during the decline of the Western Roman Empire. It saw the migration of various tribes such as the Goths, Vandals, and Huns.",
                    StartYear = 300,
                    EndYear = 700
                },
                new Era
                {
                    Id = SeededIds.Era_FirstBulgarianEmpire,
                    Name = "First Bulgarian Empire",
                    Description = "The First Bulgarian Empire was established in 681 AD and lasted until 1018 AD. It was a powerful state in the Balkans, known for its cultural achievements and military prowess, particularly under the rule of Khan Asparuh and Tsar Simeon the Great.",
                    StartYear = 681,
                    EndYear = 1018
                },
                new Era
                {
                    Id = SeededIds.Era_SecondBulgarianEmpire,
                    Name = "Second Bulgarian Empire",
                    Description = "The Second Bulgarian Empire was established in 1185 AD and lasted until 1396 AD. It was a period of cultural revival and territorial expansion, particularly under the rule of Tsar Ivan Asen II. The empire eventually fell to the Ottoman Turks.",
                    StartYear = 1185,
                    EndYear = 1396
                },
                new Era
                {
                    Id = SeededIds.Era_OttomanPeriod,
                    Name = "Ottoman Period",
                    Description = "The Ottoman Period in Bulgarian history refers to the time when Bulgaria was under Ottoman rule, from 1396 AD until the liberation in 1878 AD. This era was marked by significant social, economic, and cultural changes, as well as resistance and uprisings against Ottoman authority.",
                    StartYear = 1396,
                    EndYear = 1878
                }
            );
        }
    }
}
