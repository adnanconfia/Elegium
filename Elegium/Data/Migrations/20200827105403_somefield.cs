using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class somefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserAdditionalSkills",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalSkills_UserId",
                table: "UserAdditionalSkills",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAdditionalSkills_AspNetUsers_UserId",
                table: "UserAdditionalSkills",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAdditionalSkills_AspNetUsers_UserId",
                table: "UserAdditionalSkills");

            migrationBuilder.DropIndex(
                name: "IX_UserAdditionalSkills_UserId",
                table: "UserAdditionalSkills");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserAdditionalSkills");
        }
    }
}
