using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ChatBoxesVirtualKeywords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatBoxes_Thread_ThreadId",
                table: "ChatBoxes");

            migrationBuilder.AlterColumn<Guid>(
                name: "ThreadId",
                table: "ChatBoxes",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatBoxes_Thread_ThreadId",
                table: "ChatBoxes",
                column: "ThreadId",
                principalTable: "Thread",
                principalColumn: "ThreadId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatBoxes_Thread_ThreadId",
                table: "ChatBoxes");

            migrationBuilder.AlterColumn<Guid>(
                name: "ThreadId",
                table: "ChatBoxes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatBoxes_Thread_ThreadId",
                table: "ChatBoxes",
                column: "ThreadId",
                principalTable: "Thread",
                principalColumn: "ThreadId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
