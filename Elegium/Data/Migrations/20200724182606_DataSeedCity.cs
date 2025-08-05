using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "CountryId" },
                values: new object[] { 1, "California", 1 }
            );
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "CountryId" },
                values: new object[] { 2, "Newyork", 1 }
            );
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "CountryId" },
                values: new object[] { 3, "Ohio", 1 }
            );

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "CountryId" },
                values: new object[] { 4, "London", 2 }
            );
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "CountryId" },
                values: new object[] { 5, "Birmingham", 2 }
            );
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "CountryId" },
                values: new object[] { 6, "Karachi", 3 }
            );
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "CountryId" },
                values: new object[] { 7, "Islamabad", 3 }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
