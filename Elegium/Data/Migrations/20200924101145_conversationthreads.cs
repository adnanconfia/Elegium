using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class conversationthreads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversationThreadsDto",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    MessageId = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    When = table.Column<DateTime>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Delivered = table.Column<bool>(nullable: false),
                    ThreadId = table.Column<Guid>(nullable: true),
                    IsItFromMe = table.Column<bool>(nullable: false),
                    MyProperty = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Online = table.Column<bool>(nullable: true),
                    Opened = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    FriendlyTime = table.Column<string>(nullable: true),
                    UnreadMsgs = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationThreadsDto");
        }
    }
}
