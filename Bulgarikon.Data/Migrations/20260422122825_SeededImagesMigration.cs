using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulgarikon.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeededImagesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ArtifactId", "Caption", "CivilizationId", "EraId", "EventId", "FigureId", "PublicId", "SortOrder", "TargetType", "Url" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), null, null, null, new Guid("892d0671-4abe-5126-bf2d-fa9582ae5657"), null, null, "bulgarikon/rmru63me6tpvdovv7i8g", 0, 2, "https://res.cloudinary.com/dycfk6nzw/image/upload/v1776791925/bulgarikon/rmru63me6tpvdovv7i8g.png" },
                    { new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), null, null, null, new Guid("80a4fd24-c3fa-5a1a-a43a-642ca8375def"), null, null, "bulgarikon/ahtepshyyn47uyif8ohx", 0, 2, "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227891/bulgarikon/ahtepshyyn47uyif8ohx.png" },
                    { new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), null, null, null, new Guid("80a4fd24-c3fa-5a1a-a43a-642ca8375def"), null, null, "bulgarikon/jejoj5wmcapp8vhyi90n", 1, 2, "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227818/bulgarikon/jejoj5wmcapp8vhyi90n.png" },
                    { new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), null, null, null, new Guid("5a6c6e88-081c-5397-b779-135356530cf3"), null, null, "bulgarikon/nqidjbbeow3ping3mien", 0, 2, "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227758/bulgarikon/nqidjbbeow3ping3mien.png" },
                    { new Guid("a5a5a5a5-a5a5-a5a5-a5a5-a5a5a5a5a5a5"), null, null, null, new Guid("ab19d544-9d78-5cc7-828c-702204e8c6f7"), null, null, "bulgarikon/cc0u13xgzr5kvalgqcjv", 0, 2, "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227622/bulgarikon/cc0u13xgzr5kvalgqcjv.png" },
                    { new Guid("a6a6a6a6-a6a6-a6a6-a6a6-a6a6a6a6a6a6"), null, null, null, new Guid("5a6c6e88-081c-5397-b779-135356530cf3"), null, null, "bulgarikon/kssryzzjpscbmnghwwmw", 1, 2, "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227548/bulgarikon/kssryzzjpscbmnghwwmw.png" },
                    { new Guid("a7a7a7a7-a7a7-a7a7-a7a7-a7a7a7a7a7a7"), null, null, null, new Guid("5a6c6e88-081c-5397-b779-135356530cf3"), null, null, "bulgarikon/smprddeqzrowqd5ebbsx", 2, 2, "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227481/bulgarikon/smprddeqzrowqd5ebbsx.png" },
                    { new Guid("a8a8a8a8-a8a8-a8a8-a8a8-a8a8a8a8a8a8"), null, null, null, new Guid("d04736bd-8718-5c73-bf46-3e969f526313"), null, null, "bulgarikon/mpbbf2pdetbqdscuuc01", 0, 2, "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227311/bulgarikon/mpbbf2pdetbqdscuuc01.png" },
                    { new Guid("a9a9a9a9-a9a9-a9a9-a9a9-a9a9a9a9a9a9"), null, null, null, new Guid("582d246a-d318-52ed-8f52-1fe5eefb1b9b"), null, null, "bulgarikon/deh0osvaa1lclj5d1ywb", 0, 2, "https://res.cloudinary.com/dycfk6nzw/image/upload/v1773227201/bulgarikon/deh0osvaa1lclj5d1ywb.gif" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a5a5a5a5-a5a5-a5a5-a5a5-a5a5a5a5a5a5"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a6a6a6a6-a6a6-a6a6-a6a6-a6a6a6a6a6a6"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a7a7a7a7-a7a7-a7a7-a7a7-a7a7a7a7a7a7"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a8a8a8a8-a8a8-a8a8-a8a8-a8a8a8a8a8a8"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("a9a9a9a9-a9a9-a9a9-a9a9-a9a9a9a9a9a9"));
        }
    }
}
