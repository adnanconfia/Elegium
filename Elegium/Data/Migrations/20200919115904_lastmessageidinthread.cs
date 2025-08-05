using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class lastmessageidinthread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Message_ThreadId",
                table: "Message");

            migrationBuilder.AddColumn<Guid>(
                name: "LastMessageId",
                table: "Thread",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ThreadId",
                table: "Message",
                column: "ThreadId",
                unique: false,
                filter: "[ThreadId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Message_ThreadId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "Thread");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ThreadId",
                table: "Message",
                column: "ThreadId");
        }
    }
}
