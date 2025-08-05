using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class AddedApplicationUserInUserFundingAndFP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_UserProfiles_UserProfileId",
                table: "UserFundingAndFP");

            migrationBuilder.DropIndex(
                name: "IX_UserFundingAndFP_UserProfileId",
                table: "UserFundingAndFP");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "UserFundingAndFP");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserFundingAndFP",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_UserId",
                table: "UserFundingAndFP",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_AspNetUsers_UserId",
                table: "UserFundingAndFP",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_AspNetUsers_UserId",
                table: "UserFundingAndFP");

            migrationBuilder.DropIndex(
                name: "IX_UserFundingAndFP_UserId",
                table: "UserFundingAndFP");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserFundingAndFP");

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "UserFundingAndFP",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_UserProfileId",
                table: "UserFundingAndFP",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_UserProfiles_UserProfileId",
                table: "UserFundingAndFP",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
