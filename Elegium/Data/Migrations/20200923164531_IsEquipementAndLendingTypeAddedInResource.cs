using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class IsEquipementAndLendingTypeAddedInResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEquipment",
                table: "Resource",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LendingType",
                table: "Resource",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEquipment",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "LendingType",
                table: "Resource");
        }
    }
}
