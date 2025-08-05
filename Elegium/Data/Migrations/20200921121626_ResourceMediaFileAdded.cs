using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ResourceMediaFileAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceMediaFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    FileId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    ResourceId = table.Column<int>(nullable: true),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    CreatedAtTicks = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceMediaFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceMediaFile_Resource_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceMediaFile_ResourceId",
                table: "ResourceMediaFile",
                column: "ResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceMediaFile");
        }
    }
}
