using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class FieldsMadeNullableInResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_ResourceCondition_ConditionId",
                table: "Resource");

            migrationBuilder.AlterColumn<int>(
                name: "SalePrice",
                table: "Resource",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RentalPrice",
                table: "Resource",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MinRentalPeriod",
                table: "Resource",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MaxRentalPeriod",
                table: "Resource",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ConditionId",
                table: "Resource",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_ResourceCondition_ConditionId",
                table: "Resource",
                column: "ConditionId",
                principalTable: "ResourceCondition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_ResourceCondition_ConditionId",
                table: "Resource");

            migrationBuilder.AlterColumn<int>(
                name: "SalePrice",
                table: "Resource",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RentalPrice",
                table: "Resource",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinRentalPeriod",
                table: "Resource",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxRentalPeriod",
                table: "Resource",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConditionId",
                table: "Resource",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_ResourceCondition_ConditionId",
                table: "Resource",
                column: "ConditionId",
                principalTable: "ResourceCondition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
