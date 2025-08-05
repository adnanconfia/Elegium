using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class FavoriteAndSavedProfessionals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteProfessionals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfessionalId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProfessionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteProfessionals_UserProfiles_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteProfessionals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedProfessionals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfessionalId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedProfessionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedProfessionals_UserProfiles_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedProfessionals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProfessionals_ProfessionalId",
                table: "FavoriteProfessionals",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProfessionals_UserId",
                table: "FavoriteProfessionals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedProfessionals_ProfessionalId",
                table: "SavedProfessionals",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedProfessionals_UserId",
                table: "SavedProfessionals",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteProfessionals");

            migrationBuilder.DropTable(
                name: "SavedProfessionals");
        }
    }
}
