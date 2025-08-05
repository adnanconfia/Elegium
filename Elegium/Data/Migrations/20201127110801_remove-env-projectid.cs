using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class removeenvprojectid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_environments_Project_project_id",
                table: "environments");

            migrationBuilder.DropIndex(
                name: "IX_environments_project_id",
                table: "environments");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "environments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "environments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_environments_project_id",
                table: "environments",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_environments_Project_project_id",
                table: "environments",
                column: "project_id",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
