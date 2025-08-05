using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class _10262020_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalUserUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalUserId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalUserUnits_ProjectExternalUsers_ExternalUserId",
                        column: x => x.ExternalUserId,
                        principalTable: "ProjectExternalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalUserUnits_ProjectUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ProjectUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserUnits_ExternalUserId",
                table: "ExternalUserUnits",
                column: "ExternalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserUnits_UnitId",
                table: "ExternalUserUnits",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalUserUnits");
        }
    }
}
