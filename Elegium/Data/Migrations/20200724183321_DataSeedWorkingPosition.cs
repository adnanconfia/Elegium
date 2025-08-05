using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedWorkingPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkingPositions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Production Operator" }
            );
            migrationBuilder.InsertData(
                table: "WorkingPositions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Production Assistant" }
            );
            migrationBuilder.InsertData(
                table: "WorkingPositions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Production Manager" }
            );
            migrationBuilder.InsertData(
                table: "WorkingPositions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Film Editor" }
            );
            migrationBuilder.InsertData(
                table: "WorkingPositions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Video Producer" }
            );
            migrationBuilder.InsertData(
                table: "WorkingPositions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Media sales" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
