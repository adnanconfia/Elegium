using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class _10252020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrews_WorkingPositions_PositionId",
                table: "ProjectCrews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrews_Units_UnitId",
                table: "ProjectCrews");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCrews_PositionId",
                table: "ProjectCrews");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCrews_UnitId",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "ProjectCrews");

            migrationBuilder.CreateTable(
                name: "CrewPositions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectCrewId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewPositions_ProjectCrews_ProjectCrewId",
                        column: x => x.ProjectCrewId,
                        principalTable: "ProjectCrews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectExternalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    PositionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneHome = table.Column<string>(nullable: true),
                    PhoneMobile = table.Column<string>(nullable: true),
                    PhoneOffice = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectExternalUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectExternalUsers_WorkingPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "WorkingPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectExternalUsers_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUnits_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserGroups_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrewPositions_ProjectCrewId",
                table: "CrewPositions",
                column: "ProjectCrewId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectExternalUsers_PositionId",
                table: "ProjectExternalUsers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectExternalUsers_ProjectId",
                table: "ProjectExternalUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUnits_ProjectId",
                table: "ProjectUnits",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserGroups_ProjectId",
                table: "ProjectUserGroups",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrewPositions");

            migrationBuilder.DropTable(
                name: "ProjectExternalUsers");

            migrationBuilder.DropTable(
                name: "ProjectUnits");

            migrationBuilder.DropTable(
                name: "ProjectUserGroups");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ProjectCrews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ProjectCrews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ProjectCrews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "ProjectCrews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "ProjectCrews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrews_PositionId",
                table: "ProjectCrews",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrews_UnitId",
                table: "ProjectCrews",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrews_WorkingPositions_PositionId",
                table: "ProjectCrews",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrews_Units_UnitId",
                table: "ProjectCrews",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
