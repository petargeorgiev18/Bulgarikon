using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulgarikon.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCivilizationIdPropFromEventsEntityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Civilizations_CivilizationId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_CivilizationId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CivilizationId",
                table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CivilizationId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_CivilizationId",
                table: "Events",
                column: "CivilizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Civilizations_CivilizationId",
                table: "Events",
                column: "CivilizationId",
                principalTable: "Civilizations",
                principalColumn: "Id");
        }
    }
}
