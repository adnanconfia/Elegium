using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ApplicationUserLinkAddedInResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Resource",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resource_UserId",
                table: "Resource",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_AspNetUsers_UserId",
                table: "Resource",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_AspNetUsers_UserId",
                table: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_Resource_UserId",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Resource");
        }
    }
}
