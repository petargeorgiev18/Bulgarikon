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
                new Answer { Id = SeededIds.Answer_1, QuestionId = SeededIds.Question_Antiquity_1, Text = "Траки", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_2, QuestionId = SeededIds.Question_Antiquity_1, Text = "Келти", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_3, QuestionId = SeededIds.Question_Antiquity_1, Text = "Германи", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_4, QuestionId = SeededIds.Question_Antiquity_1, Text = "Скити", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_5, QuestionId = SeededIds.Question_Antiquity_2, Text = "Развитие на писменост и държавност", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_6, QuestionId = SeededIds.Question_Antiquity_2, Text = "Номадски начин на живот", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_7, QuestionId = SeededIds.Question_Antiquity_2, Text = "Липса на социална организация", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_8, QuestionId = SeededIds.Question_Antiquity_2, Text = "Пълна изолация от други култури", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_9, QuestionId = SeededIds.Question_Antiquity_3, Text = "Терес I", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_10, QuestionId = SeededIds.Question_Antiquity_3, Text = "Ситалк", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_11, QuestionId = SeededIds.Question_Antiquity_3, Text = "Севт III", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_12, QuestionId = SeededIds.Question_Antiquity_3, Text = "Реметалк I", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_13, QuestionId = SeededIds.Question_Antiquity_4, Text = "Свещарска гробница", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_14, QuestionId = SeededIds.Question_Antiquity_4, Text = "Казанлъшка гробница", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_15, QuestionId = SeededIds.Question_Antiquity_4, Text = "Могиланска гробница", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_16, QuestionId = SeededIds.Question_Antiquity_4, Text = "Староселска гробница", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_17, QuestionId = SeededIds.Question_Antiquity_5, Text = "Сердика", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_18, QuestionId = SeededIds.Question_Antiquity_5, Text = "Улпия Ескус", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_19, QuestionId = SeededIds.Question_Antiquity_5, Text = "Рациария", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_20, QuestionId = SeededIds.Question_Antiquity_5, Text = "Никополис ад Иструм", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_21, QuestionId = SeededIds.Question_Antiquity_6, Text = "46 г. сл. Хр.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_22, QuestionId = SeededIds.Question_Antiquity_6, Text = "29 г. пр. Хр.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_23, QuestionId = SeededIds.Question_Antiquity_6, Text = "106 г. сл. Хр.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_24, QuestionId = SeededIds.Question_Antiquity_6, Text = "212 г. сл. Хр.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_25, QuestionId = SeededIds.Question_Antiquity_7, Text = "Орфей", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_26, QuestionId = SeededIds.Question_Antiquity_7, Text = "Хомер", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_27, QuestionId = SeededIds.Question_Antiquity_7, Text = "Езоп", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_28, QuestionId = SeededIds.Question_Antiquity_7, Text = "Пиндар", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_29, QuestionId = SeededIds.Question_Antiquity_8, Text = "Съхранява знание и улеснява управлението", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_30, QuestionId = SeededIds.Question_Antiquity_8, Text = "Замества земеделието", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_31, QuestionId = SeededIds.Question_Antiquity_8, Text = "Премахва нуждата от закони", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_32, QuestionId = SeededIds.Question_Antiquity_8, Text = "Забранява търговията", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_33, QuestionId = SeededIds.Question_Antiquity_9, Text = "Площади, обществени сгради и пазари", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_34, QuestionId = SeededIds.Question_Antiquity_9, Text = "Небостъргачи от стъкло", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_35, QuestionId = SeededIds.Question_Antiquity_9, Text = "Метро линии", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_36, QuestionId = SeededIds.Question_Antiquity_9, Text = "Магистрали и летища", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_37, QuestionId = SeededIds.Question_Antiquity_10, Text = "Севт III", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_38, QuestionId = SeededIds.Question_Antiquity_10, Text = "Терес I", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_39, QuestionId = SeededIds.Question_Antiquity_10, Text = "Ситалк", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_40, QuestionId = SeededIds.Question_Antiquity_10, Text = "Котис I", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_41, QuestionId = SeededIds.Question_Antiquity_11, Text = "Панагюрско съкровище", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_42, QuestionId = SeededIds.Question_Antiquity_11, Text = "Вълчитрънско съкровище", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_43, QuestionId = SeededIds.Question_Antiquity_11, Text = "Рогозенско съкровище", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_44, QuestionId = SeededIds.Question_Antiquity_11, Text = "Боровско съкровище", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_45, QuestionId = SeededIds.Question_Antiquity_12, Text = "Константин I Велики", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_46, QuestionId = SeededIds.Question_Antiquity_12, Text = "Юлий Цезар", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_47, QuestionId = SeededIds.Question_Antiquity_12, Text = "Август", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_48, QuestionId = SeededIds.Question_Antiquity_12, Text = "Траян", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_49, QuestionId = SeededIds.Question_Antiquity_13, Text = "Учение за безсмъртието на душата", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_50, QuestionId = SeededIds.Question_Antiquity_13, Text = "Военен съюз на тракийски племена", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_51, QuestionId = SeededIds.Question_Antiquity_13, Text = "Вид тракийска керамика", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_52, QuestionId = SeededIds.Question_Antiquity_13, Text = "Тракийски танц", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_53, QuestionId = SeededIds.Question_Antiquity_14, Text = "313 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_54, QuestionId = SeededIds.Question_Antiquity_14, Text = "325 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_55, QuestionId = SeededIds.Question_Antiquity_14, Text = "330 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_56, QuestionId = SeededIds.Question_Antiquity_14, Text = "380 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_57, QuestionId = SeededIds.Question_FirstEmpire_1, Text = "681 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_58, QuestionId = SeededIds.Question_FirstEmpire_1, Text = "679 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_59, QuestionId = SeededIds.Question_FirstEmpire_1, Text = "680 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_60, QuestionId = SeededIds.Question_FirstEmpire_1, Text = "682 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_61, QuestionId = SeededIds.Question_FirstEmpire_2, Text = "Цар Симеон I", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_62, QuestionId = SeededIds.Question_FirstEmpire_2, Text = "Княз Борис I", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_63, QuestionId = SeededIds.Question_FirstEmpire_2, Text = "Хан Крум", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_64, QuestionId = SeededIds.Question_FirstEmpire_2, Text = "Хан Аспарух", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_65, QuestionId = SeededIds.Question_FirstEmpire_3, Text = "Хан Крум", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_66, QuestionId = SeededIds.Question_FirstEmpire_3, Text = "Хан Омуртаг", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_67, QuestionId = SeededIds.Question_FirstEmpire_3, Text = "Хан Тервел", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_68, QuestionId = SeededIds.Question_FirstEmpire_3, Text = "Хан Аспарух", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_69, QuestionId = SeededIds.Question_FirstEmpire_4, Text = "864 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_70, QuestionId = SeededIds.Question_FirstEmpire_4, Text = "865 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_71, QuestionId = SeededIds.Question_FirstEmpire_4, Text = "863 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_72, QuestionId = SeededIds.Question_FirstEmpire_4, Text = "866 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_73, QuestionId = SeededIds.Question_FirstEmpire_5, Text = "Хан Крум", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_74, QuestionId = SeededIds.Question_FirstEmpire_5, Text = "Хан Омуртаг", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_75, QuestionId = SeededIds.Question_FirstEmpire_5, Text = "Княз Борис I", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_76, QuestionId = SeededIds.Question_FirstEmpire_5, Text = "Цар Симеон I", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_77, QuestionId = SeededIds.Question_FirstEmpire_6, Text = "Битка при Ахелой", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_78, QuestionId = SeededIds.Question_FirstEmpire_6, Text = "Битка при Онгъла", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_79, QuestionId = SeededIds.Question_FirstEmpire_6, Text = "Битка при Върбишки проход", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_80, QuestionId = SeededIds.Question_FirstEmpire_6, Text = "Битка при Маркели", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_81, QuestionId = SeededIds.Question_FirstEmpire_7, Text = "1018 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_82, QuestionId = SeededIds.Question_FirstEmpire_7, Text = "1014 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_83, QuestionId = SeededIds.Question_FirstEmpire_7, Text = "1021 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_84, QuestionId = SeededIds.Question_FirstEmpire_7, Text = "1019 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_85, QuestionId = SeededIds.Question_SecondEmpire_1, Text = "1185 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_86, QuestionId = SeededIds.Question_SecondEmpire_1, Text = "1186 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_87, QuestionId = SeededIds.Question_SecondEmpire_1, Text = "1187 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_88, QuestionId = SeededIds.Question_SecondEmpire_1, Text = "1184 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_89, QuestionId = SeededIds.Question_SecondEmpire_2, Text = "Иван Асен II", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_90, QuestionId = SeededIds.Question_SecondEmpire_2, Text = "Калоян", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_91, QuestionId = SeededIds.Question_SecondEmpire_2, Text = "Борил", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_92, QuestionId = SeededIds.Question_SecondEmpire_2, Text = "Мицо Асен", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_93, QuestionId = SeededIds.Question_SecondEmpire_3, Text = "Асен и Петър", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_94, QuestionId = SeededIds.Question_SecondEmpire_3, Text = "Калоян и Борил", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_95, QuestionId = SeededIds.Question_SecondEmpire_3, Text = "Иван Асен и Александър", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_96, QuestionId = SeededIds.Question_SecondEmpire_3, Text = "Михаил и Теодор", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_97, QuestionId = SeededIds.Question_SecondEmpire_4, Text = "Битка при Одрин", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_98, QuestionId = SeededIds.Question_SecondEmpire_4, Text = "Битка при Клокотница", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_99, QuestionId = SeededIds.Question_SecondEmpire_4, Text = "Битка при Русион", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_100, QuestionId = SeededIds.Question_SecondEmpire_4, Text = "Битка при Сердика", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_101, QuestionId = SeededIds.Question_SecondEmpire_5, Text = "Битка при Клокотница", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_102, QuestionId = SeededIds.Question_SecondEmpire_5, Text = "Битка при Одрин", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_103, QuestionId = SeededIds.Question_SecondEmpire_5, Text = "Битка при Русион", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_104, QuestionId = SeededIds.Question_SecondEmpire_5, Text = "Битка при Сердика", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_105, QuestionId = SeededIds.Question_SecondEmpire_6, Text = "1393 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_106, QuestionId = SeededIds.Question_SecondEmpire_6, Text = "1396 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_107, QuestionId = SeededIds.Question_SecondEmpire_6, Text = "1389 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_108, QuestionId = SeededIds.Question_SecondEmpire_6, Text = "1397 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_109, QuestionId = SeededIds.Question_SecondEmpire_7, Text = "1396 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_110, QuestionId = SeededIds.Question_SecondEmpire_7, Text = "1393 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_111, QuestionId = SeededIds.Question_SecondEmpire_7, Text = "1395 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_112, QuestionId = SeededIds.Question_SecondEmpire_7, Text = "1399 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_113, QuestionId = SeededIds.Question_Ottoman_1, Text = "Девширме (кръвен данък)", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_114, QuestionId = SeededIds.Question_Ottoman_1, Text = "Спахийска система", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_115, QuestionId = SeededIds.Question_Ottoman_1, Text = "Тимарска система", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_116, QuestionId = SeededIds.Question_Ottoman_1, Text = "Читлушка система", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_117, QuestionId = SeededIds.Question_Ottoman_2, Text = "1688 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_118, QuestionId = SeededIds.Question_Ottoman_2, Text = "1686 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_119, QuestionId = SeededIds.Question_Ottoman_2, Text = "1689 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_120, QuestionId = SeededIds.Question_Ottoman_2, Text = "1683 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_121, QuestionId = SeededIds.Question_Ottoman_3, Text = "Джизя", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_122, QuestionId = SeededIds.Question_Ottoman_3, Text = "Зекят", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_123, QuestionId = SeededIds.Question_Ottoman_3, Text = "Ушр", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_124, QuestionId = SeededIds.Question_Ottoman_3, Text = "Харадж", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_125, QuestionId = SeededIds.Question_Ottoman_4, Text = "Родопите", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_126, QuestionId = SeededIds.Question_Ottoman_4, Text = "Добруджа", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_127, QuestionId = SeededIds.Question_Ottoman_4, Text = "Софийско", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_128, QuestionId = SeededIds.Question_Ottoman_4, Text = "Врачанско", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_129, QuestionId = SeededIds.Question_Ottoman_5, Text = "1598 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_130, QuestionId = SeededIds.Question_Ottoman_5, Text = "1688 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_131, QuestionId = SeededIds.Question_Ottoman_5, Text = "1686 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_132, QuestionId = SeededIds.Question_Ottoman_5, Text = "1586 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_133, QuestionId = SeededIds.Question_Ottoman_6, Text = "Въоръжени групи за съпротива", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_134, QuestionId = SeededIds.Question_Ottoman_6, Text = "Османска военна част", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_135, QuestionId = SeededIds.Question_Ottoman_6, Text = "Земеделски задруги", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_136, QuestionId = SeededIds.Question_Ottoman_6, Text = "Търговски дружества", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_137, QuestionId = SeededIds.Question_Ottoman_7, Text = "Сулеиман I", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_138, QuestionId = SeededIds.Question_Ottoman_7, Text = "Мехмед II", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_139, QuestionId = SeededIds.Question_Ottoman_7, Text = "Мурад I", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_140, QuestionId = SeededIds.Question_Ottoman_7, Text = "Баязид I", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_141, QuestionId = SeededIds.Question_Revival_1, Text = "1762 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_142, QuestionId = SeededIds.Question_Revival_1, Text = "1771 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_143, QuestionId = SeededIds.Question_Revival_1, Text = "1756 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_144, QuestionId = SeededIds.Question_Revival_1, Text = "1748 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_145, QuestionId = SeededIds.Question_Revival_2, Text = "Паисий Хилендарски", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_146, QuestionId = SeededIds.Question_Revival_2, Text = "Софроний Врачански", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_147, QuestionId = SeededIds.Question_Revival_2, Text = "Георги Раковски", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_148, QuestionId = SeededIds.Question_Revival_2, Text = "Васил Левски", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_149, QuestionId = SeededIds.Question_Revival_3, Text = "1870 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_150, QuestionId = SeededIds.Question_Revival_3, Text = "1856 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_151, QuestionId = SeededIds.Question_Revival_3, Text = "1860 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_152, QuestionId = SeededIds.Question_Revival_3, Text = "1872 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_153, QuestionId = SeededIds.Question_Revival_4, Text = "Софроний Врачански", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_154, QuestionId = SeededIds.Question_Revival_4, Text = "Паисий Хилендарски", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_155, QuestionId = SeededIds.Question_Revival_4, Text = "Петър Берон", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_156, QuestionId = SeededIds.Question_Revival_4, Text = "Васил Априлов", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_157, QuestionId = SeededIds.Question_Revival_5, Text = "Габрово", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_158, QuestionId = SeededIds.Question_Revival_5, Text = "Копривщица", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_159, QuestionId = SeededIds.Question_Revival_5, Text = "Пловдив", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_160, QuestionId = SeededIds.Question_Revival_5, Text = "Свищов", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_161, QuestionId = SeededIds.Question_Revival_6, Text = "1835 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_162, QuestionId = SeededIds.Question_Revival_6, Text = "1830 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_163, QuestionId = SeededIds.Question_Revival_6, Text = "1840 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_164, QuestionId = SeededIds.Question_Revival_6, Text = "1825 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_165, QuestionId = SeededIds.Question_Revival_7, Text = "Обществено-културни центрове", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_166, QuestionId = SeededIds.Question_Revival_7, Text = "Религиозни институции", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_167, QuestionId = SeededIds.Question_Revival_7, Text = "Търговски дружества", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_168, QuestionId = SeededIds.Question_Revival_7, Text = "Болнични заведения", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_169, QuestionId = SeededIds.Question_Revival_8, Text = "Иларион Макариополски", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_170, QuestionId = SeededIds.Question_Revival_8, Text = "Софроний Врачански", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_171, QuestionId = SeededIds.Question_Revival_8, Text = "Неофит Бозвели", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_172, QuestionId = SeededIds.Question_Revival_8, Text = "Паисий Хилендарски", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_173, QuestionId = SeededIds.Question_Revival_9, Text = "1876 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_174, QuestionId = SeededIds.Question_Revival_9, Text = "1875 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_175, QuestionId = SeededIds.Question_Revival_9, Text = "1877 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_176, QuestionId = SeededIds.Question_Revival_9, Text = "1872 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_177, QuestionId = SeededIds.Question_Revival_10, Text = "Васил Левски", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_178, QuestionId = SeededIds.Question_Revival_10, Text = "Георги Раковски", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_179, QuestionId = SeededIds.Question_Revival_10, Text = "Любен Каравелов", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_180, QuestionId = SeededIds.Question_Revival_10, Text = "Христо Ботев", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_181, QuestionId = SeededIds.Question_Revival_11, Text = "Христо Ботев", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_182, QuestionId = SeededIds.Question_Revival_11, Text = "Георги Раковски", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_183, QuestionId = SeededIds.Question_Revival_11, Text = "Васил Левски", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_184, QuestionId = SeededIds.Question_Revival_11, Text = "Любен Каравелов", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_185, QuestionId = SeededIds.Question_Revival_12, Text = "Георги Раковски", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_186, QuestionId = SeededIds.Question_Revival_12, Text = "Васил Левски", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_187, QuestionId = SeededIds.Question_Revival_12, Text = "Христо Ботев", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_188, QuestionId = SeededIds.Question_Revival_12, Text = "Любен Каравелов", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_189, QuestionId = SeededIds.Question_Revival_13, Text = "1877 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_190, QuestionId = SeededIds.Question_Revival_13, Text = "1876 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_191, QuestionId = SeededIds.Question_Revival_13, Text = "1878 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_192, QuestionId = SeededIds.Question_Revival_13, Text = "1875 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_193, QuestionId = SeededIds.Question_Revival_14, Text = "1878 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_194, QuestionId = SeededIds.Question_Revival_14, Text = "1877 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_195, QuestionId = SeededIds.Question_Revival_14, Text = "1879 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_196, QuestionId = SeededIds.Question_Revival_14, Text = "1876 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_197, QuestionId = SeededIds.Question_Revival_15, Text = "Христо Ботев", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_198, QuestionId = SeededIds.Question_Revival_15, Text = "Иван Вазов", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_199, QuestionId = SeededIds.Question_Revival_15, Text = "Георги Раковски", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_200, QuestionId = SeededIds.Question_Revival_15, Text = "Любен Каравелов", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_201, QuestionId = SeededIds.Question_Revival_16, Text = "София", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_202, QuestionId = SeededIds.Question_Revival_16, Text = "Пловдив", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_203, QuestionId = SeededIds.Question_Revival_16, Text = "Ловеч", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_204, QuestionId = SeededIds.Question_Revival_16, Text = "Търново", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_205, QuestionId = SeededIds.Question_Modern_1, Text = "1885 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_206, QuestionId = SeededIds.Question_Modern_1, Text = "1878 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_207, QuestionId = SeededIds.Question_Modern_1, Text = "1887 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_208, QuestionId = SeededIds.Question_Modern_1, Text = "1883 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_209, QuestionId = SeededIds.Question_Modern_2, Text = "1908 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_210, QuestionId = SeededIds.Question_Modern_2, Text = "1885 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_211, QuestionId = SeededIds.Question_Modern_2, Text = "1912 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_212, QuestionId = SeededIds.Question_Modern_2, Text = "1905 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_213, QuestionId = SeededIds.Question_Modern_3, Text = "Тодор Бурмов", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_214, QuestionId = SeededIds.Question_Modern_3, Text = "Стефан Стамболов", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_215, QuestionId = SeededIds.Question_Modern_3, Text = "Константин Стоилов", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_216, QuestionId = SeededIds.Question_Modern_3, Text = "Васил Радославов", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_217, QuestionId = SeededIds.Question_Modern_4, Text = "1912 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_218, QuestionId = SeededIds.Question_Modern_4, Text = "1913 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_219, QuestionId = SeededIds.Question_Modern_4, Text = "1910 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_220, QuestionId = SeededIds.Question_Modern_4, Text = "1914 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_221, QuestionId = SeededIds.Question_Modern_5, Text = "Ньойски договор", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_222, QuestionId = SeededIds.Question_Modern_5, Text = "Версайски договор", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_223, QuestionId = SeededIds.Question_Modern_5, Text = "Санстефански договор", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_224, QuestionId = SeededIds.Question_Modern_5, Text = "Букурещки договор", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_225, QuestionId = SeededIds.Question_Modern_6, Text = "1941 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_226, QuestionId = SeededIds.Question_Modern_6, Text = "1940 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_227, QuestionId = SeededIds.Question_Modern_6, Text = "1942 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_228, QuestionId = SeededIds.Question_Modern_6, Text = "1939 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_229, QuestionId = SeededIds.Question_Modern_7, Text = "Борис III", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_230, QuestionId = SeededIds.Question_Modern_7, Text = "Фердинанд I", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_231, QuestionId = SeededIds.Question_Modern_7, Text = "Симеон II", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_232, QuestionId = SeededIds.Question_Modern_7, Text = "Александър I", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_233, QuestionId = SeededIds.Question_Modern_8, Text = "1944 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_234, QuestionId = SeededIds.Question_Modern_8, Text = "1945 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_235, QuestionId = SeededIds.Question_Modern_8, Text = "1943 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_236, QuestionId = SeededIds.Question_Modern_8, Text = "1946 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_237, QuestionId = SeededIds.Question_Modern_9, Text = "1946 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_238, QuestionId = SeededIds.Question_Modern_9, Text = "1944 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_239, QuestionId = SeededIds.Question_Modern_9, Text = "1947 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_240, QuestionId = SeededIds.Question_Modern_9, Text = "1945 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_241, QuestionId = SeededIds.Question_Modern_10, Text = "Тодор Живков", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_242, QuestionId = SeededIds.Question_Modern_10, Text = "Вълко Червенков", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_243, QuestionId = SeededIds.Question_Modern_10, Text = "Антон Югов", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_244, QuestionId = SeededIds.Question_Modern_10, Text = "Георги Димитров", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_245, QuestionId = SeededIds.Question_Modern_11, Text = "1989 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_246, QuestionId = SeededIds.Question_Modern_11, Text = "1990 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_247, QuestionId = SeededIds.Question_Modern_11, Text = "1988 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_248, QuestionId = SeededIds.Question_Modern_11, Text = "1991 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_249, QuestionId = SeededIds.Question_Modern_12, Text = "Желю Желев", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_250, QuestionId = SeededIds.Question_Modern_12, Text = "Петър Стоянов", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_251, QuestionId = SeededIds.Question_Modern_12, Text = "Георги Първанов", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_252, QuestionId = SeededIds.Question_Modern_12, Text = "Росен Плевнелиев", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_253, QuestionId = SeededIds.Question_Modern_13, Text = "2004 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_254, QuestionId = SeededIds.Question_Modern_13, Text = "2005 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_255, QuestionId = SeededIds.Question_Modern_13, Text = "2003 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_256, QuestionId = SeededIds.Question_Modern_13, Text = "2006 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_257, QuestionId = SeededIds.Question_Modern_14, Text = "2007 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_258, QuestionId = SeededIds.Question_Modern_14, Text = "2004 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_259, QuestionId = SeededIds.Question_Modern_14, Text = "2005 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_260, QuestionId = SeededIds.Question_Modern_14, Text = "2008 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_261, QuestionId = SeededIds.Question_Modern_15, Text = "1991 г.", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_262, QuestionId = SeededIds.Question_Modern_15, Text = "1990 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_263, QuestionId = SeededIds.Question_Modern_15, Text = "1992 г.", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_264, QuestionId = SeededIds.Question_Modern_15, Text = "1989 г.", IsCorrect = false },

                new Answer { Id = SeededIds.Answer_265, QuestionId = SeededIds.Question_Modern_16, Text = "Желю Желев", IsCorrect = true },
                new Answer { Id = SeededIds.Answer_266, QuestionId = SeededIds.Question_Modern_16, Text = "Димитър Димитров", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_267, QuestionId = SeededIds.Question_Modern_16, Text = "Петър-Емил Митев", IsCorrect = false },
                new Answer { Id = SeededIds.Answer_268, QuestionId = SeededIds.Question_Modern_16, Text = "Николай Петков", IsCorrect = false }
            );
        }
    }
}