using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectCrewAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectCrews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    HiringDate = table.Column<DateTime>(nullable: false),
                    SeperationDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsFromDiscovery = table.Column<bool>(nullable: false),
                    ProfessionalHireRequestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCrews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCrews_ProfessionalHireRequests_ProfessionalHireRequestId",
                        column: x => x.ProfessionalHireRequestId,
                        principalTable: "ProfessionalHireRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectCrews_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectCrews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrews_ProfessionalHireRequestId",
                table: "ProjectCrews",
                column: "ProfessionalHireRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrews_ProjectId",
                table: "ProjectCrews",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCrews_UserId",
                table: "ProjectCrews",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectCrews");
        }
    }
}
