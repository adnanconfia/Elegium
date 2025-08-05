using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedEquipmentCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 1, "Camera", 1 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 2, "Lighting", 1 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 3, "Lenses", 1 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 4, "Prompting", 1 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 5, "Recorders, Monitors & Converters", 1 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 6, "Police", 2 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 7, "Prison", 2 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 8, "Security", 2 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 9, "Medical", 2 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 10, "Fire Services", 2 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 11, "Military Stuff", 3 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 12, "Luggage", 3 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 13, "Clocks", 3 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 14, "Musical Instruments", 3 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 15, "Toys & Games", 3 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 16, "Swords", 4 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 17, "Axes", 4 }
            );
            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name", "EquipmentCategoryTypeId" },
                values: new object[] { 18, "Guns", 4 }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
