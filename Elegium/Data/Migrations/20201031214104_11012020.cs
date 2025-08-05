using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class _11012020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "CrewPositions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CrewPositions_PositionId",
                table: "CrewPositions",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CrewPositions_WorkingPositions_PositionId",
                table: "CrewPositions",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrewPositions_WorkingPositions_PositionId",
                table: "CrewPositions");

            migrationBuilder.DropIndex(
                name: "IX_CrewPositions_PositionId",
                table: "CrewPositions");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "CrewPositions");
        }
    }
}
