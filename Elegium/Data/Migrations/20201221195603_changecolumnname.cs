using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class changecolumnname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "RentOrSale", table: "ProjectResources", newName: "HireOrSale");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
