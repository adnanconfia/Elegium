using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class charactersTalenttableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharId",
                table: "CharactersTalents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtraId",
                table: "CharactersTalents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharactersTalents_CharId",
                table: "CharactersTalents",
                column: "CharId");

            migrationBuilder.CreateIndex(
                name: "IX_CharactersTalents_ExtraId",
                table: "CharactersTalents",
                column: "ExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_CharactersTalents_characters_CharId",
                table: "CharactersTalents",
                column: "CharId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CharactersTalents_Extras_ExtraId",
                table: "CharactersTalents",
                column: "ExtraId",
                principalTable: "Extras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharactersTalents_characters_CharId",
                table: "CharactersTalents");

            migrationBuilder.DropForeignKey(
                name: "FK_CharactersTalents_Extras_ExtraId",
                table: "CharactersTalents");

            migrationBuilder.DropIndex(
                name: "IX_CharactersTalents_CharId",
                table: "CharactersTalents");

            migrationBuilder.DropIndex(
                name: "IX_CharactersTalents_ExtraId",
                table: "CharactersTalents");

            migrationBuilder.DropColumn(
                name: "CharId",
                table: "CharactersTalents");

            migrationBuilder.DropColumn(
                name: "ExtraId",
                table: "CharactersTalents");
        }
    }
}
