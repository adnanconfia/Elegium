using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedResourceCondition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ResourceCondition",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "New" }
            );
            migrationBuilder.InsertData(
                table: "ResourceCondition",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Used" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
