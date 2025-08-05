using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class MenuActivityDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuActivity",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[] { 1, "Dashboard", "dashboard" }
            );

            migrationBuilder.InsertData(
                table: "MenuActivity",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[] { 2, "Documents and Files", "documents" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
