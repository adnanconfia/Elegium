using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class AdminUserSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6c89554c-3d77-4d9b-ab04-1e574bdffd65", "8095c0d0-0fbf-45fa-a022-b1a8c5e74bec", "Member", "MEMBER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "476d07ec-20a0-4aba-96c7-bee848d6e34b", "2aa941d3-5846-491b-9d0b-df04697ad928", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "FirstName", "LastName", "Company", "Industry" },
                values: new object[] { "cde75b94-0c08-42e3-a27c-aa0816b25e47", "admin@gmail.com", "ADMIN@GMAIL.COM", "admin@gmail.com", "ADMIN@GMAIL.COM", true, "AQAAAAEAACcQAAAAEHy1UvP5gvOUA51Cn/2vFfcdXaXZrmETpEhlL33YFwG+uUYIZWmWwFUhdHo3FpTxvw==", "LFWZ4VRDGTFNTK6G47SSPF763ZJIUJUA", "dcfcbad5-70fa-4a54-ac51-a7d86b46f342", null, false, false, null, true, 0, "Admin", "John", "", "" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "cde75b94-0c08-42e3-a27c-aa0816b25e47", "476d07ec-20a0-4aba-96c7-bee848d6e34b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
