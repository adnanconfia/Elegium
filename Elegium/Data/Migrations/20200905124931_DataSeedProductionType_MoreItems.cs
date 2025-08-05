using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedProductionType_MoreItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 11, "Recruiting Spot" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 12, "Image Film" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 13, "Explanatory Film" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 14, "Industrial film" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 15, "Video clip" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 16, "Documentary (cinema)" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 17, "Documentary (series)" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 18, "Documentary (mini series)" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 19, "Documentary (TV series)" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 20, "Dramatized Documentary" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 21, "Newscast" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 22, "Stage Production" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 23, "Event Film" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 24, "Virtual Reality" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 25, "Augmented Reality" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 26, "Factual" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 27, "Infotainment" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 28, "Reality" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 29, "Scripted Reality" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 30, "Feature movie (TV)" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 31, "Feature movie (TV, series)" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 32, "Feature movie (TV, event)" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 33, "TV mini series" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 34, "TV show" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 35, "TV magazine" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 36, "TV report" }
            );
            migrationBuilder.InsertData(
                table: "ProductionType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 37, "Other" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
