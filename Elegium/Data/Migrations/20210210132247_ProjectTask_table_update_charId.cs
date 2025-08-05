using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectTask_table_update_charId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_CharacterId",
                table: "ProjectTasks",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_characters_CharacterId",
                table: "ProjectTasks",
                column: "CharacterId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_characters_CharacterId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_CharacterId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CharId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "ProjectTasks");
        }
    }
}
