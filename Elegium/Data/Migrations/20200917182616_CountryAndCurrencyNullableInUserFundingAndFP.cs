using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class CountryAndCurrencyNullableInUserFundingAndFP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_Countries_CountryId",
                table: "UserFundingAndFP");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_Currency_CurrencyId",
                table: "UserFundingAndFP");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "UserFundingAndFP",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "UserFundingAndFP",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_Countries_CountryId",
                table: "UserFundingAndFP",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_Currency_CurrencyId",
                table: "UserFundingAndFP",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_Countries_CountryId",
                table: "UserFundingAndFP");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFundingAndFP_Currency_CurrencyId",
                table: "UserFundingAndFP");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "UserFundingAndFP",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "UserFundingAndFP",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_Countries_CountryId",
                table: "UserFundingAndFP",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundingAndFP_Currency_CurrencyId",
                table: "UserFundingAndFP",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
