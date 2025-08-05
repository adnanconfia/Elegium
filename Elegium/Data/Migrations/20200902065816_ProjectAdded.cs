using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProjectFunctions = table.Column<string>(nullable: true),
                    ProductionTypeId = table.Column<int>(nullable: false),
                    ProductionLength = table.Column<string>(nullable: true),
                    ProductionRecordingMethod = table.Column<string>(nullable: true),
                    ProductionAspectRatio = table.Column<string>(nullable: true),
                    ProductionMode = table.Column<string>(nullable: true),
                    ProductionColor = table.Column<string>(nullable: true),
                    ProductionLanguageId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    Logo = table.Column<byte[]>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    ContactName = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    ContactPhone = table.Column<string>(nullable: true),
                    ContactFax = table.Column<string>(nullable: true),
                    isVisible = table.Column<bool>(nullable: false),
                    VisibilityArea = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_Languages_ProductionLanguageId",
                        column: x => x.ProductionLanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_ProductionType_ProductionTypeId",
                        column: x => x.ProductionTypeId,
                        principalTable: "ProductionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_CurrencyId",
                table: "Project",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProductionLanguageId",
                table: "Project",
                column: "ProductionLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProductionTypeId",
                table: "Project",
                column: "ProductionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
