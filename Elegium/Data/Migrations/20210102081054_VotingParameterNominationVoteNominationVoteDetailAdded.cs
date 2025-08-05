using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class VotingParameterNominationVoteNominationVoteDetailAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NominationVotes",
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
                    table.PrimaryKey("PK_NominationVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NominationVotes_NominationDetails_NominationDetailId",
                        column: x => x.NominationDetailId,
                        principalTable: "NominationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NominationVotes_AspNetUsers_UserVotedId",
                        column: x => x.UserVotedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VotingParameters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    MinScore = table.Column<float>(nullable: false),
                    MaxScore = table.Column<float>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NominationVoteDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NominationVoteId = table.Column<int>(nullable: false),
                    VotingParameterId = table.Column<int>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NominationVoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NominationVoteDetails_NominationVotes_NominationVoteId",
                        column: x => x.NominationVoteId,
                        principalTable: "NominationVotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NominationVoteDetails_VotingParameters_VotingParameterId",
                        column: x => x.VotingParameterId,
                        principalTable: "VotingParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NominationVoteDetails_NominationVoteId",
                table: "NominationVoteDetails",
                column: "NominationVoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NominationVoteDetails_VotingParameterId",
                table: "NominationVoteDetails",
                column: "VotingParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_NominationVotes_NominationDetailId",
                table: "NominationVotes",
                column: "NominationDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_NominationVotes_UserVotedId",
                table: "NominationVotes",
                column: "UserVotedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NominationVoteDetails");

            migrationBuilder.DropTable(
                name: "NominationVotes");

            migrationBuilder.DropTable(
                name: "VotingParameters");
        }
    }
}
