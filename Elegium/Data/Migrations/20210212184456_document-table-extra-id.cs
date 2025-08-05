using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class documenttableextraid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExtraId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_ExtraId",
                table: "DocumentFiles",
                column: "ExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Extras_ExtraId",
                table: "DocumentFiles",
                column: "ExtraId",
                principalTable: "Extras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Extras_ExtraId",
                table: "DocumentFiles");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_ExtraId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "ExtraId",
                table: "DocumentFiles");
        }
    }
}
