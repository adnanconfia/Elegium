using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class FileThumbnail
    {
        public int Id { get; set; }
        public string FileId { get; set; }
        public byte[] Thumbnail { get; set; }

        [NotMapped]
        public string FileArray { get; set; }
    }
}
