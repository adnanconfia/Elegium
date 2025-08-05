using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class VisibilityAreasDataSeedUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update VisibilityAreas set Name='Discovery Module' where Code='discovery';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
