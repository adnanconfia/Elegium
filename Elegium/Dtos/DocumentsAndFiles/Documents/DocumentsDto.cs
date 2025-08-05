using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class DocumentsDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int Id { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
    }
}
