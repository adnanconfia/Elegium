using Elegium.Models.ProjectCrews;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Shots
{
    public class Shot
    {
        public int Id { get; set; }
        public string Index { get; set; }
        public string Subject  { get; set; }
        public string Visual  { get; set; }
        public string Audio  { get; set; }
        public string Sound  { get; set; }
        public string Color  { get; set; }
        public string Lighting  { get; set; }
        public string Position  { get; set; }
        public int Schedule_hh{ get; set; }
        public int Schedule_mm { get; set; }
        public string Size { get; set; }
        public bool isDeleted { get; set; }
        public int? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual ProjectUnit ProjectUnit { get; set; }
         public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        

    }
}
