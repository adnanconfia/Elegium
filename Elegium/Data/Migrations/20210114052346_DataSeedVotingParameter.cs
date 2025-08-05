using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class DataSeedVotingParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" Insert into VotingParameters (Name, MinScore, MaxScore, isActive) values('Idea-Originality',1,10,1)
                                    Insert into VotingParameters (Name, MinScore, MaxScore, isActive) values('Storyline',1,10,1)
                                    Insert into VotingParameters (Name, MinScore, MaxScore, isActive) values('Script',1,10,1)
                                    Insert into VotingParameters (Name, MinScore, MaxScore, isActive) values('Popularity',1,10,1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
