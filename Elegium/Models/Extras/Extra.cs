using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Extras
{
    public class Extra
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Index { get; set; }
        public int Project_Id { get; set; }
        public bool? GroupOfCharacters { get; set; } = false;
        public bool? Sugggestion { get; set; } = false;
        public bool? marked { get; set; } = false;
        public string Description { get; set; }
    }
}
