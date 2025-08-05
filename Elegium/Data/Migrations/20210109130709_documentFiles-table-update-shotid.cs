using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class documentFilestableupdateshotid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShotId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_ShotId",
                table: "DocumentFiles",
                column: "ShotId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Shots_ShotId",
                table: "DocumentFiles",
                column: "ShotId",
                principalTable: "Shots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Shots_ShotId",
                table: "DocumentFiles");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_ShotId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "ShotId",
                table: "DocumentFiles");
        }
    }
}
