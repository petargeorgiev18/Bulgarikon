using Bulgarikon.Data.Models;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class ArtifactConfiguration : IEntityTypeConfiguration<Artifact>
    {
        public void Configure(EntityTypeBuilder<Artifact> builder)
        {
            builder.HasData(
                new Artifact
                {
                    Id = SeededIds.Artifact_PanagurishteTreasure,
                    Name = "Панагюрско златно съкровище",
                    Description = "Едно от най-известните тракийски златни съкровища, открито през 1949 г. край Панагюрище. Състои се от девет златни съда – амфора, ритони и чаша, тежащи общо над 6 кг. Изработено е от чисто злато в края на IV и началото на III век пр. Хр. Съдовете са украсени с митологични сцени и изображения на тракийски владетели и божества. Съкровището е шедьовър на тракийското торевтично изкуство и свидетелства за високото майсторство на тракийските златари.",
                    Year = -300,
                    Material = "Злато",
                    Location = "Панагюрище",
                    DiscoveredAt = new DateTime(1949, 12, 8),
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_ThracianTombKazanlak,
                    Name = "Казанлъшка гробница",
                    Description = "Тракийска куполна гробница от края на IV и началото на III век пр. Хр., открита край Казанлък. Известна е с уникалните си стенописи, които представляват върхово постижение на тракийската живопис. В основната камера е изобразена трапеза на владетелска двойка, а в коридора – тракийски войни и колесници. Гробницата е част от Списъка на световното наследство на ЮНЕСКО и е един от най-ценните паметници на тракийската култура.",
                    Year = -300,
                    Material = "Камък, стенописи",
                    Location = "Казанлък",
                    DiscoveredAt = new DateTime(1944, 4, 19),
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_ThracianTombSveshtari,
                    Name = "Свещарска гробница",
                    Description = "Тракийска гробница от III век пр. Хр., открита край с. Свещари. Уникална е с десетте кариатиди – полуженски, полурастителни фигури, които поддържат стените на гробницата. Архитектурата и декорацията ѝ са без аналог в тракийските земи. Гробницата е част от Списъка на световното наследство на ЮНЕСКО и свидетелства за високото развитие на тракийската архитектура и скулптура през елинистическата епоха.",
                    Year = -250,
                    Material = "Камък",
                    Location = "Свещари, Разградско",
                    DiscoveredAt = new DateTime(1982, 1, 1),
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_RogozenTreasure,
                    Name = "Рогозенско съкровище",
                    Description = "Тракийско сребърно съкровище от V-IV век пр. Хр., открито край с. Рогозен, Врачанско. Състои се от 165 съда – чаши, кани, фиали и други, с общо тегло над 20 кг сребро. Много от съдовете са позлатени и украсени с богата орнаментика и сцени от тракийската митология и бит. Това е най-голямото тракийско съкровище, откривано някога, и свидетелства за развитието на тракийското торевтично изкуство.",
                    Year = -400,
                    Material = "Сребро, злато",
                    Location = "Рогозен, Врачанско",
                    DiscoveredAt = new DateTime(1986, 1, 1),
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_ValchitranTreasure,
                    Name = "Вълчитрънско златно съкровище",
                    Description = "Тракийско златно съкровище от края на бронзовата епоха (XIII-XII век пр. Хр.), открито край с. Вълчитрън, Плевенско. Състои се от 13 златни съда с общо тегло над 12 кг злато. Най-впечатляващ е големият съд, съставен от три части, украсени с концентрични кръгове. Съкровището свидетелства за ранното развитие на металообработването по българските земи.",
                    Year = -1200,
                    Material = "Злато",
                    Location = "Вълчитрън, Плевенско",
                    DiscoveredAt = new DateTime(1924, 1, 1),
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_SofiaRomanWalls,
                    Name = "Римски стени на Сердика",
                    Description = "Останки от римските укрепления на град Сердика (днешна София), строени през II-IV век. Запазени са части от крепостните стени, кули и порти, които днес могат да се видят в центъра на София, особено в подлеза на Ларгото. Стените свидетелстват за значението на Сердика като важен административен и военен център на Римската империя.",
                    Year = 200,
                    Material = "Камък, тухли",
                    Location = "Сердика (София)",
                    DiscoveredAt = new DateTime(1950, 1, 1),
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_RomanTheatrePlovdiv,
                    Name = "Римски театър в Пловдив",
                    Description = "Един от най-добре запазените римски театри в света, построен през I век сл. Хр. по времето на император Траян. Вмества около 6000 зрители и все още функционира за културни събития. Театърът е разположен между двата хълма на Филипопол (днешен Пловдив) и е впечатляващ пример за римска архитектура и градоустройство.",
                    Year = 100,
                    Material = "Мрамор, камък",
                    Location = "Филипопол (Пловдив)",
                    DiscoveredAt = new DateTime(1968, 1, 1),
                    EraId = SeededIds.Era_Antiquity,
                    CivilizationId = SeededIds.Civ_Thracians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_MadaraHorseman,
                    Name = "Мадарски конник",
                    Description = "Уникален скален релеф от началото на VIII век, изсечен в скалите на Мадарското плато. Изобразява конник, който побеждава лъв, придружен от надписи на гръцки език. Паметникът е свързан с българските ханове Тервел, Кормесий и Омуртаг. Той е единственият подобен релеф в Европа и е част от Списъка на световното наследство на ЮНЕСКО, символ на българската държавност през Ранното средновековие.",
                    Year = 710,
                    Material = "Скала",
                    Location = "Мадара, Шуменско",
                    DiscoveredAt = new DateTime(1800, 1, 1),
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_PliskaPalace,
                    Name = "Дворец в Плиска",
                    Description = "Останки от дворцовия комплекс в първата българска столица Плиска. Дворецът на хан Омуртаг (814-831) е бил монументална сграда с представителен характер, включваща тронна зала, жилищни помещения и бани. Архитектурата му съчетава прабългарски, византийски и западноевропейски влияния. Той е свидетелство за величието на Първото българско царство.",
                    Year = 820,
                    Material = "Камък, тухли",
                    Location = "Плиска",
                    DiscoveredAt = new DateTime(1899, 1, 1),
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_ProtoBulgaricInscriptions,
                    Name = "Прабългарски надписи",
                    Description = "Колекция от каменни надписи от VIII-IX век, намерени в Плиска, Преслав и други старобългарски центрове. Надписите са на гръцки език и съдържат ценна информация за историята, хронологията, титлите и администрацията на Първото българско царство. Най-известни са надписите на хановете Крум, Омуртаг и Маламир.",
                    Year = 800,
                    Material = "Камък",
                    Location = "Плиска, Преслав",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_PliskaBasilica,
                    Name = "Голяма базилика в Плиска",
                    Description = "Най-голямата християнска катедрала в средновековна Европа, построена след покръстването на българите (864). Дълга е 102 метра и има внушителни размери. Базиликата е била главната катедрала на Българската църква през IX-X век. Днес са запазени основите ѝ, които дават представа за монументалния характер на раннохристиянската архитектура в България.",
                    Year = 870,
                    Material = "Камък",
                    Location = "Плиска",
                    DiscoveredAt = new DateTime(1899, 1, 1),
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_OldGreatBulgariaSwords,
                    Name = "Мечове от Стара Велика България",
                    Description = "Колекция от ранносредновековни мечове и въоръжение, свързани с прабългарската култура от периода на Стара Велика България (VII век). Намерени са в района на днешна Украйна и Русия и свидетелстват за високото ниво на военното дело при прабългарите. Мечовете са от тип, характерен за степните народи, и са били символ на власт и воинска чест.",
                    Year = 650,
                    Material = "Желязо, стомана",
                    Location = "Причерноморски степи",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_MigrationPeriod,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_PreslavTreasure,
                    Name = "Преславско златно съкровище",
                    Description = "Българско средновековно съкровище от X век, открито край Преслав. Състои се от златни, сребърни и позлатени накити – обеци, гривни, пръстени, копчета и апликации, както и сребърни монети. Украсени са с филигран, гранулация и емайл. Съкровището свидетелства за високото развитие на българската златарска традиция през Златния век на българската култура.",
                    Year = 950,
                    Material = "Злато, сребро",
                    Location = "Преслав",
                    DiscoveredAt = new DateTime(1978, 1, 1),
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_GoldenChurchPreslav,
                    Name = "Златна църква в Преслав",
                    Description = "Известна още като Кръгла църква или Свети Йоан Златоуст, построена в началото на X век по времето на цар Симеон I. Тя е уникален архитектурен паметник с кръгъл наос и внушителни размери. Стените ѝ са били покрити с мраморни плочи и мозайки, а подът – с цветни керамични плочки. Църквата е символ на разцвета на българската архитектура през Златния век.",
                    Year = 907,
                    Material = "Камък, мрамор, мозайки",
                    Location = "Преслав",
                    DiscoveredAt = new DateTime(1927, 1, 1),
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_PreslavCeramics,
                    Name = "Преславска рисувана керамика",
                    Description = "Уникална средновековна керамика от X век, произвеждана в Преслав. Характеризира се с бяла ангоба и многоцветна рисунка с геометрични, растителни и животински мотиви. Използвана е за украса на църкви, дворци и богати къщи. Преславската керамика е върхово постижение на българското средновековно изкуство и няма аналог в други европейски страни.",
                    Year = 930,
                    Material = "Глина, ангоба, бои",
                    Location = "Преслав",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_OmurtagColumn,
                    Name = "Колона на хан Омуртаг",
                    Description = "Каменен надпис от времето на хан Омуртаг (814-831), намерен в Плиска. Текстът описва строежа на нов дворец на Дунав и съдържа ценна информация за българската хронология и титлатура. Надписът е един от най-важните извори за ранната българска история и свидетелства за административните умения на прабългарите.",
                    Year = 822,
                    Material = "Камък",
                    Location = "Плиска",
                    DiscoveredAt = new DateTime(1905, 1, 1),
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_KrepchaInscription,
                    Name = "Крепчански надпис",
                    Description = "Каменен надпис от X век, намерен край с. Крепча, Търговищко. Съдържа информация за български владетели и събития от края на IX и началото на X век. Надписът е важен извор за историята на Първото българско царство и свидетелства за развитието на старобългарската писмена традиция.",
                    Year = 940,
                    Material = "Камък",
                    Location = "Крепча, Търговищко",
                    DiscoveredAt = new DateTime(1910, 1, 1),
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_BalshaLeadSeal,
                    Name = "Оловен печат на цар Петър",
                    Description = "Оловен печат (моливдовул) от X век, принадлежал на цар Петър I (927-969). На печата са изобразени Богородица и владетелят с надпис на гръцки. Печатът е свидетелство за дипломатическите отношения на България с Византия и други страни и за високата степен на развитие на българската администрация.",
                    Year = 940,
                    Material = "Олово",
                    Location = "Преслав",
                    DiscoveredAt = new DateTime(1960, 1, 1),
                    EraId = SeededIds.Era_FirstBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_IvanovoRockChurches,
                    Name = "Ивановски скални църкви",
                    Description = "Комплекс от скални църкви, параклиси и килии, издълбани в скалите край с. Иваново, Русенско. Църквите са създадени през XIII-XIV век и са известни с добре запазените си стенописи, които са върхово постижение на Търновската художествена школа. Стенописите изобразяват библейски сцени и светци и се отличават с изразителност и богата цветова гама. Комплексът е част от Списъка на световното наследство на ЮНЕСКО.",
                    Year = 1300,
                    Material = "Скала, стенописи",
                    Location = "Иваново, Русенско",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_BoyanaChurchFrescoes,
                    Name = "Боянски стенописи",
                    Description = "Средновековна църква в софийския квартал Бояна, известна с уникалните си стенописи от 1259 г. Стенописите включват портрети на ктиторите Калоян и Десислава, както и на цар Константин Асен и царица Ирина. Те се отличават с реализъм, психологизъм и майсторство, изпреварили времето си. Църквата е част от Списъка на световното наследство на ЮНЕСКО.",
                    Year = 1259,
                    Material = "Стенописи",
                    Location = "Бояна, София",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_TurnovoRoyalPalace,
                    Name = "Царски дворец на Царевец",
                    Description = "Останки от царския дворец на хълма Царевец във Велико Търново, резиденция на българските царе през XII-XIV век. Дворцовият комплекс включва тронна зала, жилищни помещения, дворцова църква и административни сгради. Дворецът е бил център на политическия и духовен живот на Второто българско царство. Днес са запазени основите и частично възстановени стени.",
                    Year = 1200,
                    Material = "Камък, тухли",
                    Location = "Търново",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_ShishmanCrown,
                    Name = "Корона на цар Иван Шишман",
                    Description = "Предполагаема корона на последния търновски цар Иван Шишман (1371-1395). Представлява златна диадема, украсена със скъпоценни камъни, с изображения на хералдически символи. Тя е символ на българската държавност в навечерието на османското завоевание и свидетелства за богатството и престижа на българския двор през XIV век.",
                    Year = 1380,
                    Material = "Злато, скъпоценни камъни",
                    Location = "Търново",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_AssenovaFortress,
                    Name = "Асенова крепост",
                    Description = "Средновековна крепост край Асеновград, възстановена от цар Иван Асен II през 1231 г. Крепостта има стратегическо значение и контролира пътищата към Беломорието. Запазена е църквата-кула, посветена на Богородица Петричка, с ценни стенописи от XIV век. Крепостта е свидетелство за военната мощ на Второто българско царство.",
                    Year = 1231,
                    Material = "Камък",
                    Location = "Асеновград",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_SecondBulgarianEmpire,
                    CivilizationId = SeededIds.Civ_Bulgars
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_OttomanBridges,
                    Name = "Османски мостове",
                    Description = "Колекция от каменни мостове, строени през османския период в България. Най-известни са мостът на река Арда край с. Дяволски мост, Кьопрюса край Свиленград и мостът на Колю Фичето в Бяла. Те свидетелстват за развитието на османската инфраструктура и строителни умения, както и за икономическия живот през периода.",
                    Year = 1600,
                    Material = "Камък",
                    Location = "Различни места в България",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Ottomans
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_OttomanBathPlovdiv,
                    Name = "Чифте баня в Пловдив",
                    Description = "Османска обществена баня (хамам) от XV век в Пловдив, една от най-старите и добре запазени в България. Банята е функционирала до 60-те години на XX век и днес е превърната в център за съвременно изкуство. Тя свидетелства за османската градска култура и традициите на обществените бани.",
                    Year = 1450,
                    Material = "Камък, тухли",
                    Location = "Пловдив",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Ottomans
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_OttomanClockTower,
                    Name = "Часовникова кула в Берковица",
                    Description = "Османска часовникова кула от XVIII век, една от многото запазени в България. Часовниковите кули са били важен елемент от османската градска архитектура и са служели за обществено времеизмерване. Кулата в Берковица е типичен представител на този тип съоръжения и свидетелства за градския живот през османския период.",
                    Year = 1762,
                    Material = "Камък, дърво",
                    Location = "Берковица",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Ottomans
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_OttomanMosques,
                    Name = "Османски джамии",
                    Description = "Колекция от османски джамии, запазени в България от XV-XIX век. Най-известни са Джамията на Ибрахим паша в Разград, Томбул джамия в Шумен, Баня баши джамия в София и Джумая джамия в Пловдив. Те свидетелстват за османската религиозна архитектура и за присъствието на мюсюлманско население в българските земи.",
                    Year = 1500,
                    Material = "Камък, тухли, олово",
                    Location = "Различни градове в България",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Ottomans
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_OttomanFortresses,
                    Name = "Османски крепости",
                    Description = "Колекция от османски крепости и укрепления, строени или достроявани през XV-XIX век. Най-известни са крепостите във Видин (Баба Вида), Белоградчик, Русчук (Русе) и Силистра. Те свидетелстват за военната стратегия на Османската империя за защита на дунавската граница и за вътрешния ред.",
                    Year = 1500,
                    Material = "Камък",
                    Location = "Северна България",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_OttomanPeriod,
                    CivilizationId = SeededIds.Civ_Ottomans
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_RilaMonasteryFresco,
                    Name = "Стенописи в Рилски манастир",
                    Description = "Стенописи от XIX век в Рилския манастир, дело на видни български зографи от Самоковската и Банската школа. Те покриват стените на главната църква („Св. Богородица“) и манастирските параклиси. Стенописите изобразяват библейски сцени, светци и български светци като Иван Рилски. Те са връх в развитието на българската църковна живопис през Възраждането.",
                    Year = 1840,
                    Material = "Стенописи",
                    Location = "Рилски манастир",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_FirstSchoolBuildings,
                    Name = "Сгради на първите училища",
                    Description = "Сгради на първите светски училища в България от XIX век. Най-известни са училището в Габрово (Априловска гимназия), училището в Копривщица („Св. св. Кирил и Методий“) и училището в Панагюрище. Те са символ на Възраждането и борбата за просвета. Сградите съчетават традиционна българска архитектура с европейски влияния.",
                    Year = 1835,
                    Material = "Камък, дърво",
                    Location = "Габрово, Копривщица, Панагюрище",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_RevivalIconostases,
                    Name = "Възрожденски иконостаси",
                    Description = "Дърворезбени иконостаси от XIX век в български църкви, шедьоври на възрожденското дърворезбарско изкуство. Най-известни са иконостасите в Рилския манастир, Троянския манастир, църквата „Св. Спас“ в Скопие и много други. Те се отличават с богата растителна и животинска орнаментика, колони и икони. Резбата е толкова фина, че наподобява дантела.",
                    Year = 1840,
                    Material = "Дърво, злато",
                    Location = "Различни църкви в България",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_RevivalHouses,
                    Name = "Възрожденски къщи",
                    Description = "Къщи от епохата на Възраждането, строени от богати български търговци и занаятчии. Те са запазени в архитектурните резервати на Копривщица, Пловдив (Старинен Пловдив), Жеравна, Трявна и други. Къщите съчетават традиционна българска архитектура с европейски влияния и са богато украсени с дърворезба, стенописи и дървени тавани. Те свидетелстват за икономическия разцвет на българите през Възраждането.",
                    Year = 1850,
                    Material = "Камък, дърво",
                    Location = "Копривщица, Пловдив, Жеравна",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_PaisiusHistoryCopy,
                    Name = "Копие на Паисиевата история",
                    Description = "Ръкописно копие на „История славянобългарска“ от Паисий Хилендарски. Оригиналът от 1762 г. не е запазен, но съществуват множество преписи от XVIII-XIX век. Най-известен е Софрониевият препис (1765). Ръкописът е свидетелство за разпространението на Паисиевата идея и за будителския дух на епохата.",
                    Year = 1800,
                    Material = "Хартия, мастило",
                    Location = "Зографски манастир, Рилски манастир",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_RevivalPrintingPress,
                    Name = "Възрожденски печатници",
                    Description = "Печатни машини и оборудване от първите български печатници през Възраждането. Най-известни са печатниците на Христаки Павлович в Самоков, Константин Фотинов в Смирна, както и печатницата на братя Караминкови в Цариград. Те са изиграли ключова роля за разпространението на българската книга и просвета.",
                    Year = 1830,
                    Material = "Дърво, метал",
                    Location = "Самоков, Цариград, Пловдив",
                    DiscoveredAt = new DateTime(1900, 1, 1),
                    EraId = SeededIds.Era_Revival,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_WWIISoldierEquipment,
                    Name = "Екипировка на български войник от ВСВ",
                    Description = "Колекция от екипировка, въоръжение и лични вещи на български войници от Втората световна война (1941-1945). Включва униформи, оръжия (карабини, пистолети), каски, раници, писма и снимки. Те свидетелстват за участието на България във войната и за бита на българския войник.",
                    Year = 1943,
                    Material = "Плат, метал, кожа",
                    Location = "Различни места в България",
                    DiscoveredAt = new DateTime(2000, 1, 1),
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_CommunistMonuments,
                    Name = "Паметници от социалистическата епоха",
                    Description = "Колекция от монументални паметници, създадени през социалистическия период (1944-1989). Най-известни са паметникът на Съветската армия в София, паметникът на Бузлуджа, паметникът на връх Шипка и други. Те са типични представители на социалистическия реализъм в изкуството и свидетелстват за идеологията и естетиката на епохата.",
                    Year = 1970,
                    Material = "Камък, бронз, бетон",
                    Location = "Различни места в България",
                    DiscoveredAt = new DateTime(1990, 1, 1),
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_DemocracyProclamations,
                    Name = "Прокламации от 1989 г.",
                    Description = "Листовки, плакати, вестници и други материали от времето на демократичните промени през 1989-1990 г. Те свидетелстват за политическата активност на гражданите, за митингите, протестите и кръглите маси. Тези документи са важен извор за най-новата българска история и за прехода от тоталитаризъм към демокрация.",
                    Year = 1989,
                    Material = "Хартия",
                    Location = "София, Пловдив, Варна",
                    DiscoveredAt = new DateTime(1990, 1, 1),
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_EUAccessionDocuments,
                    Name = "Договор за присъединяване към ЕС",
                    Description = "Документи, свързани с присъединяването на България към Европейския съюз на 1 януари 2007 г. Включва Договора за присъединяване, ратификационните актове, протоколи и други официални документи. Те свидетелстват за връщането на България в европейското семейство след десетилетия на изолация.",
                    Year = 2005,
                    Material = "Хартия",
                    Location = "Брюксел, София",
                    DiscoveredAt = new DateTime(2007, 1, 1),
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgarians
                },
                new Artifact
                {
                    Id = SeededIds.Artifact_ModernBulgarianFlag,
                    Name = "Национално знаме на Република България",
                    Description = "Оригинално национално знаме от началото на XXI век, използвано при официални церемонии. Знамето е трибагреник – бяло, зелено, червено, с герба на Република България. То е символ на българската държавност, независимост и национална идентичност в съвременния свят. Този екземпляр е използван при официални посещения и международни форуми.",
                    Year = 2010,
                    Material = "Плат",
                    Location = "София",
                    DiscoveredAt = new DateTime(2020, 1, 1),
                    EraId = SeededIds.Era_Modern,
                    CivilizationId = SeededIds.Civ_Bulgarians
                }
            );
        }
    }
}