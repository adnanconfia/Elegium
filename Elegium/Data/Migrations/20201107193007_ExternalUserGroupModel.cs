using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ExternalUserGroupModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalUserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalUserId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalUserGroups_ProjectExternalUsers_ExternalUserId",
                        column: x => x.ExternalUserId,
                        principalTable: "ProjectExternalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExternalUserGroups_ProjectUserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ProjectUserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserGroups_ExternalUserId",
                table: "ExternalUserGroups",
                column: "ExternalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserGroups_GroupId",
                table: "ExternalUserGroups",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalUserGroups");
        }
    }
}
