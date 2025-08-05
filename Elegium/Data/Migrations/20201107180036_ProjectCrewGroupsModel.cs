using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectCrewGroupsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectCrewGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectCrewId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCrewGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCrewGroups_ProjectUserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ProjectUserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectCrewGroups_ProjectCrews_ProjectCrewId",
                        column: x => x.ProjectCrewId,
                        principalTable: "ProjectCrews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProjectCrewGroups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrewGroups_GroupId",
                table: "ProjectCrewGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrewGroups_ProjectCrewId",
                table: "ProjectCrewGroups",
                column: "ProjectCrewId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrewGroups_UserId",
                table: "ProjectCrewGroups",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectCrewGroups");
        }
    }
}
