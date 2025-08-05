using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class updateprojectcrew1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrews_WorkingPositions_PositionId",
                table: "ProjectCrews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrews_Units_UnitId",
                table: "ProjectCrews");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "ProjectCrews",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SeperationDate",
                table: "ProjectCrews",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "ProjectCrews",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HiringDate",
                table: "ProjectCrews",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrews_WorkingPositions_PositionId",
                table: "ProjectCrews",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrews_Units_UnitId",
                table: "ProjectCrews",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrews_WorkingPositions_PositionId",
                table: "ProjectCrews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCrews_Units_UnitId",
                table: "ProjectCrews");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "ProjectCrews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SeperationDate",
                table: "ProjectCrews",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "ProjectCrews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "HiringDate",
                table: "ProjectCrews",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrews_WorkingPositions_PositionId",
                table: "ProjectCrews",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCrews_Units_UnitId",
                table: "ProjectCrews",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
