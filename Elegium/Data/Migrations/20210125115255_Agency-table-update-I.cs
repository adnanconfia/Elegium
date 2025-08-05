using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class AgencytableupdateI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgencyContacts_Agencies_AgencyId",
                table: "AgencyContacts");

            migrationBuilder.AlterColumn<int>(
                name: "AgencyId",
                table: "AgencyContacts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Agencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyContacts_Agencies_AgencyId",
                table: "AgencyContacts",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgencyContacts_Agencies_AgencyId",
                table: "AgencyContacts");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Agencies");

            migrationBuilder.AlterColumn<int>(
                name: "AgencyId",
                table: "AgencyContacts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyContacts_Agencies_AgencyId",
                table: "AgencyContacts",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
