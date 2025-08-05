using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectPartnerRequirementEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagementPhase",
                table: "ProjectPartnerRequirement");

            migrationBuilder.DropColumn(
                name: "MediaProjectPhase",
                table: "ProjectPartnerRequirement");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagementPhase",
                table: "ProjectPartnerRequirement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaProjectPhase",
                table: "ProjectPartnerRequirement",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
