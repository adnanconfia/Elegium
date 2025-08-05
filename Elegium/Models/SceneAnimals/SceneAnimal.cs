using Elegium.Models.Animals;
using Elegium.Models.ScenesandScript;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.SceneAnimals
{
    public class SceneAnimal
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        [ForeignKey("SceneId")]
        public virtual Scene Scene { get; set; }
        public int AnimalId { get; set; }
        [ForeignKey("AnimalId")]
        public virtual Animal Animal{ get; set; }
    }
}
