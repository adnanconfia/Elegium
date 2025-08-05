using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Elegium.Data.Migrations
{
    public partial class FundersRequiredDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FundersRequired",
                columns: new[] { "Id", "Name", "OrderCol" },
                values: new object[] { Guid.NewGuid(), "Only One or Two", 1 }
            );
            migrationBuilder.InsertData(
                table: "FundersRequired",
                columns: new[] { "Id", "Name", "OrderCol" },
                values: new object[] { Guid.NewGuid(), "1-3", 2 }
            );
            migrationBuilder.InsertData(
                table: "FundersRequired",
                columns: new[] { "Id", "Name", "OrderCol" },
                values: new object[] { Guid.NewGuid(), "3-6", 3 }
            );
            migrationBuilder.InsertData(
                table: "FundersRequired",
                columns: new[] { "Id", "Name", "OrderCol" },
                values: new object[] { Guid.NewGuid(), "6-10", 4 }
            );
            migrationBuilder.InsertData(
                table: "FundersRequired",
                columns: new[] { "Id", "Name", "OrderCol" },
                values: new object[] { Guid.NewGuid(), "No Limit", 5 }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
