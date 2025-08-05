using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class colnamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Width",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "UserProfiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
