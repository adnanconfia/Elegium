using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class SceneSoundtableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sceneSounds_Sounds_SpecialId",
                table: "sceneSounds");

            migrationBuilder.DropIndex(
                name: "IX_sceneSounds_SpecialId",
                table: "sceneSounds");

            migrationBuilder.DropColumn(
                name: "SpecialId",
                table: "sceneSounds");

            migrationBuilder.CreateIndex(
                name: "IX_sceneSounds_SoundId",
                table: "sceneSounds",
                column: "SoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_sceneSounds_Sounds_SoundId",
                table: "sceneSounds",
                column: "SoundId",
                principalTable: "Sounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sceneSounds_Sounds_SoundId",
                table: "sceneSounds");

            migrationBuilder.DropIndex(
                name: "IX_sceneSounds_SoundId",
                table: "sceneSounds");

            migrationBuilder.AddColumn<int>(
                name: "SpecialId",
                table: "sceneSounds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_sceneSounds_SpecialId",
                table: "sceneSounds",
                column: "SpecialId");

            migrationBuilder.AddForeignKey(
                name: "FK_sceneSounds_Sounds_SpecialId",
                table: "sceneSounds",
                column: "SpecialId",
                principalTable: "Sounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
