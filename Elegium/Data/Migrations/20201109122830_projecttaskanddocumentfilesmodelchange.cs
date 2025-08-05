using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class projecttaskanddocumentfilesmodelchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_DocumentCategory_DocumentCategoryId",
                table: "DocumentFiles");

            migrationBuilder.AddColumn<int>(
                name: "DocumentFilesId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DocumentCategoryId",
                table: "DocumentFiles",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_DocumentFilesId",
                table: "ProjectTasks",
                column: "DocumentFilesId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_ProjectTaskId",
                table: "DocumentFiles",
                column: "ProjectTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_DocumentCategory_DocumentCategoryId",
                table: "DocumentFiles",
                column: "DocumentCategoryId",
                principalTable: "DocumentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_ProjectTasks_ProjectTaskId",
                table: "DocumentFiles",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_DocumentFiles_DocumentFilesId",
                table: "ProjectTasks",
                column: "DocumentFilesId",
                principalTable: "DocumentFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_DocumentCategory_DocumentCategoryId",
                table: "DocumentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_ProjectTasks_ProjectTaskId",
                table: "DocumentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_DocumentFiles_DocumentFilesId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_DocumentFilesId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_ProjectTaskId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "DocumentFilesId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "DocumentFiles");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentCategoryId",
                table: "DocumentFiles",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_DocumentCategory_DocumentCategoryId",
                table: "DocumentFiles",
                column: "DocumentCategoryId",
                principalTable: "DocumentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
