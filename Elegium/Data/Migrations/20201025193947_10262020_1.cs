using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class _10262020_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUnits_Project_ProjectId",
                table: "ProjectUnits");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectUnits",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "CrewUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectCrewId = table.Column<int>(nullable: true),
                    UnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewUnits_ProjectCrews_ProjectCrewId",
                        column: x => x.ProjectCrewId,
                        principalTable: "ProjectCrews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrewUnits_ProjectUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ProjectUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrewUnits_ProjectCrewId",
                table: "CrewUnits",
                column: "ProjectCrewId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewUnits_UnitId",
                table: "CrewUnits",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUnits_Project_ProjectId",
                table: "ProjectUnits",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUnits_Project_ProjectId",
                table: "ProjectUnits");

            migrationBuilder.DropTable(
                name: "CrewUnits");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectUnits",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUnits_Project_ProjectId",
                table: "ProjectUnits",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
