using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class scenetableupdatewithFkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Scenes_project_id",
                table: "Scenes",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_Scenes_unit",
                table: "Scenes",
                column: "unit");

            migrationBuilder.AddForeignKey(
                name: "FK_Scenes_Project_project_id",
                table: "Scenes",
                column: "project_id",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenes_ProjectUnits_unit",
                table: "Scenes",
                column: "unit",
                principalTable: "ProjectUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scenes_Project_project_id",
                table: "Scenes");

            migrationBuilder.DropForeignKey(
                name: "FK_Scenes_ProjectUnits_unit",
                table: "Scenes");

            migrationBuilder.DropIndex(
                name: "IX_Scenes_project_id",
                table: "Scenes");

            migrationBuilder.DropIndex(
                name: "IX_Scenes_unit",
                table: "Scenes");
        }
    }
}
