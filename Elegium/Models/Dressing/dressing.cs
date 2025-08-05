using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Dressing
{
    public class dressing
    {
        public int Id { get; set; }
        public int? Index { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
    }
}
