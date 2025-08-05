using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ResourceAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentCategoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ConditionId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    MinRentalPeriod = table.Column<int>(nullable: false),
                    MaxRentalPeriod = table.Column<int>(nullable: false),
                    RentalPrice = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    Insured = table.Column<bool>(nullable: false),
                    RentalTerms = table.Column<string>(nullable: true),
                    SalePrice = table.Column<int>(nullable: false),
                    Website = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    YoutubeVideoLink = table.Column<string>(nullable: true),
                    VimeoVideoLink = table.Column<string>(nullable: true),
                    OtherTerms = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resource_ResourceCondition_ConditionId",
                        column: x => x.ConditionId,
                        principalTable: "ResourceCondition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resource_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resource_EquipmentCategory_EquipmentCategoryId",
                        column: x => x.EquipmentCategoryId,
                        principalTable: "EquipmentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resource_ConditionId",
                table: "Resource",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_CurrencyId",
                table: "Resource",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_EquipmentCategoryId",
                table: "Resource",
                column: "EquipmentCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resource");
        }
    }
}
