using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class MediaFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string FileId { get; set; }
        public string Type { get; set; } //P-> Photos, V-> Videos, A -> Audios
        public string ContentType { get; set; }
        public int Size { get; set; }
        public Album Album { get; set; }
        public int? AlbumId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public long CreatedAtTicks { get; set; } = DateTime.UtcNow.Ticks;
        public bool? AccessRight { get; set; } = false;
        public bool? Favorite { get; set; } = false;
    }
}
