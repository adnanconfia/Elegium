using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class AddedUserFundingAndFP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFundingAndFP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true),
                    BudgetUpto = table.Column<float>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    Offer = table.Column<float>(nullable: false),
                    OfferShare = table.Column<string>(nullable: true),
                    ProjectPhaseId = table.Column<Guid>(nullable: true),
                    SupportDetail = table.Column<string>(nullable: true),
                    OtherRequirements = table.Column<string>(nullable: true),
                    UserProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFundingAndFP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFundingAndFP_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFundingAndFP_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFundingAndFP_ProjectManagementPhase_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectManagementPhase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFundingAndFP_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_CountryId",
                table: "UserFundingAndFP",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_CurrencyId",
                table: "UserFundingAndFP",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_ProjectPhaseId",
                table: "UserFundingAndFP",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_UserProfileId",
                table: "UserFundingAndFP",
                column: "UserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFundingAndFP");
        }
    }
}
