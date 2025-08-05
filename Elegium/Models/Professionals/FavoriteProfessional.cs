using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Professionals
{
    public class FavoriteProfessional
    {
        public int Id { get; set; }
        public ApplicationUser Professional { get; set; }
        public string ProfessionalId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}
