using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class AddedCityInUserFundingAndFP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "UserFundingAndFP",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_CityId",
                table: "UserFundingAndFP",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_Cities_CityId",
                table: "UserFundingAndFP",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_Cities_CityId",
                table: "UserFundingAndFP");

            migrationBuilder.DropIndex(
                name: "IX_UserFundingAndFP_CityId",
                table: "UserFundingAndFP");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "UserFundingAndFP");
        }
    }
}
