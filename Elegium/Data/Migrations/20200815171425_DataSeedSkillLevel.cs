using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedSkillLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SkillLevel",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Begineer" }
            );
            migrationBuilder.InsertData(
                table: "SkillLevel",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Intermediate" }
            );
            migrationBuilder.InsertData(
                table: "SkillLevel",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Expert" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
