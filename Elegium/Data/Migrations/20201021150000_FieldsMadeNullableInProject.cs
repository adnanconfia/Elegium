using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class FieldsMadeNullableInProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFinancialParticipation_Currency_CurrencyId",
                table: "ProjectFinancialParticipation");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFunding_Currency_CurrencyId",
                table: "ProjectFunding");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectPartnersCount",
                table: "ProjectPartnerRequirement",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Budget",
                table: "ProjectPartnerRequirement",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "ProjectFunding",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "ProjectFinancialParticipation",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFinancialParticipation_Currency_CurrencyId",
                table: "ProjectFinancialParticipation",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFunding_Currency_CurrencyId",
                table: "ProjectFunding",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFinancialParticipation_Currency_CurrencyId",
                table: "ProjectFinancialParticipation");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFunding_Currency_CurrencyId",
                table: "ProjectFunding");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectPartnersCount",
                table: "ProjectPartnerRequirement",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Budget",
                table: "ProjectPartnerRequirement",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "ProjectFunding",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "ProjectFinancialParticipation",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFinancialParticipation_Currency_CurrencyId",
                table: "ProjectFinancialParticipation",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFunding_Currency_CurrencyId",
                table: "ProjectFunding",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
