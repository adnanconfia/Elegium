using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class taskstableextraId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExtraId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ExtraId",
                table: "ProjectTasks",
                column: "ExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Extras_ExtraId",
                table: "ProjectTasks",
                column: "ExtraId",
                principalTable: "Extras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Extras_ExtraId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ExtraId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ExtraId",
                table: "ProjectTasks");
        }
    }
}
