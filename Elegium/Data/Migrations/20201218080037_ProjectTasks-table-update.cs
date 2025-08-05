using Microsoft.EntityFrameworkCore.Migrations;

namespace Elegium.Data.Migrations
{
    public partial class ProjectTaskstableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SceneId",
                table: "ProjectTasks",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "cameras",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        ProjectId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cameras", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_cameras_Project_ProjectId",
            //            column: x => x.ProjectId,
            //            principalTable: "Project",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Others",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        ProjectId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Others", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Others_Project_ProjectId",
            //            column: x => x.ProjectId,
            //            principalTable: "Project",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Sounds",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        ProjectId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Sounds", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Sounds_Project_ProjectId",
            //            column: x => x.ProjectId,
            //            principalTable: "Project",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "specialEffects",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        ProjectId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_specialEffects", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_specialEffects_Project_ProjectId",
            //            column: x => x.ProjectId,
            //            principalTable: "Project",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "stunts",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        ProjectId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_stunts", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_stunts_Project_ProjectId",
            //            column: x => x.ProjectId,
            //            principalTable: "Project",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "visualEffects",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        ProjectId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_visualEffects", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_visualEffects_Project_ProjectId",
            //            column: x => x.ProjectId,
            //            principalTable: "Project",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "sceneCameras",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SceneId = table.Column<int>(nullable: false),
            //        CameraId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_sceneCameras", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_sceneCameras_cameras_CameraId",
            //            column: x => x.CameraId,
            //            principalTable: "cameras",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_sceneCameras_Scenes_SceneId",
            //            column: x => x.SceneId,
            //            principalTable: "Scenes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "sceneOthers",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SceneId = table.Column<int>(nullable: false),
            //        OtherId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_sceneOthers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_sceneOthers_Others_OtherId",
            //            column: x => x.OtherId,
            //            principalTable: "Others",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_sceneOthers_Scenes_SceneId",
            //            column: x => x.SceneId,
            //            principalTable: "Scenes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "sceneSounds",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SceneId = table.Column<int>(nullable: false),
            //        SoundId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_sceneSounds", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_sceneSounds_Scenes_SceneId",
            //            column: x => x.SceneId,
            //            principalTable: "Scenes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_sceneSounds_Sounds_SoundId",
            //            column: x => x.SoundId,
            //            principalTable: "Sounds",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "sceneSpecials",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SceneId = table.Column<int>(nullable: false),
            //        SpecialId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_sceneSpecials", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_sceneSpecials_Scenes_SceneId",
            //            column: x => x.SceneId,
            //            principalTable: "Scenes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_sceneSpecials_specialEffects_SpecialId",
            //            column: x => x.SpecialId,
            //            principalTable: "specialEffects",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "sceneStunts",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SceneId = table.Column<int>(nullable: false),
            //        StuntId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_sceneStunts", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_sceneStunts_Scenes_SceneId",
            //            column: x => x.SceneId,
            //            principalTable: "Scenes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_sceneStunts_stunts_StuntId",
            //            column: x => x.StuntId,
            //            principalTable: "stunts",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "sceneVisuals",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SceneId = table.Column<int>(nullable: false),
            //        VisualId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_sceneVisuals", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_sceneVisuals_Scenes_SceneId",
            //            column: x => x.SceneId,
            //            principalTable: "Scenes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_sceneVisuals_visualEffects_VisualId",
            //            column: x => x.VisualId,
            //            principalTable: "visualEffects",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_SceneId",
                table: "ProjectTasks",
                column: "SceneId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_SceneId",
                table: "DocumentFiles",
                column: "SceneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cameras_ProjectId",
            //    table: "cameras",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Others_ProjectId",
            //    table: "Others",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneCameras_CameraId",
            //    table: "sceneCameras",
            //    column: "CameraId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneCameras_SceneId",
            //    table: "sceneCameras",
            //    column: "SceneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneOthers_OtherId",
            //    table: "sceneOthers",
            //    column: "OtherId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneOthers_SceneId",
            //    table: "sceneOthers",
            //    column: "SceneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneSounds_SceneId",
            //    table: "sceneSounds",
            //    column: "SceneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneSounds_SoundId",
            //    table: "sceneSounds",
            //    column: "SoundId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneSpecials_SceneId",
            //    table: "sceneSpecials",
            //    column: "SceneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneSpecials_SpecialId",
            //    table: "sceneSpecials",
            //    column: "SpecialId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneStunts_SceneId",
            //    table: "sceneStunts",
            //    column: "SceneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneStunts_StuntId",
            //    table: "sceneStunts",
            //    column: "StuntId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneVisuals_SceneId",
            //    table: "sceneVisuals",
            //    column: "SceneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_sceneVisuals_VisualId",
            //    table: "sceneVisuals",
            //    column: "VisualId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Sounds_ProjectId",
            //    table: "Sounds",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_specialEffects_ProjectId",
            //    table: "specialEffects",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_stunts_ProjectId",
            //    table: "stunts",
            //    column: "ProjectId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_visualEffects_ProjectId",
            //    table: "visualEffects",
            //    column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentFiles_Scenes_SceneId",
                table: "DocumentFiles",
                column: "SceneId",
                principalTable: "Scenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Scenes_SceneId",
                table: "ProjectTasks",
                column: "SceneId",
                principalTable: "Scenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentFiles_Scenes_SceneId",
                table: "DocumentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Scenes_SceneId",
                table: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "sceneCameras");

            migrationBuilder.DropTable(
                name: "sceneOthers");

            migrationBuilder.DropTable(
                name: "sceneSounds");

            migrationBuilder.DropTable(
                name: "sceneSpecials");

            migrationBuilder.DropTable(
                name: "sceneStunts");

            migrationBuilder.DropTable(
                name: "sceneVisuals");

            migrationBuilder.DropTable(
                name: "cameras");

            migrationBuilder.DropTable(
                name: "Others");

            migrationBuilder.DropTable(
                name: "Sounds");

            migrationBuilder.DropTable(
                name: "specialEffects");

            migrationBuilder.DropTable(
                name: "stunts");

            migrationBuilder.DropTable(
                name: "visualEffects");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_SceneId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_DocumentFiles_SceneId",
                table: "DocumentFiles");

            migrationBuilder.DropColumn(
                name: "SceneId",
                table: "ProjectTasks");
        }
    }
}
