using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class UserProfileAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_Country_CountryId",
                table: "City");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingPosition",
                table: "WorkingPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageLevel",
                table: "LanguageLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Language",
                table: "Language");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyType",
                table: "CompanyType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.RenameTable(
                name: "WorkingPosition",
                newName: "WorkingPositions");

            migrationBuilder.RenameTable(
                name: "LanguageLevel",
                newName: "LanguageLevels");

            migrationBuilder.RenameTable(
                name: "Language",
                newName: "Languages");

            migrationBuilder.RenameTable(
                name: "Country",
                newName: "Countries");

            migrationBuilder.RenameTable(
                name: "CompanyType",
                newName: "CompanyTypes");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "Cities");

            migrationBuilder.RenameIndex(
                name: "IX_City_CountryId",
                table: "Cities",
                newName: "IX_Cities_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingPositions",
                table: "WorkingPositions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageLevels",
                table: "LanguageLevels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyTypes",
                table: "CompanyTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Photo = table.Column<byte[]>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    StreetAddress = table.Column<string>(nullable: true),
                    StreetAddress2 = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    IntroText = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Skype = table.Column<string>(nullable: true),
                    EnglishLevelId = table.Column<int>(nullable: true),
                    OtherLanguage = table.Column<string>(nullable: true),
                    FacebookLink = table.Column<string>(nullable: true),
                    FacebookLinkIsPublic = table.Column<bool>(nullable: false),
                    InstagramLink = table.Column<string>(nullable: true),
                    InstagramLinkIsPublic = table.Column<bool>(nullable: false),
                    LinkedinLink = table.Column<string>(nullable: true),
                    LinkedinLinkIsPublic = table.Column<bool>(nullable: false),
                    PinterestLink = table.Column<string>(nullable: true),
                    PinterestLinkIsPublic = table.Column<bool>(nullable: false),
                    TwitterLink = table.Column<string>(nullable: true),
                    TwitterLinkIsPublic = table.Column<bool>(nullable: false),
                    YoutubeLink = table.Column<string>(nullable: true),
                    YoutubeLinkIsPublic = table.Column<bool>(nullable: false),
                    ImdbLink = table.Column<string>(nullable: true),
                    ImdbLinkIsPublic = table.Column<bool>(nullable: false),
                    VimeoLink = table.Column<string>(nullable: true),
                    VimeoLinkIsPublic = table.Column<bool>(nullable: false),
                    CompanyLogo = table.Column<byte[]>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyTypeId = table.Column<int>(nullable: true),
                    CompanyLegalRepresent = table.Column<bool>(nullable: false),
                    CompanyPositionId = table.Column<int>(nullable: true),
                    CompanyStreetAddress = table.Column<string>(nullable: true),
                    CompanyStreetAddress2 = table.Column<string>(nullable: true),
                    CompanyCountryId = table.Column<int>(nullable: true),
                    CompanyCityId = table.Column<int>(nullable: true),
                    CompanyState = table.Column<string>(nullable: true),
                    CompanyPostCode = table.Column<string>(nullable: true),
                    CompanyEmail = table.Column<string>(nullable: true),
                    CompanyWeb = table.Column<string>(nullable: true),
                    CompanyPhone = table.Column<string>(nullable: true),
                    CompanyStudioAddress = table.Column<string>(nullable: true),
                    CompanyStudioAddress2 = table.Column<string>(nullable: true),
                    CompanyStudioCountryId = table.Column<int>(nullable: true),
                    CompanyStudioCityId = table.Column<int>(nullable: true),
                    CompanyStudioState = table.Column<string>(nullable: true),
                    CompanyStudioPostCode = table.Column<string>(nullable: true),
                    CompanyStudioEmail = table.Column<string>(nullable: true),
                    CompanyStudioWeb = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Cities_CompanyCityId",
                        column: x => x.CompanyCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Countries_CompanyCountryId",
                        column: x => x.CompanyCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_WorkingPositions_CompanyPositionId",
                        column: x => x.CompanyPositionId,
                        principalTable: "WorkingPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Cities_CompanyStudioCityId",
                        column: x => x.CompanyStudioCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Countries_CompanyStudioCountryId",
                        column: x => x.CompanyStudioCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_LanguageLevels_EnglishLevelId",
                        column: x => x.EnglishLevelId,
                        principalTable: "LanguageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CityId",
                table: "UserProfiles",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CompanyCityId",
                table: "UserProfiles",
                column: "CompanyCityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CompanyCountryId",
                table: "UserProfiles",
                column: "CompanyCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CompanyPositionId",
                table: "UserProfiles",
                column: "CompanyPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CompanyStudioCityId",
                table: "UserProfiles",
                column: "CompanyStudioCityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CompanyStudioCountryId",
                table: "UserProfiles",
                column: "CompanyStudioCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CompanyTypeId",
                table: "UserProfiles",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CountryId",
                table: "UserProfiles",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_EnglishLevelId",
                table: "UserProfiles",
                column: "EnglishLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingPositions",
                table: "WorkingPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageLevels",
                table: "LanguageLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyTypes",
                table: "CompanyTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "WorkingPositions",
                newName: "WorkingPosition");

            migrationBuilder.RenameTable(
                name: "Languages",
                newName: "Language");

            migrationBuilder.RenameTable(
                name: "LanguageLevels",
                newName: "LanguageLevel");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Country");

            migrationBuilder.RenameTable(
                name: "CompanyTypes",
                newName: "CompanyType");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "City");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_CountryId",
                table: "City",
                newName: "IX_City_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingPosition",
                table: "WorkingPosition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Language",
                table: "Language",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageLevel",
                table: "LanguageLevel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyType",
                table: "CompanyType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_City_Country_CountryId",
                table: "City",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
