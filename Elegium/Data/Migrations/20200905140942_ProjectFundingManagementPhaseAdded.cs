using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectFundingManagementPhaseAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectFundingManagementPhase",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectManagementPhasesId1 = table.Column<Guid>(nullable: true),
                    ProjectManagementPhasesId = table.Column<string>(nullable: true),
                    ProjectFundingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFundingManagementPhase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFundingManagementPhase_ProjectFunding_ProjectFundingId",
                        column: x => x.ProjectFundingId,
                        principalTable: "ProjectFunding",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectFundingManagementPhase_ProjectManagementPhase_ProjectManagementPhasesId1",
                        column: x => x.ProjectManagementPhasesId1,
                        principalTable: "ProjectManagementPhase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFundingManagementPhase_ProjectFundingId",
                table: "ProjectFundingManagementPhase",
                column: "ProjectFundingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFundingManagementPhase_ProjectManagementPhasesId1",
                table: "ProjectFundingManagementPhase",
                column: "ProjectManagementPhasesId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFundingManagementPhase");
        }
    }
}
