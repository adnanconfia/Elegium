using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class somechangesannouncementmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnouncementId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Announcements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AnnouncementId",
                table: "Comments",
                column: "AnnouncementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Announcements_AnnouncementId",
                table: "Comments",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Announcements_AnnouncementId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AnnouncementId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AnnouncementId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Announcements");
        }
    }
}
