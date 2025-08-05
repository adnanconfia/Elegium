using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class commentstableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SceneId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SceneId",
                table: "Comments",
                column: "SceneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Scenes_SceneId",
                table: "Comments",
                column: "SceneId",
                principalTable: "Scenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Scenes_SceneId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_SceneId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "SceneId",
                table: "Comments");
        }
    }
}
