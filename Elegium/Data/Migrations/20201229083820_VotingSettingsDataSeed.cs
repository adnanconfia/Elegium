using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class VotingSettingsDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into VotingSettings (SettingCode,SettingDesc,SettingValue) values('NOMINATION_STARTED','Start/Stop the nomination process','N');
                insert into VotingSettings (SettingCode,SettingDesc,SettingValue) values('VOTING_STARTED','Start/Stop the voting process','N');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
