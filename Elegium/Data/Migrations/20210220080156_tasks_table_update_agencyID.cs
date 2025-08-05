using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class tasks_table_update_agencyID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgencyId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AgencyId",
                table: "ProjectTasks",
                column: "AgencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Agencies_AgencyId",
                table: "ProjectTasks",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Agencies_AgencyId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_AgencyId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "ProjectTasks");
        }
    }
}
