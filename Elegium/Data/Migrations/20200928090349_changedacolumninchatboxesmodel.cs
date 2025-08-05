using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class changedacolumninchatboxesmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "ChatBoxes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatBoxes_ReceiverId",
                table: "ChatBoxes",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatBoxes_AspNetUsers_ReceiverId",
                table: "ChatBoxes",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatBoxes_AspNetUsers_ReceiverId",
                table: "ChatBoxes");

            migrationBuilder.DropIndex(
                name: "IX_ChatBoxes_ReceiverId",
                table: "ChatBoxes");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "ChatBoxes");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId1",
                table: "ChatBoxes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverIdId",
                table: "ChatBoxes",
                type: "nvarchar(450)",
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
    }
}
