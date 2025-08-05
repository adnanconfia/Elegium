using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class EnglishLevelDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
  update LanguageLevels set Name='Beginner' where Id=1;
  update LanguageLevels set Name='Intermediate' where Id=2;
  insert into LanguageLevels (Name) values('Advanced');
  insert into LanguageLevels (Name) values('Proficient');
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
