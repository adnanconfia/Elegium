using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class somecolumnsinuserprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Age",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Build",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EyeColor",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HairColor",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkinColor",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "UserProfiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Build",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "EyeColor",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "HairColor",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "SkinColor",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "UserProfiles");
        }
    }
}
