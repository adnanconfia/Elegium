using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class SavedAndFavoriteFundingAndFPAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteFundingAndFPs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFundingAndFPId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteFundingAndFPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteFundingAndFPs_UserFundingAndFP_UserFundingAndFPId",
                        column: x => x.UserFundingAndFPId,
                        principalTable: "UserFundingAndFP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteFundingAndFPs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedFundingAndFPs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFundingAndFPId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedFundingAndFPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedFundingAndFPs_UserFundingAndFP_UserFundingAndFPId",
                        column: x => x.UserFundingAndFPId,
                        principalTable: "UserFundingAndFP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedFundingAndFPs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteFundingAndFPs_UserFundingAndFPId",
                table: "FavoriteFundingAndFPs",
                column: "UserFundingAndFPId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteFundingAndFPs_UserId",
                table: "FavoriteFundingAndFPs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedFundingAndFPs_UserFundingAndFPId",
                table: "SavedFundingAndFPs",
                column: "UserFundingAndFPId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedFundingAndFPs_UserId",
                table: "SavedFundingAndFPs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteFundingAndFPs");

            migrationBuilder.DropTable(
                name: "SavedFundingAndFPs");
        }
    }
}
