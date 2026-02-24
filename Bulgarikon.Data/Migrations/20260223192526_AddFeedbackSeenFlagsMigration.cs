using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulgarikon.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackSeenFlagsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReplySeenByUser",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeenByAdmin",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReplySeenByUser",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsSeenByAdmin",
                table: "Feedbacks");
        }
    }
}
