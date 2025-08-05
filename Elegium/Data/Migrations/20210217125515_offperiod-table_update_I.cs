using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class offperiodtable_update_I : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndDateTime",
                table: "offperiods",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartDateTime",
                table: "offperiods",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "offperiods");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "offperiods");
        }
    }
}
