using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Elegium.Data.Migrations
{
    public partial class ProjectManagementPhaseDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProjectManagementPhase",
                columns: new[] { "Id", "Name" },
                values: new object[] { Guid.NewGuid(), "Development" }
            );
            migrationBuilder.InsertData(
                table: "ProjectManagementPhase",
                columns: new[] { "Id", "Name" },
                values: new object[] { Guid.NewGuid(), "Pre-Production" }
            );
            migrationBuilder.InsertData(
                table: "ProjectManagementPhase",
                columns: new[] { "Id", "Name" },
                values: new object[] { Guid.NewGuid(), "Production" }
            );
            migrationBuilder.InsertData(
                table: "ProjectManagementPhase",
                columns: new[] { "Id", "Name" },
                values: new object[] { Guid.NewGuid(), "Principal Photography" }
            );
            migrationBuilder.InsertData(
                table: "ProjectManagementPhase",
                columns: new[] { "Id", "Name" },
                values: new object[] { Guid.NewGuid(), "Wrap" }
            );
            migrationBuilder.InsertData(
                table: "ProjectManagementPhase",
                columns: new[] { "Id", "Name" },
                values: new object[] { Guid.NewGuid(), "Post-Production" }
            );
            migrationBuilder.InsertData(
                table: "ProjectManagementPhase",
                columns: new[] { "Id", "Name" },
                values: new object[] { Guid.NewGuid(), "Distribution" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
