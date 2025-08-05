using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProfessionalHireRequestMediaFileAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfessionalHireRequestMediaFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    FileId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    ProfessionalHireRequestId = table.Column<int>(nullable: true),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    CreatedAtTicks = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalHireRequestMediaFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessionalHireRequestMediaFiles_ProfessionalHireRequests_ProfessionalHireRequestId",
                        column: x => x.ProfessionalHireRequestId,
                        principalTable: "ProfessionalHireRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalHireRequestMediaFiles_ProfessionalHireRequestId",
                table: "ProfessionalHireRequestMediaFiles",
                column: "ProfessionalHireRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessionalHireRequestMediaFiles");
        }
    }
}
