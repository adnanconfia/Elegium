using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class userprojectmenumodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProjectMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    MenuActivityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjectMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProjectMenu_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProjectMenu_MenuActivity_MenuActivityId",
                        column: x => x.MenuActivityId,
                        principalTable: "MenuActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjectMenu_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProjectMenu_ApplicationUserId",
                table: "UserProjectMenu",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjectMenu_MenuActivityId",
                table: "UserProjectMenu",
                column: "MenuActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjectMenu_ProjectId",
                table: "UserProjectMenu",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProjectMenu");
        }
    }
}
