using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedProjectDashboardPanels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into ProjectDashboardPanels (PanelId,PanelLabel,EffectAllowed) values('myProjects','My Projects','move');
                insert into ProjectDashboardPanels (PanelId,PanelLabel,EffectAllowed) values('myCreatedTasks','My Created Tasks','move');
                insert into ProjectDashboardPanels (PanelId,PanelLabel,EffectAllowed) values('myTasks','My Tasks','move');
                insert into ProjectDashboardPanels (PanelId,PanelLabel,EffectAllowed) values('stats','Stats','move');
                insert into ProjectDashboardPanels (PanelId,PanelLabel,EffectAllowed) values('crewCommunication','Crew Communication','move');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
