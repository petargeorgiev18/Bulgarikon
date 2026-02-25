using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulgarikon.Data.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteEventCivilizationToCivilizationEntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCivilizations_Civilizations_CivilizationId",
                table: "EventCivilizations");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCivilizations_Civilizations_CivilizationId",
                table: "EventCivilizations",
                column: "CivilizationId",
                principalTable: "Civilizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCivilizations_Civilizations_CivilizationId",
                table: "EventCivilizations");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCivilizations_Civilizations_CivilizationId",
                table: "EventCivilizations",
                column: "CivilizationId",
                principalTable: "Civilizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
