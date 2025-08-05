using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class agencytableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProejectId",
                table: "Agencies",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Agencies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_ProejectId",
                table: "Agencies",
                column: "ProejectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agencies_Project_ProejectId",
                table: "Agencies",
                column: "ProejectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agencies_Project_ProejectId",
                table: "Agencies");

            migrationBuilder.DropIndex(
                name: "IX_Agencies_ProejectId",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "ProejectId",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Agencies");
        }
    }
}
