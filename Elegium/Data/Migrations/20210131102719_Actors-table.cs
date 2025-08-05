using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class Actorstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneHome = table.Column<string>(nullable: true),
                    PhoneMobile = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    FirstStreet = table.Column<string>(nullable: true),
                    FirstCity = table.Column<string>(nullable: true),
                    FirstPostalCode = table.Column<string>(nullable: true),
                    FirstCountry = table.Column<string>(nullable: true),
                    SecondStreet = table.Column<string>(nullable: true),
                    SecondCity = table.Column<string>(nullable: true),
                    SecondPostalCode = table.Column<string>(nullable: true),
                    SecondCountry = table.Column<string>(nullable: true),
                    ProdStreet = table.Column<string>(nullable: true),
                    ProdCity = table.Column<string>(nullable: true),
                    ProdPostalCode = table.Column<string>(nullable: true),
                    ProdCountry = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    AgeMin = table.Column<int>(nullable: false),
                    AgeMax = table.Column<int>(nullable: false),
                    Languages = table.Column<string>(nullable: true),
                    Talent = table.Column<string>(nullable: true),
                    VocalRange = table.Column<string>(nullable: true),
                    Instruments = table.Column<string>(nullable: true),
                    Ethnicity = table.Column<string>(nullable: true),
                    BodyType = table.Column<string>(nullable: true),
                    EyeColor = table.Column<string>(nullable: true),
                    HairLength = table.Column<string>(nullable: true),
                    HairColor = table.Column<string>(nullable: true),
                    HairType = table.Column<string>(nullable: true),
                    FacialHair = table.Column<string>(nullable: true),
                    WillCutHair = table.Column<bool>(nullable: false),
                    WillShave = table.Column<bool>(nullable: false),
                    WearsGlasses = table.Column<bool>(nullable: false),
                    HasTattoo = table.Column<bool>(nullable: false),
                    HasPiercings = table.Column<bool>(nullable: false),
                    AppearanceNote = table.Column<string>(nullable: true),
                    ShirtSize = table.Column<string>(nullable: true),
                    PantsSize = table.Column<string>(nullable: true),
                    DressSize = table.Column<string>(nullable: true),
                    BraSize = table.Column<string>(nullable: true),
                    GloveSize = table.Column<string>(nullable: true),
                    ShoeSize = table.Column<string>(nullable: true),
                    BodyHeight = table.Column<string>(nullable: true),
                    NeckToFloor = table.Column<string>(nullable: true),
                    HeadGirth = table.Column<string>(nullable: true),
                    NeckGirth = table.Column<string>(nullable: true),
                    SoulderLength = table.Column<string>(nullable: true),
                    BackWidth = table.Column<string>(nullable: true),
                    BackLength = table.Column<string>(nullable: true),
                    ChestGirth = table.Column<string>(nullable: true),
                    UnderChestGirth = table.Column<string>(nullable: true),
                    NeckToBreast = table.Column<string>(nullable: true),
                    NeckToWaist = table.Column<string>(nullable: true),
                    FrontWaistLength = table.Column<string>(nullable: true),
                    WaistCircumference = table.Column<string>(nullable: true),
                    HipSize = table.Column<string>(nullable: true),
                    HipHeightFromWaist = table.Column<string>(nullable: true),
                    ArmLength = table.Column<string>(nullable: true),
                    UpperArmGirth = table.Column<string>(nullable: true),
                    ArmBedGirth = table.Column<string>(nullable: true),
                    ArmBedLength = table.Column<string>(nullable: true),
                    HandGirth = table.Column<string>(nullable: true),
                    Inseam = table.Column<string>(nullable: true),
                    InnerLengthOfLeg = table.Column<string>(nullable: true),
                    OuterLengthOfLeg = table.Column<string>(nullable: true),
                    CalfGirth = table.Column<string>(nullable: true),
                    ThighGirth = table.Column<string>(nullable: true),
                    KneeHeight = table.Column<string>(nullable: true),
                    KneeCircumference = table.Column<string>(nullable: true),
                    AnkleWidth = table.Column<string>(nullable: true),
                    FootLength = table.Column<string>(nullable: true),
                    FrontFootWidth = table.Column<string>(nullable: true),
                    FootWidth = table.Column<string>(nullable: true),
                    IsVeg = table.Column<bool>(nullable: false),
                    IsVegan = table.Column<bool>(nullable: false),
                    IsHalal = table.Column<bool>(nullable: false),
                    IsKosher = table.Column<bool>(nullable: false),
                    Allergies = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Is_deleted = table.Column<bool>(nullable: false),
                    AgencyId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actors_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Actors_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actors_AgencyId",
                table: "Actors",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_ProjectId",
                table: "Actors",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actors");
        }
    }
}
