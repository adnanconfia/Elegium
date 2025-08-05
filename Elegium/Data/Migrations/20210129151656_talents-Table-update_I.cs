using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class talentsTableupdate_I : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talents_Agencies_AgencyId",
                table: "Talents");

            migrationBuilder.AlterColumn<int>(
                name: "AgencyId",
                table: "Talents",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Talents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_ProjectId",
                table: "Talents",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Talents_Agencies_AgencyId",
                table: "Talents",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Talents_Project_ProjectId",
                table: "Talents",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talents_Agencies_AgencyId",
                table: "Talents");

            migrationBuilder.DropForeignKey(
                name: "FK_Talents_Project_ProjectId",
                table: "Talents");

            migrationBuilder.DropIndex(
                name: "IX_Talents_ProjectId",
                table: "Talents");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Talents");

            migrationBuilder.AlterColumn<int>(
                name: "AgencyId",
                table: "Talents",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Talents_Agencies_AgencyId",
                table: "Talents",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
