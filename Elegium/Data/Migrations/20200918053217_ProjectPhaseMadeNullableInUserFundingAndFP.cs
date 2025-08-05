using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectPhaseMadeNullableInUserFundingAndFP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_ProjectManagementPhase_ProjectPhaseId",
                table: "UserFundingAndFP");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectPhaseId",
                table: "UserFundingAndFP",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_ProjectManagementPhase_ProjectPhaseId",
                table: "UserFundingAndFP",
                column: "ProjectPhaseId",
                principalTable: "ProjectManagementPhase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_ProjectManagementPhase_ProjectPhaseId",
                table: "UserFundingAndFP");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectPhaseId",
                table: "UserFundingAndFP",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_ProjectManagementPhase_ProjectPhaseId",
                table: "UserFundingAndFP",
                column: "ProjectPhaseId",
                principalTable: "ProjectManagementPhase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
