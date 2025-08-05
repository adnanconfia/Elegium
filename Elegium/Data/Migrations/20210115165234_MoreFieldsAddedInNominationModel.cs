using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class MoreFieldsAddedInNominationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<bool>(
                name: "IsVotingFinished",
                table: "Nominations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVotingStarted",
                table: "Nominations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "VotingFinishDateTime",
                table: "Nominations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VotingStartDateTime",
                table: "Nominations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVotingFinished",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "IsVotingStarted",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "VotingFinishDateTime",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "VotingStartDateTime",
                table: "Nominations");

            
        }
    }
}
