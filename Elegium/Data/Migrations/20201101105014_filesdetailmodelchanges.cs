using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class filesdetailmodelchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileTask_DocumentCategory_DocumentCategoryId",
                table: "FileTask");

            migrationBuilder.DropIndex(
                name: "IX_FileTask_DocumentCategoryId",
                table: "FileTask");

            migrationBuilder.DropColumn(
                name: "DocumentCategoryId",
                table: "FileTask");

            migrationBuilder.AddColumn<int>(
                name: "DocumentFilesId",
                table: "FileTask",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileTask_DocumentFilesId",
                table: "FileTask",
                column: "DocumentFilesId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileTask_DocumentFiles_DocumentFilesId",
                table: "FileTask",
                column: "DocumentFilesId",
                principalTable: "DocumentFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileTask_DocumentFiles_DocumentFilesId",
                table: "FileTask");

            migrationBuilder.DropIndex(
                name: "IX_FileTask_DocumentFilesId",
                table: "FileTask");

            migrationBuilder.DropColumn(
                name: "DocumentFilesId",
                table: "FileTask");

            migrationBuilder.AddColumn<int>(
                name: "DocumentCategoryId",
                table: "FileTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileTask_DocumentCategoryId",
                table: "FileTask",
                column: "DocumentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileTask_DocumentCategory_DocumentCategoryId",
                table: "FileTask",
                column: "DocumentCategoryId",
                principalTable: "DocumentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
