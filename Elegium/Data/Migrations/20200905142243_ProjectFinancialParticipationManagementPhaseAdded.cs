using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectFinancialParticipationManagementPhaseAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectFinancialParticipationManagementPhase",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectManagementPhasesId1 = table.Column<Guid>(nullable: true),
                    ProjectManagementPhasesId = table.Column<string>(nullable: true),
                    ProjectFinancialParticipationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFinancialParticipationManagementPhase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFinancialParticipationManagementPhase_ProjectFinancialParticipation_ProjectFinancialParticipationId",
                        column: x => x.ProjectFinancialParticipationId,
                        principalTable: "ProjectFinancialParticipation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectFinancialParticipationManagementPhase_ProjectManagementPhase_ProjectManagementPhasesId1",
                        column: x => x.ProjectManagementPhasesId1,
                        principalTable: "ProjectManagementPhase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFinancialParticipationManagementPhase_ProjectFinancialParticipationId",
                table: "ProjectFinancialParticipationManagementPhase",
                column: "ProjectFinancialParticipationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFinancialParticipationManagementPhase_ProjectManagementPhasesId1",
                table: "ProjectFinancialParticipationManagementPhase",
                column: "ProjectManagementPhasesId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFinancialParticipationManagementPhase");
        }
    }
}
