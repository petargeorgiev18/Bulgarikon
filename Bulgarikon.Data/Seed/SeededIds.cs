using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulgarikon.Data.Seed
{
    public static class SeededIds
    {
        // Era IDs
        public static readonly Guid Era_Antiquity = Guid.Parse("76164f60-cb04-400a-90f0-97203f0e420a");
        public static readonly Guid Era_MigrationPeriod = Guid.Parse("e1a57a73-e7af-4db9-a713-eea9b9f579e2");
        public static readonly Guid Era_FirstBulgarianEmpire = Guid.Parse("c5c979ed-aead-4f32-8c53-36218218fbbd");
        public static readonly Guid Era_SecondBulgarianEmpire = Guid.Parse("471c687f-caef-4cf6-8a8b-3dad51058485");
        public static readonly Guid Era_OttomanPeriod = Guid.Parse("68a72d82-601f-49c1-821c-850a82646c21");

        // Civilization IDs
        public static readonly Guid Civ_Thracians = Guid.Parse("d1a57a73-e7af-4db9-a713-eea9b9f579e2");
        public static readonly Guid Civ_Bulgars = Guid.Parse("20000000-0000-0000-0000-000000000002");
        public static readonly Guid Civ_Slavs = Guid.Parse("f1a57a73-e7af-4db9-a713-eea9b9f579e2");
        public static readonly Guid Civ_Byzantines = Guid.Parse("ad1d6a1a-6122-420d-908b-55fbe5eb18ca");
        public static readonly Guid Civ_Ottomans = Guid.Parse("9f578916-414a-4d35-92b3-fbc187d382dd");

        // EventsIDs
        public static readonly Guid Event_BattlePliska = Guid.Parse("91f06e17-4c8d-4f46-92b7-6e4d2a8c2001");
        public static readonly Guid Event_BattleVarbishkiProhod = Guid.Parse("a2f17f28-5d9e-4a37-a1c8-7f5e3b9d2002");
        public static readonly Guid Event_FallTurnovo = Guid.Parse("b3f28a39-6e1f-4b28-b2d9-8a6f4c1e2003");
        public static readonly Guid Event_AprilUprising = Guid.Parse("c4f39b4a-7f2a-4c19-83ea-9b7a5d2f2004");
        public static readonly Guid Event_Union1885 = Guid.Parse("d5f4ac5b-8a3b-4d10-94fb-ac8b6e3f2005");

        // ArtifactsIDs
        public static readonly Guid Artifact_GoldMask = Guid.Parse("47ab19c2-8d3f-4e91-a7c2-1f8e9b3d1001");
        public static readonly Guid Artifact_Sword = Guid.Parse("58bc2ad3-9e4f-4b82-b6d3-2a9f7c4e1002");
        public static readonly Guid Artifact_Inscription = Guid.Parse("69cd3be4-1f5a-4c73-95e4-3b1a8d5f1003");
        public static readonly Guid Artifact_Ring = Guid.Parse("7ade4cf5-2a6b-4d64-a4f5-4c2b9e6a1004");
        public static readonly Guid Artifact_Pottery = Guid.Parse("8bef5d06-3b7c-4e55-b3a6-5d3c1f7b1005");

        // FiguresIDs
        public static readonly Guid Figure_KhanAsparuh = Guid.Parse("e6f5bd6c-9b4c-4e01-a5fc-bd9c7f4a3001");
        public static readonly Guid Figure_TzarSimeon = Guid.Parse("f7a6ce7d-1c5d-4f92-b6ad-ce1d8a5b3002");
        public static readonly Guid Figure_Levski = Guid.Parse("08b7df8e-2d6e-4a83-97be-df2e9b6c3003");
        public static readonly Guid Figure_Botev = Guid.Parse("19c8e09f-3e7f-4b74-a8cf-e03f1c7d3004");
        public static readonly Guid Figure_Kaloyan = Guid.Parse("2ad9f1a0-4f8a-4c65-b9d0-f14a2d8e3005");

        // QuizzesIDs
        public static readonly Guid Quiz_Antiquity_1 = Guid.Parse("5e16b7c8-1d9f-4a72-83e4-9f2c7b1d7001");
        public static readonly Guid Quiz_Antiquity_2 = Guid.Parse("6f27c8d9-3a5e-4b91-a6f2-1d8c7e2b7002");
        public static readonly Guid Quiz_FirstEmpire_1 = Guid.Parse("7a38d9e1-6b2c-4f83-b7d1-4e9c2f3a7003");
        public static readonly Guid Quiz_SecondEmpire_1 = Guid.Parse("8b49e1f2-7c3d-4a92-91e8-5f2a7d3c7004");
        public static readonly Guid Quiz_OttomanPeriod_1 = Guid.Parse("9c5af213-8d4e-4c71-a3f9-6b1e2d7f7005");

        // QuestionsIDs
        public static readonly Guid Question_1 = Guid.Parse("ad61b324-9e5f-4b82-b7d2-3c8e1f4a8001");
        public static readonly Guid Question_2 = Guid.Parse("be72c435-1f6a-4c91-92e3-4d9f2a5b8002");
        public static readonly Guid Question_3 = Guid.Parse("cf83d546-2a7b-4e13-a6f4-5e1c3b6d8003");
        public static readonly Guid Question_4 = Guid.Parse("d094e657-3b8c-4f24-b7a5-6f2d4c7e8004");
        public static readonly Guid Question_5 = Guid.Parse("e1a5f768-4c9d-4a35-91e6-7a3e5d8f8005");


        // AnswersIDs
        public static readonly Guid Answer_1 = Guid.Parse("f2b60789-5d1e-4b46-a7f8-8b4f6e9a9001");
        public static readonly Guid Answer_2 = Guid.Parse("03c7189a-6e2f-4c57-b9a1-9c5a7f1b9002");
        public static readonly Guid Answer_3 = Guid.Parse("14d829ab-7f3a-4d68-81b2-ad6b8c2c9003");
        public static readonly Guid Answer_4 = Guid.Parse("25e93abc-8a4b-4e79-92c3-be7c9d3d9004");
        public static readonly Guid Answer_5 = Guid.Parse("36fa4bcd-9b5c-4f8a-a3d4-cf8d1e4e9005");
    }
}
