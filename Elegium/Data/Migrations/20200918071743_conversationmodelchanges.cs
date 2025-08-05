using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class conversationmodelchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Message_Conversation_ConversationId1",
            //    table: "Message");

            //migrationBuilder.DropIndex(
            //    name: "IX_Message_ConversationId1",
            //    table: "Message");

            //migrationBuilder.DropColumn(
            //    name: "ConversationId1",
            //    table: "Message");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "ConversationId",
            //    table: "Message",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Message_ConversationId",
            //    table: "Message",
            //    column: "ConversationId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Message_Conversation_ConversationId",
            //    table: "Message",
            //    column: "ConversationId",
            //    principalTable: "Conversation",
            //    principalColumn: "ConversationId",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Message_Conversation_ConversationId",
            //    table: "Message");

            //migrationBuilder.DropIndex(
            //    name: "IX_Message_ConversationId",
            //    table: "Message");

            //migrationBuilder.AlterColumn<string>(
            //    name: "ConversationId",
            //    table: "Message",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(Guid));

            //migrationBuilder.AddColumn<Guid>(
            //    name: "ConversationId1",
            //    table: "Message",
            //    type: "uniqueidentifier",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Message_ConversationId1",
            //    table: "Message",
            //    column: "ConversationId1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Message_Conversation_ConversationId1",
            //    table: "Message",
            //    column: "ConversationId1",
            //    principalTable: "Conversation",
            //    principalColumn: "ConversationId",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
