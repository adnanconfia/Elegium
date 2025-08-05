using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class BudgetMandatoryInProjectPartnerReq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update ProjectPartnerRequirement set Budget=0 where Budget is null");
            migrationBuilder.AlterColumn<int>(
                name: "Budget",
                table: "ProjectPartnerRequirement",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Budget",
                table: "ProjectPartnerRequirement",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
