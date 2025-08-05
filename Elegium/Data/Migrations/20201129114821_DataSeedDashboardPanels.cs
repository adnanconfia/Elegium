using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedDashboardPanels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into DashboardPanels (PanelId,PanelLabel,EffectAllowed) values('myProjects','My Projects','move');
                insert into DashboardPanels (PanelId,PanelLabel,EffectAllowed) values('myCreatedTasks','My Created Tasks','move');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
