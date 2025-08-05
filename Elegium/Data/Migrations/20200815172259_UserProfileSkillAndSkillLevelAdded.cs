using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class UserProfileSkillAndSkillLevelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillLevelId",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_SkillId",
                table: "UserProfiles",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_SkillLevelId",
                table: "UserProfiles",
                column: "SkillLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Skill_SkillId",
                table: "UserProfiles",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_SkillLevel_SkillLevelId",
                table: "UserProfiles",
                column: "SkillLevelId",
                principalTable: "SkillLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Skill_SkillId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_SkillLevel_SkillLevelId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_SkillId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_SkillLevelId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "SkillLevelId",
                table: "UserProfiles");
        }
    }
}
