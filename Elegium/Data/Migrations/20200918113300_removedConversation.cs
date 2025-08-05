using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class removedConversation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Message_Conversation_ConversationId",
            //    table: "Message");

            //migrationBuilder.DropTable(
            //    name: "Conversation");

            //migrationBuilder.DropIndex(
            //    name: "IX_Message_ConversationId",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "ConversationId",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "Delivered",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "Unread",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "When",
            //    table: "Message");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<Guid>(
            //    name: "ConversationId",
            //    table: "Message",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.AddColumn<bool>(
            //    name: "Delivered",
            //    table: "Message",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "Unread",
            //    table: "Message",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "When",
            //    table: "Message",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.CreateTable(
            //    name: "Conversation",
            //    columns: table => new
            //    {
            //        ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        SenderId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
            //    name: "IX_Message_ConversationId",
            //    table: "Message",
            //    column: "ConversationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Conversation_ReceiverId",
            //    table: "Conversation",
            //    column: "ReceiverId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Conversation_SenderId",
            //    table: "Conversation",
            //    column: "SenderId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Message_Conversation_ConversationId",
            //    table: "Message",
            //    column: "ConversationId",
            //    principalTable: "Conversation",
            //    principalColumn: "ConversationId",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
