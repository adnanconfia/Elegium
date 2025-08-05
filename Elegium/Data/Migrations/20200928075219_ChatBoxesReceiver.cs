using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ChatBoxesReceiver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverId1",
                table: "ChatBoxes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverIdId",
                table: "ChatBoxes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatBoxes_ReceiverId1",
                table: "ChatBoxes",
                column: "ReceiverId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChatBoxes_ReceiverIdId",
                table: "ChatBoxes",
                column: "ReceiverIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatBoxes_AspNetUsers_ReceiverId1",
                table: "ChatBoxes",
                column: "ReceiverId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatBoxes_AspNetUsers_ReceiverIdId",
                table: "ChatBoxes",
                column: "ReceiverIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatBoxes_AspNetUsers_ReceiverId1",
                table: "ChatBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatBoxes_AspNetUsers_ReceiverIdId",
                table: "ChatBoxes");

            migrationBuilder.DropIndex(
                name: "IX_ChatBoxes_ReceiverId1",
                table: "ChatBoxes");

            migrationBuilder.DropIndex(
                name: "IX_ChatBoxes_ReceiverIdId",
                table: "ChatBoxes");

            migrationBuilder.DropColumn(
                name: "ReceiverId1",
                table: "ChatBoxes");

            migrationBuilder.DropColumn(
                name: "ReceiverIdId",
                table: "ChatBoxes");
        }
    }
}
