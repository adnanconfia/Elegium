using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class comments_table_actorId_talentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActorId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TalentId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "talentsId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ActorId",
                table: "Comments",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_talentsId",
                table: "Comments",
                column: "talentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Actors_ActorId",
                table: "Comments",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Talents_talentsId",
                table: "Comments",
                column: "talentsId",
                principalTable: "Talents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Actors_ActorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Talents_talentsId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ActorId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_talentsId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TalentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "talentsId",
                table: "Comments");
        }
    }
}
