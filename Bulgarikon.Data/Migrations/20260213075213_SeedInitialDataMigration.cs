using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulgarikon.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Eras",
                columns: new[] { "Id", "Description", "EndYear", "Name", "StartYear" },
                values: new object[,]
                {
                    { new Guid("471c687f-caef-4cf6-8a8b-3dad51058485"), "Възстановено през 1185 г., Второто българско царство преживява културен подем и териториално разширение, като значим владетел е цар Иван Асен II. Периодът завършва с османското завоевание.", 1396, "Второ българско царство", 1185 },
                    { new Guid("68a72d82-601f-49c1-821c-850a82646c21"), "Периодът на османска власт над българските земи (края на XIV век – 1878 г.) е време на дълбоки социални и културни промени, но и на съпротива и националноосвободителни движения.", 1878, "Османско владичество", 1396 },
                    { new Guid("76164f60-cb04-400a-90f0-97203f0e420a"), "Античността обхваща най-ранните периоди от човешката история и възхода на древните цивилизации. Характерна е с развитието на земеделие, градове, писменост и ранни форми на държавност.", 500, "Античност", -3000 },
                    { new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), "Държавата, утвърдена след 681 г., се превръща във водеща сила на Балканите. Периодът е белязан от християнизацията и културния разцвет, особено при цар Симеон Велики.", 1018, "Първо българско царство", 681 },
                    { new Guid("e1a57a73-e7af-4db9-a713-eea9b9f579e2"), "Период на значителни миграции в Европа (IV–VII век), свързан с разместване на племена и народи и с упадъка на Западната Римска империя.", 700, "Велико преселение на народите", 300 }
                });

            migrationBuilder.InsertData(
                table: "Artifacts",
                columns: new[] { "Id", "CivilizationId", "Description", "DiscoveredAt", "EraId", "ImageUrl", "Location", "Material", "Name", "Year" },
                values: new object[,]
                {
                    { new Guid("7ade4cf5-2a6b-4d64-a4f5-4c2b9e6a1004"), null, "Пръстен-печат, използван за удостоверяване на документи и принадлежност към аристокрацията.", new DateTime(2015, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("471c687f-caef-4cf6-8a8b-3dad51058485"), null, "Търново", "Сребро", "Пръстен-печат", 1230 },
                    { new Guid("8bef5d06-3b7c-4e55-b3a6-5d3c1f7b1005"), null, "Керамичен съд от бита. Използван за съхранение на храни и течности, типичен за ежедневието на епохата.", new DateTime(2003, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("68a72d82-601f-49c1-821c-850a82646c21"), null, "Централна България", "Керамика", "Керамичен съд", 1700 }
                });

            migrationBuilder.InsertData(
                table: "Civilizations",
                columns: new[] { "Id", "Description", "EndYear", "EraId", "ImageUrl", "Name", "StartYear", "Type" },
                values: new object[,]
                {
                    { new Guid("2f0c3a11-7b8e-4f6a-9d10-5c6b7a8e9002"), "Народ със степен произход, който участва в създаването на българската държава и оформянето на ранната българска народност.", 900, new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), null, "Прабългари", 600, 3 },
                    { new Guid("9f578916-414a-4d35-92b3-fbc187d382dd"), "Държавата, под чиято власт българските земи остават до Освобождението през 1878 г.", 1922, new Guid("68a72d82-601f-49c1-821c-850a82646c21"), null, "Османска империя", 1299, 2 },
                    { new Guid("ad1d6a1a-6122-420d-908b-55fbe5eb18ca"), "Източната Римска империя – доминираща сила в Източното Средиземноморие и важен съперник/партньор на България.", 1453, new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), null, "Византийска империя", 330, 1 },
                    { new Guid("d1a57a73-e7af-4db9-a713-eea9b9f579e2"), "Древно население на Балканите, оставило богато културно наследство и значими археологически находки.", 46, new Guid("76164f60-cb04-400a-90f0-97203f0e420a"), null, "Траки", -1500, 3 },
                    { new Guid("f1a57a73-e7af-4db9-a713-eea9b9f579e2"), "Славянските племена се заселват на Балканите и имат ключова роля в етногенезиса и културата на региона.", 900, new Guid("e1a57a73-e7af-4db9-a713-eea9b9f579e2"), null, "Славяни", 500, 0 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "EndYear", "EraId", "Location", "StartYear", "Title" },
                values: new object[,]
                {
                    { new Guid("91f06e17-4c8d-4f46-92b7-6e4d2a8c2001"), "През 680 г. хан Аспарух разгромява византийската армия на император Константин IV в укрепения лагер при Онгъла. Победата води до признаването на българската държава през 681 г.", 680, new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), "Онгъла (Северна Добруджа)", 680, "Битката при Онгъла" },
                    { new Guid("a2f17f28-5d9e-4a37-a1c8-7f5e3b9d2002"), "Сражение, при което хан Крум нанася тежко поражение на византийците след похода на Никифор I.", 811, new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), "Върбишки проход", 811, "Битката във Върбишкия проход" },
                    { new Guid("b3f28a39-6e1f-4b28-b2d9-8a6f4c1e2003"), "Превземането на столицата Търново от османците – ключов момент в падането на Второто българско царство.", 1393, new Guid("471c687f-caef-4cf6-8a8b-3dad51058485"), "Търново", 1393, "Падането на Търново" },
                    { new Guid("c4f39b4a-7f2a-4c19-83ea-9b7a5d2f2004"), "Въстание срещу османската власт, което предизвиква международен отзвук и подкрепя каузата за освобождение.", 1876, new Guid("68a72d82-601f-49c1-821c-850a82646c21"), "Българските земи", 1876, "Априлско въстание" },
                    { new Guid("d5f4ac5b-8a3b-4d10-94fb-ac8b6e3f2005"), "Обединяване на Княжество България и Източна Румелия – ключов акт за националното обединение.", 1885, new Guid("68a72d82-601f-49c1-821c-850a82646c21"), "Пловдив", 1885, "Съединението на България" }
                });

            migrationBuilder.InsertData(
                table: "Figures",
                columns: new[] { "Id", "BirthDate", "BirthYear", "CivilizationId", "DeathDate", "DeathYear", "Description", "EraId", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("08b7df8e-2d6e-4a83-97be-df2e9b6c3003"), new DateTime(1837, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1837, null, new DateTime(1873, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1873, "Апостол на свободата. Организатор на вътрешната революционна мрежа за освобождението на България.", new Guid("68a72d82-601f-49c1-821c-850a82646c21"), null, "Васил Левски" },
                    { new Guid("19c8e09f-3e7f-4b74-a8cf-e03f1c7d3004"), new DateTime(1848, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1848, null, new DateTime(1876, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1876, "Поет и революционер. Символ на борбата за национално освобождение през Възраждането.", new Guid("68a72d82-601f-49c1-821c-850a82646c21"), null, "Христо Ботев" },
                    { new Guid("2ad9f1a0-4f8a-4c65-b9d0-f14a2d8e3005"), null, 1168, null, null, 1207, "Владетел на Второто българско царство. Провежда активна външна политика и укрепва държавата.", new Guid("471c687f-caef-4cf6-8a8b-3dad51058485"), null, "Цар Калоян" },
                    { new Guid("f7a6ce7d-1c5d-4f92-b6ad-ce1d8a5b3002"), null, 864, null, new DateTime(927, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 927, "Владетел, при когото България достига значителен политически и културен подем (Златен век).", new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), null, "Цар Симеон I Велики" }
                });

            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "EraId", "Title" },
                values: new object[,]
                {
                    { new Guid("5e16b7c8-1d9f-4a72-83e4-9f2c7b1d7001"), new Guid("76164f60-cb04-400a-90f0-97203f0e420a"), "Античност: основни знания" },
                    { new Guid("6f27c8d9-3a5e-4b91-a6f2-1d8c7e2b7002"), new Guid("76164f60-cb04-400a-90f0-97203f0e420a"), "Античност: култура и общество" },
                    { new Guid("7a38d9e1-6b2c-4f83-b7d1-4e9c2f3a7003"), new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), "Първо българско царство: ключови факти" },
                    { new Guid("8b49e1f2-7c3d-4a92-91e8-5f2a7d3c7004"), new Guid("471c687f-caef-4cf6-8a8b-3dad51058485"), "Второ българско царство: владетели и събития" },
                    { new Guid("9c5af213-8d4e-4c71-a3f9-6b1e2d7f7005"), new Guid("68a72d82-601f-49c1-821c-850a82646c21"), "Османско владичество: борби и възраждане" }
                });

            migrationBuilder.InsertData(
                table: "Artifacts",
                columns: new[] { "Id", "CivilizationId", "Description", "DiscoveredAt", "EraId", "ImageUrl", "Location", "Material", "Name", "Year" },
                values: new object[,]
                {
                    { new Guid("47ab19c2-8d3f-4e91-a7c2-1f8e9b3d1001"), new Guid("d1a57a73-e7af-4db9-a713-eea9b9f579e2"), "Ритуална златна маска, свързвана с тракийски погребални практики. Използвана като символ за богатство и статус.", new DateTime(2012, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("76164f60-cb04-400a-90f0-97203f0e420a"), null, "Тракия", "Злато", "Златна маска (символичен артефакт)", -400 },
                    { new Guid("58bc2ad3-9e4f-4b82-b6d3-2a9f7c4e1002"), new Guid("2f0c3a11-7b8e-4f6a-9d10-5c6b7a8e9002"), "Оръжие от ранното Средновековие. Използвано в бойни сблъсъци и като знак за воинска принадлежност.", new DateTime(2008, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), null, "Североизточна България", "Желязо", "Меч (ранносредновековен)", 800 },
                    { new Guid("69cd3be4-1f5a-4c73-95e4-3b1a8d5f1003"), new Guid("2f0c3a11-7b8e-4f6a-9d10-5c6b7a8e9002"), "Каменен надпис, свидетелстващ за административни и културни практики в българските земи през Средновековието.", new DateTime(1999, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), null, "Плиска", "Камък", "Каменен надпис", 900 }
                });

            migrationBuilder.InsertData(
                table: "Figures",
                columns: new[] { "Id", "BirthDate", "BirthYear", "CivilizationId", "DeathDate", "DeathYear", "Description", "EraId", "ImageUrl", "Name" },
                values: new object[] { new Guid("e6f5bd6c-9b4c-4e01-a5fc-bd9c7f4a3001"), null, 640, new Guid("2f0c3a11-7b8e-4f6a-9d10-5c6b7a8e9002"), null, 701, "Основател на Дунавска България. Свързан с утвърждаването на българската държава на Балканите.", new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"), null, "Хан Аспарух" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "QuizId", "Text" },
                values: new object[,]
                {
                    { new Guid("03c7189a-6e2f-4c57-b9a1-9c5a7f1b8007"), new Guid("8b49e1f2-7c3d-4a92-91e8-5f2a7d3c7004"), "Коя година се свързва с възстановяването на българската държава (1185)?" },
                    { new Guid("14d829ab-7f3a-4d68-81b2-ad6b8c2c8008"), new Guid("8b49e1f2-7c3d-4a92-91e8-5f2a7d3c7004"), "Кой владетел е свързан с разширение и подем на Второто царство?" },
                    { new Guid("25e93abc-8a4b-4e79-92c3-be7c9d3d8009"), new Guid("9c5af213-8d4e-4c71-a3f9-6b1e2d7f7005"), "Кое събитие е част от националноосвободителните борби?" },
                    { new Guid("36fa4bcd-9b5c-4f8a-a3d4-cf8d1e4e8010"), new Guid("9c5af213-8d4e-4c71-a3f9-6b1e2d7f7005"), "Коя година е Освобождението на България?" },
                    { new Guid("ad61b324-9e5f-4b82-b7d2-3c8e1f4a8001"), new Guid("5e16b7c8-1d9f-4a72-83e4-9f2c7b1d7001"), "Кое население е сред древните обитатели на Балканите?" },
                    { new Guid("be72c435-1f6a-4c91-92e3-4d9f2a5b8002"), new Guid("5e16b7c8-1d9f-4a72-83e4-9f2c7b1d7001"), "Коя е една характерна черта на античните цивилизации?" },
                    { new Guid("cf83d546-2a7b-4e13-a6f4-5e1c3b6d8003"), new Guid("6f27c8d9-3a5e-4b91-a6f2-1d8c7e2b7002"), "Какво е значението на писмеността за древните общества?" },
                    { new Guid("d094e657-3b8c-4f24-b7a5-6f2d4c7e8004"), new Guid("6f27c8d9-3a5e-4b91-a6f2-1d8c7e2b7002"), "Кое е типично за античната градска култура?" },
                    { new Guid("e1a5f768-4c9d-4a35-91e6-7a3e5d8f8005"), new Guid("7a38d9e1-6b2c-4f83-b7d1-4e9c2f3a7003"), "Коя година традиционно се приема за начало на българската държава?" },
                    { new Guid("f2b60789-5d1e-4b46-a7f8-8b4f6e9a8006"), new Guid("7a38d9e1-6b2c-4f83-b7d1-4e9c2f3a7003"), "Кой владетел е свързан с културния разцвет през Златния век?" }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "IsCorrect", "QuestionId", "Text" },
                values: new object[,]
                {
                    { new Guid("03c7189a-6e2f-4c57-b9a1-9c5a7f1b9002"), false, new Guid("ad61b324-9e5f-4b82-b7d2-3c8e1f4a8001"), "Викинги" },
                    { new Guid("08a6cf2e-2d58-4e83-7efe-df8091a3b3ee"), false, new Guid("25e93abc-8a4b-4e79-92c3-be7c9d3d8009"), "Основаване на Рим" },
                    { new Guid("08b7df8e-2d6e-4a83-97be-df2e9b6c9018"), false, new Guid("e1a5f768-4c9d-4a35-91e6-7a3e5d8f8005"), "1018 г." },
                    { new Guid("14d829ab-7f3a-4d68-81b2-ad6b8c2c9003"), false, new Guid("ad61b324-9e5f-4b82-b7d2-3c8e1f4a8001"), "Ацтеки" },
                    { new Guid("19b7d03f-3e69-4f94-80a0-e091a2b4c4ff"), false, new Guid("25e93abc-8a4b-4e79-92c3-be7c9d3d8009"), "Откриване на Америка" },
                    { new Guid("19c8e09f-3e7f-4b74-a8cf-e03f1c7d9019"), false, new Guid("e1a5f768-4c9d-4a35-91e6-7a3e5d8f8005"), "1185 г." },
                    { new Guid("25e93abc-8a4b-4e79-92c3-be7c9d3d9004"), false, new Guid("ad61b324-9e5f-4b82-b7d2-3c8e1f4a8001"), "Самураи" },
                    { new Guid("2ac8e150-4f7a-4a05-91b1-f1a2b3c5d500"), false, new Guid("25e93abc-8a4b-4e79-92c3-be7c9d3d8009"), "Френска революция" },
                    { new Guid("2ad9f1a0-4f8a-4c65-b9d0-f14a2d8e9020"), false, new Guid("e1a5f768-4c9d-4a35-91e6-7a3e5d8f8005"), "1878 г." },
                    { new Guid("36fa4bcd-9b5c-4f8a-a3d4-cf8d1e4e9005"), true, new Guid("be72c435-1f6a-4c91-92e3-4d9f2a5b8002"), "Развитие на писменост и държавност" },
                    { new Guid("3bd9f261-508b-4b16-a2c2-02b3c4d6e611"), true, new Guid("36fa4bcd-9b5c-4f8a-a3d4-cf8d1e4e8010"), "1878 г." },
                    { new Guid("3beaf2b1-508b-4d56-a1e1-02b3c4d5e621"), true, new Guid("f2b60789-5d1e-4b46-a7f8-8b4f6e9a8006"), "Цар Симеон Велики" },
                    { new Guid("47ab19c2-8d3f-4e91-a7c2-1f8e9b3d9006"), false, new Guid("be72c435-1f6a-4c91-92e3-4d9f2a5b8002"), "Интернет и електронна търговия" },
                    { new Guid("4cea0372-619c-4c27-b3d3-13c4d5e7f722"), false, new Guid("36fa4bcd-9b5c-4f8a-a3d4-cf8d1e4e8010"), "1396 г." },
                    { new Guid("4cfb03c2-619c-4e67-b2f2-13c4d5e6f732"), false, new Guid("f2b60789-5d1e-4b46-a7f8-8b4f6e9a8006"), "Паисий Хилендарски" },
                    { new Guid("58bc2ad3-9e4f-4b82-b6d3-2a9f7c4e9007"), false, new Guid("be72c435-1f6a-4c91-92e3-4d9f2a5b8002"), "Космически полети" },
                    { new Guid("5d0c14d3-72ad-4f78-c3a3-24d5e6f70843"), false, new Guid("f2b60789-5d1e-4b46-a7f8-8b4f6e9a8006"), "Васил Левски" },
                    { new Guid("5dfb1483-72ad-4d38-c4e4-24d5e6f80833"), false, new Guid("36fa4bcd-9b5c-4f8a-a3d4-cf8d1e4e8010"), "1018 г." },
                    { new Guid("69cd3be4-1f5a-4c73-95e4-3b1a8d5f9008"), false, new Guid("be72c435-1f6a-4c91-92e3-4d9f2a5b8002"), "Съвременни парламенти" },
                    { new Guid("6e0c2594-83be-4e49-d5f5-35e6f7081934"), false, new Guid("36fa4bcd-9b5c-4f8a-a3d4-cf8d1e4e8010"), "1453 г." },
                    { new Guid("6e1d25e4-83be-4a89-d4b4-35e6f7081944"), false, new Guid("f2b60789-5d1e-4b46-a7f8-8b4f6e9a8006"), "Христо Ботев" },
                    { new Guid("7ade4cf5-2a6b-4d64-a4f5-4c2b9e6a9009"), true, new Guid("cf83d546-2a7b-4e13-a6f4-5e1c3b6d8003"), "Съхранява знание и улеснява управлението" },
                    { new Guid("7f2e36f5-94cf-4b9a-e5c5-46f708192a55"), true, new Guid("03c7189a-6e2f-4c57-b9a1-9c5a7f1b8007"), "1185 г." },
                    { new Guid("802f47a6-a5d0-4c0b-f6d6-5708192b3b66"), false, new Guid("03c7189a-6e2f-4c57-b9a1-9c5a7f1b8007"), "681 г." },
                    { new Guid("8bef5d06-3b7c-4e55-b3a6-5d3c1f7b9010"), false, new Guid("cf83d546-2a7b-4e13-a6f4-5e1c3b6d8003"), "Замества земеделието" },
                    { new Guid("913058b7-b6e1-4d1c-07e7-68192a3c4c77"), false, new Guid("03c7189a-6e2f-4c57-b9a1-9c5a7f1b8007"), "1396 г." },
                    { new Guid("91f06e17-4c8d-4f46-92b7-6e4d2a8c9011"), false, new Guid("cf83d546-2a7b-4e13-a6f4-5e1c3b6d8003"), "Премахва нуждата от закони" },
                    { new Guid("a24069c8-c7f2-4e2d-18f8-792a3b4d5d88"), false, new Guid("03c7189a-6e2f-4c57-b9a1-9c5a7f1b8007"), "330 г." },
                    { new Guid("a2f17f28-5d9e-4a37-a1c8-7f5e3b9d9012"), false, new Guid("cf83d546-2a7b-4e13-a6f4-5e1c3b6d8003"), "Забранява търговията" },
                    { new Guid("b3517ad9-d803-4f3e-29a9-8a3b4c5e6e99"), true, new Guid("14d829ab-7f3a-4d68-81b2-ad6b8c2c8008"), "Иван Асен II" },
                    { new Guid("b3f28a39-6e1f-4b28-b2d9-8a6f4c1e9013"), true, new Guid("d094e657-3b8c-4f24-b7a5-6f2d4c7e8004"), "Площади, обществени сгради и пазари" },
                    { new Guid("c4628bea-e914-4a4f-3aba-9b4c5d6f7faa"), false, new Guid("14d829ab-7f3a-4d68-81b2-ad6b8c2c8008"), "Кубрат" },
                    { new Guid("c4f39b4a-7f2a-4c19-83ea-9b7a5d2f9014"), false, new Guid("d094e657-3b8c-4f24-b7a5-6f2d4c7e8004"), "Небостъргачи от стъкло" },
                    { new Guid("d5739cfb-fa25-4b50-4bcb-ac5d6e7080bb"), false, new Guid("14d829ab-7f3a-4d68-81b2-ad6b8c2c8008"), "Аспарух" },
                    { new Guid("d5f4ac5b-8a3b-4d10-94fb-ac8b6e3f9015"), false, new Guid("d094e657-3b8c-4f24-b7a5-6f2d4c7e8004"), "Метро линии" },
                    { new Guid("e684ad0c-0b36-4c61-5cdc-bd6e7f8191cc"), false, new Guid("14d829ab-7f3a-4d68-81b2-ad6b8c2c8008"), "Крум" },
                    { new Guid("e6f5bd6c-9b4c-4e01-a5fc-bd9c7f4a9016"), false, new Guid("d094e657-3b8c-4f24-b7a5-6f2d4c7e8004"), "Магистрали и летища" },
                    { new Guid("f2b60789-5d1e-4b46-a7f8-8b4f6e9a9001"), true, new Guid("ad61b324-9e5f-4b82-b7d2-3c8e1f4a8001"), "Траки" },
                    { new Guid("f795be1d-1c47-4d72-6ded-ce7f8092a2dd"), true, new Guid("25e93abc-8a4b-4e79-92c3-be7c9d3d8009"), "Априлско въстание" },
                    { new Guid("f7a6ce7d-1c5d-4f92-b6ad-ce1d8a5b9017"), true, new Guid("e1a5f768-4c9d-4a35-91e6-7a3e5d8f8005"), "681 г." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("03c7189a-6e2f-4c57-b9a1-9c5a7f1b9002"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("08a6cf2e-2d58-4e83-7efe-df8091a3b3ee"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("08b7df8e-2d6e-4a83-97be-df2e9b6c9018"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("14d829ab-7f3a-4d68-81b2-ad6b8c2c9003"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("19b7d03f-3e69-4f94-80a0-e091a2b4c4ff"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("19c8e09f-3e7f-4b74-a8cf-e03f1c7d9019"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("25e93abc-8a4b-4e79-92c3-be7c9d3d9004"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("2ac8e150-4f7a-4a05-91b1-f1a2b3c5d500"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("2ad9f1a0-4f8a-4c65-b9d0-f14a2d8e9020"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("36fa4bcd-9b5c-4f8a-a3d4-cf8d1e4e9005"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("3bd9f261-508b-4b16-a2c2-02b3c4d6e611"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("3beaf2b1-508b-4d56-a1e1-02b3c4d5e621"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("47ab19c2-8d3f-4e91-a7c2-1f8e9b3d9006"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("4cea0372-619c-4c27-b3d3-13c4d5e7f722"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("4cfb03c2-619c-4e67-b2f2-13c4d5e6f732"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("58bc2ad3-9e4f-4b82-b6d3-2a9f7c4e9007"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("5d0c14d3-72ad-4f78-c3a3-24d5e6f70843"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("5dfb1483-72ad-4d38-c4e4-24d5e6f80833"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("69cd3be4-1f5a-4c73-95e4-3b1a8d5f9008"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("6e0c2594-83be-4e49-d5f5-35e6f7081934"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("6e1d25e4-83be-4a89-d4b4-35e6f7081944"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("7ade4cf5-2a6b-4d64-a4f5-4c2b9e6a9009"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("7f2e36f5-94cf-4b9a-e5c5-46f708192a55"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("802f47a6-a5d0-4c0b-f6d6-5708192b3b66"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("8bef5d06-3b7c-4e55-b3a6-5d3c1f7b9010"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("913058b7-b6e1-4d1c-07e7-68192a3c4c77"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("91f06e17-4c8d-4f46-92b7-6e4d2a8c9011"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("a24069c8-c7f2-4e2d-18f8-792a3b4d5d88"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("a2f17f28-5d9e-4a37-a1c8-7f5e3b9d9012"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("b3517ad9-d803-4f3e-29a9-8a3b4c5e6e99"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("b3f28a39-6e1f-4b28-b2d9-8a6f4c1e9013"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("c4628bea-e914-4a4f-3aba-9b4c5d6f7faa"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("c4f39b4a-7f2a-4c19-83ea-9b7a5d2f9014"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("d5739cfb-fa25-4b50-4bcb-ac5d6e7080bb"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("d5f4ac5b-8a3b-4d10-94fb-ac8b6e3f9015"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("e684ad0c-0b36-4c61-5cdc-bd6e7f8191cc"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("e6f5bd6c-9b4c-4e01-a5fc-bd9c7f4a9016"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("f2b60789-5d1e-4b46-a7f8-8b4f6e9a9001"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("f795be1d-1c47-4d72-6ded-ce7f8092a2dd"));

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: new Guid("f7a6ce7d-1c5d-4f92-b6ad-ce1d8a5b9017"));

            migrationBuilder.DeleteData(
                table: "Artifacts",
                keyColumn: "Id",
                keyValue: new Guid("47ab19c2-8d3f-4e91-a7c2-1f8e9b3d1001"));

            migrationBuilder.DeleteData(
                table: "Artifacts",
                keyColumn: "Id",
                keyValue: new Guid("58bc2ad3-9e4f-4b82-b6d3-2a9f7c4e1002"));

            migrationBuilder.DeleteData(
                table: "Artifacts",
                keyColumn: "Id",
                keyValue: new Guid("69cd3be4-1f5a-4c73-95e4-3b1a8d5f1003"));

            migrationBuilder.DeleteData(
                table: "Artifacts",
                keyColumn: "Id",
                keyValue: new Guid("7ade4cf5-2a6b-4d64-a4f5-4c2b9e6a1004"));

            migrationBuilder.DeleteData(
                table: "Artifacts",
                keyColumn: "Id",
                keyValue: new Guid("8bef5d06-3b7c-4e55-b3a6-5d3c1f7b1005"));

            migrationBuilder.DeleteData(
                table: "Civilizations",
                keyColumn: "Id",
                keyValue: new Guid("9f578916-414a-4d35-92b3-fbc187d382dd"));

            migrationBuilder.DeleteData(
                table: "Civilizations",
                keyColumn: "Id",
                keyValue: new Guid("ad1d6a1a-6122-420d-908b-55fbe5eb18ca"));

            migrationBuilder.DeleteData(
                table: "Civilizations",
                keyColumn: "Id",
                keyValue: new Guid("f1a57a73-e7af-4db9-a713-eea9b9f579e2"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("91f06e17-4c8d-4f46-92b7-6e4d2a8c2001"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("a2f17f28-5d9e-4a37-a1c8-7f5e3b9d2002"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b3f28a39-6e1f-4b28-b2d9-8a6f4c1e2003"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("c4f39b4a-7f2a-4c19-83ea-9b7a5d2f2004"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("d5f4ac5b-8a3b-4d10-94fb-ac8b6e3f2005"));

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: new Guid("08b7df8e-2d6e-4a83-97be-df2e9b6c3003"));

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: new Guid("19c8e09f-3e7f-4b74-a8cf-e03f1c7d3004"));

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: new Guid("2ad9f1a0-4f8a-4c65-b9d0-f14a2d8e3005"));

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: new Guid("e6f5bd6c-9b4c-4e01-a5fc-bd9c7f4a3001"));

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: new Guid("f7a6ce7d-1c5d-4f92-b6ad-ce1d8a5b3002"));

            migrationBuilder.DeleteData(
                table: "Civilizations",
                keyColumn: "Id",
                keyValue: new Guid("2f0c3a11-7b8e-4f6a-9d10-5c6b7a8e9002"));

            migrationBuilder.DeleteData(
                table: "Civilizations",
                keyColumn: "Id",
                keyValue: new Guid("d1a57a73-e7af-4db9-a713-eea9b9f579e2"));

            migrationBuilder.DeleteData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("e1a57a73-e7af-4db9-a713-eea9b9f579e2"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("03c7189a-6e2f-4c57-b9a1-9c5a7f1b8007"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("14d829ab-7f3a-4d68-81b2-ad6b8c2c8008"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("25e93abc-8a4b-4e79-92c3-be7c9d3d8009"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("36fa4bcd-9b5c-4f8a-a3d4-cf8d1e4e8010"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ad61b324-9e5f-4b82-b7d2-3c8e1f4a8001"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("be72c435-1f6a-4c91-92e3-4d9f2a5b8002"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("cf83d546-2a7b-4e13-a6f4-5e1c3b6d8003"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d094e657-3b8c-4f24-b7a5-6f2d4c7e8004"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e1a5f768-4c9d-4a35-91e6-7a3e5d8f8005"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("f2b60789-5d1e-4b46-a7f8-8b4f6e9a8006"));

            migrationBuilder.DeleteData(
                table: "Quizzes",
                keyColumn: "Id",
                keyValue: new Guid("5e16b7c8-1d9f-4a72-83e4-9f2c7b1d7001"));

            migrationBuilder.DeleteData(
                table: "Quizzes",
                keyColumn: "Id",
                keyValue: new Guid("6f27c8d9-3a5e-4b91-a6f2-1d8c7e2b7002"));

            migrationBuilder.DeleteData(
                table: "Quizzes",
                keyColumn: "Id",
                keyValue: new Guid("7a38d9e1-6b2c-4f83-b7d1-4e9c2f3a7003"));

            migrationBuilder.DeleteData(
                table: "Quizzes",
                keyColumn: "Id",
                keyValue: new Guid("8b49e1f2-7c3d-4a92-91e8-5f2a7d3c7004"));

            migrationBuilder.DeleteData(
                table: "Quizzes",
                keyColumn: "Id",
                keyValue: new Guid("9c5af213-8d4e-4c71-a3f9-6b1e2d7f7005"));

            migrationBuilder.DeleteData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("471c687f-caef-4cf6-8a8b-3dad51058485"));

            migrationBuilder.DeleteData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("68a72d82-601f-49c1-821c-850a82646c21"));

            migrationBuilder.DeleteData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("76164f60-cb04-400a-90f0-97203f0e420a"));

            migrationBuilder.DeleteData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("c5c979ed-aead-4f32-8c53-36218218fbbd"));
        }
    }
}
