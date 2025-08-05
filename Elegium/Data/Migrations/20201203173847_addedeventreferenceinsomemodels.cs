using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addedeventreferenceinsomemodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "ProjectTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "DocumentFiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_EventId",
                table: "ProjectTasks",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_EventId",
                table: "DocumentFiles",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EventId",
                table: "Comments",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Events_EventId",
                table: "Comments",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Events_EventId",
                table: "DocumentFiles",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Events_EventId",
                table: "ProjectTasks",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Events_EventId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Events_EventId",
                table: "DocumentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Events_EventId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_EventId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_EventId",
                table: "DocumentFiles");

            migrationBuilder.DropIndex(
                name: "IX_Comments_EventId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Comments");
        }
    }
}
