using Bulgarikon.Data.Models;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasData(
                new Question { Id = SeededIds.Question_1, QuizId = SeededIds.Quiz_Antiquity_1, Text = "Кое население е сред древните обитатели на Балканите?" },
                new Question { Id = SeededIds.Question_2, QuizId = SeededIds.Quiz_Antiquity_1, Text = "Коя е една характерна черта на античните цивилизации?" },

                new Question { Id = SeededIds.Question_3, QuizId = SeededIds.Quiz_Antiquity_2, Text = "Какво е значението на писмеността за древните общества?" },
                new Question { Id = SeededIds.Question_4, QuizId = SeededIds.Quiz_Antiquity_2, Text = "Кое е типично за античната градска култура?" },

                new Question { Id = SeededIds.Question_5, QuizId = SeededIds.Quiz_FirstEmpire_1, Text = "Коя година традиционно се приема за начало на българската държава?" },
                new Question { Id = SeededIds.Question_6, QuizId = SeededIds.Quiz_FirstEmpire_1, Text = "Кой владетел е свързан с културния разцвет през Златния век?" },
            
                new Question { Id = SeededIds.Question_7, QuizId = SeededIds.Quiz_SecondEmpire_1, Text = "Коя година се свързва с възстановяването на българската държава (1185)?" },
                new Question { Id = SeededIds.Question_8, QuizId = SeededIds.Quiz_SecondEmpire_1, Text = "Кой владетел е свързан с разширение и подем на Второто царство?" },

                new Question { Id = SeededIds.Question_9, QuizId = SeededIds.Quiz_OttomanPeriod_1, Text = "Кое събитие е част от националноосвободителните борби?" },
                new Question { Id = SeededIds.Question_10, QuizId = SeededIds.Quiz_OttomanPeriod_1, Text = "Коя година е Освобождението на България?" }
            );
        }
    }
}
