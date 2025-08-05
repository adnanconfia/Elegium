using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class CrewPositionsRenamedToProjectCrewPositions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrewPositions_WorkingPositions_PositionId",
                table: "CrewPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_CrewPositions_ProjectCrews_ProjectCrewId",
                table: "CrewPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CrewPositions",
                table: "CrewPositions");

            migrationBuilder.RenameTable(
                name: "CrewPositions",
                newName: "ProjectCrewPositions");

            migrationBuilder.RenameIndex(
                name: "IX_CrewPositions_ProjectCrewId",
                table: "ProjectCrewPositions",
                newName: "IX_ProjectCrewPositions_ProjectCrewId");

            migrationBuilder.RenameIndex(
                name: "IX_CrewPositions_PositionId",
                table: "ProjectCrewPositions",
                newName: "IX_ProjectCrewPositions_PositionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectCrewPositions",
                table: "ProjectCrewPositions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrewPositions_WorkingPositions_PositionId",
                table: "ProjectCrewPositions",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrewPositions_ProjectCrews_ProjectCrewId",
                table: "ProjectCrewPositions",
                column: "ProjectCrewId",
                principalTable: "ProjectCrews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrewPositions_WorkingPositions_PositionId",
                table: "ProjectCrewPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrewPositions_ProjectCrews_ProjectCrewId",
                table: "ProjectCrewPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectCrewPositions",
                table: "ProjectCrewPositions");

            migrationBuilder.RenameTable(
                name: "ProjectCrewPositions",
                newName: "CrewPositions");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectCrewPositions_ProjectCrewId",
                table: "CrewPositions",
                newName: "IX_CrewPositions_ProjectCrewId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectCrewPositions_PositionId",
                table: "CrewPositions",
                newName: "IX_CrewPositions_PositionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrewPositions",
                table: "CrewPositions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CrewPositions_WorkingPositions_PositionId",
                table: "CrewPositions",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrewPositions_ProjectCrews_ProjectCrewId",
                table: "CrewPositions",
                column: "ProjectCrewId",
                principalTable: "ProjectCrews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
