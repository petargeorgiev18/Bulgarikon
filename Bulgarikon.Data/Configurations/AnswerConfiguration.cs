using Bulgarikon.Data.Models;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasData(
                // Q1
                new Answer { Id = SeededIds.Answer_1, QuestionId = SeededIds.Question_1, Text = "Траки", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_2, QuestionId = SeededIds.Question_1, Text = "Викинги", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_3, QuestionId = SeededIds.Question_1, Text = "Ацтеки", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_4, QuestionId = SeededIds.Question_1, Text = "Самураи", IsCorrect = false },

                // Q2
                new Answer { Id = SeededIds.Answer_5, QuestionId = SeededIds.Question_2, Text = "Развитие на писменост и държавност", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_6, QuestionId = SeededIds.Question_2, Text = "Интернет и електронна търговия", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_7, QuestionId = SeededIds.Question_2, Text = "Космически полети", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_8, QuestionId = SeededIds.Question_2, Text = "Съвременни парламенти", IsCorrect = false },

                // Q3
                new Answer { Id = SeededIds.Answer_9, QuestionId = SeededIds.Question_3, Text = "Съхранява знание и улеснява управлението", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_10, QuestionId = SeededIds.Question_3, Text = "Замества земеделието", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_11, QuestionId = SeededIds.Question_3, Text = "Премахва нуждата от закони", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_12, QuestionId = SeededIds.Question_3, Text = "Забранява търговията", IsCorrect = false },

                // Q4
                new Answer { Id = SeededIds.Answer_13, QuestionId = SeededIds.Question_4, Text = "Площади, обществени сгради и пазари", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_14, QuestionId = SeededIds.Question_4, Text = "Небостъргачи от стъкло", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_15, QuestionId = SeededIds.Question_4, Text = "Метро линии", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_16, QuestionId = SeededIds.Question_4, Text = "Магистрали и летища", IsCorrect = false },

                // Q5
                new Answer { Id = SeededIds.Answer_17, QuestionId = SeededIds.Question_5, Text = "681 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_18, QuestionId = SeededIds.Question_5, Text = "1018 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_19, QuestionId = SeededIds.Question_5, Text = "1185 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_20, QuestionId = SeededIds.Question_5, Text = "1878 г.", IsCorrect = false },

                // Q6
                new Answer { Id = SeededIds.Answer_21, QuestionId = SeededIds.Question_6, Text = "Цар Симеон Велики", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_22, QuestionId = SeededIds.Question_6, Text = "Паисий Хилендарски", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_23, QuestionId = SeededIds.Question_6, Text = "Васил Левски", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_24, QuestionId = SeededIds.Question_6, Text = "Христо Ботев", IsCorrect = false },

                // Q7
                new Answer { Id = SeededIds.Answer_25, QuestionId = SeededIds.Question_7, Text = "1185 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_26, QuestionId = SeededIds.Question_7, Text = "681 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_27, QuestionId = SeededIds.Question_7, Text = "1396 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_28, QuestionId = SeededIds.Question_7, Text = "330 г.", IsCorrect = false },

                // Q8
                new Answer { Id = SeededIds.Answer_29, QuestionId = SeededIds.Question_8, Text = "Иван Асен II", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_30, QuestionId = SeededIds.Question_8, Text = "Кубрат", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_31, QuestionId = SeededIds.Question_8, Text = "Аспарух", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_32, QuestionId = SeededIds.Question_8, Text = "Крум", IsCorrect = false },

                // Q9
                new Answer { Id = SeededIds.Answer_33, QuestionId = SeededIds.Question_9, Text = "Априлско въстание", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_34, QuestionId = SeededIds.Question_9, Text = "Основаване на Рим", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_35, QuestionId = SeededIds.Question_9, Text = "Откриване на Америка", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_36, QuestionId = SeededIds.Question_9, Text = "Френска революция", IsCorrect = false },

                // Q10
                new Answer { Id = SeededIds.Answer_37, QuestionId = SeededIds.Question_10, Text = "1878 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_38, QuestionId = SeededIds.Question_10, Text = "1396 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_39, QuestionId = SeededIds.Question_10, Text = "1018 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_40, QuestionId = SeededIds.Question_10, Text = "1453 г.", IsCorrect = false }
            );
        }
    }
}