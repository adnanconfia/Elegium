using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectFundingAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectFunding",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    Requirements = table.Column<string>(nullable: true),
                    FundersRequiredId1 = table.Column<Guid>(nullable: true),
                    FundersRequiredId = table.Column<string>(nullable: true),
                    BenefitsOffer = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFunding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFunding_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectFunding_FundersRequired_FundersRequiredId1",
                        column: x => x.FundersRequiredId1,
                        principalTable: "FundersRequired",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectFunding_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFunding_CurrencyId",
                table: "ProjectFunding",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFunding_FundersRequiredId1",
                table: "ProjectFunding",
                column: "FundersRequiredId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFunding_ProjectId",
                table: "ProjectFunding",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFunding");
        }
    }
}
