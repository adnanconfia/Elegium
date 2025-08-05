using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class Actor_table_update_I : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DOB",
                table: "Actors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealName",
                table: "Actors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "RealName",
                table: "Actors");
        }
    }
}
