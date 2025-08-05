using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class replacecommenturl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update Notification  set Url = replace(url,'ProjectManagement/#','#/')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
