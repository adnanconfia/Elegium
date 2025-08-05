using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class _11022020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectExternalUsers_WorkingPositions_PositionId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectExternalUsers_PositionId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "ProjectExternalUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "ProjectExternalUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectExternalUsers_PositionId",
                table: "ProjectExternalUsers",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectExternalUsers_WorkingPositions_PositionId",
                table: "ProjectExternalUsers",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
