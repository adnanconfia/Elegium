using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedEquipmentCategoryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EquipmentCategoryType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Film & Video Equipment" }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategoryType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Film, TV & Theatre Costumes" }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategoryType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Film, TV & Theatre Props" }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategoryType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Armoury" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
