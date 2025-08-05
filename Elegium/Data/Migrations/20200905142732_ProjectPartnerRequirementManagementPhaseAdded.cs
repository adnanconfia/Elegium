using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectPartnerRequirementManagementPhaseAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectPartnerRequirementManagementPhase",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectManagementPhasesId1 = table.Column<Guid>(nullable: true),
                    ProjectManagementPhasesId = table.Column<string>(nullable: true),
                    ProjectPartnerRequirementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPartnerRequirementManagementPhase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPartnerRequirementManagementPhase_ProjectManagementPhase_ProjectManagementPhasesId1",
                        column: x => x.ProjectManagementPhasesId1,
                        principalTable: "ProjectManagementPhase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectPartnerRequirementManagementPhase_ProjectPartnerRequirement_ProjectPartnerRequirementId",
                        column: x => x.ProjectPartnerRequirementId,
                        principalTable: "ProjectPartnerRequirement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPartnerRequirementManagementPhase_ProjectManagementPhasesId1",
                table: "ProjectPartnerRequirementManagementPhase",
                column: "ProjectManagementPhasesId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPartnerRequirementManagementPhase_ProjectPartnerRequirementId",
                table: "ProjectPartnerRequirementManagementPhase",
                column: "ProjectPartnerRequirementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectPartnerRequirementManagementPhase");
        }
    }
}
