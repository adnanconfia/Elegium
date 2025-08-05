using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addednamecolinfiletask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "FileTaskAssignedTo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FileTaskAssignedTo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "FileTaskAssignedTo");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FileTaskAssignedTo");
        }
    }
}
