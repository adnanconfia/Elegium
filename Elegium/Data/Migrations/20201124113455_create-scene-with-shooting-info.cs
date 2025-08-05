using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class createscenewithshootinginfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estime_mm",
                table: "Scenes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estime_ss",
                table: "Scenes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScriptPages",
                table: "Scenes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "scheduled_hh",
                table: "Scenes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "scheduled_mm",
                table: "Scenes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "unit",
                table: "Scenes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estime_mm",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "Estime_ss",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "ScriptPages",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "scheduled_hh",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "scheduled_mm",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "Scenes");
        }
    }
}
