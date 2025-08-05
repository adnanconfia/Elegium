using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class removecolumnsfrommessagetable : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
