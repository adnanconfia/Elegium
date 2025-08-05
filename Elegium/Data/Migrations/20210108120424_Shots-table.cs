using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class Shotstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(nullable: true),
                    Visual = table.Column<string>(nullable: true),
                    Audio = table.Column<string>(nullable: true),
                    Sound = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Lighting = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Schedule_mm = table.Column<int>(nullable: false),
                    Schedule_ss = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: true),
                    SceneId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shots_Scenes_SceneId",
                        column: x => x.SceneId,
                        principalTable: "Scenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shots_ProjectUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ProjectUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shots_SceneId",
                table: "Shots",
                column: "SceneId");

            migrationBuilder.CreateIndex(
                name: "IX_Shots_UnitId",
                table: "Shots",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shots");
        }
    }
}
