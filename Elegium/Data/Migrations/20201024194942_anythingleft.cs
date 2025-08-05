using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class anythingleft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkupText",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "RawText",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "MarkupText",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RawText",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
