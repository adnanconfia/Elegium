using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Professionals
{
    public class ProfessionalHireRequestMediaFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileId { get; set; }
        public string Type { get; set; } //P-> Photos, F-> File
        public string ContentType { get; set; }
        public int Size { get; set; }
        public ProfessionalHireRequest ProfessionalHireRequest { get; set; }
        public int? ProfessionalHireRequestId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public long CreatedAtTicks { get; set; } = DateTime.UtcNow.Ticks;
    }
}
