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
                new Question { Id = SeededIds.Question_Antiquity_1, QuizId = SeededIds.Quiz_Antiquity_1, Text = "Кое население е сред древните обитатели на Балканите?" },
                new Question { Id = SeededIds.Question_Antiquity_2, QuizId = SeededIds.Quiz_Antiquity_1, Text = "Коя е една характерна черта на античните цивилизации?" },
                new Question { Id = SeededIds.Question_Antiquity_3, QuizId = SeededIds.Quiz_Antiquity_1, Text = "Кой тракийски владетел основава Одриското царство?" },
                new Question { Id = SeededIds.Question_Antiquity_4, QuizId = SeededIds.Quiz_Antiquity_1, Text = "Коя тракийска гробница е известна с кариатидите си?" },
                new Question { Id = SeededIds.Question_Antiquity_5, QuizId = SeededIds.Quiz_Antiquity_1, Text = "Кой римски град днес е известен като София?" },
                new Question { Id = SeededIds.Question_Antiquity_6, QuizId = SeededIds.Quiz_Antiquity_1, Text = "Коя година римляните окончателно покоряват тракийските земи?" },
                new Question { Id = SeededIds.Question_Antiquity_7, QuizId = SeededIds.Quiz_Antiquity_1, Text = "Кой легендарен певец е с тракийски произход?" },

                new Question { Id = SeededIds.Question_Antiquity_8, QuizId = SeededIds.Quiz_Antiquity_2, Text = "Какво е значението на писмеността за древните общества?" },
                new Question { Id = SeededIds.Question_Antiquity_9, QuizId = SeededIds.Quiz_Antiquity_2, Text = "Кое е типично за античната градска култура?" },
                new Question { Id = SeededIds.Question_Antiquity_10, QuizId = SeededIds.Quiz_Antiquity_2, Text = "Кой тракийски владетел основава Севтополис?" },
                new Question { Id = SeededIds.Question_Antiquity_11, QuizId = SeededIds.Quiz_Antiquity_2, Text = "Кое тракийско съкровище е открито край Панагюрище?" },
                new Question { Id = SeededIds.Question_Antiquity_12, QuizId = SeededIds.Quiz_Antiquity_2, Text = "Кой римски император има тракийски произход?" },
                new Question { Id = SeededIds.Question_Antiquity_13, QuizId = SeededIds.Quiz_Antiquity_2, Text = "Какво представлява Орфизмът?" },
                new Question { Id = SeededIds.Question_Antiquity_14, QuizId = SeededIds.Quiz_Antiquity_2, Text = "Коя година е Миланският едикт?" },

                new Question { Id = SeededIds.Question_FirstEmpire_1, QuizId = SeededIds.Quiz_FirstEmpire_1, Text = "Коя година традиционно се приема за начало на българската държава?" },
                new Question { Id = SeededIds.Question_FirstEmpire_2, QuizId = SeededIds.Quiz_FirstEmpire_1, Text = "Кой владетел е свързан с културния разцвет през Златния век?" },
                new Question { Id = SeededIds.Question_FirstEmpire_3, QuizId = SeededIds.Quiz_FirstEmpire_1, Text = "Кой хан разгромява византийския император Никифор I през 811 г.?" },
                new Question { Id = SeededIds.Question_FirstEmpire_4, QuizId = SeededIds.Quiz_FirstEmpire_1, Text = "Коя година е покръстването на българите?" },
                new Question { Id = SeededIds.Question_FirstEmpire_5, QuizId = SeededIds.Quiz_FirstEmpire_1, Text = "Кой владетел създава първия писмен законник?" },
                new Question { Id = SeededIds.Question_FirstEmpire_6, QuizId = SeededIds.Quiz_FirstEmpire_1, Text = "Коя битка през 917 г. утвърждава българското надмощие на Балканите?" },
                new Question { Id = SeededIds.Question_FirstEmpire_7, QuizId = SeededIds.Quiz_FirstEmpire_1, Text = "Коя година Византия завладява България?" },

                new Question { Id = SeededIds.Question_SecondEmpire_1, QuizId = SeededIds.Quiz_SecondEmpire_1, Text = "Коя година се свързва с възстановяването на българската държава?" },
                new Question { Id = SeededIds.Question_SecondEmpire_2, QuizId = SeededIds.Quiz_SecondEmpire_1, Text = "Кой владетел е свързан с разширение и подем на Второто царство?" },
                new Question { Id = SeededIds.Question_SecondEmpire_3, QuizId = SeededIds.Quiz_SecondEmpire_1, Text = "Кои братя ръководят въстанието от 1185 г.?" },
                new Question { Id = SeededIds.Question_SecondEmpire_4, QuizId = SeededIds.Quiz_SecondEmpire_1, Text = "Коя битка през 1205 г. разгромява кръстоносците?" },
                new Question { Id = SeededIds.Question_SecondEmpire_5, QuizId = SeededIds.Quiz_SecondEmpire_1, Text = "Коя битка през 1230 г. носи победа на Иван Асен II?" },
                new Question { Id = SeededIds.Question_SecondEmpire_6, QuizId = SeededIds.Quiz_SecondEmpire_1, Text = "Коя година пада Търново под османска власт?" },
                new Question { Id = SeededIds.Question_SecondEmpire_7, QuizId = SeededIds.Quiz_SecondEmpire_1, Text = "Коя година пада Видинското царство?" },

                new Question { Id = SeededIds.Question_Ottoman_1, QuizId = SeededIds.Quiz_OttomanPeriod_1, Text = "Коя система за събиране на момчета е налагана от османците?" },
                new Question { Id = SeededIds.Question_Ottoman_2, QuizId = SeededIds.Quiz_OttomanPeriod_1, Text = "Коя година е Чипровското въстание?" },
                new Question { Id = SeededIds.Question_Ottoman_3, QuizId = SeededIds.Quiz_OttomanPeriod_1, Text = "Кой данък са плащали християните в Османската империя?" },
                new Question { Id = SeededIds.Question_Ottoman_4, QuizId = SeededIds.Quiz_OttomanPeriod_1, Text = "Кой район е засегнат от ислямизация през XVII век?" },
                new Question { Id = SeededIds.Question_Ottoman_5, QuizId = SeededIds.Quiz_OttomanPeriod_1, Text = "Коя година е Първото търновско въстание?" },
                new Question { Id = SeededIds.Question_Ottoman_6, QuizId = SeededIds.Quiz_OttomanPeriod_1, Text = "Какво представляват хайдушките чети?" },
                new Question { Id = SeededIds.Question_Ottoman_7, QuizId = SeededIds.Quiz_OttomanPeriod_1, Text = "Кой османски султан завладява българските земи?" },

                new Question { Id = SeededIds.Question_Revival_1, QuizId = SeededIds.Quiz_Revival_1, Text = "Коя година е написана 'История славянобългарска'?" },
                new Question { Id = SeededIds.Question_Revival_2, QuizId = SeededIds.Quiz_Revival_1, Text = "Кой е авторът на 'История славянобългарска'?" },
                new Question { Id = SeededIds.Question_Revival_3, QuizId = SeededIds.Quiz_Revival_1, Text = "Коя година е учредена Българската екзархия?" },
                new Question { Id = SeededIds.Question_Revival_4, QuizId = SeededIds.Quiz_Revival_1, Text = "Кой възрожденец издава първата печатна българска книга?" },
                new Question { Id = SeededIds.Question_Revival_5, QuizId = SeededIds.Quiz_Revival_1, Text = "Къде е открито първото светско училище в България?" },
                new Question { Id = SeededIds.Question_Revival_6, QuizId = SeededIds.Quiz_Revival_1, Text = "Коя година е основано училището в Габрово?" },
                new Question { Id = SeededIds.Question_Revival_7, QuizId = SeededIds.Quiz_Revival_1, Text = "Какво представляват читалищата?" },
                new Question { Id = SeededIds.Question_Revival_8, QuizId = SeededIds.Quiz_Revival_1, Text = "Кой възрожденец оглавява църковната борба?" },

                new Question { Id = SeededIds.Question_Revival_9, QuizId = SeededIds.Quiz_Revival_2, Text = "Коя година е Априлското въстание?" },
                new Question { Id = SeededIds.Question_Revival_10, QuizId = SeededIds.Quiz_Revival_2, Text = "Кой е главният организатор на Вътрешната революционна организация?" },
                new Question { Id = SeededIds.Question_Revival_11, QuizId = SeededIds.Quiz_Revival_2, Text = "Кой поет загива във Врачанския балкан през 1876 г.?" },
                new Question { Id = SeededIds.Question_Revival_12, QuizId = SeededIds.Quiz_Revival_2, Text = "Кой революционер създава първите български легии?" },
                new Question { Id = SeededIds.Question_Revival_13, QuizId = SeededIds.Quiz_Revival_2, Text = "Коя година започва Руско-турската война?" },
                new Question { Id = SeededIds.Question_Revival_14, QuizId = SeededIds.Quiz_Revival_2, Text = "Коя година е подписан Санстефанският мирен договор?" },
                new Question { Id = SeededIds.Question_Revival_15, QuizId = SeededIds.Quiz_Revival_2, Text = "Кой е авторът на стихотворението 'Майце си'?" },
                new Question { Id = SeededIds.Question_Revival_16, QuizId = SeededIds.Quiz_Revival_2, Text = "Къде е обесен Васил Левски?" },

                new Question { Id = SeededIds.Question_Modern_1, QuizId = SeededIds.Quiz_Modern_1, Text = "Коя година е Съединението на България?" },
                new Question { Id = SeededIds.Question_Modern_2, QuizId = SeededIds.Quiz_Modern_1, Text = "Коя година България обявява независимост?" },
                new Question { Id = SeededIds.Question_Modern_3, QuizId = SeededIds.Quiz_Modern_1, Text = "Кой е първият министър-председател на България?" },
                new Question { Id = SeededIds.Question_Modern_4, QuizId = SeededIds.Quiz_Modern_1, Text = "Коя година започват Балканските войни?" },
                new Question { Id = SeededIds.Question_Modern_5, QuizId = SeededIds.Quiz_Modern_1, Text = "Кой договор слага край на Първата световна война за България?" },
                new Question { Id = SeededIds.Question_Modern_6, QuizId = SeededIds.Quiz_Modern_1, Text = "Коя година България се присъединява към Тристранния пакт?" },
                new Question { Id = SeededIds.Question_Modern_7, QuizId = SeededIds.Quiz_Modern_1, Text = "Кой български цар умира през 1943 г.?" },
                new Question { Id = SeededIds.Question_Modern_8, QuizId = SeededIds.Quiz_Modern_1, Text = "Коя година е превратът на 9 септември?" },

                new Question { Id = SeededIds.Question_Modern_9, QuizId = SeededIds.Quiz_Modern_2, Text = "Коя година е премахната монархията в България?" },
                new Question { Id = SeededIds.Question_Modern_10, QuizId = SeededIds.Quiz_Modern_2, Text = "Кой ръководи България най-дълго (1954-1989)?" },
                new Question { Id = SeededIds.Question_Modern_11, QuizId = SeededIds.Quiz_Modern_2, Text = "Коя година е свален Тодор Живков?" },
                new Question { Id = SeededIds.Question_Modern_12, QuizId = SeededIds.Quiz_Modern_2, Text = "Кой е първият демократично избран президент на България?" },
                new Question { Id = SeededIds.Question_Modern_13, QuizId = SeededIds.Quiz_Modern_2, Text = "Коя година България става член на НАТО?" },
                new Question { Id = SeededIds.Question_Modern_14, QuizId = SeededIds.Quiz_Modern_2, Text = "Коя година България става член на Европейския съюз?" },
                new Question { Id = SeededIds.Question_Modern_15, QuizId = SeededIds.Quiz_Modern_2, Text = "Коя година е приета новата Конституция на България?" },
                new Question { Id = SeededIds.Question_Modern_16, QuizId = SeededIds.Quiz_Modern_2, Text = "Кой дисидент е автор на книгата 'Фашизмът'?" }
            );
        }
    }
}