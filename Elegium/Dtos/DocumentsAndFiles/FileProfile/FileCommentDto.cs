using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class FileCommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string MarkupText { get; set; }
        public int DocumentFileId { get; set; }
        public List<MentionDto> MentionUsers { get; set; }
        public string UserId { get; set; }
        public string Created { get; set; }
        public string UserName { get; set; }
    }
}
