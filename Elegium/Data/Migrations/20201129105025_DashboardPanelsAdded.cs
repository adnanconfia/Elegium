using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DashboardPanelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DashboardPanels",
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
                    table.PrimaryKey("PK_DashboardPanels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DashboardSelectedPanels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PanelId = table.Column<string>(nullable: true),
                    PanelLabel = table.Column<string>(nullable: true),
                    EffectAllowed = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardSelectedPanels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DashboardSelectedPanels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardSelectedPanels_UserId",
                table: "DashboardSelectedPanels",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardPanels");

            migrationBuilder.DropTable(
                name: "DashboardSelectedPanels");
        }
    }
}
