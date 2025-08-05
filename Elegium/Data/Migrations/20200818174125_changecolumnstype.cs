using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class changecolumnstype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccessRight",
                table: "MediaFiles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Favorite",
                table: "MediaFiles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AccessRight",
                table: "Album",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Favorite",
                table: "Album",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessRight",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "AccessRight",
                table: "Album");

            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "Album");
        }
    }
}
