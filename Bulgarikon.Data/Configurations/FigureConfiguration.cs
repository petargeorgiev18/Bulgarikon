using Bulgarikon.Data.Models;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class FigureConfiguration : IEntityTypeConfiguration<Figure>
    {
        public void Configure(EntityTypeBuilder<Figure> builder)
        {
            builder.HasData(
                new Figure
                {
                    Id = SeededIds.Figure_KhanAsparuh,
                    Name = "Хан Аспарух",
                    Description = "Основател на Дунавска България. Свързан с утвърждаването на българската държава на Балканите.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 640,
                    DeathYear = 701,
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_TzarSimeon,
                    Name = "Цар Симеон I Велики",
                    Description = "Владетел, при когото България достига значителен политически и културен подем (Златен век).",
                    BirthDate = null, 
                    DeathDate = new DateTime(927, 5, 27),
                    ImageUrl = null,
                    BirthYear = 864,
                    DeathYear = 927,
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = null
                },
                new Figure
                {
                    Id = SeededIds.Figure_Kaloyan,
                    Name = "Цар Калоян",
                    Description = "Владетел на Второто българско царство. Провежда активна външна политика и укрепва държавата.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1168,
                    DeathYear = 1207,
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = null
                },
                new Figure
                {
                    Id = SeededIds.Figure_Levski,
                    Name = "Васил Левски",
                    Description = "Апостол на свободата. Организатор на вътрешната революционна мрежа за освобождението на България.",
                    BirthDate = new DateTime(1837, 7, 18),
                    DeathDate = new DateTime(1873, 2, 18),
                    ImageUrl = null,
                    BirthYear = 1837,
                    DeathYear = 1873,
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = null
                },
                new Figure
                {
                    Id = SeededIds.Figure_Botev,
                    Name = "Христо Ботев",
                    Description = "Поет и революционер. Символ на борбата за национално освобождение през Възраждането.",
                    BirthDate = new DateTime(1848, 1, 6),
                    DeathDate = new DateTime(1876, 6, 2),
                    ImageUrl = null,
                    BirthYear = 1848,
                    DeathYear = 1876,
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = null
                }
            );
        }
    }
}