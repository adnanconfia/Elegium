using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class CalenderEventCategoryIdMadeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Events_CalenderCategories_CalenderCategoryId",
            //    table: "Events");
            migrationBuilder.Sql("delete from Events;");
            migrationBuilder.AlterColumn<int>(
                name: "CalenderCategoryId",
                table: "Events",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_CalenderCategories_CalenderCategoryId",
                table: "Events",
                column: "CalenderCategoryId",
                principalTable: "CalenderCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_CalenderCategories_CalenderCategoryId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "CalenderCategoryId",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_CalenderCategories_CalenderCategoryId",
                table: "Events",
                column: "CalenderCategoryId",
                principalTable: "CalenderCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
