using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneVehicle
{
    public class SceneVehicle
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle.Vehicle Vehicle { get; set; }
    }
}
