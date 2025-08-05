using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Environment
{
    public class Environment
    {
        public int Id { get; set; }
      
        public string Evironment_Name { get; set; }

    }
}
