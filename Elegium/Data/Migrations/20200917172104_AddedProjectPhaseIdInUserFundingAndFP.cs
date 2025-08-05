using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class AddedProjectPhaseIdInUserFundingAndFP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId1",
                table: "UserFundingAndFP",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
