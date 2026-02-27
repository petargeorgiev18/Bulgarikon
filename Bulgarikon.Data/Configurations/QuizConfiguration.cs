using Bulgarikon.Data.Models;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasData(
                new Quiz
                {
                    Id = SeededIds.Quiz_Antiquity_1,
                    Title = "Античност: Траки и римско наследство",
                    EraId = SeededIds.Era_Antiquity
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_Antiquity_2,
                    Title = "Античност: владетели и паметници",
                    EraId = SeededIds.Era_Antiquity
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_FirstEmpire_1,
                    Title = "Първо българско царство: владетели и битки",
                    EraId = SeededIds.Era_FirstBulgarianEmpire
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_SecondEmpire_1,
                    Title = "Второ българско царство: възстановяване и разцвет",
                    EraId = SeededIds.Era_SecondBulgarianEmpire
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_OttomanPeriod_1,
                    Title = "Османско владичество: съпротива и оцеляване",
                    EraId = SeededIds.Era_OttomanPeriod
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_Revival_1,
                    Title = "Българско възраждане: просвета и борба",
                    EraId = SeededIds.Era_Revival
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_Revival_2,
                    Title = "Революционни борби и Освобождение",
                    EraId = SeededIds.Era_Revival
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_Modern_1,
                    Title = "Трета българска държава",
                    EraId = SeededIds.Era_Modern
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_Modern_2,
                    Title = "Социализъм и демокрация",
                    EraId = SeededIds.Era_Modern
                }
            );
        }
    }
}