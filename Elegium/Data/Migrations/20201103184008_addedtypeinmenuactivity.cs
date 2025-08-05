using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class addedtypeinmenuactivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "MenuActivity",
                nullable: true);


            migrationBuilder.InsertData(
                table: "MenuActivity",
                columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
                values: new object[] { 3, "Crew", "crew", "CREW", "General" }
            );

            migrationBuilder.InsertData(
                table: "MenuActivity",
                columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
                values: new object[] { 4, "Announcements", "announcements", "ANNOUNCEMENTS", "General" }
            );

            migrationBuilder.InsertData(
               table: "MenuActivity",
               columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
               values: new object[] { 5, "Tasks", "tasks", "TASKS", "General" }
           );


            migrationBuilder.InsertData(
               table: "MenuActivity",
               columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
               values: new object[] { 6, "Calendar", "calendar", "CALENDAR", "General" }
           );


            migrationBuilder.InsertData(
              table: "MenuActivity",
              columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
              values: new object[] { 7, "Project Settings", "projectsettings", "PROJECTSETTINGS", "General" }
          );

            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 8, "Scenes & Script", "scenesandscripts", "SCENESSCRIPT", " Breakdowns & more" }
         );

            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 9, "Shots", "shots", "SHOTS", " Breakdowns & more" }
         );

            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 10, "Storyboard", "storyboard", "STORYBOARD", " Breakdowns & more" }
         );
            //

            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 11, "Content items", "contentitems", "CONTENTITEMS", " Breakdowns & more" }
         );
            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 12, "Production Calendar", "scheduling", "PRODUCTIONCALENDAR", "Planning" }
         );
            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 13, "Shooting scheduling", "shootingscheduling", "SHOOTINGSCHEDULING", "Planning" }
         );

            //

            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 14, "Call sheets", "callsheets", "CALLSHEETS", "Planning" }
         );
            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 15, "Cast", "cast", "CAST", " Department specific" }
         );
            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 16, "VFX, Stunts & more", "misc", "STUNTS", " Department specific" }
         );

            migrationBuilder.InsertData(
             table: "MenuActivity",
             columns: new[] { "Id", "Name", "Url", "Icon", "Type" },
             values: new object[] { 17, "Locations & Sets", "locations", "LOCATIONSSETS", " Department specific" }
         );


            migrationBuilder.Sql($"update MenuActivity set Type='General' , Icon = 'FILESDOCUMENTS' where id=2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "MenuActivity");
        }
    }
}
