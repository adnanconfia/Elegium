using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class removesetprojectid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sets_Project_project_id",
                table: "sets");

            migrationBuilder.DropIndex(
                name: "IX_sets_project_id",
                table: "sets");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "sets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "sets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_sets_project_id",
                table: "sets",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_sets_Project_project_id",
                table: "sets",
                column: "project_id",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
