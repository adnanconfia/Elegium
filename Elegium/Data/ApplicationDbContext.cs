using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Elegium.Configuration;
using Elegium.Dtos.Chat;
using Elegium.Models;
using Elegium.Models.Characters;
using Elegium.Models.Chat;
using Elegium.Models.Extras;
using Elegium.Models.FundingAndFP;
using Elegium.Models.Notifications;
using Elegium.Models.Professionals;
using Elegium.Models.ProjectCrews;
using Elegium.Models.Projects;
using Elegium.Models.Resources;
using Elegium.Models.SceneCharacters;
using Elegium.Models.ScenesandScript;
using Elegium.Models.ScenesExtra;
using Elegium.Models.Environment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using Elegium.Models.Set;
using Elegium.Models.Overview;
using Elegium.Models.Calendar;
using Elegium.Models.Costumes;
using Elegium.Models.SceneCostumes;
using Elegium.Models.Makeup;
using Elegium.Models.Construction;
using Elegium.Models.SceneConstruction;
using Elegium.Models.Dressing;
using Elegium.Models.SceneDressings;
using Elegium.Models.Props;
using Elegium.Models.SceneProps;
using Elegium.Models.Graphics;
using Elegium.Models.SceneGraphics;
using Elegium.Models.Vehicle;
using Elegium.Models.SceneVehicle;
using Elegium.Models.Animals;
using Elegium.Models.SceneAnimals;
using Elegium.Models.VisualEffects;
using Elegium.Models.SceneVisuals;
using Elegium.Models.SpecialEffects;
using Elegium.Models.SceneSpecials;
using Elegium.Models.Sounds;
using Elegium.Models.SceneSounds;
using Elegium.Models.Cameras;
using Elegium.Models.SceneCameras;
using Elegium.Models.Stunts;
using Elegium.Models.SceneStunts;
using Elegium.Models.Others;
using Elegium.Models.SceneOthers;
using Elegium.Models.SceneMakeups;
using Elegium.Models.Voting;
using Elegium.Models.Shots;
using Elegium.Models.Agency;
using Elegium.Models.Actor;
using Elegium.Models.CharacterTalent;
using Elegium.Models.Links;
using Elegium.Models.OffPeriods;

