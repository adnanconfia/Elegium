using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedDashboardPanels2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into DashboardPanels (PanelId,PanelLabel,EffectAllowed) values('myTasks','My Tasks','move');
                insert into DashboardPanels (PanelId,PanelLabel,EffectAllowed) values('stats','Stats','move');
                insert into DashboardPanels (PanelId,PanelLabel,EffectAllowed) values('crewCommunication','Crew Communication','move');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
