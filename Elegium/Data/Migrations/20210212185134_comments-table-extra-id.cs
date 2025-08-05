using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class commentstableextraid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExtraId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ExtraId",
                table: "Comments",
                column: "ExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Extras_ExtraId",
                table: "Comments",
                column: "ExtraId",
                principalTable: "Extras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Extras_ExtraId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ExtraId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ExtraId",
                table: "Comments");
        }
    }
}
