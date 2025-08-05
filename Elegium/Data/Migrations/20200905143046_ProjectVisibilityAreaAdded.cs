using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectVisibilityAreaAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectVisibilityArea",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisibilityAreaId1 = table.Column<Guid>(nullable: true),
                    VisibilityAreaId = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectVisibilityArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectVisibilityArea_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectVisibilityArea_VisibilityAreas_VisibilityAreaId1",
                        column: x => x.VisibilityAreaId1,
                        principalTable: "VisibilityAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectVisibilityArea_ProjectId",
                table: "ProjectVisibilityArea",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectVisibilityArea_VisibilityAreaId1",
                table: "ProjectVisibilityArea",
                column: "VisibilityAreaId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectVisibilityArea");
        }
    }
}
