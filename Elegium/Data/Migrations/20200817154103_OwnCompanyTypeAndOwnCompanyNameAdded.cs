using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class OwnCompanyTypeAndOwnCompanyNameAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnCompanyName",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnCompanyTypeId",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_OwnCompanyTypeId",
                table: "UserProfiles",
                column: "OwnCompanyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_CompanyTypes_OwnCompanyTypeId",
                table: "UserProfiles",
                column: "OwnCompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_CompanyTypes_OwnCompanyTypeId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_OwnCompanyTypeId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "OwnCompanyName",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "OwnCompanyTypeId",
                table: "UserProfiles");
        }
    }
}
