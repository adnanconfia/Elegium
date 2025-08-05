using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class documentFile_table_characterID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_CharId",
                table: "DocumentFiles",
                column: "CharId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_characters_CharId",
                table: "DocumentFiles",
                column: "CharId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_characters_CharId",
                table: "DocumentFiles");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_CharId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "CharId",
                table: "DocumentFiles");
        }
    }
}
