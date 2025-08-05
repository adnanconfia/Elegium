using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Graphics
{
    public class Graphic
    {
        public int Id { get; set; }
        public int? Index { get; set; }
        public string Name { get; set; }

        public int? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
    }
}
