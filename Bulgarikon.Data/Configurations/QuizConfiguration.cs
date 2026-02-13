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
                    Title = "Античност: основни знания",
                    EraId = SeededIds.Era_Antiquity
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_Antiquity_2,
                    Title = "Античност: култура и общество",
                    EraId = SeededIds.Era_Antiquity
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_FirstEmpire_1,
                    Title = "Първо българско царство: ключови факти",
                    EraId = SeededIds.Era_FirstBulgarianEmpire
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_SecondEmpire_1,
                    Title = "Второ българско царство: владетели и събития",
                    EraId = SeededIds.Era_SecondBulgarianEmpire
                },
                new Quiz
                {
                    Id = SeededIds.Quiz_OttomanPeriod_1,
                    Title = "Османско владичество: борби и възраждане",
                    EraId = SeededIds.Era_OttomanPeriod
                }
            );
        }
    }
}