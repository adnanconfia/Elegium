using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DocumentstableupdateAgency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgencyID",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_AgencyID",
                table: "DocumentFiles",
                column: "AgencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Agencies_AgencyID",
                table: "DocumentFiles",
                column: "AgencyID",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Agencies_AgencyID",
                table: "DocumentFiles");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_AgencyID",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "AgencyID",
                table: "DocumentFiles");
        }
    }
}
