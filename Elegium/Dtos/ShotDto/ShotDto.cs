using Elegium.Models.Shots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.ShotDto
{
    public class ShotDto
    {
        public int Id { get; set; }
        
        public string Index { get; set; }
        public string Subject { get; set; }
        public string Visual { get; set; }
        public string Audio { get; set; }
        public string Sound { get; set; }
        public string Color { get; set; }
        public string Lighting { get; set; }
        public string Position { get; set; }
        public int Schedule_hh { get; set; }
        public bool isDeleted { get; set; }
        public int Schedule_mm { get; set; }
        public int? UnitId { get; set; }
        public  ShotDetailDto? Size { get; set; }
        public List<ShotDetailDto> Type { get; set; }
        public List<ShotDetailDto> Equipment { get; set; }
        public ShotDetailDto? Movement { get; set; }
        public List<ShotDetailDto> Vfx { get; set; }
        public List<ShotDetailDto> Camera { get; set; }
        public List<ShotDetailDto> Lens { get; set; }
        public ShotDetailDto? Fps { get; set; }

        public List<ShotDetailDto> SpecialEquipment { get; set; }
        public int SceneId { get; set; }
       
    }
}
