using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class updatescenestbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "scheduled_mm",
                table: "Scenes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "scheduled_hh",
                table: "Scenes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Estime_ss",
                table: "Scenes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Estime_mm",
                table: "Scenes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CastType",
                table: "Scenes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommnityInformation",
                table: "Scenes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOmitted",
                table: "Scenes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfShots",
                table: "Scenes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScriptDay",
                table: "Scenes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScriptPage",
                table: "Scenes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CastType",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "CommnityInformation",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "IsOmitted",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "NumberOfShots",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "ScriptDay",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "ScriptPage",
                table: "Scenes");

            migrationBuilder.AlterColumn<string>(
                name: "scheduled_mm",
                table: "Scenes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "scheduled_hh",
                table: "Scenes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Estime_ss",
                table: "Scenes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Estime_mm",
                table: "Scenes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
