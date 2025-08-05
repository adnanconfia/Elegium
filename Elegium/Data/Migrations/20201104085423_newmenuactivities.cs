using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class newmenuactivities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 18, "Costumes", "costumes", "COSTUMES", " Department specific" }
         );
            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 19, "Makup & Hair", "makeuphair", "MAKEUPHAIR", " Department specific" }
         );
            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 20, "Production design", "productiondesign", "PRODUCTIONDESIGN", " Department specific" }
         );
            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 21, "Financing", "financing", "FINANCING", "Planning" }
         );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
