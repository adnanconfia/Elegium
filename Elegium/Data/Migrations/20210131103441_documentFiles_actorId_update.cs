using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class documentFiles_actorId_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActorId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_ActorId",
                table: "DocumentFiles",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Actors_ActorId",
                table: "DocumentFiles",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Actors_ActorId",
                table: "DocumentFiles");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_ActorId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "DocumentFiles");
        }
    }
}
