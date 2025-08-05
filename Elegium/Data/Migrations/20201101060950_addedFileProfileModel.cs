using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addedFileProfileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileComment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    MarkupText = table.Column<string>(nullable: true),
                    DocumentFileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileComment_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileComment_DocumentFiles_DocumentFileId",
                        column: x => x.DocumentFileId,
                        principalTable: "DocumentFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileLink",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    DocumentFileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileLink_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileLink_DocumentFiles_DocumentFileId",
                        column: x => x.DocumentFileId,
                        principalTable: "DocumentFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileTask",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    HasDeadline = table.Column<bool>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    DocumentCategoryId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Completed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileTask_DocumentCategory_DocumentCategoryId",
                        column: x => x.DocumentCategoryId,
                        principalTable: "DocumentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileTask_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VersionFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(nullable: true),
                    DocumentFileId = table.Column<int>(nullable: false),
                    FileId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    MimeType = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserFriendlySize = table.Column<string>(nullable: true),
                    CreateAtTicks = table.Column<long>(nullable: false),
                    Default = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersionFiles_DocumentFiles_DocumentFileId",
                        column: x => x.DocumentFileId,
                        principalTable: "DocumentFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionFiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileTaskAssignedTo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    FileTaskId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTaskAssignedTo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileTaskAssignedTo_FileTask_FileTaskId",
                        column: x => x.FileTaskId,
                        principalTable: "FileTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileTaskAssignedTo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileComment_ApplicationUserId",
                table: "FileComment",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileComment_DocumentFileId",
                table: "FileComment",
                column: "DocumentFileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileLink_ApplicationUserId",
                table: "FileLink",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileLink_DocumentFileId",
                table: "FileLink",
                column: "DocumentFileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileTask_DocumentCategoryId",
                table: "FileTask",
                column: "DocumentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FileTask_UserId",
                table: "FileTask",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileTaskAssignedTo_FileTaskId",
                table: "FileTaskAssignedTo",
                column: "FileTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_FileTaskAssignedTo_UserId",
                table: "FileTaskAssignedTo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionFiles_DocumentFileId",
                table: "VersionFiles",
                column: "DocumentFileId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionFiles_UserId",
                table: "VersionFiles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileComment");

            migrationBuilder.DropTable(
                name: "FileLink");

            migrationBuilder.DropTable(
                name: "FileTaskAssignedTo");

            migrationBuilder.DropTable(
                name: "VersionFiles");

            migrationBuilder.DropTable(
                name: "FileTask");
        }
    }
}
