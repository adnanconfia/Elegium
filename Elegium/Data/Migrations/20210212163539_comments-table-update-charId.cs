using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class commentstableupdatecharId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "characterId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_characterId",
                table: "Comments",
                column: "characterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_characters_characterId",
                table: "Comments",
                column: "characterId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_characters_characterId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_characterId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CharId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "characterId",
                table: "Comments");
        }
    }
}
