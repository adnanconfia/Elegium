using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addedlastmessageidinthread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LastMessageId",
                table: "Thread",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Thread_LastMessageId",
                table: "Thread",
                column: "LastMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Thread_Thread_LastMessageId",
                table: "Thread",
                column: "LastMessageId",
                principalTable: "Thread",
                principalColumn: "ThreadId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thread_Thread_LastMessageId",
                table: "Thread");

            migrationBuilder.DropIndex(
                name: "IX_Thread_LastMessageId",
                table: "Thread");

            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "Thread");
        }
    }
}
