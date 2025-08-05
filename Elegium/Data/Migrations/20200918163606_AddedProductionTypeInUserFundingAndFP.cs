using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class AddedProductionTypeInUserFundingAndFP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductionTypeId",
                table: "UserFundingAndFP",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFundingAndFP_ProductionTypeId",
                table: "UserFundingAndFP",
                column: "ProductionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_ProductionType_ProductionTypeId",
                table: "UserFundingAndFP",
                column: "ProductionTypeId",
                principalTable: "ProductionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_ProductionType_ProductionTypeId",
                table: "UserFundingAndFP");

            migrationBuilder.DropIndex(
                name: "IX_UserFundingAndFP_ProductionTypeId",
                table: "UserFundingAndFP");

            migrationBuilder.DropColumn(
                name: "ProductionTypeId",
                table: "UserFundingAndFP");
        }
    }
}
