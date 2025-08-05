using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedForScenesIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                update Extras set ""Index"" =
                (select newi from(select ROW_NUMBER() over(partition by project_id order by project_id) + 199 newi, i.Id, i.Project_Id from Extras i ) as t where t.Id = Extras.Id and t.Project_Id = Extras.Project_Id) from Extras;


                update Costumes set ""Index"" =
                (select newi from(select ROW_NUMBER() over(partition by projectid order by projectid)+999 newi, i.Id, i.ProjectId from Costumes i ) as t where t.Id = Costumes.Id and t.ProjectId = Costumes.ProjectId) from Costumes;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
