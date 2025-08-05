using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Set
{
    public class Set
    {
        public int Id { get; set; }

        public string Set_name { get; set; }
        public int? ProjectId { get; set; }
   
    }
}
