using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class SceneVisualstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sceneVisuals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SceneId = table.Column<int>(nullable: false),
                    VisualId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sceneVisuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sceneVisuals_Scenes_SceneId",
                        column: x => x.SceneId,
                        principalTable: "Scenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sceneVisuals_visualEffects_VisualId",
                        column: x => x.VisualId,
                        principalTable: "visualEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sceneVisuals_SceneId",
                table: "sceneVisuals",
                column: "SceneId");

            migrationBuilder.CreateIndex(
                name: "IX_sceneVisuals_VisualId",
                table: "sceneVisuals",
                column: "VisualId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sceneVisuals");
        }
    }
}
