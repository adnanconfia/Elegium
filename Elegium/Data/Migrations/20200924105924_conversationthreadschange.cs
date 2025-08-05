using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class conversationthreadschange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ConversationThreadsDto");

            migrationBuilder.DropColumn(
                name: "Delivered",
                table: "ConversationThreadsDto");

            migrationBuilder.DropColumn(
                name: "FriendlyTime",
                table: "ConversationThreadsDto");

            migrationBuilder.DropColumn(
                name: "IsItFromMe",
                table: "ConversationThreadsDto");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "ConversationThreadsDto");

            migrationBuilder.DropColumn(
                name: "Online",
                table: "ConversationThreadsDto");

            migrationBuilder.DropColumn(
                name: "Opened",
                table: "ConversationThreadsDto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ConversationThreadsDto",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delivered",
                table: "ConversationThreadsDto",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FriendlyTime",
                table: "ConversationThreadsDto",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsItFromMe",
                table: "ConversationThreadsDto",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "ConversationThreadsDto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Online",
                table: "ConversationThreadsDto",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Opened",
                table: "ConversationThreadsDto",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
