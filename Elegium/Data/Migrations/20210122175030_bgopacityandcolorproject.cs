using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class bgopacityandcolorproject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BgOpacity",
                table: "UserProfiles",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "BgColor",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BgOpacity",
                table: "Project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BgColor",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "BgOpacity",
                table: "Project");

            migrationBuilder.AlterColumn<double>(
                name: "BgOpacity",
                table: "UserProfiles",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
