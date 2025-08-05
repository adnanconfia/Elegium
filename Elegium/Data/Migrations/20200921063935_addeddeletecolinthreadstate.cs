using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addeddeletecolinthreadstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_Message_ThreadId",
            //    table: "Message");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ThreadReadState",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "LastMessageId",
            //    table: "Thread",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "ConnectionTime",
            //    table: "Connection",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.CreateIndex(
            //    name: "IX_Message_ThreadId",
            //    table: "Message",
            //    column: "ThreadId",
            //    unique: true,
            //    filter: "[ThreadId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Message_ThreadId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ThreadReadState");

            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "Thread");

            migrationBuilder.DropColumn(
                name: "ConnectionTime",
                table: "Connection");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ThreadId",
                table: "Message",
                column: "ThreadId");
        }
    }
}
