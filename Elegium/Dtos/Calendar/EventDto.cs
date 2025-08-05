using Elegium.Dtos.ProjectDtos;
using Elegium.Models.Calendar;
using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.Calendar
{
    public class EventDto
    {
        public EventDto()
        {
            AssignedTo = new List<MentionDto>();
            AdditionalViewers = new List<MentionDto>();
        }
        public List<MentionDto> AssignedTo { get; set; }
        public List<MentionDto> AdditionalViewers { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }
        public bool AllDay { get; set; }
        public CalenderCategory CalenderCategory { get; set; }
        public int? CalenderCategoryId { get; set; }
        public DateTime Created { get; set; }
        public bool Deleted { get; set; }
    }
}
