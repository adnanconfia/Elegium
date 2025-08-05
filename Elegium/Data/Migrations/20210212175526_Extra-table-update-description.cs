using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class Extratableupdatedescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Extras",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GroupOfCharacters",
                table: "Extras",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sugggestion",
                table: "Extras",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Extras");

            migrationBuilder.DropColumn(
                name: "GroupOfCharacters",
                table: "Extras");

            migrationBuilder.DropColumn(
                name: "Sugggestion",
                table: "Extras");
        }
    }
}
