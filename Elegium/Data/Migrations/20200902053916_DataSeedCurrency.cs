using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "Name", "Symbol", "UsdDifference" },
                values: new object[] { 1, "USD", "$", 0 }
            );
            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "Name", "Symbol", "UsdDifference" },
                values: new object[] { 2, "GBP", "£", 0 }
            );
            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "Name", "Symbol", "UsdDifference" },
                values: new object[] { 3, "EUR", "€", 0 }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
