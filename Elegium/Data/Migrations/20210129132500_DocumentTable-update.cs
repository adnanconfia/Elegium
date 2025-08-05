using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DocumentTableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TalentId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_TalentId",
                table: "DocumentFiles",
                column: "TalentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Talents_TalentId",
                table: "DocumentFiles",
                column: "TalentId",
                principalTable: "Talents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Talents_TalentId",
                table: "DocumentFiles");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_TalentId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "TalentId",
                table: "DocumentFiles");
        }
    }
}
