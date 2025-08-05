using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectDisputeAndDisputeDetailAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectDisputes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    DisputeDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDisputes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDisputes_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectDisputes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDisputeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectDisputeId = table.Column<int>(nullable: false),
                    EnteryDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDisputeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDisputeDetails_ProjectDisputes_ProjectDisputeId",
                        column: x => x.ProjectDisputeId,
                        principalTable: "ProjectDisputes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDisputeDetails_ProjectDisputeId",
                table: "ProjectDisputeDetails",
                column: "ProjectDisputeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDisputes_ProjectId",
                table: "ProjectDisputes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDisputes_UserId",
                table: "ProjectDisputes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectDisputeDetails");

            migrationBuilder.DropTable(
                name: "ProjectDisputes");
        }
    }
}
