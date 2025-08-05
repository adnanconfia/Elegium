using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class WorkingPositionAddedInProfessionalHireRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkingPositionId",
                table: "ProfessionalHireRequests",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalHireRequests_WorkingPositionId",
                table: "ProfessionalHireRequests",
                column: "WorkingPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalHireRequests_WorkingPositions_WorkingPositionId",
                table: "ProfessionalHireRequests",
                column: "WorkingPositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalHireRequests_WorkingPositions_WorkingPositionId",
                table: "ProfessionalHireRequests");

            migrationBuilder.DropIndex(
                name: "IX_ProfessionalHireRequests_WorkingPositionId",
                table: "ProfessionalHireRequests");

            migrationBuilder.DropColumn(
                name: "WorkingPositionId",
                table: "ProfessionalHireRequests");
        }
    }
}
