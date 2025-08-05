using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addedsomefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Favorite",
                table: "MediaFiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Favorite",
                table: "Album",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "Album");
        }
    }
}
