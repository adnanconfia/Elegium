using Elegium.Models.Actor;
using Elegium.Models.Calendar;
using Elegium.Models.Characters;
using Elegium.Models.Extras;
using Elegium.Models.ScenesandScript;
using Elegium.Models.Shots;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class DocumentFiles
    {
        #region Generic file fields
        public string ContentType { get; set; }
        public string FileId { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string Type { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string UserFriendlySize { get; set; }
        public long CreateAtTicks { get; set; } = DateTime.UtcNow.Ticks;
        public bool Default { get; set; }
        #endregion


        #region Referencing models

        //for document category in document and files.
        public DocumentCategory DocumentCategory { get; set; }
        public int? DocumentCategoryId { get; set; }

        //for project tasks
        public ProjectTask ProjectTask { get; set; }
        public int? ProjectTaskId { get; set; }

        //for announcements
        public Announcement Announcement { get; set; }
        public int? AnnouncementId { get; set; }

       
        public int? SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; } 
        public int? ShotId { get; set; }
        [ForeignKey("ShotId")]
        public virtual Shot Shot { get; set; }    
        public int? AgencyID { get; set; }
        [ForeignKey("AgencyID")]
        public virtual Agency.Agency Agency { get; set; }
        public int? TalentId { get; set; }
        [ForeignKey("TalentId")]
        public virtual Talents Talents { get; set; }
        public int? ActorId { get; set; }
        [ForeignKey("ActorId")]
        public virtual Actors Actors { get; set; }   
        public int? CharId { get; set; }
        [ForeignKey("CharId")]
        public virtual Character Character { get; set; } 
        public int? ExtraId { get; set; }
        [ForeignKey("ExtraId")]
        public virtual Extra Extra { get; set; }
        public Event Event { get; set; }
        public int? EventId { get; set; }

        public virtual ProjectResource ProjectResource { get; set; }
        public int? ProjectResourceId { get; set; }
        #endregion
    }
}
