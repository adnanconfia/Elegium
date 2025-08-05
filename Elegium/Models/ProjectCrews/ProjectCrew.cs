using Elegium.Models.Professionals;
using Elegium.Models.Projects;
using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Dtos;

namespace Elegium.Models.ProjectCrews
{
    public class ProjectCrewListModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string PositionName { get; set; }
        public string UserId { get; set; }
        public bool IsExternalUser { get; set; }
        public bool? Online { get; set; } = false;
        public ExternalUserFile defaultImageId { get; set; }
        public ProjectCrew Crew { get; set; }
        public ProjectExternalUser externalUser { get; set; }
        public List<string> CrewPostions { get; set; }
        public UserProfileDto CrewUserProfile { get; set; }
    }
    public class ProjectCrewModel
    {
        public ProjectCrew ProjectCrew { get; set; }
        public UserProfile UserProfile { get; set; }

        public int[] ProjectCrewPositions { get; set; }
        public int[] ProjectCrewUnits { get; set; }
    }
    public class ProjectCrew
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        //public UserProfile UserProfile { get; set; }
        //public int UserProfileId { get; set; }
        public DateTime? HiringDate { get; set; } = DateTime.UtcNow;
        public DateTime? SeperationDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFromDiscovery { get; set; } //either hired from discovery or not
        public ProfessionalHireRequest? ProfessionalHireRequest { get; set; }
        public int? ProfessionalHireRequestId { get; set; }

        //public string Email { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public virtual WorkingPosition Position { get; set; }
        //public int? PositionId { get; set; }
        public string PersonalMessage { get; set; }
        public bool HideMember { get; set; }
        //public Unit Unit{ get; set; }
        //public int? UnitId { get; set; }
        public DateTime? ContainsLOI { get; set; }
        public DateTime? ContainsDealMemo { get; set; }
        public DateTime? ContractCreated { get; set; }
        public DateTime? ContractSent { get; set; }
        public DateTime? ContractSigned { get; set; }
        public string Message { get; set; }

        public string CrewRights { get; set; }
        public string AnnouncementRights { get; set; }
        public string AddressBookRights { get; set; }
        public string CompanyCalendarRights { get; set; }
        public string ProductionCalendarRights { get; set; }
        public string ActorDatabaseRights { get; set; }
        public string LocationDatabaseRights { get; set; }
        public string CostumeDatabaseRights { get; set; }
        public string ProbItemDatabaseRights { get; set; }
        public string CreateDeleteProjectsRights { get; set; }
        public string SubscriptionsRights { get; set; }
        public string SettingsRights { get; set; }

    }
}
