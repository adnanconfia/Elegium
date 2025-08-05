using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class PartnerUserIdNullableInProjectPartner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPartner_UserProfiles_PartnerUserId",
                table: "ProjectPartner");

            migrationBuilder.AlterColumn<int>(
                name: "PartnerUserId",
                table: "ProjectPartner",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPartner_UserProfiles_PartnerUserId",
                table: "ProjectPartner",
                column: "PartnerUserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPartner_UserProfiles_PartnerUserId",
                table: "ProjectPartner");

            migrationBuilder.AlterColumn<int>(
                name: "PartnerUserId",
                table: "ProjectPartner",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPartner_UserProfiles_PartnerUserId",
                table: "ProjectPartner",
                column: "PartnerUserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
