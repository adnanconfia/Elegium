using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class Tasks_table_actorId_talentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActorId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TalentID",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ActorId",
                table: "ProjectTasks",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_TalentID",
                table: "ProjectTasks",
                column: "TalentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Actors_ActorId",
                table: "ProjectTasks",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Talents_TalentID",
                table: "ProjectTasks",
                column: "TalentID",
                principalTable: "Talents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Actors_ActorId",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Talents_TalentID",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ActorId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_TalentID",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "TalentID",
                table: "ProjectTasks");
        }
    }
}
