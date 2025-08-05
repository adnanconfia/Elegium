using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class indexingwork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"


update Constructions set ""Index"" = 

(select newi from(

select ROW_NUMBER() over(partition by projectid order by projectid) + 1999 newi, i.Id, i.ProjectId from Constructions i ) as t

where t.Id = Constructions.Id
and t.ProjectId = Constructions.ProjectId);


            update dressings set ""Index"" =

            (select newi from(

            select ROW_NUMBER() over(partition by projectid order by projectid) +isnull((select max(""Index"") from Constructions s where s.ProjectId = i.ProjectId),1999) newi , i.Id, i.ProjectId from dressings i ) as t

where t.Id = dressings.Id
and t.ProjectId = dressings.ProjectId);

            update Props set ""Index"" =

            (select newi from(

            select ROW_NUMBER() over(partition by projectid order by projectid) +isnull((select max(""Index"") from dressings s where s.ProjectId = i.ProjectId),isnull((select max(""Index"") from Constructions s where s.ProjectId = i.ProjectId),1999)) newi , i.Id, i.ProjectId from Props i ) as t

where t.Id = Props.Id
and t.ProjectId = Props.ProjectId);

            update Graphics set ""Index"" =

            (select newi from(

            select ROW_NUMBER() over(partition by projectid order by projectid) +isnull((select max(""Index"") from Props s where s.ProjectId = i.ProjectId),isnull((select max(""Index"") from dressings s where s.ProjectId = i.ProjectId),isnull((select max(""Index"") from Constructions s where s.ProjectId = i.ProjectId),1999))) newi , i.Id, i.ProjectId from Graphics i ) as t

where t.Id = Graphics.Id
and t.ProjectId = Graphics.ProjectId);

            update Vehicles set ""Index"" =

            (select newi from(

            select ROW_NUMBER() over(partition by projectid order by projectid) +isnull((select max(""Index"") from Graphics s where s.ProjectId = i.ProjectId),isnull((select max(""Index"") from Props s where s.ProjectId = i.ProjectId),isnull((select max(""Index"") from dressings s where s.ProjectId = i.ProjectId),isnull((select max(""Index"") from Constructions s where s.ProjectId = i.ProjectId),1999)))) newi , i.Id, i.ProjectId from Vehicles i ) as t

where t.Id = Vehicles.Id
and t.ProjectId = Vehicles.ProjectId);


");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
