using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class FileTaskDto
    {
        public List<MentionDto> AssignedTo { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public bool HasDeadline { get; set; }
        public int DocumentFilesId { get; set; }
        public string Deadline { get; set; }
        public int Id { get; set; }
        public bool Completed { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string ClassName { get; set; }
    }
}
