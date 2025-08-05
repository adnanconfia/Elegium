using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class Character_table_update_II : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GroupOfCharacters",
                table: "characters",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sugggestion",
                table: "characters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupOfCharacters",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "Sugggestion",
                table: "characters");
        }
    }
}
