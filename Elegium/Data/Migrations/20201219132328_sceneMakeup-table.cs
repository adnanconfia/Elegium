using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class sceneMakeuptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sceneMakeups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SceneId = table.Column<int>(nullable: false),
                    MakeupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sceneMakeups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sceneMakeups_Makeups_MakeupId",
                        column: x => x.MakeupId,
                        principalTable: "Makeups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sceneMakeups_Scenes_SceneId",
                        column: x => x.SceneId,
                        principalTable: "Scenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sceneMakeups_MakeupId",
                table: "sceneMakeups",
                column: "MakeupId");

            migrationBuilder.CreateIndex(
                name: "IX_sceneMakeups_SceneId",
                table: "sceneMakeups",
                column: "SceneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sceneMakeups");
        }
    }
}
