using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string MarkupText { get; set; }
        public int? DocumentCategoryId { get; set; }
        public List<MentionDto> MentionUsers { get; set; }
        public string UserId { get; set; }
        public string Created { get; set; }
        public string UserName { get; set; }
        public int? DocumentFileId { get; set; }
        public int? ProjectTaskId { get; set; }
        public int? ProjectId { get; set; }
        public int? AnnouncementId { get; set; }
        public int? EventId { get; set; }
        public int? SceneId { get; set; }
        public int? CharId { get; set; }
        public int? ExtraId { get; set; }
        public int? ActorId { get; set; }
        public int? TalentId { get; set; }
    }
}
