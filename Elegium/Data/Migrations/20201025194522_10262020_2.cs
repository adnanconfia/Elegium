using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class _10262020_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrewUnits_ProjectCrews_ProjectCrewId",
                table: "CrewUnits");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectCrewId",
                table: "CrewUnits",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CrewUnits_ProjectCrews_ProjectCrewId",
                table: "CrewUnits",
                column: "ProjectCrewId",
                principalTable: "ProjectCrews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrewUnits_ProjectCrews_ProjectCrewId",
                table: "CrewUnits");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectCrewId",
                table: "CrewUnits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CrewUnits_ProjectCrews_ProjectCrewId",
                table: "CrewUnits",
                column: "ProjectCrewId",
                principalTable: "ProjectCrews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
