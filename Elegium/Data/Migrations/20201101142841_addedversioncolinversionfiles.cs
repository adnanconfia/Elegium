using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addedversioncolinversionfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "VersionFiles");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "VersionFiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "VersionFiles");

            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "VersionFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
