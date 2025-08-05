using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class somemodelchangesinprojectresource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                table: "ProjectResources");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ProjectResources",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "HireOrSale",
                table: "ProjectResources",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "OriginalResourceId",
                table: "ProjectResources",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectResources_OriginalResourceId",
                table: "ProjectResources",
                column: "OriginalResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectResources_Resource_OriginalResourceId",
                table: "ProjectResources",
                column: "OriginalResourceId",
                principalTable: "Resource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectResources_Resource_OriginalResourceId",
                table: "ProjectResources");

            migrationBuilder.DropIndex(
                name: "IX_ProjectResources_OriginalResourceId",
                table: "ProjectResources");

            migrationBuilder.DropColumn(
                name: "OriginalResourceId",
                table: "ProjectResources");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "ProjectResources",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HireOrSale",
                table: "ProjectResources",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "ProjectResources",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
