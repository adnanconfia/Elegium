using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class chatrelatedmodelschange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Thread_ThreadId1",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadReadState_Message_MessageId1",
                table: "ThreadReadState");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadUsers_Thread_ThreadId1",
                table: "ThreadUsers");

            migrationBuilder.DropIndex(
                name: "IX_ThreadUsers_ThreadId1",
                table: "ThreadUsers");

            migrationBuilder.DropIndex(
                name: "IX_ThreadReadState_MessageId1",
                table: "ThreadReadState");

            migrationBuilder.DropIndex(
                name: "IX_Message_ThreadId1",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ThreadId1",
                table: "ThreadUsers");

            migrationBuilder.DropColumn(
                name: "MessageId1",
                table: "ThreadReadState");

            migrationBuilder.DropColumn(
                name: "ThreadId1",
                table: "Message");

            migrationBuilder.AlterColumn<Guid>(
                name: "ThreadId",
                table: "ThreadUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ThreadUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "MessageId",
                table: "ThreadReadState",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ThreadId",
                table: "Message",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Message",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Message",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ThreadUsers_ThreadId",
                table: "ThreadUsers",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadReadState_MessageId",
                table: "ThreadReadState",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ThreadId",
                table: "Message",
                column: "ThreadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Thread_ThreadId",
                table: "Message",
                column: "ThreadId",
                principalTable: "Thread",
                principalColumn: "ThreadId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadReadState_Message_MessageId",
                table: "ThreadReadState",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadUsers_Thread_ThreadId",
                table: "ThreadUsers",
                column: "ThreadId",
                principalTable: "Thread",
                principalColumn: "ThreadId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Thread_ThreadId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadReadState_Message_MessageId",
                table: "ThreadReadState");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadUsers_Thread_ThreadId",
                table: "ThreadUsers");

            migrationBuilder.DropIndex(
                name: "IX_ThreadUsers_ThreadId",
                table: "ThreadUsers");

            migrationBuilder.DropIndex(
                name: "IX_ThreadReadState_MessageId",
                table: "ThreadReadState");

            migrationBuilder.DropIndex(
                name: "IX_Message_ThreadId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ThreadUsers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "ThreadId",
                table: "ThreadUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ThreadId1",
                table: "ThreadUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MessageId",
                table: "ThreadReadState",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "MessageId1",
                table: "ThreadReadState",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ThreadId",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ThreadId1",
                table: "Message",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThreadUsers_ThreadId1",
                table: "ThreadUsers",
                column: "ThreadId1");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadReadState_MessageId1",
                table: "ThreadReadState",
                column: "MessageId1");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ThreadId1",
                table: "Message",
                column: "ThreadId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Thread_ThreadId1",
                table: "Message",
                column: "ThreadId1",
                principalTable: "Thread",
                principalColumn: "ThreadId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadReadState_Message_MessageId1",
                table: "ThreadReadState",
                column: "MessageId1",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadUsers_Thread_ThreadId1",
                table: "ThreadUsers",
                column: "ThreadId1",
                principalTable: "Thread",
                principalColumn: "ThreadId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
