using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class removedapplicationuserfromfiletaskassignedto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileTaskAssignedTo_AspNetUsers_UserId",
                table: "FileTaskAssignedTo");

            migrationBuilder.DropIndex(
                name: "IX_FileTaskAssignedTo_UserId",
                table: "FileTaskAssignedTo");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FileTaskAssignedTo",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FileTaskAssignedTo",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileTaskAssignedTo_UserId",
                table: "FileTaskAssignedTo",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileTaskAssignedTo_AspNetUsers_UserId",
                table: "FileTaskAssignedTo",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
