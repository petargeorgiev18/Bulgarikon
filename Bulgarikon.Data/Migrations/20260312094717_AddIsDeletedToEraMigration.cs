using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulgarikon.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToEraMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Eras",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("582d246a-d318-52ed-8f52-1fe5eefb1b9b"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("5a6c6e88-081c-5397-b779-135356530cf3"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("80a4fd24-c3fa-5a1a-a43a-642ca8375def"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("892d0671-4abe-5126-bf2d-fa9582ae5657"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("9ca2a3b4-b071-5d3e-bc21-6131f2af752c"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("ab19d544-9d78-5cc7-828c-702204e8c6f7"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Eras",
                keyColumn: "Id",
                keyValue: new Guid("d04736bd-8718-5c73-bf46-3e969f526313"),
                column: "IsDeleted",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Eras");
        }
    }
}
