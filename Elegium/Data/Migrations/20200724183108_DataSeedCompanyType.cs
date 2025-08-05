using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedCompanyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CompanyTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "One Man Band" }
            );
            migrationBuilder.InsertData(
                table: "CompanyTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Partnership" }
            );
            migrationBuilder.InsertData(
                table: "CompanyTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Full-Service Company" }
            );
            migrationBuilder.InsertData(
                table: "CompanyTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Project-Specific Company" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
