using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseAssetManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class EditTableMaintenanceLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_maintenanceLogs_AspNetUsers_PerformedById",
                table: "maintenanceLogs");

            migrationBuilder.RenameColumn(
                name: "PerformedById",
                table: "maintenanceLogs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_maintenanceLogs_PerformedById",
                table: "maintenanceLogs",
                newName: "IX_maintenanceLogs_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_maintenanceLogs_AspNetUsers_UserId",
                table: "maintenanceLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_maintenanceLogs_AspNetUsers_UserId",
                table: "maintenanceLogs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "maintenanceLogs",
                newName: "PerformedById");

            migrationBuilder.RenameIndex(
                name: "IX_maintenanceLogs_UserId",
                table: "maintenanceLogs",
                newName: "IX_maintenanceLogs_PerformedById");

            migrationBuilder.AddForeignKey(
                name: "FK_maintenanceLogs_AspNetUsers_PerformedById",
                table: "maintenanceLogs",
                column: "PerformedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
