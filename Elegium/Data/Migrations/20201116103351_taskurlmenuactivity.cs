using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class taskurlmenuactivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"update menuactivity set url = 'tasks.mytasks'
where id = 5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
