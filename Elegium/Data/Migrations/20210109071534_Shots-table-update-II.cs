using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ShotstableupdateII : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Schedule_ss",
                table: "Shots");

            migrationBuilder.AddColumn<int>(
                name: "Schedule_hh",
                table: "Shots",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Shots",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Schedule_hh",
                table: "Shots");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Shots");

            migrationBuilder.AddColumn<int>(
                name: "Schedule_ss",
                table: "Shots",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
