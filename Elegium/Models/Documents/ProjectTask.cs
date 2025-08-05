using Elegium.Interfaces;
using Elegium.Models.Actor;
using Elegium.Models.Calendar;
using Elegium.Models.Characters;
using Elegium.Models.Extras;
using Elegium.Models.Projects;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class ProjectTask : ITask
    {
        #region generic task fields
        public ProjectTask()
        {
            SubTasks = new HashSet<ProjectTask>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasDeadline { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public bool Completed { get; set; }
        public bool Deleted { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProjectTask> SubTasks { get; set; }
        public string Section { get; set; }
        public Project Project { get; set; }
        public int? ProjectId { get; set; }
        #endregion

        #region referencing models
        public virtual DocumentCategory DocumentCategory { get; set; }
        public int? DocumentCategoryId { get; set; }        
        public virtual ProjectTask ParentTask { get; set; }
        public int? ParentTaskId { get; set; }
        public DocumentFiles DocumentFiles { get; set; }
        public int? DocumentFilesId { get; set; }
        public Event Event { get; set; }
        public int? EventId { get; set; }

        public Scene scene { get; set; }
        public int? SceneId { get; set; }
        public Character Character { get; set; }
        public int? CharId { get; set; }
        public Extra extra { get; set; }
        public int? ExtraId { get; set; }
        public Actors Actor { get; set; }
        public int? ActorId { get; set; }
        public Talents Talent { get; set; }
        public int? TalentID { get; set; }
        public Agency.Agency Agency { get; set; }
        public int? AgencyId { get; set; }
        #endregion
    }
}
