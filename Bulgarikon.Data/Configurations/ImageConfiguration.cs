using Bulgarikon.Data.Models;
using Bulgarikon.Data.Models.Enums;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasData(
                new Image
                {
                    Id = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                    Url = "https://res.cloudinary.com/dycfk6nzw/image/upload/v1776791925/bulgarikon/rmru63me6tpvdovv7i8g.png",
                    PublicId = "bulgarikon/rmru63me6tpvdovv7i8g",
                    SortOrder = 0,
                    TargetType = ImageTargetType.Era,
                    EraId = SeededIds.Era_Antiquity
                },
                new Image
                {
                    Id = Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"),
                    Url = "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227891/bulgarikon/ahtepshyyn47uyif8ohx.png",
                    PublicId = "bulgarikon/ahtepshyyn47uyif8ohx",
                    SortOrder = 0,
                    TargetType = ImageTargetType.Era,
                    EraId = SeededIds.Era_Modern
                },
                new Image
                {
                    Id = Guid.Parse("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"),
                    Url = "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227818/bulgarikon/jejoj5wmcapp8vhyi90n.png",
                    PublicId = "bulgarikon/jejoj5wmcapp8vhyi90n",
                    SortOrder = 1,
                    TargetType = ImageTargetType.Era,
                    EraId = SeededIds.Era_Modern
                },
                new Image
                {
                    Id = Guid.Parse("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"),
                    Url = "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227758/bulgarikon/nqidjbbeow3ping3mien.png",
                    PublicId = "bulgarikon/nqidjbbeow3ping3mien",
                    SortOrder = 0,
                    TargetType = ImageTargetType.Era,
                    EraId = SeededIds.Era_OttomanPeriod
                },
                new Image
                {
                    Id = Guid.Parse("a5a5a5a5-a5a5-a5a5-a5a5-a5a5a5a5a5a5"),
                    Url = "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227622/bulgarikon/cc0u13xgzr5kvalgqcjv.png",
                    PublicId = "bulgarikon/cc0u13xgzr5kvalgqcjv",
                    SortOrder = 0,
                    TargetType = ImageTargetType.Era,
                    EraId = SeededIds.Era_Revival
                },
                new Image
                {
                    Id = Guid.Parse("a6a6a6a6-a6a6-a6a6-a6a6-a6a6a6a6a6a6"),
                    Url = "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227548/bulgarikon/kssryzzjpscbmnghwwmw.png",
                    PublicId = "bulgarikon/kssryzzjpscbmnghwwmw",
                    SortOrder = 1,
                    TargetType = ImageTargetType.Era,
                    EraId = SeededIds.Era_OttomanPeriod
                },
                new Image
                {
                    Id = Guid.Parse("a7a7a7a7-a7a7-a7a7-a7a7-a7a7a7a7a7a7"),
                    Url = "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227481/bulgarikon/smprddeqzrowqd5ebbsx.png",
                    PublicId = "bulgarikon/smprddeqzrowqd5ebbsx",
                    SortOrder = 2,
                    TargetType = ImageTargetType.Era,
                    EraId = SeededIds.Era_OttomanPeriod
                },
                new Image
                {
                    Id = Guid.Parse("a8a8a8a8-a8a8-a8a8-a8a8-a8a8a8a8a8a8"),
                    Url = "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227311/bulgarikon/mpbbf2pdetbqdscuuc01.png",
                    PublicId = "bulgarikon/mpbbf2pdetbqdscuuc01",
                    SortOrder = 0,
                    TargetType = ImageTargetType.Era,
                    EraId = SeededIds.Era_SecondBulgarianEmpire
                },
                new Image
                {
                    Id = Guid.Parse("a9a9a9a9-a9a9-a9a9-a9a9-a9a9a9a9a9a9"),
                    Url = "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227201/bulgarikon/deh0osvaa1lclj5d1ywb.gif",
                    PublicId = "bulgarikon/deh0osvaa1lclj5d1ywb",
                    SortOrder = 0,
                    TargetType = ImageTargetType.Era,
                    EraId = SeededIds.Era_FirstBulgarianEmpire
                }
            );
        }
    }
}