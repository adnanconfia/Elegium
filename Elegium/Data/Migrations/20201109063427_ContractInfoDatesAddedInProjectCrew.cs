using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ContractInfoDatesAddedInProjectCrew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_DocumentCategory_DocumentCategoryId",
                table: "ProjectTasks");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentCategoryId",
                table: "ProjectTasks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ParentTaskId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContainsDealMemo",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContainsLOI",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractCreated",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractSent",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractSigned",
                table: "ProjectCrews",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ParentTaskId",
                table: "ProjectTasks",
                column: "ParentTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_DocumentCategory_DocumentCategoryId",
                table: "ProjectTasks",
                column: "DocumentCategoryId",
                principalTable: "DocumentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectTasks_ParentTaskId",
                table: "ProjectTasks",
                column: "ParentTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_DocumentCategory_DocumentCategoryId",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectTasks_ParentTaskId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ParentTaskId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ParentTaskId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ContainsDealMemo",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "ContainsLOI",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "ContractCreated",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "ContractSent",
                table: "ProjectCrews");

            migrationBuilder.DropColumn(
                name: "ContractSigned",
                table: "ProjectCrews");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentCategoryId",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_DocumentCategory_DocumentCategoryId",
                table: "ProjectTasks",
                column: "DocumentCategoryId",
                principalTable: "DocumentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
