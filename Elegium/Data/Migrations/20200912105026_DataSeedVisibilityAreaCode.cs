using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedVisibilityAreaCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE VisibilityAreas SET CODE='discovery' WHERE NAME='Discovery';
                UPDATE VisibilityAreas SET CODE='social' WHERE NAME='Social Networking';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
