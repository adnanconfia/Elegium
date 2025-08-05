using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class fundingFPrequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FundingFPRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    FundingOrFP = table.Column<string>(nullable: true),
                    Offer = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    SenderId = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    OfferOrLooking = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundingFPRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundingFPRequests_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FundingFPRequests_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FundingFPRequests_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundingFPRequests_OwnerId",
                table: "FundingFPRequests",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FundingFPRequests_ProjectId",
                table: "FundingFPRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FundingFPRequests_SenderId",
                table: "FundingFPRequests",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundingFPRequests");
        }
    }
}
