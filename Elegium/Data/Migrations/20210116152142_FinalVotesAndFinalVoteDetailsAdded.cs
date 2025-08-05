using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class FinalVotesAndFinalVoteDetailsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalVotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NominationDetailId = table.Column<int>(nullable: false),
                    TotalScore = table.Column<float>(nullable: false),
                    UserVotedId = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalVotes_NominationDetails_NominationDetailId",
                        column: x => x.NominationDetailId,
                        principalTable: "NominationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinalVotes_AspNetUsers_UserVotedId",
                        column: x => x.UserVotedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinalVoteDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinalVoteId = table.Column<int>(nullable: false),
                    VotingParameterId = table.Column<int>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalVoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalVoteDetails_FinalVotes_FinalVoteId",
                        column: x => x.FinalVoteId,
                        principalTable: "FinalVotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinalVoteDetails_VotingParameters_VotingParameterId",
                        column: x => x.VotingParameterId,
                        principalTable: "VotingParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalVoteDetails_FinalVoteId",
                table: "FinalVoteDetails",
                column: "FinalVoteId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalVoteDetails_VotingParameterId",
                table: "FinalVoteDetails",
                column: "VotingParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalVotes_NominationDetailId",
                table: "FinalVotes",
                column: "NominationDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalVotes_UserVotedId",
                table: "FinalVotes",
                column: "UserVotedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalVoteDetails");

            migrationBuilder.DropTable(
                name: "FinalVotes");
        }
    }
}
