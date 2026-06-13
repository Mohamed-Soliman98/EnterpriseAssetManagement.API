using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseAssetManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformedByToMaintenanceLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PerformedById",
                table: "maintenanceLogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_maintenanceLogs_PerformedById",
                table: "maintenanceLogs",
                column: "PerformedById");

            migrationBuilder.AddForeignKey(
                name: "FK_maintenanceLogs_AspNetUsers_PerformedById",
                table: "maintenanceLogs",
                column: "PerformedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_maintenanceLogs_AspNetUsers_PerformedById",
                table: "maintenanceLogs");

            migrationBuilder.DropIndex(
                name: "IX_maintenanceLogs_PerformedById",
                table: "maintenanceLogs");

            migrationBuilder.DropColumn(
                name: "PerformedById",
                table: "maintenanceLogs");
        }
    }
}
