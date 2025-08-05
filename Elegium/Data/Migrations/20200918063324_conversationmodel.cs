using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class conversationmodel : Migration
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
            //    name: "ReceiverId",
            //    table: "Message");

            //migrationBuilder.AddColumn<string>(
            //    name: "ConversationId",
            //    table: "Message",
            //    nullable: true);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "ConversationId1",
            //    table: "Message",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "Conversation",
            //    columns: table => new
            //    {
            //        ConversationId = table.Column<Guid>(nullable: false),
            //        SenderId = table.Column<string>(nullable: true),
            //        ReceiverId = table.Column<string>(nullable: true),
            //        CreatedDate = table.Column<DateTime>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Conversation", x => x.ConversationId);
            //        table.ForeignKey(
            //            name: "FK_Conversation_AspNetUsers_ReceiverId",
            //            column: x => x.ReceiverId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Conversation_AspNetUsers_SenderId",
            //            column: x => x.SenderId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Message_ConversationId1",
            //    table: "Message",
            //    column: "ConversationId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Conversation_ReceiverId",
            //    table: "Conversation",
            //    column: "ReceiverId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Conversation_SenderId",
            //    table: "Conversation",
            //    column: "SenderId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Message_Conversation_ConversationId1",
            //    table: "Message",
            //    column: "ConversationId1",
            //    principalTable: "Conversation",
            //    principalColumn: "ConversationId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Message_Conversation_ConversationId1",
            //    table: "Message");

            //migrationBuilder.DropTable(
            //    name: "Conversation");

            //migrationBuilder.DropIndex(
            //    name: "IX_Message_ConversationId1",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "ConversationId",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "ConversationId1",
            //    table: "Message");

            //migrationBuilder.AddColumn<string>(
            //    name: "ReceiverId",
            //    table: "Message",
            //    type: "nvarchar(450)",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Message_ReceiverId",
            //    table: "Message",
            //    column: "ReceiverId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Message_AspNetUsers_ReceiverId",
            //    table: "Message",
            //    column: "ReceiverId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
