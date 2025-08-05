using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class TypeRenamedToHireOrSaleInResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Resource");

            migrationBuilder.AddColumn<string>(
                name: "HireOrSale",
                table: "Resource",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HireOrSale",
                table: "Resource");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Resource",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
