using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class CalederCategories_Modified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CalenderCategories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "CalenderCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CalenderCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalenderCategories_ProjectId",
                table: "CalenderCategories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CalenderCategories_UserId",
                table: "CalenderCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalenderCategories_Project_ProjectId",
                table: "CalenderCategories",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalenderCategories_AspNetUsers_UserId",
                table: "CalenderCategories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalenderCategories_Project_ProjectId",
                table: "CalenderCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CalenderCategories_AspNetUsers_UserId",
                table: "CalenderCategories");

            migrationBuilder.DropIndex(
                name: "IX_CalenderCategories_ProjectId",
                table: "CalenderCategories");

            migrationBuilder.DropIndex(
                name: "IX_CalenderCategories_UserId",
                table: "CalenderCategories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CalenderCategories");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CalenderCategories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CalenderCategories");
        }
    }
}
