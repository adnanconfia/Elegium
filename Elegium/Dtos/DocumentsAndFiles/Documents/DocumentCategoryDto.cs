using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class DocumentCategoryDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int Id { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public int ProjectId { get; set; }
        public int DocumentId { get; set; }
        public DocumentFiles FileDto { get; set; }
        public int FileCount { get; set; }
        public bool HasFile { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
        public string FileId { get; set; }
        //public  MyProperty { get; set; }
    }
}
