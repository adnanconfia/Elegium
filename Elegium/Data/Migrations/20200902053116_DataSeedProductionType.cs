using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedProductionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Feature movie (cinema)" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Short film" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Commercial" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "TV series" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Documentary (TV)" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Corporate film" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "Product Film" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 8, "Business Movie" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 9, "Photography" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 10, "Social Media" }
            );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
