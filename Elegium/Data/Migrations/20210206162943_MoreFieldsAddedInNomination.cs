using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class MoreFieldsAddedInNomination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Nominations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Nominations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Nominations",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Nominations",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ProductionTypeId",
                table: "Nominations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TermsAndConditions",
                table: "Nominations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_CountryId",
                table: "Nominations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_CurrencyId",
                table: "Nominations",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_ProductionTypeId",
                table: "Nominations",
                column: "ProductionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nominations_Countries_CountryId",
                table: "Nominations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Nominations_Currency_CurrencyId",
                table: "Nominations",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Nominations_ProductionType_ProductionTypeId",
                table: "Nominations",
                column: "ProductionTypeId",
                principalTable: "ProductionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nominations_Countries_CountryId",
                table: "Nominations");

            migrationBuilder.DropForeignKey(
                name: "FK_Nominations_Currency_CurrencyId",
                table: "Nominations");

            migrationBuilder.DropForeignKey(
                name: "FK_Nominations_ProductionType_ProductionTypeId",
                table: "Nominations");

            migrationBuilder.DropIndex(
                name: "IX_Nominations_CountryId",
                table: "Nominations");

            migrationBuilder.DropIndex(
                name: "IX_Nominations_CurrencyId",
                table: "Nominations");

            migrationBuilder.DropIndex(
                name: "IX_Nominations_ProductionTypeId",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "ProductionTypeId",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "TermsAndConditions",
                table: "Nominations");
        }
    }
}
