using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class SceneStuntstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sceneStunts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SceneId = table.Column<int>(nullable: false),
                    StuntId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sceneStunts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sceneStunts_Scenes_SceneId",
                        column: x => x.SceneId,
                        principalTable: "Scenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sceneStunts_stunts_StuntId",
                        column: x => x.StuntId,
                        principalTable: "stunts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sceneStunts_SceneId",
                table: "sceneStunts",
                column: "SceneId");

            migrationBuilder.CreateIndex(
                name: "IX_sceneStunts_StuntId",
                table: "sceneStunts",
                column: "StuntId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sceneStunts");
        }
    }
}
