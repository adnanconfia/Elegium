using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class externaluserdetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CV",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certificates",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContainsDealMemo",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContainsLOI",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractCreated",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractSent",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractSigned",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicenseClasses",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationTraining",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDrivingLicense",
                table: "ProjectExternalUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHalal",
                table: "ProjectExternalUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsKosher",
                table: "ProjectExternalUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVegan",
                table: "ProjectExternalUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVegetarian",
                table: "ProjectExternalUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionDescription",
                table: "ProjectExternalUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "ProjectExternalUsers",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserCity",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserCountryId",
                table: "ProjectExternalUsers",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserPostalCode",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserStreet",
                table: "ProjectExternalUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalUserContact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalUserId = table.Column<int>(nullable: false),
                    Position = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneHome = table.Column<string>(nullable: true),
                    PhoneMobile = table.Column<string>(nullable: true),
                    PhoneOffice = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalUserContact_ProjectExternalUsers_ExternalUserId",
                        column: x => x.ExternalUserId,
                        principalTable: "ProjectExternalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalUserContractFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(nullable: true),
                    FileId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    ExternalUserId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    MimeType = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UserFriendlySize = table.Column<string>(nullable: true),
                    CreateAtTicks = table.Column<long>(nullable: false),
                    Default = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserContractFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalUserContractFile_ProjectExternalUsers_ExternalUserId",
                        column: x => x.ExternalUserId,
                        principalTable: "ProjectExternalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalUserDraftFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(nullable: true),
                    FileId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    ExternalUserId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    MimeType = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UserFriendlySize = table.Column<string>(nullable: true),
                    CreateAtTicks = table.Column<long>(nullable: false),
                    Default = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserDraftFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalUserDraftFile_ProjectExternalUsers_ExternalUserId",
                        column: x => x.ExternalUserId,
                        principalTable: "ProjectExternalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalUserFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(nullable: true),
                    FileId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    ExternalUserId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    MimeType = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UserFriendlySize = table.Column<string>(nullable: true),
                    CreateAtTicks = table.Column<long>(nullable: false),
                    Default = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalUserFile_ProjectExternalUsers_ExternalUserId",
                        column: x => x.ExternalUserId,
                        principalTable: "ProjectExternalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectExternalUsers_PositionId",
                table: "ProjectExternalUsers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectExternalUsers_UserCountryId",
                table: "ProjectExternalUsers",
                column: "UserCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserContact_ExternalUserId",
                table: "ExternalUserContact",
                column: "ExternalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserContractFile_ExternalUserId",
                table: "ExternalUserContractFile",
                column: "ExternalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserDraftFile_ExternalUserId",
                table: "ExternalUserDraftFile",
                column: "ExternalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalUserFile_ExternalUserId",
                table: "ExternalUserFile",
                column: "ExternalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectExternalUsers_WorkingPositions_PositionId",
                table: "ProjectExternalUsers",
                column: "PositionId",
                principalTable: "WorkingPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectExternalUsers_Countries_UserCountryId",
                table: "ProjectExternalUsers",
                column: "UserCountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectExternalUsers_WorkingPositions_PositionId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectExternalUsers_Countries_UserCountryId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "ExternalUserContact");

            migrationBuilder.DropTable(
                name: "ExternalUserContractFile");

            migrationBuilder.DropTable(
                name: "ExternalUserDraftFile");

            migrationBuilder.DropTable(
                name: "ExternalUserFile");

            migrationBuilder.DropIndex(
                name: "IX_ProjectExternalUsers_PositionId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectExternalUsers_UserCountryId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "CV",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "Certificates",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "ContainsDealMemo",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "ContainsLOI",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "ContractCreated",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "ContractSent",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "ContractSigned",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "DrivingLicenseClasses",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "EducationTraining",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "IsDrivingLicense",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "IsHalal",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "IsKosher",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "IsVegan",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "IsVegetarian",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "PositionDescription",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "UserCity",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "UserCountryId",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "UserPostalCode",
                table: "ProjectExternalUsers");

            migrationBuilder.DropColumn(
                name: "UserStreet",
                table: "ProjectExternalUsers");
        }
    }
}
