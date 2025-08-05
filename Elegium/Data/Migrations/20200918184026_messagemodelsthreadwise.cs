using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class messagemodelsthreadwise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Message_AspNetUsers_ReceiverId",
            //    table: "Message");

            //migrationBuilder.DropIndex(
            //    name: "IX_Message_ReceiverId",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "Delivered",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "ReceiverId",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "Unread",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "When",
            //    table: "Message");

            migrationBuilder.AddColumn<string>(
                name: "ThreadId",
                table: "Message",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ThreadId1",
                table: "Message",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Thread",
                columns: table => new
                {
                    ThreadId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thread", x => x.ThreadId);
                    table.ForeignKey(
                        name: "FK_Thread_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThreadReadState",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MessageId1 = table.Column<Guid>(nullable: true),
                    MessageId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    When = table.Column<DateTime>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Delivered = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadReadState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThreadReadState_Message_MessageId1",
                        column: x => x.MessageId1,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThreadReadState_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThreadUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ThreadId1 = table.Column<Guid>(nullable: true),
                    ThreadId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThreadUsers_Thread_ThreadId1",
                        column: x => x.ThreadId1,
                        principalTable: "Thread",
                        principalColumn: "ThreadId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThreadUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_ThreadId1",
                table: "Message",
                column: "ThreadId1");

            migrationBuilder.CreateIndex(
                name: "IX_Thread_UserId",
                table: "Thread",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadReadState_MessageId1",
                table: "ThreadReadState",
                column: "MessageId1");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadReadState_UserId",
                table: "ThreadReadState",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadUsers_ThreadId1",
                table: "ThreadUsers",
                column: "ThreadId1");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadUsers_UserId",
                table: "ThreadUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Thread_ThreadId1",
                table: "Message",
                column: "ThreadId1",
                principalTable: "Thread",
                principalColumn: "ThreadId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Thread_ThreadId1",
                table: "Message");

            migrationBuilder.DropTable(
                name: "ThreadReadState");

            migrationBuilder.DropTable(
                name: "ThreadUsers");

            migrationBuilder.DropTable(
                name: "Thread");

            migrationBuilder.DropIndex(
                name: "IX_Message_ThreadId1",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ThreadId1",
                table: "Message");

            migrationBuilder.AddColumn<bool>(
                name: "Delivered",
                table: "Message",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Unread",
                table: "Message",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "When",
                table: "Message",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Message_ReceiverId",
                table: "Message",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_ReceiverId",
                table: "Message",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
