using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectDashboardPanelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectDashboardPanels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PanelId = table.Column<string>(nullable: true),
                    PanelLabel = table.Column<string>(nullable: true),
                    EffectAllowed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDashboardPanels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDashboardSelectedPanels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PanelId = table.Column<string>(nullable: true),
                    PanelLabel = table.Column<string>(nullable: true),
                    EffectAllowed = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDashboardSelectedPanels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDashboardSelectedPanels_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectDashboardSelectedPanels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDashboardSelectedPanels_ProjectId",
                table: "ProjectDashboardSelectedPanels",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDashboardSelectedPanels_UserId",
                table: "ProjectDashboardSelectedPanels",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectDashboardPanels");

            migrationBuilder.DropTable(
                name: "ProjectDashboardSelectedPanels");
        }
    }
}
