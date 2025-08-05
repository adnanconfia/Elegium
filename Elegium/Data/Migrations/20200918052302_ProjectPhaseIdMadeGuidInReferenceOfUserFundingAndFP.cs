using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectPhaseIdMadeGuidInReferenceOfUserFundingAndFP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_ProjectManagementPhase_ProjectPhaseId1",
                table: "UserFundingAndFP");

            migrationBuilder.DropIndex(
                name: "IX_UserFundingAndFP_ProjectPhaseId1",
                table: "UserFundingAndFP");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId1",
                table: "UserFundingAndFP");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectPhaseId",
                table: "UserFundingAndFP",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_ProjectPhaseId",
                table: "UserFundingAndFP",
                column: "ProjectPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_ProjectManagementPhase_ProjectPhaseId",
                table: "UserFundingAndFP",
                column: "ProjectPhaseId",
                principalTable: "ProjectManagementPhase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_ProjectManagementPhase_ProjectPhaseId",
                table: "UserFundingAndFP");

            migrationBuilder.DropIndex(
                name: "IX_UserFundingAndFP_ProjectPhaseId",
                table: "UserFundingAndFP");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectPhaseId",
                table: "UserFundingAndFP",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId1",
                table: "UserFundingAndFP",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_ProjectPhaseId1",
                table: "UserFundingAndFP",
                column: "ProjectPhaseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_ProjectManagementPhase_ProjectPhaseId1",
                table: "UserFundingAndFP",
                column: "ProjectPhaseId1",
                principalTable: "ProjectManagementPhase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
