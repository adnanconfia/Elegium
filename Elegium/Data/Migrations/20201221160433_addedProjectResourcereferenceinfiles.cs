using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addedProjectResourcereferenceinfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectResourceId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_ProjectResourceId",
                table: "DocumentFiles",
                column: "ProjectResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_ProjectResources_ProjectResourceId",
                table: "DocumentFiles",
                column: "ProjectResourceId",
                principalTable: "ProjectResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_ProjectResources_ProjectResourceId",
                table: "DocumentFiles");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_ProjectResourceId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "ProjectResourceId",
                table: "DocumentFiles");
        }
    }
}
