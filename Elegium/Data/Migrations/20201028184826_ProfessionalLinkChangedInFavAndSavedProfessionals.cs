using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProfessionalLinkChangedInFavAndSavedProfessionals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProfessionals_UserProfiles_ProfessionalId",
                table: "FavoriteProfessionals");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedProfessionals_UserProfiles_ProfessionalId",
                table: "SavedProfessionals");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessionalId",
                table: "SavedProfessionals",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessionalId",
                table: "FavoriteProfessionals",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProfessionals_AspNetUsers_ProfessionalId",
                table: "FavoriteProfessionals",
                column: "ProfessionalId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedProfessionals_AspNetUsers_ProfessionalId",
                table: "SavedProfessionals",
                column: "ProfessionalId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProfessionals_AspNetUsers_ProfessionalId",
                table: "FavoriteProfessionals");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedProfessionals_AspNetUsers_ProfessionalId",
                table: "SavedProfessionals");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionalId",
                table: "SavedProfessionals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionalId",
                table: "FavoriteProfessionals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProfessionals_UserProfiles_ProfessionalId",
                table: "FavoriteProfessionals",
                column: "ProfessionalId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedProfessionals_UserProfiles_ProfessionalId",
                table: "SavedProfessionals",
                column: "ProfessionalId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
