using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectPartnerAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectPartner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectPartnerRole = table.Column<string>(nullable: true),
                    FinancialParticipationRequired = table.Column<bool>(nullable: false),
                    FinancialShare = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    PartnerUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPartner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPartner_UserProfiles_PartnerUserId",
                        column: x => x.PartnerUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectPartner_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPartner_PartnerUserId",
                table: "ProjectPartner",
                column: "PartnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPartner_ProjectId",
                table: "ProjectPartner",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectPartner");
        }
    }
}
