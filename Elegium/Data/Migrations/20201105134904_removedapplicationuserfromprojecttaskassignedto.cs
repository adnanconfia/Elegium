using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class removedapplicationuserfromprojecttaskassignedto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTaskAssignedTo_AspNetUsers_UserId",
                table: "ProjectTaskAssignedTo");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTaskAssignedTo_UserId",
                table: "ProjectTaskAssignedTo");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectTaskAssignedTo",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectTaskAssignedTo",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskAssignedTo_UserId",
                table: "ProjectTaskAssignedTo",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTaskAssignedTo_AspNetUsers_UserId",
                table: "ProjectTaskAssignedTo",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
