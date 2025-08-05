using Elegium.Dtos.ShotDto;
using Elegium.Models.Animals;
using Elegium.Models.Cameras;
using Elegium.Models.Characters;
using Elegium.Models.Construction;
using Elegium.Models.Costumes;
using Elegium.Models.Dressing;
using Elegium.Models.Extras;
using Elegium.Models.Graphics;
using Elegium.Models.Others;
using Elegium.Models.SceneCostumes;
using Elegium.Models.Sounds;
using Elegium.Models.SpecialEffects;
using Elegium.Models.Stunts;
using Elegium.Models.Vehicle;
using Elegium.Models.VisualEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.ScenesandScript
{
    public class Allscenes
    {
        public List<string> ExtraId { get; set; }
        public Nullable<int> Id { get; set; }
        public string Set_name { get; set; }
        public string Env_name { get; set; }
        public List<string> CharacterId { get; set; }
        public Scene scene { get; set; }
        public List<Character> character { get; set; }
        public List<Extra> extra { get; set; }
        public List<construction> construction { get; set; }
        public List<dressing> dressings { get; set; }
        public List<Props.Props> Prop { get; set; }
        public List<Graphic> graphics { get; set; }
        public List<Vehicle.Vehicle> vehicles { get; set; }
        public List<Animal> animals { get; set; }
        public List<VisualEffect> visualEffects { get; set; }
        public List<SpecialEffect> specialEffects { get; set; }
        public List<Sound> sound { get; set; }
        public List<Camera> cameras { get; set; }
        public List<Stunt> stunts { get; set; }
        public List<other> others { get; set; }
        public List<SceneCostumes.SceneCostumes> costumes { get; set; }
        public List<SceneMakeups.SceneMakeup> makeups { get; set; }
        public List<ShotDto> Shots { get; set; }


    }
}
