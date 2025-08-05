using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class sceneMakeuptableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "sceneMakeups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtraId",
                table: "sceneMakeups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "sceneMakeups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_sceneMakeups_CharacterId",
                table: "sceneMakeups",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_sceneMakeups_ExtraId",
                table: "sceneMakeups",
                column: "ExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_sceneMakeups_characters_CharacterId",
                table: "sceneMakeups",
                column: "CharacterId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sceneMakeups_Extras_ExtraId",
                table: "sceneMakeups",
                column: "ExtraId",
                principalTable: "Extras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sceneMakeups_characters_CharacterId",
                table: "sceneMakeups");

            migrationBuilder.DropForeignKey(
                name: "FK_sceneMakeups_Extras_ExtraId",
                table: "sceneMakeups");

            migrationBuilder.DropIndex(
                name: "IX_sceneMakeups_CharacterId",
                table: "sceneMakeups");

            migrationBuilder.DropIndex(
                name: "IX_sceneMakeups_ExtraId",
                table: "sceneMakeups");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "sceneMakeups");

            migrationBuilder.DropColumn(
                name: "ExtraId",
                table: "sceneMakeups");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "sceneMakeups");
        }
    }
}