namespace Elegium.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    if(relationship.PrincipalEntityType.Name == "Elegium.Models.ApplicationUser")
            //        relationship.DeleteBehavior = DeleteBehavior.Cascade;
            //}
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Connection>()
            //    .HasOne(a => a.User)
            //    .WithMany(a => a.Connections)
            //    .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Thread>()
                .HasOne(a => a.LastMessage)
                .WithOne(a => a.Thread)
                .HasForeignKey<Message>(a => a.ThreadId);

            modelBuilder.Entity<ConversationThreadsDto>().HasNoKey();


            modelBuilder.Entity<ProjectTask>()
                .HasOne(a => a.DocumentFiles)
                .WithMany()
                .HasForeignKey(a => a.DocumentFilesId);


            modelBuilder.Entity<DocumentFiles>()
                .HasOne(a => a.ProjectTask)
                .WithMany()
                .HasForeignKey(a => a.ProjectTaskId);

            modelBuilder.Entity<ProjectTask>()
                .HasMany(a => a.SubTasks)
                .WithOne(a => a.ParentTask)
                .HasForeignKey(a => a.ParentTaskId);
            modelBuilder.Entity<Models.Environment.Environment>().HasData(new Models.Environment.Environment
            {
                Id = 1,
                Evironment_Name = "DAY-INT"
            },
            new Models.Environment.Environment
            {
                Id = 2,
                Evironment_Name = "DAY-EXT"
            },
            new Models.Environment.Environment
            {
                Id = 3,
                Evironment_Name = "NIGHT-INT"
            }, new Models.Environment.Environment
            {
                Id = 4,
                Evironment_Name = "NIGHT-EXT"
            }

            );
            modelBuilder.Entity<Set>().HasData(new Set
            {
                Id = 1,
                Set_name = "Ships Ramses / Deck"
            },
           new Set
           {
               Id = 2,
               Set_name = "Ship Ramses / Foredeck"
           },
           new Set
           {
               Id = 3,
               Set_name = "Ship Ramses / Dining Hall"
           }, new Set
           {
               Id = 4,
               Set_name = "Ship Ramses / Fahra’s Cabin"
           }

           );


            //modelBuilder.Entity<Message>()
            //    .HasOne(a => a.Sender)
            //    .WithMany(a => a.Messages)
            //    .HasForeignKey(a => a.SenderId);
        }
        public DbSet<offperiod> offperiods { get; set; }
        public DbSet<link> link { get; set; }
        public DbSet<CharactersTalent> CharactersTalents { get; set; }
        public DbSet<Actors> Actors { get; set; }
        public DbSet<Talents> Talents { get; set; }
        public DbSet<AgencyContact> AgencyContacts { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<ShotDetail> ShotDetails { get; set; }
        public DbSet<Shot> Shots { get; set; }
        public DbSet<SceneMakeup> sceneMakeups { get; set; }
        public DbSet<ProjectCrewPosition> ProjectCrewPositions { get; set; }
        public DbSet<SceneOther>  sceneOthers { get; set; }
        public DbSet<other>  Others { get; set; }
        public DbSet<SceneStunt>  sceneStunts { get; set; }
        public DbSet<Stunt>  stunts { get; set; }
        public DbSet<SceneCamera>  sceneCameras { get; set; }
        public DbSet<Camera>  cameras { get; set; }
        public DbSet<SceneSound>  sceneSounds { get; set; }
        public DbSet<Sound>  Sounds { get; set; }
        public DbSet<SceneSpecial>  sceneSpecials { get; set; }
        public DbSet<SpecialEffect>  specialEffects { get; set; }
        public DbSet<SceneVisual>  sceneVisuals { get; set; }
        public DbSet<VisualEffect>  visualEffects { get; set; }
        public DbSet<SceneAnimal>  sceneAnimals { get; set; }
        public DbSet<Animal>  Animals { get; set; }
        public DbSet<SceneVehicle>  sceneVehicles { get; set; }
        public DbSet<Vehicle>  vehicles { get; set; }
        public DbSet<SceneDressing>  sceneDressings { get; set; }
        public DbSet<SceneGraphic>  sceneGraphics { get; set; }
        public DbSet<Graphic>  Graphics { get; set; }
        public DbSet<Props>  Props { get; set; }
        public DbSet<SceneProps>  sceneProps { get; set; }
        public DbSet<dressing>  dressings { get; set; }
        public DbSet<Costume> Costumes { get; set; }
        public DbSet<SceneConstruction> sceneConstructions { get; set; }
        public DbSet<construction> Constructions { get; set; }
        public DbSet<Makeup> Makeups { get; set; }
        public DbSet<SceneCostumes> SceneCostumes { get; set; }
        public DbSet<Set> sets { get; set; }
        public DbSet<Models.Environment.Environment> environments { get; set; }
        public DbSet<SceneCharacter> sceneCharacters { get; set; }
        public DbSet<ScenesExtra> scenesExtras { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<Character> characters { get; set; }
        public DbSet<Extra> Extras { get; set; }
        public DbSet<ProjectUserGroup> ProjectUserGroups { get; set; }
        public DbSet<ProjectUnit> ProjectUnits { get; set; }
        public DbSet<ProjectCrewUnit> CrewUnits { get; set; }
        public DbSet<ExternalUserUnit> ExternalUserUnits { get; set; }
        public DbSet<ProjectExternalUser> ProjectExternalUsers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LanguageLevel> LanguageLevels { get; set; }
        public DbSet<WorkingPosition> WorkingPositions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<PromotionCategory> PromotionCategory { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<SkillLevel> SkillLevel { get; set; }
        public DbSet<EquipmentCategoryType> EquipmentCategoryType { get; set; }
        public DbSet<EquipmentCategory> EquipmentCategory { get; set; }
        public DbSet<UserEquipment> UserEquipment { get; set; }
        public DbSet<UserCredit> UserCredit { get; set; }
        public DbSet<Album> Album { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<UserPromotionCategory> UserPromotionCategory { get; set; }
        public DbSet<UserOtherLanguages> UserOtherLanguages { get; set; }
        public DbSet<UserAdditionalSkills> UserAdditionalSkills { get; set; }
        public DbSet<UserMessages> UserMessages { get; set; }
        public DbSet<ProductionType> ProductionType { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Models.Projects.Project> Project { get; set; }
        public DbSet<SavedProject> SavedProjects { get; set; }
        public DbSet<FavoriteProject> FavoriteProjects { get; set; }
        public DbSet<ProjectPartnerRequirement> ProjectPartnerRequirement { get; set; }
        public DbSet<ProjectPartner> ProjectPartner { get; set; }
        public DbSet<ProjectManagementPhases> ProjectManagementPhase { get; set; }

        public DbSet<VisibilityAreas> VisibilityAreas { get; set; }
        public DbSet<FundersRequired> FundersRequired { get; set; }
        public DbSet<ProjectFunding> ProjectFunding { get; set; }
        public DbSet<ProjectFundingManagementPhase> ProjectFundingManagementPhase { get; set; }
        public DbSet<ProjectFinancialParticipation> ProjectFinancialParticipation { get; set; }
        public DbSet<ProjectFinancialParticipationManagementPhase> ProjectFinancialParticipationManagementPhase { get; set; }
        public DbSet<ProjectPartnerRequirementManagementPhase> ProjectPartnerRequirementManagementPhase { get; set; }
        public DbSet<ProjectVisibilityArea> ProjectVisibilityArea { get; set; }
        public DbSet<PaymentGateway> PaymentGateway { get; set; }
        public DbSet<UserFundingAndFP> UserFundingAndFP { get; set; }
        public DbSet<Resource> Resource { get; set; }
        public DbSet<SavedResource> SavedResource { get; set; }
        public DbSet<FavoriteResource> FavoriteResource { get; set; }
        public DbSet<ResourceCondition> ResourceCondition { get; set; }
        public DbSet<ResourceMediaFile> ResourceMediaFile { get; set; }
        public DbSet<Elegium.Models.Chat.Message> Message { get; set; }
        public DbSet<Thread> Thread { get; set; }
        public DbSet<ThreadUsers> ThreadUsers { get; set; }
        public DbSet<ThreadReadState> ThreadReadState { get; set; }
        public DbSet<Connection> Connection { get; set; }
        public DbSet<UserFollowing> UserFollowing { get; set; }
        public DbSet<ConversationThreadsDto> ConversationThreadsDto { get; set; }
        public DbSet<MessageFiles> MessageFiles { get; set; }
        public DbSet<ChatBoxes> ChatBoxes { get; set; }
        public DbSet<NotificationType> NotificationType { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<ProfessionalHireRequest> ProfessionalHireRequests { get; set; }
        public DbSet<ProjectCrew> ProjectCrews { get; set; }
        public DbSet<ProjectDispute> ProjectDisputes { get; set; }
        public DbSet<ProjectDisputeDetail> ProjectDisputeDetails { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<DocumentCategory> DocumentCategory { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectTaskAssignedTo> ProjectTaskAssignedTo { get; set; }
        public DbSet<DocumentFiles> DocumentFiles { get; set; }
        public DbSet<FavoriteProfessional> FavoriteProfessionals { get; set; }
        public DbSet<SavedProfessional> SavedProfessionals { get; set; }
        public DbSet<FavoriteProjectPartner> FavoriteProjectPartners { get; set; }
        public DbSet<SavedProjectPartner> SavedProjectPartners { get; set; }
        public DbSet<FavoriteFundingAndFP> FavoriteFundingAndFPs { get; set; }
        public DbSet<SavedFundingAndFP> SavedFundingAndFPs { get; set; }
        public DbSet<MenuActivity> MenuActivity { get; set; }
        public DbSet<UserProjectMenu> UserProjectMenu { get; set; }

        public DbSet<FileComment> FileComment { get; set; }
        public DbSet<FileTask> FileTask { get; set; }
        public DbSet<FileTaskAssignedTo> FileTaskAssignedTo { get; set; }
        public DbSet<FileLink> FileLink { get; set; }
        public DbSet<VersionFiles> VersionFiles { get; set; }
        public DbSet<ProfessionalHireRequestMediaFile> ProfessionalHireRequestMediaFiles { get; set; }
        public DbSet<ProjectCrewGroup> ProjectCrewGroups { get; set; }//
        public DbSet<ExternalUserGroup> ExternalUserGroups { get; set; }//
        public DbSet<DraftContractFile> DraftContractFiles { get; set; }//
        public DbSet<Department> Departments { get; set; }//
        public DbSet<ExternalUserContact> ExternalUserContact { get; set; }//
        public DbSet<ExternalUserContractFile> ExternalUserContractFile { get; set; }//
        public DbSet<ExternalUserDraftFile> ExternalUserDraftFile { get; set; }//
        public DbSet<ExternalUserFile> ExternalUserFile { get; set; }//

        public DbSet<ContractDocument> ContractDocuments { get; set; }

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementsAssignedTo> AnnouncementsAssignedTo { get; set; }
        public DbSet<DashboardPanel> DashboardPanels { get; set; }
        public DbSet<DashboardSelectedPanel> DashboardSelectedPanels { get; set; }
        public DbSet<ProjectDashboardPanel> ProjectDashboardPanels { get; set; }
        public DbSet<ProjectDashboardSelectedPanel> ProjectDashboardSelectedPanels { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventsAssignedTo> EventsAssignedTo { get; set; }
        public DbSet<EventsAdditionalViewer> EventsAdditionalViewers { get; set; }
        public DbSet<CalenderCategory> CalenderCategories { get; set; }
        public DbSet<FileThumbnail> FileThumbnails { get; set; }//ProjectResource
        public DbSet<ProjectResource> ProjectResources { get; set; }//
        public DbSet<FundingFPRequests> FundingFPRequests { get; set; }//ProjectPartnerRequest
        public DbSet<ProjectPartnerRequest> ProjectPartnerRequests { get; set; }
        public DbSet<VotingSetting> VotingSettings { get; set; }
        public DbSet<Nomination> Nominations { get; set; }
        public DbSet<NominationDetail> NominationDetails { get; set; }
        public DbSet<VotingParameter> VotingParameters { get; set; }
        public DbSet<NominationVote> NominationVotes { get; set; }
        public DbSet<NominationVoteDetail> NominationVoteDetails { get; set; }
        public DbSet<FinalVote> FinalVotes { get; set; }
        public DbSet<FinalVoteDetail> FinalVoteDetails { get; set; }


    }
}
