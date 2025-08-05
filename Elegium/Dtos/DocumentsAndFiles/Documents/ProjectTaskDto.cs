using Elegium.Models;
using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class ProjectTaskDto
    {
        public List<MentionDto> AssignedTo { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public bool HasDeadline { get; set; }
        public int? DocumentCategoryId { get; set; }
        public string Deadline { get; set; }
        public int Id { get; set; }
        public bool Completed { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string ClassName { get; set; }
        public int? DocumentFilesId { get; set; }
        public List<ProjectTaskDto> SubTasks { get; set; }
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
        public ProjectTask ParentTask { get; set; }
        public string TaskDeadlineDay { get; set; }
        public string TaskMonth { get; set; }
        public int? ParentTaskId { get; set; }
        public string Section { get; set; }
        public bool HasSection { get; set; }
        public string SectionUrl { get; set; }
        public DocumentCategory DocumentCategory { get; set; }
        public int? EventId { get; set; }
        public int? SceneId { get; set; }
        public int? CharId { get; set; }
        public int? ExtraId { get; set; }
        public int? TalentId { get; set; }
        public int? AgencyId { get; set; }
        public int? ActorID { get; set; }
    }
}
