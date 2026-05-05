using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasaly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addRecipientData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientEmail",
                table: "Shipments",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "Shipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientPhone",
                table: "Shipments",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientEmail",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "RecipientPhone",
                table: "Shipments");
        }
    }
}
