using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Construction
{
    public class construction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Index { get; set; }
        public int ProjectId { get; set; }
    
    }
}
