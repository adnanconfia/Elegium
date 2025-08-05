using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class CalenderEventMorePropsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalenderCategoryId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Events",
                nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "EventId",
            //    table: "DocumentFiles",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "EventsAdditionalViewers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    EventId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsAdditionalViewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventsAdditionalViewers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CalenderCategoryId",
                table: "Events",
                column: "CalenderCategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_DocumentFiles_EventId",
            //    table: "DocumentFiles",
            //    column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventsAdditionalViewers_EventId",
                table: "EventsAdditionalViewers",
                column: "EventId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DocumentFiles_Events_EventId",
            //    table: "DocumentFiles",
            //    column: "EventId",
            //    principalTable: "Events",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Events_CalenderCategories_CalenderCategoryId",
            //    table: "Events",
            //    column: "CalenderCategoryId",
            //    principalTable: "CalenderCategories",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Events_EventId",
                table: "DocumentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_CalenderCategories_CalenderCategoryId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventsAdditionalViewers");

            migrationBuilder.DropIndex(
                name: "IX_Events_CalenderCategoryId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_EventId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "CalenderCategoryId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "DocumentFiles");
        }
    }
}
