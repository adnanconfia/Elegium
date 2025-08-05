using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class _11092020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DraftContractFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(nullable: true),
                    FileId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    ProjectCrewId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    MimeType = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UserFriendlySize = table.Column<string>(nullable: true),
                    CreateAtTicks = table.Column<long>(nullable: false),
                    Default = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftContractFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftContractFiles_ProjectCrews_ProjectCrewId",
                        column: x => x.ProjectCrewId,
                        principalTable: "ProjectCrews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftContractFiles_ProjectCrewId",
                table: "DraftContractFiles",
                column: "ProjectCrewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftContractFiles");
        }
    }
}
