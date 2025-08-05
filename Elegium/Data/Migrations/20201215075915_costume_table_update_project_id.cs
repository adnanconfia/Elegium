using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class costume_table_update_project_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PorjectId",
                table: "Costumes");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Costumes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Costumes");

            migrationBuilder.AddColumn<int>(
                name: "PorjectId",
                table: "Costumes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
