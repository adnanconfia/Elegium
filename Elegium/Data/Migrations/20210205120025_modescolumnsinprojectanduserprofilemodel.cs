using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class modescolumnsinprojectanduserprofilemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CinematicMode",
                table: "UserProfiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DarkMode",
                table: "UserProfiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GlassMode",
                table: "UserProfiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CinematicMode",
                table: "Project",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DarkMode",
                table: "Project",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GlassMode",
                table: "Project",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CinematicMode",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "DarkMode",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "GlassMode",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CinematicMode",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "DarkMode",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "GlassMode",
                table: "Project");
        }
    }
}
