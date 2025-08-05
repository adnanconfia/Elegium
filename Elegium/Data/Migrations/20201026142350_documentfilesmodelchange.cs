using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class documentfilesmodelchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserFriendlySize",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_UserId",
                table: "DocumentFiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_AspNetUsers_UserId",
                table: "DocumentFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_AspNetUsers_UserId",
                table: "DocumentFiles");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_UserId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "UserFriendlySize",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DocumentFiles");
        }
    }
}
