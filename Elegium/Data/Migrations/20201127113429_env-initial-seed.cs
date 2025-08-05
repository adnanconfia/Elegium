using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class envinitialseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "environments",
                columns: new[] { "Id", "Evironment_Name" },
                values: new object[,]
                {
                    { 1, "DAY-INT" },
                    { 2, "DAY-EXT" },
                    { 3, "NIGHT-INT" },
                    { 4, "NIGHT-EXT" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "environments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "environments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "environments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "environments",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
