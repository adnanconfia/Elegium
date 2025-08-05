using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class updateprojectcrewclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActorDatabaseRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressBookRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnnouncementRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCalendarRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostumeDatabaseRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateDeleteProjectsRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrewRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HideMember",
                table: "ProjectCrews",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationDatabaseRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalMessage",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "ProjectCrews",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProbItemDatabaseRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionCalendarRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SettingsRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionsRights",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "ProjectCrews",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrews_PositionId",
                table: "ProjectCrews",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrews_UnitId",
                table: "ProjectCrews",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrews_WorkingPositions_PositionId",
                table: "ProjectCrews",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrews_Units_UnitId",
                table: "ProjectCrews",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrews_WorkingPositions_PositionId",
                table: "ProjectCrews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrews_Units_UnitId",
                table: "ProjectCrews");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCrews_PositionId",
                table: "ProjectCrews");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCrews_UnitId",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "ActorDatabaseRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "AddressBookRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "AnnouncementRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "CompanyCalendarRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "CostumeDatabaseRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "CreateDeleteProjectsRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "CrewRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "HideMember",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "LocationDatabaseRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "PersonalMessage",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "ProbItemDatabaseRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "ProductionCalendarRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "SettingsRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "SubscriptionsRights",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "ProjectCrews");
        }
    }
}
