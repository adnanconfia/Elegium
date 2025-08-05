using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addedAppuserreferenceinnotificationtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "NotificationType",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationType_ApplicationUserId",
                table: "NotificationType",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationType_AspNetUsers_ApplicationUserId",
                table: "NotificationType",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationType_AspNetUsers_ApplicationUserId",
                table: "NotificationType");

            migrationBuilder.DropIndex(
                name: "IX_NotificationType_ApplicationUserId",
                table: "NotificationType");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "NotificationType");
        }
    }
}
