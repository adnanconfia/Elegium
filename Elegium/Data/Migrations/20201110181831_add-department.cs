using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class adddepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectExternalUsers_WorkingPositions_PositionId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectExternalUsers_Countries_UserCountryId",
                table: "ProjectExternalUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UserCountryId",
                table: "ProjectExternalUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "ProjectExternalUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectExternalUsers_DepartmentId",
                table: "ProjectExternalUsers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectExternalUsers_Departments_DepartmentId",
                table: "ProjectExternalUsers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectExternalUsers_WorkingPositions_PositionId",
                table: "ProjectExternalUsers",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectExternalUsers_Countries_UserCountryId",
                table: "ProjectExternalUsers",
                column: "UserCountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectExternalUsers_Departments_DepartmentId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectExternalUsers_WorkingPositions_PositionId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectExternalUsers_Countries_UserCountryId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectExternalUsers_DepartmentId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ProjectExternalUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UserCountryId",
                table: "ProjectExternalUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "ProjectExternalUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectExternalUsers_WorkingPositions_PositionId",
                table: "ProjectExternalUsers",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectExternalUsers_Countries_UserCountryId",
                table: "ProjectExternalUsers",
                column: "UserCountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
