using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedPromotionCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "All Categories" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Hair, Makeup & Styling" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Film, Television & Stage Crew" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Music" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Dancing" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "VFX & Special Effects" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "Photography" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 8, "Acting" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 9, "Modeling" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 10, "Props & Constumes" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 11, "Stunts" }
            );
            migrationBuilder.InsertData(
                table: "PromotionCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 12, "Extras" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
