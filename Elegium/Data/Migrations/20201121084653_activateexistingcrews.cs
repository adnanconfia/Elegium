using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class activateexistingcrews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update ProjectCrews set IsActive = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
