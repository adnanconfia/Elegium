using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectPhasesMadeSingleSelect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectManagementPhaseId",
                table: "ProjectPartnerRequirement",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectManagementPhaseId1",
                table: "ProjectPartnerRequirement",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagementPhaseId",
                table: "ProjectFunding",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectManagementPhaseId1",
                table: "ProjectFunding",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagementPhaseId",
                table: "ProjectFinancialParticipation",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectManagementPhaseId1",
                table: "ProjectFinancialParticipation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPartnerRequirement_ProjectManagementPhaseId1",
                table: "ProjectPartnerRequirement",
                column: "ProjectManagementPhaseId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFunding_ProjectManagementPhaseId1",
                table: "ProjectFunding",
                column: "ProjectManagementPhaseId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFinancialParticipation_ProjectManagementPhaseId1",
                table: "ProjectFinancialParticipation",
                column: "ProjectManagementPhaseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFinancialParticipation_ProjectManagementPhase_ProjectManagementPhaseId1",
                table: "ProjectFinancialParticipation",
                column: "ProjectManagementPhaseId1",
                principalTable: "ProjectManagementPhase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFunding_ProjectManagementPhase_ProjectManagementPhaseId1",
                table: "ProjectFunding",
                column: "ProjectManagementPhaseId1",
                principalTable: "ProjectManagementPhase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPartnerRequirement_ProjectManagementPhase_ProjectManagementPhaseId1",
                table: "ProjectPartnerRequirement",
                column: "ProjectManagementPhaseId1",
                principalTable: "ProjectManagementPhase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFinancialParticipation_ProjectManagementPhase_ProjectManagementPhaseId1",
                table: "ProjectFinancialParticipation");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFunding_ProjectManagementPhase_ProjectManagementPhaseId1",
                table: "ProjectFunding");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPartnerRequirement_ProjectManagementPhase_ProjectManagementPhaseId1",
                table: "ProjectPartnerRequirement");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPartnerRequirement_ProjectManagementPhaseId1",
                table: "ProjectPartnerRequirement");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFunding_ProjectManagementPhaseId1",
                table: "ProjectFunding");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFinancialParticipation_ProjectManagementPhaseId1",
                table: "ProjectFinancialParticipation");

            migrationBuilder.DropColumn(
                name: "ProjectManagementPhaseId",
                table: "ProjectPartnerRequirement");

            migrationBuilder.DropColumn(
                name: "ProjectManagementPhaseId1",
                table: "ProjectPartnerRequirement");

            migrationBuilder.DropColumn(
                name: "ProjectManagementPhaseId",
                table: "ProjectFunding");

            migrationBuilder.DropColumn(
                name: "ProjectManagementPhaseId1",
                table: "ProjectFunding");

            migrationBuilder.DropColumn(
                name: "ProjectManagementPhaseId",
                table: "ProjectFinancialParticipation");

            migrationBuilder.DropColumn(
                name: "ProjectManagementPhaseId1",
                table: "ProjectFinancialParticipation");
        }
    }
}
