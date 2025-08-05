using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class sceneCostumestableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "SceneCostumes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtraId",
                table: "SceneCostumes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "SceneCostumes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SceneCostumes_CharacterId",
                table: "SceneCostumes",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SceneCostumes_ExtraId",
                table: "SceneCostumes",
                column: "ExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_SceneCostumes_characters_CharacterId",
                table: "SceneCostumes",
                column: "CharacterId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SceneCostumes_Extras_ExtraId",
                table: "SceneCostumes",
                column: "ExtraId",
                principalTable: "Extras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SceneCostumes_characters_CharacterId",
                table: "SceneCostumes");

            migrationBuilder.DropForeignKey(
                name: "FK_SceneCostumes_Extras_ExtraId",
                table: "SceneCostumes");

            migrationBuilder.DropIndex(
                name: "IX_SceneCostumes_CharacterId",
                table: "SceneCostumes");

            migrationBuilder.DropIndex(
                name: "IX_SceneCostumes_ExtraId",
                table: "SceneCostumes");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "SceneCostumes");

            migrationBuilder.DropColumn(
                name: "ExtraId",
                table: "SceneCostumes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "SceneCostumes");
        }
    }
}
