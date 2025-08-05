using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class AnnouncementDto
    {
        public AnnouncementDto()
        {
            AssignedTo = new List<MentionDto>();
        }
        public List<MentionDto> AssignedTo { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public bool HasDeadline { get; set; }
        public string Deadline { get; set; }
        public int Id { get; set; }
        public bool Completed { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public bool PinTop { get; set; }
        public string Created { get; set; }
        public string ProjectName { get; set; }
        public string ProjectColor { get; set; }
        public bool IncludeExternal { get; set; } = true;
    }
}
