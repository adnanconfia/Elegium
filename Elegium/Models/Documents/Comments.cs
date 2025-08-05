using Elegium.Models.Actor;
using Elegium.Models.Calendar;
using Elegium.Models.Characters;
using Elegium.Models.Extras;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class Comment
    {
        #region generic comment fields
        public int Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string Text { get; set; }
        public string MarkupText { get; set; }

        #endregion

        #region referencing models
        public virtual DocumentCategory DocumentCategory { get; set; }
        public int? DocumentCategoryId { get; set; }
        public virtual ProjectTask ProjectTask { get; set; }
        public int? ProjectTaskId { get; set; }
        public virtual DocumentFiles DocumentFile { get; set; }
        public int? DocumentFileId { get; set; }

        //for announcements
        public Announcement Announcement { get; set; }
        public int? AnnouncementId { get; set; }
        public Event Event { get; set; }
        public int? EventId { get; set; }
        public Scene scene { get; set; }
        public int? SceneId { get; set; }
        public Character character { get; set; }
        public int? CharId { get; set; }
        public Extra Extra { get; set; }
        public int? ExtraId { get; set; }
        public Actors Actor { get; set; }
        public int? ActorId { get; set; }
        public Talents talents { get; set; }
        public int? TalentId { get; set; }

        #endregion
    }
}
