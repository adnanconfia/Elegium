using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addedbgimagecolsinprojectanduserprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "BackgroundImage",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "BackgroundImage",
                table: "Project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImage",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "BackgroundImage",
                table: "Project");
        }
    }
}
