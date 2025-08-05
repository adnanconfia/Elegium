using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Shots
{
    public class ShotDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public int ShotId { get; set; }
        [ForeignKey("ShotId")]
        public virtual Shot Shot { get; set; }
    }
}
