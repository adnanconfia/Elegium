using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class setinitialseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "sets",
                columns: new[] { "Id", "Set_name" },
                values: new object[,]
                {
                    { 1, "Ships Ramses / Deck" },
                    { 2, "Ship Ramses / Foredeck" },
                    { 3, "Ship Ramses / Dining Hall" },
                    { 4, "Ship Ramses / Fahra’s Cabin" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
