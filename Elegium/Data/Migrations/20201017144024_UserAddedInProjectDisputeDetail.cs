using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class UserAddedInProjectDisputeDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProjectDisputeDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDisputeDetails_UserId",
                table: "ProjectDisputeDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDisputeDetails_AspNetUsers_UserId",
                table: "ProjectDisputeDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDisputeDetails_AspNetUsers_UserId",
                table: "ProjectDisputeDetails");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDisputeDetails_UserId",
                table: "ProjectDisputeDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProjectDisputeDetails");
        }
    }
}
