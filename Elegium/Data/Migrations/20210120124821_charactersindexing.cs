using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class charactersindexing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

          
            migrationBuilder.Sql(@"
update characters set ""Index"" = 
(select newi from(select ROW_NUMBER() over(partition by project_id order by project_id) newi, i.Id, i.Project_Id from characters i ) as t where t.Id = characters.Id and t.Project_Id = characters.Project_Id)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
