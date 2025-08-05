using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class announcementsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnouncementId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    PinTop = table.Column<bool>(nullable: false),
                    HasDeadline = table.Column<bool>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Announcements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementsAssignedTo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    AnnouncementId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementsAssignedTo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementsAssignedTo_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_AnnouncementId",
                table: "DocumentFiles",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ProjectId",
                table: "Announcements",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_UserId",
                table: "Announcements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementsAssignedTo_AnnouncementId",
                table: "AnnouncementsAssignedTo",
                column: "AnnouncementId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Announcements_AnnouncementId",
                table: "DocumentFiles",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Announcements_AnnouncementId",
                table: "DocumentFiles");

            migrationBuilder.DropTable(
                name: "AnnouncementsAssignedTo");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_AnnouncementId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "AnnouncementId",
                table: "DocumentFiles");
        }
    }
}
