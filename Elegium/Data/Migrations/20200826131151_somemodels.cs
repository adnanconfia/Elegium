using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class somemodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserOtherLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    LanguageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOtherLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOtherLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOtherLanguages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPromotionCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    PromotionCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPromotionCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPromotionCategory_PromotionCategory_PromotionCategoryId",
                        column: x => x.PromotionCategoryId,
                        principalTable: "PromotionCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPromotionCategory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOtherLanguages_LanguageId",
                table: "UserOtherLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOtherLanguages_UserId",
                table: "UserOtherLanguages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPromotionCategory_PromotionCategoryId",
                table: "UserPromotionCategory",
                column: "PromotionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPromotionCategory_UserId",
                table: "UserPromotionCategory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOtherLanguages");

            migrationBuilder.DropTable(
                name: "UserPromotionCategory");
        }
    }
}
