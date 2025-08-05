using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Elegium.Data.Migrations
{
    public partial class VisibilityAreasDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VisibilityAreas",
                columns: new[] { "Id", "Name" },
                values: new object[] { Guid.NewGuid(), "Discovery" }
            );
            migrationBuilder.InsertData(
                table: "VisibilityAreas",
                columns: new[] { "Id", "Name" },
                values: new object[] { Guid.NewGuid(), "Social Networking" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
