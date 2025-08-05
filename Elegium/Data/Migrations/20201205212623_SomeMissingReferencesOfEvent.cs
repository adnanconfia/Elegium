using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class SomeMissingReferencesOfEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "EventId",
            //    table: "DocumentFiles",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_DocumentFiles_EventId",
            //    table: "DocumentFiles",
            //    column: "EventId");


            //migrationBuilder.AddForeignKey(
            //    name: "FK_DocumentFiles_Events_EventId",
            //    table: "DocumentFiles",
            //    column: "EventId",
            //    principalTable: "Events",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_DocumentFiles_Events_EventId",
            //    table: "DocumentFiles");

            //migrationBuilder.DropIndex(
            //    name: "IX_DocumentFiles_EventId",
            //    table: "DocumentFiles");

            //migrationBuilder.DropColumn(
            //    name: "EventId",
            //    table: "DocumentFiles");
        }
    }
}
