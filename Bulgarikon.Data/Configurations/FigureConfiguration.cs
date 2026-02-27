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
                    Id = SeededIds.Figure_Orpheus,
                    Name = "Орфей",
                    Description = "Легендарен тракийски певец и музикант от митологията, смятан за основоположник на орфическите мистерии." +
                    " Според митовете, с песните си успявал да омайва дивите зверове, дърветата и скалите. Участва в похода на" +
                    " аргонавтите и слиза в подземното царство, за да върне съпругата си Евридика. Тракийският произход на Орфей подчертава значението на тракийската култура за европейската цивилизация.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = null,
                    DeathYear = null,
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Figure
                {
                    Id = SeededIds.Figure_Teres,
                    Name = "Терес I",
                    Description = "Основател на Одриското царство (V в. пр. Хр.) – първата тракийска държава на Балканите." +
                    " Терес обединява множество тракийски племена и създава мощно царство, което води активни отношения с Атина" +
                    " и други гръцки полиси. Полага основите на тракийската държавност и военно-политическа организация.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = -500,
                    DeathYear = -445,
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Figure
                {
                    Id = SeededIds.Figure_Sitalkes,
                    Name = "Ситалк",
                    Description = "Цар на Одриското царство (431-424 г. пр. Хр.), при когото държавата достига най-големия си разцвет." +
                    " Сключва съюз с Атина в Пелопонеските войни и разширява границите на царството от Черно море до река Струма." +
                    " Утвърждава тракийската държава като значим фактор в региона.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = -470,
                    DeathYear = -424,
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Figure
                {
                    Id = SeededIds.Figure_SeuthesIII,
                    Name = "Севт III",
                    Description = "Тракийски владетел от края на IV и началото на III век пр. Хр., управлявал след походите на" +
                    " Александър Македонски. Основава нова столица – Севтополис, чиито разкрити останки са сред най-значимите" +
                    " археологически открития. Известен с бронзовата си глава, намерена в гробницата му край Казанлък, която е един от символите на тракийското изкуство.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = -330,
                    DeathYear = -300,
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Figure
                {
                    Id = SeededIds.Figure_Spartacus,
                    Name = "Спартак",
                    Description = "Тракийски воин, станал известен като водач на най-голямото въстание на роби срещу Римската република" +
                    " (73-71 г. пр. Хр.). Роден в тракийското племе меди, пленен и продаден за гладиатор. Въстанието му събира огромна" +
                    " армия и нанася сериозни поражения на Рим, преди да бъде потушено. Спартак се превръща в символ на борбата за свобода.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = -111,
                    DeathYear = -71,
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Figure
                {
                    Id = SeededIds.Figure_KhanKubrat,
                    Name = "Хан Кубрат",
                    Description = "Владетел на прабългарите, основал през 632 г. държавата Стара Велика България в причерноморските степи." +
                    " Израснал и възпитаван във византийския двор, Кубрат успява да обедини прабългарските племена и да създаде мощна" +
                    " държава. Сключва съюз с Византия и умира около 665 г. Заветът му към синовете да не се разделят става символ на държавно единство.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 605,
                    DeathYear = 665,
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_KhanAsparuh,
                    Name = "Хан Аспарух",
                    Description = "Основател на Дунавска България (681 г.). Третият син на хан Кубрат, който след разпадането" +
                    " на Стара Велика България повежда част от прабългарите на запад. Установява се в района на Добруджа и през 680 г." +
                    " разгромява византийската армия при Онгъла. Мирният договор от 681 г. признава българската държава и задължава Византия да плаща данък на българите.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 640,
                    DeathYear = 701,
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_KhanTervel,
                    Name = "Хан Тервел",
                    Description = "Български владетел (701-721), който укрепва държавата и разширява териториите ѝ." +
                    " Оказва съществена помощ на сваления византийски император Юстиниан II да си възвърне престола," +
                    " за което получава титлата 'кесар' – първата известна титла, дадена на чужд владетел от Византия. Изиграва ключова роля в спирането на арабското нашествие към Европа при обсадата на Константинопол (717-718).",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 675,
                    DeathYear = 721,
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_KhanKrum,
                    Name = "Хан Крум",
                    Description = "Български владетел (803-814), известен с военните си победи и административни реформи." +
                    " Разширява значително територията на България, унищожава Аварския каганат. През 811 г. разгромява и убива" +
                    " византийския император Никифор I във Върбишкия проход. Създава първия писмен законник – 'Закон за съдене на людете'. Опитва се да превземе Константинопол, но умира внезапно.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 755,
                    DeathYear = 814,
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_KhanOmurtag,
                    Name = "Хан Омуртаг",
                    Description = "Български владетел (814-831), син на хан Крум. Укрепва държавата и сключва 30-годишен мирен договор" +
                    " с Византия. Известен с мащабното си строителство – възстановява разрушената Плиска, построява дворци," +
                    " езически храмове и крепости. Неговите надписи дават ценна информация за ранната българска история и администрация.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 780,
                    DeathYear = 831,
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_KnyazBoris,
                    Name = "Княз Борис I Михаил (Покръстител)",
                    Description = "Български владетел (852-889), извършил едно от най-важните дела в българската история" +
                    " – покръстването на българите (864). Приема християнството от Византия, но успява да издейства" +
                    " създаването на автокефална българска църква. Посреща учениците на Кирил и Методий и подкрепя развитието на славянската писменост. Полага основите на българската християнска култура и църковна независимост.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 828,
                    DeathYear = 907,
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_TzarSimeon,
                    Name = "Цар Симеон I Велики",
                    Description = "Най-великият български владетел (893-927), при когото България достига своя златен век." +
                    " Образован в Константинопол, води успешни войни с Византия, разширявайки територията до три морета." +
                    " През 913 г. е коронован за 'цар на българи и ромеи'. Разгромява византийците при Ахелой (917). Покровителства книжовници, строят се църкви и манастири, развива се Преславската книжовна школа.",
                    BirthDate = null,
                    DeathDate = new DateTime(927, 5, 27),
                    ImageUrl = null,
                    BirthYear = 864,
                    DeathYear = 927,
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_CyrilMethodius,
                    Name = "Св. св. Кирил и Методий",
                    Description = "Солунски братя, византийски учени и мисионери, създатели на славянската азбука (глаголица)" +
                    " и преводачи на богослужебни текстове на славянски език. През 863 г. заминават за Великоморавия," +
                    " където създават славянската писменост. След смъртта им техните ученици намират убежище в България при княз Борис I, където продължават делото им. Обявени са за съпокровители на Европа.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 815,
                    DeathYear = 885,
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = null
                },
                new Figure
                {
                    Id = SeededIds.Figure_ClementOhridski,
                    Name = "Свети Климент Охридски",
                    Description = "Средновековен български учен и книжовник, един от най-видните ученици на Кирил и Методий." +
                    " Работи в Охрид, където основава Охридската книжовна школа и обучава хиляди ученици. Смята се за създател" +
                    " на кирилицата. Автор е на множество църковни текстове и първият български епископ със славянски език в богослужението.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 840,
                    DeathYear = 916,
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_NahumPreslavski,
                    Name = "Свети Наум Преславски",
                    Description = "Български книжовник, ученик на Кирил и Методий. Работи първо в Преслав, а по-късно в Охрид заедно" +
                    " с Климент. Основава манастир на Охридското езеро, който носи неговото име. Допринася значително за развитието" +
                    " на старобългарската книжнина и утвърждаването на славянската писменост.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 830,
                    DeathYear = 910,
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_TzarPetar,
                    Name = "Цар Петър I",
                    Description = "Български владетел (927-969), син на цар Симеон I. Управлението му започва с дълъг мирен период" +
                    " и признаване на българската патриаршия. Жени се за византийската принцеса Мария Лакапина. В края на управлението " +
                    "му държавата отслабва поради вътрешни размирици и нашествия на унгарци и печенеги. Канонизиран е за светец поради благочестивия си живот.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 912,
                    DeathYear = 969,
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Asen,
                    Name = "Асен I",
                    Description = "Един от водачите на въстанието срещу византийската власт (1185) заедно с брат си Петър." +
                    " След успеха на въстанието става съвладетел на възстановената българска държава. Управлява до 1196 г.," +
                    " когато е убит от своя братовчед Ивачко. Смятан за един от възстановителите на българската държавност.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1150,
                    DeathYear = 1196,
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Petar,
                    Name = "Петър IV",
                    Description = "Български владетел, заедно с брат си Асен ръководи въстанието срещу византийската власт (1185)." +
                    " След възстановяването на българската държава управлява като цар до 1197 г. Отстъпва престола на по-младия" +
                    " си брат Калоян. Играе ключова роля в утвърждаването на възстановената българска държава.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1140,
                    DeathYear = 1197,
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Kaloyan,
                    Name = "Цар Калоян",
                    Description = "Владетел на Второто българско царство (1197-1207), най-малкият брат на Асен и Петър." +
                    " Провежда активна външна политика и укрепва държавата. През 1205 г. разгромява кръстоносците в битката" +
                    " при Одрин и пленява латинския император Балдуин Фландърски. Води преговори с папата и получава кралска корона, но загива при неясни обстоятелства през 1207 г.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1168,
                    DeathYear = 1207,
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_TzarIvanAsenII,
                    Name = "Цар Иван Асен II",
                    Description = "Най-значимият владетел на Второто българско царство (1218-1241)." +
                    " При него държавата достига най-голямото си териториално разширение до три морета. През 1230 г." +
                    " разгромява епирския деспот Теодор Комнин при Клокотница. Управлението му се характеризира с икономически и културен подем. Търново се превръща във водещ политически и културен център на Балканите.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1190,
                    DeathYear = 1241,
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_EvtimiyTurnovski,
                    Name = "Патриарх Евтимий Търновски",
                    Description = "Последен български патриарх преди османското завоевание (1375-1393)." +
                    " Ръководи Търновската книжовна школа, извършва правописна реформа и оказва влияние върху книжовния живот" +
                    " в православния свят. Организира отбраната на Търново при османската обсада (1393). След падането на града е заточен, а делото му продължава от неговите ученици.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1325,
                    DeathYear = 1402,
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_TeodorSvetoslav,
                    Name = "Тодор Светослав",
                    Description = "Български цар (1300-1321) от династията Тертеровци. Укрепва централната власт и възстановява българското" +
                    " влияние в Североизточна България. Води успешни войни с Византия и възвръща важни черноморски градове." +
                    " Управлението му бележи период на стабилизация след тежките години на татарски нашествия.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1270,
                    DeathYear = 1321,
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_GeorgiVoivoda,
                    Name = "Георги войвода",
                    Description = "Български войвода, ръководил съпротива срещу османската власт в края на XIV и началото на XV век." +
                    " Действа в района на Софийско и Видинско. Събира хайдушки чети и нанася удари на османските власти." +
                    " Символ на съпротивата в първите десетилетия на османското владичество.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1370,
                    DeathYear = 1420,
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Karaibrahim,
                    Name = "Караибрахим",
                    Description = "Легендарен български хайдутин от XVII век, действал в Родопите и Тракия." +
                    " Според народните песни и предания, той защитава българското население от османските насилия и разпределя" +
                    " плячката между бедните. Образът му се превръща в символ на народния закрилник и борец срещу несправедливостта.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1650,
                    DeathYear = 1700,
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_ChavdarVoivoda,
                    Name = "Чавдар войвода",
                    Description = "Известен български хайдутин от XVI-XVII век, действал в района на Софийско и Пиротско." +
                    " Става народен герой, възпят в множество песни и легенди. С четата си нанася удари на османските власти" +
                    " и защитава местното население. Символизира хайдушкото движение като форма на съпротива.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1580,
                    DeathYear = 1630,
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_PeyuVoivoda,
                    Name = "Пейо войвода",
                    Description = "Български хайдутин от XVIII век, действал в района на Тракия и Родопите." +
                    " Прославен в народните песни като защитник на бедните и борец срещу османския гнет. " +
                    "Четата му действа дълги години и успява да се противопоставя на османските власти. Символ на хайдушката традиция и съпротива.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1720,
                    DeathYear = 1770,
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_StrahilVoivoda,
                    Name = "Страхил войвода",
                    Description = "Български хайдутин, ръководил съпротива срещу османската власт в района на Пирот и Западните покрайнини." +
                    " Става популярен герой от народните песни, възпяващи неговите подвизи. С четата си успява дълго време да се" +
                    " противопоставя на османските власти и да защитава местното българско население.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1750,
                    DeathYear = 1800,
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Paisius,
                    Name = "Паисий Хилендарски",
                    Description = "Български възрожденец, автор на 'История славянобългарска' (1762) – книгата," +
                    " която поставя началото на Българското възраждане. Хилендарският монах призовава своите сънародници" +
                    " да не забравят своя произход, език и история и да се гордеят с българското си име. Неговият труд пробужда националното самосъзнание на българите.",
                    BirthDate = null,
                    DeathDate = null,
                    ImageUrl = null,
                    BirthYear = 1722,
                    DeathYear = 1773,
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_SofroniyVrachanski,
                    Name = "Софроний Врачански",
                    Description = "Български възрожденец, книжовник и първият български епископ след Паисий." +
                    " Автор на първата печатна българска книга – 'Неделник' (1806). Продължава делото на Паисий и" +
                    " разпространява неговата история. Активно участва в борбата за българска църковна независимост и развитието на образованието.",
                    BirthDate = new DateTime(1739, 3, 11),
                    DeathDate = new DateTime(1813, 9, 23),
                    ImageUrl = null,
                    BirthYear = 1739,
                    DeathYear = 1813,
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Rakovski,
                    Name = "Георги Сава Раковски",
                    Description = "Български революционер, писател и журналист, един от идеолозите на националноосвободителното движение." +
                    " Създава първите български легии в Белград, разработва планове за освобождение на България. Автор е на множество" +
                    " трудове, сред които 'Горски пътник'. Смятан за основоположник на организираното революционно движение.",
                    BirthDate = new DateTime(1821, 4, 14),
                    DeathDate = new DateTime(1867, 10, 9),
                    ImageUrl = null,
                    BirthYear = 1821,
                    DeathYear = 1867,
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Karavelov,
                    Name = "Любен Каравелов",
                    Description = "Български писател, журналист и революционер, един от водачите на БРЦК. Автор на множество разкази," +
                    " повести и публицистични статии, в които разкрива тежкото положение на българския народ. Редактор е на вестник" +
                    " 'Свобода' и 'Независимост'. Играе ключова роля в организирането на революционното движение.",
                    BirthDate = new DateTime(1834, 11, 7),
                    DeathDate = new DateTime(1879, 1, 21),
                    ImageUrl = null,
                    BirthYear = 1834,
                    DeathYear = 1879,
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Levski,
                    Name = "Васил Левски",
                    Description = "Апостол на свободата, идеолог и организатор на българското националноосвободително движение." +
                    " Създава Вътрешната революционна организация (ВРО) и мрежа от революционни комитети в цяла България." +
                    " Разработва устав и програма за освобождението на България чрез народна революция. Заловен и обесен от османските власти на 18 февруари 1873 г. в София. Обявен за светец от Българската православна църква.",
                    BirthDate = new DateTime(1837, 7, 18),
                    DeathDate = new DateTime(1873, 2, 18),
                    ImageUrl = null,
                    BirthYear = 1837,
                    DeathYear = 1873,
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Botev,
                    Name = "Христо Ботев",
                    Description = "Поет, революционер и национален герой, един от най-ярките представители на Българското възраждане." +
                    " Автор на безсмъртни стихотворения като 'Майце си', 'На прощаване', 'Хаджи Димитър'. През 1876 г. организира чета" +
                    " и загива във Врачанския балкан в борбата за освобождение на България. Денят на неговата смърт (2 юни) се чества като Ден на Ботев и на загиналите за свободата.",
                    BirthDate = new DateTime(1848, 1, 6),
                    DeathDate = new DateTime(1876, 6, 2),
                    ImageUrl = null,
                    BirthYear = 1848,
                    DeathYear = 1876,
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_Stambolov,
                    Name = "Стефан Стамболов",
                    Description = "Български политик, държавник и национал-революционер, министър-председател на България (1887-1894)." +
                    " Един от най-значимите български политици след Освобождението. Провежда политика на твърда ръка и модернизация" +
                    " на страната. Укрепва икономиката, строи жп линии, развива индустрията. Противник на руското влияние в България. Убит през 1895 г.",
                    BirthDate = new DateTime(1854, 1, 31),
                    DeathDate = new DateTime(1895, 7, 18),
                    ImageUrl = null,
                    BirthYear = 1854,
                    DeathYear = 1895,
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_GeorgiDimitrov,
                    Name = "Георги Димитров",
                    Description = "Български комунистически политик, ръководител на Коминтерна и министър-председател" +
                    " на България (1946-1949). Прославя се като защитник на Лайпцигския процес (1933) срещу нацистите." +
                    " След 9 септември 1944 г. оглавява българското правителство и ръководи изграждането на социалистическата държава. Умира в Москва през 1949 г.",
                    BirthDate = new DateTime(1882, 6, 18),
                    DeathDate = new DateTime(1949, 7, 2),
                    ImageUrl = null,
                    BirthYear = 1882,
                    DeathYear = 1949,
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_TodorZhivkov,
                    Name = "Тодор Живков",
                    Description = "Български комунистически ръководител, управлявал България почти 35 години" +
                    " (1954-1989) – най-дългото управление в историята на страната. Периодът му се характеризира с" +
                    " относителна стабилност, икономическо развитие, но и с репресии и ограничаване на свободите. Под негово ръководство България приема нова конституция (1971). Отстранен от власт на 10 ноември 1989 г.",
                    BirthDate = new DateTime(1911, 9, 7),
                    DeathDate = new DateTime(1998, 8, 5),
                    ImageUrl = null,
                    BirthYear = 1911,
                    DeathYear = 1998,
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_ZhelyuZhelev,
                    Name = "Желю Желев",
                    Description = "Български политик и дисидент, първият демократично избран президент на България" +
                    " (1990-1997). Един от основателите на Съюза на демократичните сили (СДС). Автор на книгата 'Фашизмът" +
                    "' (1982), заради която е подлаган на репресии. Изиграва ключова роля за демократичния преход на България след 1989 г.",
                    BirthDate = new DateTime(1935, 3, 3),
                    DeathDate = new DateTime(2015, 1, 30),
                    ImageUrl = null,
                    BirthYear = 1935,
                    DeathYear = 2015,
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Figure
                {
                    Id = SeededIds.Figure_BotevModern,
                    Name = "Христо Ботев (памет)",
                    Description = "Христо Ботев остава вечен символ на борбата за свобода и национален идеал." +
                    " Денят на неговата смърт – 2 юни – се отбелязва като Ден на Ботев и на загиналите за свободата" +
                    " и независимостта на България. Всяка година в този ден сирените известяват минута мълчание в памет на героите, а неговите стихове продължават да вдъхновяват поколения българи.",
                    BirthDate = new DateTime(1848, 1, 6),
                    DeathDate = new DateTime(1876, 6, 2),
                    ImageUrl = null,
                    BirthYear = 1848,
                    DeathYear = 1876,
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgars
                }
            );
        }
    }
}