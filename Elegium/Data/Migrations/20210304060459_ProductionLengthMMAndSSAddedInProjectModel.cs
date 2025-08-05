using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProductionLengthMMAndSSAddedInProjectModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductionLengthMM",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionLengthSS",
                table: "Project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductionLengthMM",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProductionLengthSS",
                table: "Project");
        }
    }
}
