using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addcompanyattr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyCity",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyCountryId",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPostalCode",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyStreet",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectExternalUsers_CompanyCountryId",
                table: "ProjectExternalUsers",
                column: "CompanyCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectExternalUsers_Countries_CompanyCountryId",
                table: "ProjectExternalUsers",
                column: "CompanyCountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectExternalUsers_Countries_CompanyCountryId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectExternalUsers_CompanyCountryId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "CompanyCity",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "CompanyCountryId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "CompanyPostalCode",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "CompanyStreet",
                table: "ProjectExternalUsers");
        }
    }
}
