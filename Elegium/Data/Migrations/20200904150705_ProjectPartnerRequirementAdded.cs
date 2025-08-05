using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectPartnerRequirementAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectPartnerRequirement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Budget = table.Column<string>(nullable: true),
                    ManagementPhase = table.Column<string>(nullable: true),
                    MediaProjectPhase = table.Column<string>(nullable: true),
                    YourFinancialShare = table.Column<string>(nullable: true),
                    ProjectPartnersCount = table.Column<int>(nullable: false),
                    SynopsisCompleted = table.Column<bool>(nullable: false),
                    PlotCompleted = table.Column<bool>(nullable: false),
                    ScreenplayCompleted = table.Column<bool>(nullable: false),
                    ScreenplayWorkRequired = table.Column<bool>(nullable: false),
                    NeedFinancialParticipation = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPartnerRequirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPartnerRequirement_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPartnerRequirement_ProjectId",
                table: "ProjectPartnerRequirement",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectPartnerRequirement");
        }
    }
}
