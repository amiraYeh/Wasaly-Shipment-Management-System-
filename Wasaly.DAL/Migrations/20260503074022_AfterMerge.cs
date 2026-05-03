using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasaly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AfterMerge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_CourierAssignments_CourierAssignmentId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_DeliveryOTP_DeliveryOTPId",
                table: "Shipments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_CourierAssignmentId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_DeliveryOTPId",
                table: "Shipments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_DeliveryOTP_Code",
                table: "DeliveryOTP");

            migrationBuilder.DropColumn(
                name: "CourierAssignmentId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DeliveryOTPId",
                table: "Shipments");

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentId",
                table: "ShipmentTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveredAt",
                table: "Shipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "OTPCode",
                table: "DeliveryOTP",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_DeliveryOTP_Code_Format",
                table: "DeliveryOTP",
                sql: "LEN([OTPCode]) = 6 AND [OTPCode] NOT LIKE '%[^0-9]%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DeliveryOTP_Code_Format",
                table: "DeliveryOTP");

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentId",
                table: "ShipmentTrackings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveredAt",
                table: "Shipments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourierAssignmentId",
                table: "Shipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryOTPId",
                table: "Shipments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OTPCode",
                table: "DeliveryOTP",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WasalyIdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_WasalyIdentityUserId",
                        column: x => x.WasalyIdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_CourierAssignmentId",
                table: "Shipments",
                column: "CourierAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DeliveryOTPId",
                table: "Shipments",
                column: "DeliveryOTPId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_DeliveryOTP_Code",
                table: "DeliveryOTP",
                sql: "[OTPCode] >= 100000 AND [OTPCode] <= 999999");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_WasalyIdentityUserId",
                table: "Notifications",
                column: "WasalyIdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_CourierAssignments_CourierAssignmentId",
                table: "Shipments",
                column: "CourierAssignmentId",
                principalTable: "CourierAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_DeliveryOTP_DeliveryOTPId",
                table: "Shipments",
                column: "DeliveryOTPId",
                principalTable: "DeliveryOTP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
