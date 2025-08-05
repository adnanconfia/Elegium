using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class projecttaskreferenceincommentmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_DocumentCategory_DocumentCategoryId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentCategoryId",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DocumentFileId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DocumentFileId",
                table: "Comments",
                column: "DocumentFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProjectTaskId",
                table: "Comments",
                column: "ProjectTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_DocumentCategory_DocumentCategoryId",
                table: "Comments",
                column: "DocumentCategoryId",
                principalTable: "DocumentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_DocumentFiles_DocumentFileId",
                table: "Comments",
                column: "DocumentFileId",
                principalTable: "DocumentFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ProjectTasks_ProjectTaskId",
                table: "Comments",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_DocumentCategory_DocumentCategoryId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_DocumentFiles_DocumentFileId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ProjectTasks_ProjectTaskId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DocumentFileId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ProjectTaskId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DocumentFileId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentCategoryId",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_DocumentCategory_DocumentCategoryId",
                table: "Comments",
                column: "DocumentCategoryId",
                principalTable: "DocumentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
