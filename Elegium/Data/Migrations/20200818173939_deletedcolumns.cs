using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class deletedcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccessRight",
                table: "MediaFiles",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Favorite",
                table: "MediaFiles",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AccessRight",
                table: "Album",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Favorite",
                table: "Album",
                type: "bit",
                nullable: true);
        }
    }
}
