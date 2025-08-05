using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Models.SceneCharacters;
using Elegium.Models.ScenesandScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Elegium.Models.ScenesExtra;
using Elegium.Models.Characters;
using Elegium.Models.Extras;
using Elegium.Models.Set;
using Elegium.Models.Costumes;
using Elegium.Dtos.CostumeDto;
using Elegium.Models.Makeup;
using Elegium.Models.Construction;
using Elegium.Models.SceneConstruction;
using Elegium.Models.SceneDressings;
using Elegium.Models.Dressing;
using Elegium.Models.SceneProps;
using Elegium.Models.Props;
using Elegium.Models.Graphics;
using Elegium.Models.SceneGraphics;
using Elegium.Models.Vehicle;
using Elegium.Models.SceneVehicle;
using Elegium.Models.Animals;
using Elegium.Models.SceneAnimals;
using Elegium.Models.SceneVisuals;
using Elegium.Models.VisualEffects;
using Elegium.Models.SpecialEffects;
using Elegium.Models.SceneSpecials;
using Elegium.Models.SceneSounds;
using Elegium.Models.Sounds;
using Elegium.Models.SceneCameras;
using Elegium.Models.Cameras;
using Elegium.Models.SceneStunts;
using Elegium.Models.Stunts;
using Elegium.Models.Others;
using Elegium.Models.SceneOthers;
using Elegium.Dtos.MakeupDto;
using Elegium.Models.SceneMakeups;
using Elegium.Models.SceneCostumes;
using Elegium.Dtos;
using Elegium.ExtensionMethods;
using Elegium.Models.Shots;
using Elegium.Dtos.ShotDto;
using Elegium.Models;
using Elegium.Dtos.AgencyDto;
using Elegium.Models.Agency;
using Elegium.Models.Actor;
using Elegium.Models.CharacterTalent;
using Elegium.Models.Links;
using Elegium.Models.OffPeriods;
using AutoMapper;
using System.Reflection;
using Newtonsoft.Json;
using Elegium.Dtos.charactersDto;
using Elegium.Dtos.ActorsDto;
using Elegium.Dtos.TalentsDto;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]

    public class ScenesandScriptController : ControllerBase
    {


        private readonly ApplicationDbContext _context;
        public ScenesandScriptController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetEnvironment(int projectId)
        {
            try
            {
                var env = _context.environments.ToList();
                return Ok(env);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSets(int projectId)
        {
            try
            {
                var set = _context.sets.Where(x => x.ProjectId == projectId).ToList();
                return Ok(set);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetScenes(int projectId)
        {

            try
            {

                var scenes = _context.Scenes.Select(x => new { ExtraId = "", CharacterId = "", scene = x, Id = x.Id }).Where(x => x.scene.project_id == projectId && x.scene.isDeleted == false).ToList();



                var Scenechlist = _context.Scenes.Select(x => new { ExtraId = "", CharacterId = "", scene = x, Id = x.Id }).Where(x => x.scene.project_id == projectId && x.scene.isDeleted == false).Join(_context.sceneCharacters, sch => sch.Id, ch => ch.Scene.Id, (sch, ch) => new { sch.ExtraId, sch.Id, ch.CharacterId, sch.scene }).ToList();
                var Sceneexlist = _context.Scenes.Select(x => new { ExtraId = "", CharacterId = "", scene = x, Id = x.Id }).Where(x => x.scene.project_id == projectId && x.scene.isDeleted == false).Join(_context.scenesExtras, sch => sch.Id, ch => ch.Scene.Id, (sch, ch) => new { ch.ExtraId, ch.Scene.Id, sch.CharacterId, ch.Scene }).ToList();
                var env = _context.Scenes.Select(x => new { ExtraId = "", CharacterId = "", scene = x, Id = x.Id }).Where(x => x.scene.project_id == projectId && x.scene.isDeleted == false).Join(_context.environments, sch => sch.scene.environment_id, ch => ch.Id, (sch, ch) => new { sch.ExtraId, sch.scene.Id, sch.CharacterId, sch.scene, ch.Evironment_Name }).ToList();
                var set = _context.Scenes.Select(x => new { ExtraId = "", CharacterId = "", scene = x, Id = x.Id }).Where(x => x.scene.project_id == projectId && x.scene.isDeleted == false).Join(_context.sets, sch => sch.scene.setId, ch => ch.Id, (sch, ch) => new { sch.ExtraId, sch.scene.Id, sch.CharacterId, sch.scene, ch.Set_name }).ToList();
                var character = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneCharacters, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Character, ch }).ToList();
                var extra = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.scenesExtras, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Extra, ch }).ToList();
                var constructions = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneConstructions, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Construction, ch }).ToList();
                var dressing = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneDressings, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Dressing, ch }).ToList();
                var _prop = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneProps, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Props, ch }).ToList();
                var _graphic = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneGraphics, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Graphics, ch }).ToList();
                var _vehicle = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneVehicles, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Vehicle, ch }).ToList();
                var _animal = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneAnimals, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Animal, ch }).ToList();
                var _visual = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneVisuals, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.VisualEffect, ch }).ToList();
                var _special = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneSpecials, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.SpecialEffect, ch }).ToList();
                var _sound = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneSounds, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Sounds, ch }).ToList();
                var _camera = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneCameras, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Camera, ch }).ToList();
                var _stunt = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneStunts, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Stunt, ch }).ToList();
                var _other = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneOthers, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Other, ch }).ToList();
                var _costumes = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.SceneCostumes, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Costumes, ch, ch.CostumeId, ch.ExtraId, ch.CharacterId, ch.SceneId, ch.ProjectId }).Where(x => x.CharacterId != null || x.ExtraId != null).ToList();
                var _makeup = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.sceneMakeups, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch.Makeup, ch, ch.MakeupId, ch.ExtraId, ch.CharacterId, ch.SceneId, ch.ProjectId }).Where(x => x.CharacterId != null || x.ExtraId != null).ToList();
                var _Shots = _context.Scenes.Where(x => x.project_id == projectId && x.isDeleted == false).Join(_context.Shots, sch => sch.Id, ch => ch.SceneId, (sch, ch) => new { ch, ch.SceneId }).Where(x => x.ch.isDeleted == false).ToList();

                List<Allscenes> allscenes = new List<Allscenes>();
                foreach (var _scene in scenes)
                {
                    Allscenes allscenes1 = new Allscenes();
                    allscenes1.Id = _scene.Id;
                    allscenes1.CharacterId = new List<string>();
                    allscenes1.ExtraId = new List<string>();
                    allscenes1.character = new List<Character>();
                    allscenes1.extra = new List<Extra>();
                    allscenes1.construction = new List<construction>();
                    allscenes1.dressings = new List<dressing>();
                    allscenes1.Prop = new List<Props>();
                    allscenes1.graphics = new List<Graphic>();
                    allscenes1.vehicles = new List<Vehicle>();
                    allscenes1.animals = new List<Animal>();
                    allscenes1.visualEffects = new List<VisualEffect>();
                    allscenes1.specialEffects = new List<SpecialEffect>();
                    allscenes1.sound = new List<Sound>();
                    allscenes1.cameras = new List<Camera>();
                    allscenes1.stunts = new List<Stunt>();
                    allscenes1.others = new List<other>();
                    allscenes1.costumes = new List<SceneCostumes>();
                    allscenes1.makeups = new List<SceneMakeup>();
                    allscenes1.Shots = new List<ShotDto>();
                    allscenes1.Set_name = "";
                    allscenes1.Env_name = "";

                    allscenes1.scene = _scene.scene;
                    allscenes.Add(allscenes1);
                }

                foreach (var _scene in Scenechlist)
                {

                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == _scene.Id)
                        {
                            string ch = _scene.CharacterId.ToString();
                            allscene.CharacterId.Add(ch);
                        }

                    }
                }
                foreach (var _scene in Sceneexlist)
                {
                    foreach (var allscene in allscenes)
                    {
                        if (allscene.Id == _scene.Id)
                        {
                            string ch = _scene.ExtraId.ToString();
                            allscene.ExtraId.Add(ch);
                        }
                    }
                }
                foreach (var e in env)
                {

                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == e.scene.Id)
                        {
                            allscene.Env_name = e.Evironment_Name;
                        }

                    }
                }
                foreach (var s in set)
                {

                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == s.scene.Id)
                        {
                            allscene.Set_name = s.Set_name;
                        }

                    }
                }
                foreach (var c in character)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == c.ch.SceneId)
                        {
                            allscene.character.Add(c.ch.Character);
                        }

                    }

                }
                foreach (var e in extra)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == e.ch.SceneId)
                        {
                            allscene.extra.Add(e.ch.Extra);
                        }

                    }

                }
                foreach (var c in constructions)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == c.ch.SceneId)
                        {
                            allscene.construction.Add(c.ch.Construction);
                        }

                    }

                }
                foreach (var d in dressing)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == d.ch.SceneId)
                        {
                            allscene.dressings.Add(d.ch.Dressing);
                        }

                    }

                }
                foreach (var p in _prop)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == p.ch.SceneId)
                        {
                            allscene.Prop.Add(p.ch.Props);
                        }

                    }

                }
                foreach (var g in _graphic)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == g.ch.SceneId)
                        {
                            allscene.graphics.Add(g.ch.Graphics);
                        }

                    }

                }
                foreach (var v in _vehicle)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == v.ch.SceneId)
                        {
                            allscene.vehicles.Add(v.ch.Vehicle);
                        }

                    }

                }
                foreach (var a in _animal)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == a.ch.SceneId)
                        {
                            allscene.animals.Add(new Animal { Id = a.Animal.Id, Name = a.Animal.Name });
                        }

                    }

                }
                foreach (var v in _visual)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == v.ch.SceneId)
                        {
                            allscene.visualEffects.Add(new VisualEffect { Id = v.VisualEffect.Id, Name = v.VisualEffect.Name });
                        }

                    }

                }
                foreach (var s in _special)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == s.ch.SceneId)
                        {
                            allscene.specialEffects.Add(new SpecialEffect { Id = s.SpecialEffect.Id, Name = s.SpecialEffect.Name });
                        }

                    }

                }
                foreach (var s in _sound)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == s.ch.SceneId)
                        {
                            allscene.sound.Add(new Sound { Id = s.Sounds.Id, Name = s.Sounds.Name });
                        }

                    }

                }
                foreach (var c in _camera)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == c.ch.SceneId)
                        {
                            allscene.cameras.Add(new Camera { Id = c.Camera.Id, Name = c.Camera.Name });
                        }

                    }

                }
                foreach (var s in _stunt)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == s.ch.SceneId)
                        {
                            allscene.stunts.Add(new Stunt { Id = s.Stunt.Id, Name = s.Stunt.Name });
                        }

                    }

                }
                foreach (var o in _other)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == o.ch.SceneId)
                        {
                            allscene.others.Add(new other { Id = o.Other.Id, Name = o.Other.Name });
                        }

                    }

                }
                foreach (var o in _costumes)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == o.ch.SceneId)
                        {

                            allscene.costumes.Add(new SceneCostumes { Id = o.ch.Id, Costumes = o.Costumes, CharacterId = o.CharacterId, ExtraId = o.ExtraId, ProjectId = o.ProjectId, SceneId = o.SceneId });
                        }

                    }

                }
                foreach (var o in _makeup)
                {
                    foreach (var allscene in allscenes)
                    {


                        if (allscene.Id == o.ch.SceneId)
                        {

                            allscene.makeups.Add(new SceneMakeup { Id = o.ch.Id, Makeup = o.Makeup, CharacterId = o.CharacterId, ExtraId = o.ExtraId, ProjectId = o.ProjectId, SceneId = o.SceneId });
                        }

                    }

                }

                foreach (var s in _Shots)
                {


                    foreach (var allscene in allscenes)
                    {
                        if (allscene.Id == s.ch.SceneId)
                        {
                            ShotDto _shot = new ShotDto();
                            _shot.Type = new List<ShotDetailDto>();
                            _shot.Movement = new ShotDetailDto();
                            _shot.Equipment = new List<ShotDetailDto>();
                            _shot.Camera = new List<ShotDetailDto>();
                            _shot.Vfx = new List<ShotDetailDto>();
                            _shot.Lens = new List<ShotDetailDto>();
                            _shot.SpecialEquipment = new List<ShotDetailDto>();
                            _shot.Id = s.ch.Id;
                            _shot.Index = s.ch.Index;
                            _shot.Audio = s.ch.Audio;
                            _shot.Color = s.ch.Color;
                            _shot.Size = new ShotDetailDto { Name = s.ch.Size, Type = "size" };
                            _shot.Sound = s.ch.Sound;
                            _shot.Subject = s.ch.Subject;
                            _shot.UnitId = s.ch.UnitId;
                            _shot.Visual = s.ch.Visual;
                            _shot.SceneId = s.SceneId;
                            _shot.Position = s.ch.Position;
                            _shot.Lighting = s.ch.Lighting;
                            _shot.Schedule_hh = s.ch.Schedule_hh;
                            _shot.Schedule_mm = s.ch.Schedule_mm;
                            _shot.UnitId = s.ch.UnitId;


                            var data = _context.ShotDetails.Where(x => x.ShotId == s.ch.Id).ToList();
                            foreach (var d in data)
                            {
                                if (d.Type == "height" || d.Type == "height" || d.Type == "framing" || d.Type == "Dutch" || d.Type == "focus")
                                {
                                    ShotDetailDto shotDetailDto = new ShotDetailDto();
                                    shotDetailDto.Name = d.Name;
                                    shotDetailDto.Type = d.Type;

                                    _shot.Type.Add(shotDetailDto);
                                }
                                if (d.Type == "mck" || d.Type == "direction" || d.Type == "track")
                                {
                                    ShotDetailDto shotDetailDto = new ShotDetailDto();
                                    shotDetailDto.Name = d.Name;
                                    shotDetailDto.Type = d.Type;

                                    _shot.Equipment.Add(shotDetailDto);
                                }
                                if (d.Type == "movement")
                                {
                                    ShotDetailDto shotDetailDto = new ShotDetailDto();
                                    shotDetailDto.Name = d.Name;
                                    shotDetailDto.Type = d.Type;

                                    _shot.Movement = shotDetailDto;
                                }
                                if (d.Type == "vfx")
                                {
                                    ShotDetailDto shotDetailDto = new ShotDetailDto();
                                    shotDetailDto.Name = d.Name;
                                    shotDetailDto.Type = d.Type;

                                    _shot.Vfx.Add(shotDetailDto);
                                }
                                if (d.Type == "cam")
                                {
                                    ShotDetailDto shotDetailDto = new ShotDetailDto();
                                    shotDetailDto.Name = d.Name;
                                    shotDetailDto.Type = d.Type;

                                    _shot.Camera.Add(shotDetailDto);
                                }
                                if (d.Type == "fps")
                                {
                                    ShotDetailDto shotDetailDto = new ShotDetailDto();
                                    shotDetailDto.Name = d.Name;
                                    shotDetailDto.Type = d.Type;

                                    _shot.Fps = shotDetailDto;
                                }
                                if (d.Type == "se")
                                {
                                    ShotDetailDto shotDetailDto = new ShotDetailDto();
                                    shotDetailDto.Name = d.Name;
                                    shotDetailDto.Type = d.Type;

                                    _shot.SpecialEquipment.Add(shotDetailDto);
                                }
                                if (d.Type == "view" || d.Type == "prime")
                                {
                                    ShotDetailDto shotDetailDto = new ShotDetailDto();
                                    shotDetailDto.Name = d.Name;
                                    shotDetailDto.Type = d.Type;

                                    _shot.Lens.Add(shotDetailDto);
                                }

                            }


                            allscene.Shots.Add(_shot);

                        }
                    }


                }

                //var final_list = allscenes.Select(x => new
                //{
                //    x.CharacterId,
                //    x.ExtraId,
                //    x.Id,
                //    x.scene,
                //    x.Set_name,
                //    x.Env_name
                //}).ToList();

                //var scene = _context.sceneCharacters.Where(x => x.Scene.project_id == projectId).Select(x => new
                //{

                //    list = x
                //});
                //var scene = _context.scenesExtras.Join(_context.Scenes, scn => scn.SceneId, ext => ext.Id, (scn, ext) => new {scn.ExtraId,scn.Scene,ext.Id }).Where(x => x.Scene.project_id == projectId).ToList();
                //var newscene = scene.GroupBy(x =>x.Id).Select(x=>new { x});

                return Ok(allscenes);
            }
            catch (Exception ex)
            {

                throw;

            }
            //return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTalent([FromBody] Talents talent)
        {
            try
            {
                if (talent.Id == 0) {
                    _context.Add(talent);
                    await _context.SaveChangesAsync();
                    return Ok(talent);
                }
                else
                {
                    _context.Entry(talent).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok();
                }

            } catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]

        public async Task<IActionResult> CreateActor([FromBody] Actors actors)
        {
            try
            {
                if (actors.Id == 0) {
                    _context.Add(actors);
                    await _context.SaveChangesAsync();
                    return Ok(actors);
                }
                else
                {
                    _context.Entry(actors).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok();
                }

            } catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAgency([FromBody] AgencyDto agencyDto)
        {
            try
            {
                if (agencyDto.Agency.Id == 0)
                {
                    Agency agency = agencyDto.Agency;
                    var agencyContact = agencyDto.AgencyContact;
                    _context.Add(agency);
                    await _context.SaveChangesAsync();
                    foreach (var a in agencyContact)
                    {
                        a.AgencyId = agency.Id;
                        _context.Add(a);
                        await _context.SaveChangesAsync();
                    }

                    return Ok(agency);
                }
                else
                {
                    Agency agency = agencyDto.Agency;
                    var agencyContact = agencyDto.AgencyContact;
                    _context.Entry(agency).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    foreach (var a in agencyContact)
                    {
                        if (a.Id == 0)
                        {
                            a.AgencyId = agency.Id;
                            _context.AgencyContacts.Add(a);
                            await _context.SaveChangesAsync();


                        }
                        else
                        {
                            a.AgencyId = agency.Id;
                            _context.Entry(a).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }


                    return Ok();
                }

            } catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateConstruction([FromBody] construction construction)
        {
            try
            {
                if (construction is null)
                {

                    throw new ArgumentNullException(nameof(construction));
                }
                else
                {
                    List<int?> con = new List<int?>();
                    var cos = _context.Constructions.Where(x => x.ProjectId == construction.ProjectId).ToList();
                    if (cos.Count > 0)
                    {
                        con.Add(cos.Last().Index);
                    }
                    var dress = _context.dressings.Where(x => x.ProjectId == construction.ProjectId).ToList();
                    if (dress.Count > 0)
                    {
                        con.Add(dress.Last().Index);
                    }
                    var prop = _context.Props.Where(x => x.ProjectId == construction.ProjectId).ToList();
                    if (prop.Count > 0)
                    {
                        con.Add(prop.Last().Index);
                    }
                    var Graphics = _context.Graphics.Where(x => x.ProjectId == construction.ProjectId).ToList();
                    if (Graphics.Count > 0)
                    {
                        con.Add(Graphics.Last().Index);
                    }
                    var vehicles = _context.vehicles.Where(x => x.ProjectId == construction.ProjectId).ToList();
                    if (vehicles.Count > 0)
                    {
                        con.Add(vehicles.Last().Index);
                    }
                    con.Sort();
                    con.Reverse();
                    if (con.Count == 0)
                    {
                        construction.Index = 2000;
                    }
                    else
                    {


                        construction.Index = con.Last() + 1;

                    }
                    _context.Constructions.Add(construction);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCharacterTalentByExId(int exId)
        {
            try
            {
                var ls = _context.CharactersTalents.Where(x => x.ExtraId == exId).ToList();


                return Ok(ls);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCharacterTalentByChId(int charId)
        {
            try
            {
                var ls = _context.CharactersTalents.Where(x => x.CharId == charId).ToList();


                return Ok(ls);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCharacterTalent([FromBody] CharactersTalent charactersTalent)
        {
            try
            {
                if (charactersTalent.Id == 0)
                {
                    _context.Add(charactersTalent);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    _context.Entry(charactersTalent).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacterTalent(int id)
        {
            try
            {
                var ls = _context.CharactersTalents.Where(x => x.Id == id).FirstOrDefault();
                _context.Remove(ls);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgencyContact(int id)
        {
            try
            {
                var ls = _context.AgencyContacts.Where(x => x.Id == id).FirstOrDefault();
                _context.Remove(ls);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{CharId}")]
        public async Task<IActionResult> CharToExtra(int CharId)
        {
            try
            {
                var _char = _context.characters.Where(x => x.Id == CharId).FirstOrDefault();
                var ex = _context.Extras.Where(x => x.Project_Id == _char.Project_Id).ToList();
                if (ex.Count == 0)
                {
                    _char.Index = 200;
                }
                else
                {

                    _char.Index = ex.Last().Index + 1;
                }
                var extra = new Extra();

                extra.Name = _char.Name;
                extra.Index = _char.Index;
                extra.Project_Id = _char.Project_Id;
                extra.Sugggestion = _char.Sugggestion;
                extra.Description = _char.Description;
                extra.GroupOfCharacters = _char.GroupOfCharacters;
                _context.Extras.Add(extra);
                await _context.SaveChangesAsync();


                var CharActor = _context.CharactersTalents.Where(x => x.CharId == CharId).ToList();
                foreach (var c in CharActor)
                {
                    c.ExtraId = extra.Id;
                    c.CharId = null;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                var tasks = _context.ProjectTasks.Where(x => x.CharId == CharId).ToList();
                foreach (var c in tasks)
                {
                    c.CharId = null;
                    c.ExtraId = extra.Id;

                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var doc = _context.DocumentFiles.Where(x => x.CharId == CharId).ToList();
                foreach (var c in doc)
                {
                    c.CharId = null;
                    c.ExtraId = extra.Id;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var comments = _context.Comments.Where(x => x.CharId == CharId).ToList();
                foreach (var c in comments)
                {
                    c.CharId = null;
                    c.ExtraId = extra.Id;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                _context.characters.Remove(_char);
                await _context.SaveChangesAsync();
                return Ok(extra);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
     
        [HttpGet("{ActorId}")]
        public async Task<IActionResult> ActorToTalent (int ActorId)
        {
            try
            {
                var actor = _context.Actors.Where(x => x.Id == ActorId).FirstOrDefault();

                

                Talents t = JsonConvert.DeserializeObject<Talents>(JsonConvert.SerializeObject(actor));
                t.Id = 0;
                _context.Talents.Add(t);
                await _context.SaveChangesAsync();
                var tasks = _context.ProjectTasks.Where(x => x.ActorId == ActorId).ToList();
                foreach (var c in tasks)
                {
                    c.TalentID = t.Id;
                    c.ActorId = null;

                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var doc = _context.DocumentFiles.Where(x => x.ActorId == ActorId).ToList();
                foreach (var c in doc)
                {
                    
                    c.TalentId = t.Id;
                    c.ActorId = null;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var comments = _context.Comments.Where(x => x.ActorId == ActorId).ToList();
                foreach (var c in comments)
                {
                    c.TalentId = t.Id;
                    c.ActorId = null;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var links = _context.link.Where(x => x.ActorId == ActorId).ToList();
                foreach (var c in links)
                {
                    c.TalentId = t.Id;
                    c.ActorId = null;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                    var offperiod = _context.offperiods.Where(x => x.ActorId == ActorId).ToList();
                foreach (var c in offperiod)
                {
                    c.TalentId = t.Id;
                    c.ActorId = null;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                actor.Is_deleted = true;
                _context.Entry(actor).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(t.Id);
            }catch(Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{ActorId}")]
        public async Task<IActionResult> TalentToActor(int ActorId)
        {
            try
            {
                var actor = _context.Talents.Where(x => x.Id == ActorId).FirstOrDefault();



                Actors t = JsonConvert.DeserializeObject<Actors>(JsonConvert.SerializeObject(actor));
                t.Id = 0;
                _context.Actors.Add(t);
                await _context.SaveChangesAsync();
                var tasks = _context.ProjectTasks.Where(x => x.TalentID == ActorId).ToList();
                foreach (var c in tasks)
                {
                    c.TalentID = null; 
                    c.ActorId = t.Id; 

                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var doc = _context.DocumentFiles.Where(x => x.TalentId == ActorId).ToList();
                foreach (var c in doc)
                {

                    c.TalentId = null;
                    c.ActorId = t.Id;

                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var comments = _context.Comments.Where(x => x.TalentId == ActorId).ToList();
                foreach (var c in comments)
                {
                    c.TalentId = null;
                    c.ActorId = t.Id;

                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var links = _context.link.Where(x => x.TalentId == ActorId).ToList();
                foreach (var c in links)
                {
                    c.TalentId = null;
                    c.ActorId = t.Id;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var offperiod = _context.offperiods.Where(x => x.TalentId == ActorId).ToList();
                foreach (var c in offperiod)
                {
                    c.TalentId = null;
                    c.ActorId = t.Id;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                actor.Is_deleted = true;
                _context.Entry(actor).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(t.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{CharId}")]
        public async Task<IActionResult> ExtraToChar(int CharId)
        {
            try
            {
                var _char = _context.Extras.Where(x => x.Id == CharId).FirstOrDefault();
                var ex = _context.characters.Where(x => x.Project_Id == _char.Project_Id).ToList();
                if (ex.Count == 0)
                {
                    _char.Index = 1;
                }
                else
                {

                    _char.Index = ex.Last().Index + 1;
                }
                var extra = new Character();

                extra.Name = _char.Name;
                extra.Index = _char.Index;
                extra.Project_Id = _char.Project_Id;
                extra.Sugggestion = _char.Sugggestion;
                extra.Description = _char.Description;
                extra.GroupOfCharacters = _char.GroupOfCharacters;
                _context.characters.Add(extra);
                await _context.SaveChangesAsync();


                var CharActor = _context.CharactersTalents.Where(x => x.ExtraId == CharId).ToList();
                foreach (var c in CharActor)
                {
                    c.CharId = extra.Id;
                    c.ExtraId = null;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                var tasks = _context.ProjectTasks.Where(x => x.ExtraId == CharId).ToList();
                foreach (var c in tasks)
                {
                    c.CharId = extra.Id;
                    c.ExtraId = null;

                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var doc = _context.DocumentFiles.Where(x => x.ExtraId == CharId).ToList();
                foreach (var c in doc)
                {
                    c.CharId = extra.Id;
                    c.ExtraId = null;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var comments = _context.Comments.Where(x => x.ExtraId == CharId).ToList();
                foreach (var c in comments)
                {
                    c.CharId = extra.Id;
                    c.ExtraId = null;
                    _context.Entry(c).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                _context.Extras.Remove(_char);
                await _context.SaveChangesAsync();
                return Ok(extra);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DuplicateShot([FromBody] int shotId)
        {
            try
            {
                var shot = _context.Shots.Where(x => x.Id == shotId).FirstOrDefault();
                shot.Id = 0;
                _context.Shots.Add(shot);
                await _context.SaveChangesAsync();
                var details = _context.ShotDetails.Where(x => x.ShotId == shotId).ToList();
                foreach (var d in details)
                {
                    d.Id = 0;
                    d.ShotId = shot.Id;
                    _context.ShotDetails.Add(d);
                    await _context.SaveChangesAsync();
                }
                return Ok();

            } catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateShot([FromBody] ShotDto _Shot)
        {
            try
            {
                if (_Shot.Id == 0)
                {
                    Shot shot = new Shot();
                    shot.Index = _Shot.Index;
                    shot.Audio = _Shot.Audio;
                    shot.isDeleted = _Shot.isDeleted;
                    shot.Lighting = _Shot.Lighting;
                    shot.Position = _Shot.Position;
                    shot.SceneId = _Shot.SceneId;
                    shot.Color = _Shot.Color;
                    shot.Schedule_hh = _Shot.Schedule_hh;
                    shot.Schedule_mm = _Shot.Schedule_mm;
                    if (_Shot.Size is null)
                    {
                        shot.Size = null;
                    }
                    else
                    {
                        shot.Size = _Shot.Size.Name;
                    }
                    shot.Sound = _Shot.Sound;
                    shot.Subject = _Shot.Subject;
                    shot.UnitId = _Shot.UnitId;
                    shot.Visual = _Shot.Visual;
                    _context.Shots.Add(shot);
                    await _context.SaveChangesAsync();

                    foreach (var type in _Shot.Type)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var type in _Shot.Equipment)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }



                    foreach (var type in _Shot.Vfx)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var type in _Shot.Camera)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var type in _Shot.Lens)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }
                    if (_Shot.Fps != null)
                    {

                        ShotDetail _shotDetail = new ShotDetail();
                        _shotDetail.Name = _Shot.Fps.Name;
                        _shotDetail.Type = _Shot.Fps.Type;
                        _shotDetail.ShotId = shot.Id;
                        _context.Add(_shotDetail);
                        await _context.SaveChangesAsync();
                    }
                    if (_Shot.Movement != null)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = _Shot.Movement.Name;
                        shotDetail.Type = _Shot.Movement.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var type in _Shot.SpecialEquipment)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }
                    return Ok(shot);
                }
                else
                {
                    Shot shot = new Shot();
                    shot.Id = _Shot.Id;
                    shot.Index = _Shot.Index;
                    shot.Audio = _Shot.Audio;
                    shot.isDeleted = _Shot.isDeleted;
                    shot.Lighting = _Shot.Lighting;
                    shot.Position = _Shot.Position;
                    shot.SceneId = _Shot.SceneId;
                    shot.Color = _Shot.Color;
                    shot.Schedule_hh = _Shot.Schedule_hh;
                    shot.Schedule_mm = _Shot.Schedule_mm;
                    if (_Shot.Size is null)
                    {
                        shot.Size = null;
                    }
                    else
                    {
                        shot.Size = _Shot.Size.Name;
                    }
                    shot.Sound = _Shot.Sound;
                    shot.Subject = _Shot.Subject;
                    shot.UnitId = _Shot.UnitId;
                    shot.Visual = _Shot.Visual;
                    _context.Entry(shot).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    var sdetails = _context.ShotDetails.Where(x => x.ShotId == shot.Id).ToList();
                    foreach (var s in sdetails)
                    {
                        _context.Remove(s);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var type in _Shot.Type)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }
                    foreach (var type in _Shot.Equipment)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }



                    foreach (var type in _Shot.Vfx)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var type in _Shot.Camera)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var type in _Shot.Lens)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }
                    if (_Shot.Fps != null)
                    {

                        ShotDetail _shotDetail = new ShotDetail();
                        _shotDetail.Name = _Shot.Fps.Name;
                        _shotDetail.Type = _Shot.Fps.Type;
                        _shotDetail.ShotId = shot.Id;
                        _context.Add(_shotDetail);
                        await _context.SaveChangesAsync();
                    }
                    if (_Shot.Movement != null)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = _Shot.Movement.Name;
                        shotDetail.Type = _Shot.Movement.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var type in _Shot.SpecialEquipment)
                    {
                        ShotDetail shotDetail = new ShotDetail();
                        shotDetail.Name = type.Name;
                        shotDetail.Type = type.Type;
                        shotDetail.ShotId = shot.Id;
                        _context.Add(shotDetail);
                        await _context.SaveChangesAsync();
                    }
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateGraphic([FromBody] Graphic graphic)
        {
            try
            {
                if (graphic is null)
                {

                    throw new ArgumentNullException(nameof(graphic));
                }
                else
                {
                    List<int?> con = new List<int?>();
                    var cos = _context.Constructions.Where(x => x.ProjectId == graphic.ProjectId).ToList();
                    if (cos.Count > 0)
                    {
                        con.Add(cos.Last().Index);
                    }
                    var dress = _context.dressings.Where(x => x.ProjectId == graphic.ProjectId).ToList();
                    if (dress.Count > 0)
                    {
                        con.Add(dress.Last().Index);
                    }
                    var prop = _context.Props.Where(x => x.ProjectId == graphic.ProjectId).ToList();
                    if (prop.Count > 0)
                    {
                        con.Add(prop.Last().Index);
                    }
                    var Graphics = _context.Graphics.Where(x => x.ProjectId == graphic.ProjectId).ToList();
                    if (Graphics.Count > 0)
                    {
                        con.Add(Graphics.Last().Index);
                    }
                    var vehicles = _context.vehicles.Where(x => x.ProjectId == graphic.ProjectId).ToList();
                    if (vehicles.Count > 0)
                    {
                        con.Add(vehicles.Last().Index);
                    }
                    con.Sort();
                    con.Reverse();
                    if (con.Count == 0)
                    {
                        graphic.Index = 2000;
                    }
                    else
                    {


                        graphic.Index = con.First() + 1;

                    }
                    _context.Graphics.Add(graphic);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCostume([FromBody] CostumeDto costume)
        {
            try
            {
                if (costume is null)
                {
                    throw new ArgumentNullException(nameof(costume));
                }
                else if (costume.Id == 0)
                {
                    Costume _costume = new Costume();

                    var con = _context.Costumes.Where(x => x.ProjectId == costume.ProjectId).ToList();

                    if (con.Count == 0)
                    {
                        _costume.Index = 1000;
                    }
                    else
                    {


                        _costume.Index = con.Last().Index + 1;

                    }



                    _costume.Name = costume.Name;
                    _costume.ProjectId = costume.ProjectId;
                    _context.Costumes.Add(_costume);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    if (costume.CharacterId != null)
                    {
                        var costumelist = _context.SceneCostumes.Where(x => x.CharacterId == costume.CharacterId && x.SceneId == costume.sceneId).ToList();
                        if (costumelist.Count > 0)
                        {
                            foreach (var _cost in costumelist)
                            {
                                _cost.CharacterId = null;
                                _context.Entry(_cost).State = EntityState.Modified;
                                _context.SaveChanges();
                            }
                        }

                    }
                    if (costume.ExtraId != null)
                    {
                        var costumelist = _context.SceneCostumes.Where(x => x.ExtraId == costume.ExtraId && x.SceneId == costume.sceneId).ToList();

                        if (costumelist.Count > 0)
                        {
                            foreach (var _cost in costumelist)
                            {
                                _cost.ExtraId = null;
                                _context.Entry(_cost).State = EntityState.Modified;
                                _context.SaveChanges();
                            }
                        }

                    }
                    var costumes = _context.SceneCostumes.Where(x => x.CostumeId == costume.Id && x.SceneId == costume.sceneId).FirstOrDefault();

                    if (costumes is null)
                    {
                        _context.SceneCostumes.Add(new SceneCostumes { CostumeId = costume.Id, CharacterId = costume.CharacterId, ExtraId = costume.ExtraId, SceneId = costume.sceneId, ProjectId = costume.ProjectId });
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (costumes.CharacterId is null)
                        {
                            costumes.CharacterId = costume.CharacterId;
                        }
                        if (costumes.ExtraId is null)
                        {
                            costumes.ExtraId = costume.ExtraId;
                        }
                        _context.Entry(costumes).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCostumes(int projectId)
        {
            try
            {
                var costumes = _context.Costumes.Where(x => x.ProjectId == projectId).ToList();
                return Ok(costumes);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveMakeup([FromBody] Makeup makeup)
        {
            try
            {
                if (makeup.CharacterId == 0)
                {
                    var Makeups = _context.sceneMakeups.Where(x => x.MakeupId == makeup.Id).ToList();


                    foreach (var m in Makeups)
                    {
                        m.MakeupId = makeup.Id;

                        m.CharacterId = null;


                        _context.Entry(m).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    return Ok();
                }
                else if (makeup.ExtraId == 0)
                {
                    var Makeups = _context.sceneMakeups.Where(x => x.MakeupId == makeup.Id).ToList();

                    foreach (var m in Makeups)
                    {
                        m.MakeupId = makeup.Id;

                        m.ExtraId = null;


                        _context.Entry(m).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    return Ok();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMakeup([FromBody] MakeupDto makeup)
        {
            try
            {
                if (makeup is null)
                {
                    throw new ArgumentNullException(nameof(makeup));
                }
                else if (makeup.Id == 0)
                {

                    Makeup _makeup = new Makeup();
                    _makeup.ExtraId = makeup.ExtraId;
                    _makeup.CharacterId = makeup.CharacterId;
                    _makeup.Name = makeup.Name;
                    _makeup.ProjectId = makeup.ProjectId;
                    _context.Makeups.Add(_makeup);
                    await _context.SaveChangesAsync();

                    return Ok();

                }
                else
                {

                    _context.sceneMakeups.Add(new SceneMakeup { MakeupId = makeup.Id, SceneId = makeup.sceneId, ExtraId = makeup.ExtraId, CharacterId = makeup.CharacterId, ProjectId = makeup.ProjectId });
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetMakeup(int projectId)
        {
            try
            {
                var makeup = _context.Makeups.Where(x => x.ProjectId == projectId).ToList();
                return Ok(makeup);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateScene([FromBody] Allscenes allscenes)
        {

            if (allscenes.scene is null)
            {
                throw new ArgumentNullException(nameof(allscenes));
            }


            try
            {
                if (allscenes.scene.Id == 0)
                {
                    _context.Scenes.Add(allscenes.scene);
                    await _context.SaveChangesAsync();
                    foreach (var character in allscenes.CharacterId)
                    {
                        _context.sceneCharacters.Add(new SceneCharacter { SceneId = allscenes.scene.Id, CharacterId = int.Parse(character) });
                        await _context.SaveChangesAsync();
                    }

                    foreach (var extra in allscenes.ExtraId)
                    {
                        _context.scenesExtras.Add(new ScenesExtra { SceneId = allscenes.scene.Id, ExtraId = int.Parse(extra) });
                        await _context.SaveChangesAsync();
                    }
                    return Ok(new
                    {
                        success = true,
                        Msg = "Scene Created successfully!",
                        sceneId = allscenes.scene.Id
                    });
                }
                else
                {
                    _context.Entry(allscenes.scene).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {

                throw;

            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScene(int id)
        {
            try
            {
                var scene = await _context.Scenes.FindAsync(id);
                if (scene == null)
                {
                    return NotFound();
                }
                scene.isDeleted = true;
                _context.Entry(scene).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCharacters(int projectid)
        {
            try
            {
                var characters =await _context.characters.Where(x => x.Project_Id == projectid).Select( c =>new CharacterDto {

                Id =c.Id,
                    Name = c.Name,
                    GroupOfCharacters = c.GroupOfCharacters,
                    Sugggestion = c.Sugggestion,
                    Description=c.Description,
                    marked=c.marked,
                    Index=c.Index,
                    Project_Id=c.Project_Id,


                }).ToListAsync();
                foreach(var c in characters){
                    var dbList = await (from d in _context.DocumentFiles
                                        join user in _context.Users
                                        on d.UserId equals user.Id
                                        where d.CharId == c.Id && d.Default == true
                                        select new DocumentFilesDto()
                                        {
                                            ContentType = d.ContentType,
                                            DocumentCategoryId = d.DocumentCategoryId.Value,
                                            Extension = d.Extension,
                                            FileId = d.FileId,
                                            Id = d.Id,
                                            MimeType = d.MimeType,
                                            Name = d.Name,
                                            Type = d.Type,
                                            UserFriendlySize = d.UserFriendlySize,
                                            UserId = d.UserId,
                                            UserName = user.GetUserName(),
                                            RelativeTime = d.Created.GetRelativeTime(),
                                            UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                            Default = d.Default,

                                        }).FirstOrDefaultAsync();
                    if(dbList is null)
                    {
                        c.Default = false;
                        c.HasFile = false;
                        c.file = null;
                    }
                    else
                    {
                        c.Default = true;
                        c.HasFile = true;
                        c.file = dbList;
                    }
                }
                return Ok(characters);
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetAgency(int projectid)
        {
            try
            {
                var agency =await _context.Agencies.Where(x => x.ProjectId == projectid && x.Is_Deleted == false).ToListAsync();
                List<AgencyDto> agencyDtos = new List<AgencyDto>();
                foreach (var a in agency)
                {
                    var agencyContact = _context.AgencyContacts.Where(x => x.AgencyId == a.Id).ToList();
                    AgencyDto agencyDto = new AgencyDto();
                    agencyDto.Agency = a;
                    agencyDto.AgencyContact = agencyContact;
                    agencyDtos.Add(agencyDto);
                }

                foreach(var c in agencyDtos) {
              
                        var dbList = await (from d in _context.DocumentFiles
                                            join user in _context.Users
                                            on d.UserId equals user.Id
                                            where d.AgencyID == c.Agency.Id && d.Default == true
                                            select new DocumentFilesDto()
                                            {
                                                ContentType = d.ContentType,
                                                DocumentCategoryId = d.DocumentCategoryId.Value,
                                                Extension = d.Extension,
                                                FileId = d.FileId,
                                                Id = d.Id,
                                                MimeType = d.MimeType,
                                                Name = d.Name,
                                                Type = d.Type,
                                                UserFriendlySize = d.UserFriendlySize,
                                                UserId = d.UserId,
                                                UserName = user.GetUserName(),
                                                RelativeTime = d.Created.GetRelativeTime(),
                                                UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                                Default = d.Default,

                                            }).FirstOrDefaultAsync();
                        if (dbList is null)
                        {
                            c.Default = false;
                            c.HasFile = false;
                            c.file = null;
                        }
                        else
                        {
                            c.Default = true;
                            c.HasFile = true;
                            c.file = dbList;
                        }
                    }
                
                return Ok(agencyDtos);
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetTalent(int projectid)
        {
            try
            {
                var talent = await _context.Talents.Where(x => x.ProjectId == projectid && x.Is_deleted == false).Select(a => new talentDto
                {

                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email,
                
                    PhoneHome = a.PhoneHome,
              
                    PhoneMobile = a.PhoneMobile,
                    Fax = a.Fax,
                    FirstStreet = a.FirstStreet,
                    FirstCity = a.FirstCity,
                    FirstPostalCode = a.FirstPostalCode,
                    FirstCountry = a.FirstCountry,
                    SecondStreet = a.SecondStreet,
                    SecondCity = a.SecondCity,
                    SecondPostalCode = a.SecondPostalCode,
                    SecondCountry = a.SecondCountry,
                    ProdStreet = a.ProdStreet,
                    ProdCity = a.ProdCity,
                    ProdPostalCode = a.ProdPostalCode,
                    ProdCountry = a.ProdCountry,
                    Gender = a.Gender,
                    AgeMin = a.AgeMin,
                    AgeMax = a.AgeMax,
                    Languages = a.Languages,
                    Talent = a.Talent,
                    VocalRange = a.VocalRange,
                    Instruments = a.Instruments,
                    Ethnicity = a.Ethnicity,
                    BodyType = a.BodyType,
                    EyeColor = a.EyeColor,
                    HairLength = a.HairLength,
                    HairColor = a.HairColor,
                    HairType = a.HairType,
                    FacialHair = a.FacialHair,
                    WillCutHair = a.WillCutHair,
                    WillShave = a.WillShave,
                    WearsGlasses = a.WearsGlasses,
                    HasTattoo = a.HasTattoo,
                    HasPiercings = a.HasPiercings,
                    AppearanceNote = a.AppearanceNote,
                    ShirtSize = a.ShirtSize,
                    PantsSize = a.PantsSize,
                    DressSize = a.DressSize,
                    BraSize = a.BraSize,
                    GloveSize = a.GloveSize,
                    ShoeSize = a.ShoeSize,
                    BodyHeight = a.BodyHeight,
                    NeckToFloor = a.NeckToFloor,
                    HeadGirth = a.HeadGirth,
                    NeckGirth = a.NeckGirth,
                    SoulderLength = a.SoulderLength,
                    BackWidth = a.BackWidth,
                    BackLength = a.BackLength,
                    ChestGirth = a.ChestGirth,
                    UnderChestGirth = a.UnderChestGirth,
                    NeckToBreast = a.NeckToBreast,
                    NeckToWaist = a.NeckToWaist,
                    FrontWaistLength = a.FrontWaistLength,
                    WaistCircumference = a.WaistCircumference,
                    HipSize = a.HipSize,
                    HipHeightFromWaist = a.HipHeightFromWaist,
                    ArmLength = a.ArmLength,
                    UpperArmGirth = a.UpperArmGirth,
                    ArmBedGirth = a.ArmBedGirth,
                    ArmBedLength = a.ArmBedLength,
                    HandGirth = a.HandGirth,
                    Inseam = a.Inseam,
                    InnerLengthOfLeg = a.InnerLengthOfLeg,
                    OuterLengthOfLeg = a.OuterLengthOfLeg,
                    CalfGirth = a.CalfGirth,
                    ThighGirth = a.ThighGirth,
                    KneeHeight = a.KneeHeight,
                    KneeCircumference = a.KneeCircumference,
                    AnkleWidth = a.AnkleWidth,
                    FootLength = a.FootLength,
                    FrontFootWidth = a.FrontFootWidth,
                    FootWidth = a.FootWidth,
                    IsVeg = a.IsVeg,
                    IsVegan = a.IsVegan,
                    IsHalal = a.IsHalal,
                    IsKosher = a.IsKosher,
                    Allergies = a.Allergies,
                    Note = a.Note,
                    Is_deleted = a.Is_deleted,

                    AgencyId = a.AgencyId,



                    ProjectId = a.ProjectId
                }).ToListAsync();
                foreach (var c in talent)
                {
                    var dbList = await (from d in _context.DocumentFiles
                                        join user in _context.Users
                                        on d.UserId equals user.Id
                                        where d.TalentId == c.Id && d.Default == true
                                        select new DocumentFilesDto()
                                        {
                                            ContentType = d.ContentType,
                                            DocumentCategoryId = d.DocumentCategoryId.Value,
                                            Extension = d.Extension,
                                            FileId = d.FileId,
                                            Id = d.Id,
                                            MimeType = d.MimeType,
                                            Name = d.Name,
                                            Type = d.Type,
                                            UserFriendlySize = d.UserFriendlySize,
                                            UserId = d.UserId,
                                            UserName = user.GetUserName(),
                                            RelativeTime = d.Created.GetRelativeTime(),
                                            UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                            Default = d.Default,

                                        }).FirstOrDefaultAsync();
                    if (dbList is null)
                    {
                        c.Default = false;
                        c.HasFile = false;
                        c.file = null;
                    }
                    else
                    {
                        c.Default = true;
                        c.HasFile = true;
                        c.file = dbList;
                    }
                }
                
                return Ok(talent);

            } catch (Exception ex) {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetActor(int projectid)
        {
            try
            {
               var actors =await _context.Actors.Where(x => x.ProjectId == projectid && x.Is_deleted == false).Select(a=>new ActorDto {

                    Id = a.Id,
                    Name=a.Name,
                    Email=a.Email,
                   Union=a.Union,
        PhoneHome =a.PhoneHome,
        RealName =a.RealName,
        DOB =a.DOB,
        PhoneMobile =a.PhoneMobile,
        Fax =a.Fax,
        FirstStreet =a.FirstStreet,
        FirstCity =a.FirstCity,
        FirstPostalCode =a.FirstPostalCode,
        FirstCountry =a.FirstCountry,
        SecondStreet =a.SecondStreet,
        SecondCity =a.SecondCity,
        SecondPostalCode = a.SecondPostalCode,
        SecondCountry = a.SecondCountry,
        ProdStreet = a.ProdStreet,
        ProdCity = a.ProdCity,
        ProdPostalCode = a.ProdPostalCode,
        ProdCountry = a.ProdCountry,
        Gender = a.Gender,
        AgeMin = a.AgeMin,
         AgeMax = a.AgeMax,
        Languages = a.Languages,
        Talent = a.Talent,
        VocalRange = a.VocalRange,
        Instruments = a.Instruments,
        Ethnicity = a.Ethnicity,
        BodyType = a.BodyType,
        EyeColor = a.EyeColor,
        HairLength = a.HairLength,
        HairColor = a.HairColor,
        HairType = a.HairType,
        FacialHair = a.FacialHair,
         WillCutHair = a.WillCutHair,
         WillShave = a.WillShave,
         WearsGlasses = a.WearsGlasses,
       HasTattoo = a.HasTattoo,
       HasPiercings = a.HasPiercings,
        AppearanceNote = a.AppearanceNote,
        ShirtSize = a.ShirtSize,
        PantsSize = a.PantsSize,
        DressSize = a.DressSize,
        BraSize = a.BraSize,
        GloveSize = a.GloveSize,
        ShoeSize = a.ShoeSize,
        BodyHeight = a.BodyHeight,
        NeckToFloor = a.NeckToFloor,
        HeadGirth = a.HeadGirth,
        NeckGirth = a.NeckGirth,
        SoulderLength = a.SoulderLength,
        BackWidth = a.BackWidth,
        BackLength = a.BackLength,
        ChestGirth = a.ChestGirth,
        UnderChestGirth = a.UnderChestGirth,
        NeckToBreast = a.NeckToBreast,
        NeckToWaist = a.NeckToWaist,
        FrontWaistLength = a.FrontWaistLength,
        WaistCircumference = a.WaistCircumference,
        HipSize = a.HipSize,
        HipHeightFromWaist = a.HipHeightFromWaist,
        ArmLength = a.ArmLength,
        UpperArmGirth = a.UpperArmGirth,
        ArmBedGirth = a.ArmBedGirth,
        ArmBedLength = a.ArmBedLength,
        HandGirth = a.HandGirth,
        Inseam = a.Inseam,
        InnerLengthOfLeg = a.InnerLengthOfLeg,
        OuterLengthOfLeg = a.OuterLengthOfLeg,
        CalfGirth = a.CalfGirth,
        ThighGirth = a.ThighGirth,
        KneeHeight = a.KneeHeight,
        KneeCircumference = a.KneeCircumference,
        AnkleWidth = a.AnkleWidth,
        FootLength = a.FootLength,
        FrontFootWidth = a.FrontFootWidth,
        FootWidth = a.FootWidth,
         IsVeg = a.IsVeg,
        IsVegan = a.IsVegan,
        IsHalal = a.IsHalal,
    IsKosher =a.IsKosher,
        Allergies =a.Allergies,
        Note =a.Note,
      Is_deleted =a.Is_deleted,

      AgencyId =a.AgencyId,



       ProjectId =a.ProjectId
                }).ToListAsync();
                foreach (var c in actors)
                {
                    var dbList = await (from d in _context.DocumentFiles
                                        join user in _context.Users
                                        on d.UserId equals user.Id
                                        where d.ActorId == c.Id && d.Default == true
                                        select new DocumentFilesDto()
                                        {
                                            ContentType = d.ContentType,
                                            DocumentCategoryId = d.DocumentCategoryId.Value,
                                            Extension = d.Extension,
                                            FileId = d.FileId,
                                            Id = d.Id,
                                            MimeType = d.MimeType,
                                            Name = d.Name,
                                            Type = d.Type,
                                            UserFriendlySize = d.UserFriendlySize,
                                            UserId = d.UserId,
                                            UserName = user.GetUserName(),
                                            RelativeTime = d.Created.GetRelativeTime(),
                                            UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                            Default = d.Default,

                                        }).FirstOrDefaultAsync();
                    if (dbList is null)
                    {
                        c.Default = false;
                        c.HasFile = false;
                        c.file = null;
                    }
                    else
                    {
                        c.Default = true;
                        c.HasFile = true;
                        c.file = dbList;
                    }
                }
                return Ok(actors);

            } catch (Exception ex) {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetExtras(int projectid)
        {
            try
            {
                var Extras =await  _context.Extras.Where(x => x.Project_Id == projectid).Select(c => new CharacterDto
                {

                    Id = c.Id,
                    Name = c.Name,
                    GroupOfCharacters = c.GroupOfCharacters,
                    Sugggestion = c.Sugggestion,
                    Description = c.Description,
                    marked = c.marked,
                    Index = c.Index,
                    Project_Id = c.Project_Id,


                }).ToListAsync();
                foreach (var c in Extras)
                {
                    var dbList = await (from d in _context.DocumentFiles
                                        join user in _context.Users
                                        on d.UserId equals user.Id
                                        where d.ExtraId == c.Id && d.Default == true
                                        select new DocumentFilesDto()
                                        {
                                            ContentType = d.ContentType,
                                            DocumentCategoryId = d.DocumentCategoryId.Value,
                                            Extension = d.Extension,
                                            FileId = d.FileId,
                                            Id = d.Id,
                                            MimeType = d.MimeType,
                                            Name = d.Name,
                                            Type = d.Type,
                                            UserFriendlySize = d.UserFriendlySize,
                                            UserId = d.UserId,
                                            UserName = user.GetUserName(),
                                            RelativeTime = d.Created.GetRelativeTime(),
                                            UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                            Default = d.Default,

                                        }).FirstOrDefaultAsync();
                    if (dbList is null)
                    {
                        c.Default = false;
                        c.HasFile = false;
                        c.file = null;
                    }
                    else
                    {
                        c.Default = true;
                        c.HasFile = true;
                        c.file = dbList;
                    }
                }
                return Ok(Extras);
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetConstruction(int projectid)
        {
            try
            {
                var construction = _context.Constructions.Where(x => x.ProjectId == projectid);
                return Ok(construction);

            }
            catch (Exception ex)
            {
                throw;
            }


        }


        [HttpGet]
        public async Task<IActionResult> GetDressing(int projectid)
        {
            try
            {
                var dressings = _context.dressings.Where(x => x.ProjectId == projectid);
                return Ok(dressings);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetProps(int projectid)
        {
            try
            {
                var prop = _context.Props.Where(x => x.ProjectId == projectid);
                return Ok(prop);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetGraphic(int projectid)
        {
            try
            {
                var prop = _context.Graphics.Where(x => x.ProjectId == projectid);
                return Ok(prop);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetVehicle(int projectid)
        {
            try
            {
                var prop = _context.vehicles.Where(x => x.ProjectId == projectid);
                return Ok(prop);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetAnimal(int projectid)
        {
            try
            {
                var animal = _context.Animals.Where(x => x.ProjectId == projectid);
                return Ok(animal);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetVisual(int projectid)
        {
            try
            {
                var visual = _context.visualEffects.Where(x => x.ProjectId == projectid);
                return Ok(visual);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetSpecial(int projectid)
        {
            try
            {
                var special = _context.specialEffects.Where(x => x.ProjectId == projectid);
                return Ok(special);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetSound(int projectid)
        {
            try
            {
                var sound = _context.Sounds.Where(x => x.ProjectId == projectid);
                return Ok(sound);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetCamera(int projectid)
        {
            try
            {
                var camera = _context.cameras.Where(x => x.ProjectId == projectid);
                return Ok(camera);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetStunt(int projectid)
        {
            try
            {
                var stunt = _context.stunts.Where(x => x.ProjectId == projectid);
                return Ok(stunt);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetOther(int projectid)
        {
            try
            {
                var other = _context.Others.Where(x => x.ProjectId == projectid);
                return Ok(other);

            }
            catch (Exception ex)
            {
                throw;
            }


        }
        [HttpPost]
        public async Task<IActionResult> CreateChar([FromBody] Character character)
        {
            try
            {
                if (character.Id == 0)
                {
                    var ch = _context.characters.Where(x => x.Project_Id == character.Project_Id).ToList();
                    if (ch.Count == 0)
                    {
                        character.Index = 1;
                    }
                    else
                    {

                        character.Index = ch.Last().Index + 1;
                        
                    }
                    _context.characters.Add(character);
                    await _context.SaveChangesAsync();
                    return Ok(character);
                }
                else
                {
                    if (character.GroupOfCharacters == false)
                    {
                        var charT = _context.CharactersTalents.Where(x => x.Is_CastFixed == true && x.CharId == character.Id).ToList();
                        for (int i = 0; i < charT.Count - 1; i++)
                        {

                            charT[i].Is_CastFixed = false;
                            _context.Entry(charT[i]).State = EntityState.Modified;
                            _context.SaveChanges();

                        }
                    }
                    _context.Entry(character).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(character);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpPost]
        public async Task<IActionResult> Createext([FromBody] Extra extra)
        {
            try
            {
                if (extra.Id == 0)
                {
                    var ex = _context.Extras.Where(x => x.Project_Id == extra.Project_Id).ToList();
                    if (ex.Count == 0)
                    {
                        extra.Index = 200;
                    }
                    else
                    {

                        extra.Index = ex.Last().Index + 1;
                    }
                    _context.Extras.Add(extra);
                    await _context.SaveChangesAsync();
                    return Ok(extra);
                }
                else
                {
                    if (extra.GroupOfCharacters == false)
                    {
                        var charT = _context.CharactersTalents.Where(x => x.Is_CastFixed == true && x.ExtraId == extra.Id).ToList();
                        for (int i = 0; i < charT.Count - 1; i++)
                        {
                            charT[i].Is_CastFixed = false;
                            _context.Entry(charT[i]).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                    _context.Entry(extra).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(extra);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateDressing([FromBody] dressing dressing)
        {
            try
            {
                List<int?> con = new List<int?>();
                var cos = _context.Constructions.Where(x => x.ProjectId == dressing.ProjectId).ToList();
                if (cos.Count > 0) {
                    con.Add(cos.Last().Index);
                }
                var dress = _context.dressings.Where(x => x.ProjectId == dressing.ProjectId).ToList();
                if (dress.Count > 0)
                {
                    con.Add(dress.Last().Index);
                }
                var prop = _context.Props.Where(x => x.ProjectId == dressing.ProjectId).ToList();
                if (prop.Count > 0)
                {
                    con.Add(prop.Last().Index);
                }
                var Graphics = _context.Graphics.Where(x => x.ProjectId == dressing.ProjectId).ToList();
                if (Graphics.Count > 0)
                {
                    con.Add(Graphics.Last().Index);
                }
                var vehicles = _context.vehicles.Where(x => x.ProjectId == dressing.ProjectId).ToList();
                if (vehicles.Count > 0)
                {
                    con.Add(vehicles.Last().Index);
                }


                con.Sort();
                con.Reverse();
                if (con.Count == 0)
                {
                    dressing.Index = 2000;
                }
                else
                {


                    dressing.Index = con.First() + 1;

                }

                _context.dressings.Add(dressing);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateSet([FromBody] Set set)
        {
            try
            {
                _context.sets.Add(set);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateProps([FromBody] Props _prop)
        {
            try
            {
                List<int?> con = new List<int?>();
                var cos = _context.Constructions.Where(x => x.ProjectId == _prop.ProjectId).ToList();
                if (cos.Count > 0)
                {
                    con.Add(cos.Last().Index);
                }
                var dress = _context.dressings.Where(x => x.ProjectId == _prop.ProjectId).ToList();
                if (dress.Count > 0)
                {
                    con.Add(dress.Last().Index);
                }
                var prop = _context.Props.Where(x => x.ProjectId == _prop.ProjectId).ToList();
                if (prop.Count > 0)
                {
                    con.Add(prop.Last().Index);
                }
                var Graphics = _context.Graphics.Where(x => x.ProjectId == _prop.ProjectId).ToList();
                if (Graphics.Count > 0)
                {
                    con.Add(Graphics.Last().Index);
                }
                var vehicles = _context.vehicles.Where(x => x.ProjectId == _prop.ProjectId).ToList();
                if (vehicles.Count > 0)
                {
                    con.Add(vehicles.Last().Index);
                }
                con.Sort();
                con.Reverse();
                if (con.Count == 0)
                {
                    _prop.Index = 2000;
                }
                else
                {


                    _prop.Index = con.First() + 1;

                }
                _context.Props.Add(_prop);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] Vehicle vehicle)
        {
            try
            {
                List<int?> con = new List<int?>();
                var cos = _context.Constructions.Where(x => x.ProjectId == vehicle.ProjectId).ToList();
                if (cos.Count > 0)
                {
                    con.Add(cos.Last().Index);
                }
                var dress = _context.dressings.Where(x => x.ProjectId == vehicle.ProjectId).ToList();
                if (dress.Count > 0)
                {
                    con.Add(dress.Last().Index);
                }
                var prop = _context.Props.Where(x => x.ProjectId == vehicle.ProjectId).ToList();
                if (prop.Count > 0)
                {
                    con.Add(prop.Last().Index);
                }
                var Graphics = _context.Graphics.Where(x => x.ProjectId == vehicle.ProjectId).ToList();
                if (Graphics.Count > 0)
                {
                    con.Add(Graphics.Last().Index);
                }
                var vehicles = _context.vehicles.Where(x => x.ProjectId == vehicle.ProjectId).ToList();
                if (vehicles.Count > 0)
                {
                    con.Add(vehicles.Last().Index);
                }
                con.Sort();
                con.Reverse();
                if (con.Count == 0)
                {
                    vehicle.Index = 2000;
                }
                else
                {


                    vehicle.Index = con.First() + 1;

                }
                _context.vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] Animal animal)
        {
            try
            {
                _context.Animals.Add(animal);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateVisual([FromBody] VisualEffect visual)
        {
            try
            {
                _context.visualEffects.Add(visual);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateSpecial([FromBody] SpecialEffect special)
        {
            try
            {
                _context.specialEffects.Add(special);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateSound([FromBody] Sound sound)
        {
            try
            {
                _context.Sounds.Add(sound);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateCamera([FromBody] Camera camera)
        {
            try
            {
                _context.cameras.Add(camera);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateStunt([FromBody] Stunt stunt)
        {
            try
            {
                _context.stunts.Add(stunt);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateOther([FromBody] other other)
        {
            try
            {
                _context.Others.Add(other);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteShot(int id)
        {
            try
            {

                var _shot = _context.Shots.Where(x => x.Id == id).FirstOrDefault();
                _shot.isDeleted = true;
                _context.Entry(_shot).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteChar([FromBody] SceneCharacter sceneCharacter)
        {
            try
            {
                var chr = _context.sceneCharacters.SingleOrDefault(x => x.CharacterId == sceneCharacter.CharacterId && x.SceneId == sceneCharacter.SceneId);
                _context.sceneCharacters.Remove(chr);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChar(int id)
        {
            try
            {
                var scenechr = _context.sceneCharacters.Where(x => x.CharacterId == id).ToList();
                foreach (var _chr in scenechr)
                {
                    _context.sceneCharacters.Remove(_chr);
                    await _context.SaveChangesAsync();
                }

                var Costumechr = _context.SceneCostumes.Where(x => x.CharacterId == id).ToList();
                foreach (var _chr in Costumechr)
                {
                    _context.SceneCostumes.Remove(_chr);
                    await _context.SaveChangesAsync();
                }
                var Makeupchr = _context.sceneMakeups.Where(x => x.CharacterId == id).ToList();
                foreach (var _chr in Makeupchr)
                {
                    _context.sceneMakeups.Remove(_chr);
                    await _context.SaveChangesAsync();
                }
                var charT = _context.CharactersTalents.Where(x => x.CharId == id).ToList();
                foreach (var _chr in charT)
                {
                    _context.CharactersTalents.Remove(_chr);
                    await _context.SaveChangesAsync();
                }
                var doc = _context.DocumentFiles.Where(x => x.CharId == id).ToList();
                foreach (var _chr in doc)
                {
                    _context.DocumentFiles.Remove(_chr);
                    await _context.SaveChangesAsync();
                }
                var chr = _context.characters.SingleOrDefault(x => x.Id == id);
                _context.characters.Remove(chr);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExtra(int id)
        {
            try
            {
                var scenechr = _context.scenesExtras.Where(x => x.ExtraId == id).ToList();
                foreach (var _chr in scenechr)
                {
                    _context.scenesExtras.Remove(_chr);
                    await _context.SaveChangesAsync();
                }

                var Costumechr = _context.SceneCostumes.Where(x => x.ExtraId == id).ToList();
                foreach (var _chr in Costumechr)
                {
                    _context.SceneCostumes.Remove(_chr);
                    await _context.SaveChangesAsync();
                }
                var Makeupchr = _context.sceneMakeups.Where(x => x.ExtraId == id).ToList();
                foreach (var _chr in Makeupchr)
                {
                    _context.sceneMakeups.Remove(_chr);
                    await _context.SaveChangesAsync();
                }
                var tasks = _context.ProjectTasks.Where(x => x.ExtraId == id).ToList();
                foreach (var t in tasks)
                {
                    t.Deleted = true;
                    _context.Entry(t).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                var doc = _context.DocumentFiles.Where(x => x.ExtraId == id).ToList();
                foreach (var t in doc)
                {

                    _context.DocumentFiles.Remove(t);
                    _context.SaveChanges();
                }
                var comment = _context.Comments.Where(x => x.ExtraId == id).ToList();
                foreach (var t in comment)
                {
                    _context.Comments.Remove(t);
                    _context.SaveChanges();
                }
                var Actors = _context.CharactersTalents.Where(x => x.ExtraId == id).ToList();
                foreach (var t in Actors)
                {
                    _context.CharactersTalents.Remove(t);

                    _context.SaveChanges();
                }
                var chr = _context.Extras.SingleOrDefault(x => x.Id == id);
                _context.Extras.Remove(chr);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConstruction([FromBody] SceneConstruction sceneConstruction)
        {
            try
            {
                var construction = _context.sceneConstructions.SingleOrDefault(x => x.ConstructionId == sceneConstruction.ConstructionId && x.SceneId == sceneConstruction.SceneId);
                _context.sceneConstructions.Remove(construction);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpPost]
        public async Task<IActionResult> DeleteDressing([FromBody] SceneDressing sceneDressing)
        {
            try
            {
                var dressing = _context.sceneDressings.SingleOrDefault(x => x.DressingId == sceneDressing.DressingId && x.SceneId == sceneDressing.SceneId);
                _context.sceneDressings.Remove(dressing);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteExtra([FromBody] ScenesExtra scenesExtra)
        {
            try
            {
                var ext = _context.scenesExtras.SingleOrDefault(x => x.ExtraId == scenesExtra.ExtraId && x.SceneId == scenesExtra.SceneId);
                _context.scenesExtras.Remove(ext);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteProps([FromBody] SceneProps sceneProps)
        {
            try
            {
                var prop = _context.sceneProps.SingleOrDefault(x => x.PropsId == sceneProps.PropsId && x.SceneId == sceneProps.SceneId);
                _context.sceneProps.Remove(prop);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteVehicle([FromBody] SceneVehicle sceneVehicle)
        {
            try
            {
                var vehicle = _context.sceneVehicles.SingleOrDefault(x => x.VehicleId == sceneVehicle.VehicleId && x.SceneId == sceneVehicle.SceneId);
                _context.sceneVehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteAnimal([FromBody] SceneAnimal sceneAnimal)
        {
            try
            {
                var animal = _context.sceneAnimals.SingleOrDefault(x => x.AnimalId == sceneAnimal.AnimalId && x.SceneId == sceneAnimal.SceneId);
                _context.sceneAnimals.Remove(animal);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteGraphic([FromBody] SceneGraphic sceneGraphic)
        {
            try
            {
                var graphic = _context.sceneGraphics.SingleOrDefault(x => x.GraphicId == sceneGraphic.GraphicId && x.SceneId == sceneGraphic.SceneId);
                _context.sceneGraphics.Remove(graphic);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteVisual([FromBody] SceneVisual sceneVisual)
        {
            try
            {
                var visual = _context.sceneVisuals.SingleOrDefault(x => x.VisualId == sceneVisual.VisualId && x.SceneId == sceneVisual.SceneId);
                _context.sceneVisuals.Remove(visual);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteSpecial([FromBody] SceneSpecial sceneSpecial)
        {
            try
            {
                var special = _context.sceneSpecials.SingleOrDefault(x => x.SpecialId == sceneSpecial.SpecialId && x.SceneId == sceneSpecial.SceneId);
                _context.sceneSpecials.Remove(special);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteSound([FromBody] SceneSound sceneSound)
        {
            try
            {
                var sound = _context.sceneSounds.SingleOrDefault(x => x.SoundId == sceneSound.SoundId && x.SceneId == sceneSound.SceneId);
                _context.sceneSounds.Remove(sound);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteCamera([FromBody] SceneCamera sceneCamera)
        {
            try
            {
                var camera = _context.sceneCameras.SingleOrDefault(x => x.CameraId == sceneCamera.CameraId && x.SceneId == sceneCamera.SceneId);
                _context.sceneCameras.Remove(camera);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteStunt([FromBody] SceneStunt sceneStunt)
        {
            try
            {
                var stunt = _context.sceneStunts.SingleOrDefault(x => x.StuntId == sceneStunt.StuntId && x.SceneId == sceneStunt.SceneId);
                _context.sceneStunts.Remove(stunt);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteOther([FromBody] SceneOther sceneOther)
        {
            try
            {
                var other = _context.sceneOthers.SingleOrDefault(x => x.OtherId == sceneOther.OtherId && x.SceneId == sceneOther.SceneId);
                _context.sceneOthers.Remove(other);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddGraphic([FromBody] SceneGraphic sceneGraphic)
        {
            try
            {


                _context.sceneGraphics.Add(sceneGraphic);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] SceneVehicle sceneVehicle)
        {
            try
            {


                _context.sceneVehicles.Add(sceneVehicle);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddDressing([FromBody] SceneDressing sceneDressing)
        {
            try
            {


                _context.sceneDressings.Add(sceneDressing);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddCharacter([FromBody] SceneCharacter sceneCharacter)
        {
            try
            {

                var ch = _context.sceneCharacters.Where(x => x.SceneId == sceneCharacter.SceneId && x.CharacterId == sceneCharacter.CharacterId).FirstOrDefault();
                if (ch is null)
                {
                    _context.sceneCharacters.Add(sceneCharacter);
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddConstruction([FromBody] SceneConstruction sceneConstruction)
        {
            try
            {
                _context.sceneConstructions.Add(sceneConstruction);
                await _context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddExtra([FromBody] ScenesExtra scenesExtra)
        {
            try
            {


                var ch = _context.scenesExtras.Where(x => x.SceneId == scenesExtra.SceneId && x.ExtraId == scenesExtra.ExtraId).FirstOrDefault();
                if (ch is null)
                {
                    _context.scenesExtras.Add(scenesExtra);
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddProps([FromBody] SceneProps sceneProps)
        {
            try
            {


                _context.sceneProps.Add(sceneProps);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddAnimal([FromBody] SceneAnimal sceneAnimal)
        {
            try
            {


                _context.sceneAnimals.Add(sceneAnimal);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddVisual([FromBody] SceneVisual sceneVisual)
        {
            try
            {


                _context.sceneVisuals.Add(sceneVisual);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddSpecial([FromBody] SceneSpecial sceneSpecial)
        {
            try
            {


                _context.sceneSpecials.Add(sceneSpecial);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddSound([FromBody] SceneSound sceneSound)
        {
            try
            {


                _context.sceneSounds.Add(sceneSound);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddCamera([FromBody] SceneCamera sceneCamera)
        {
            try
            {


                _context.sceneCameras.Add(sceneCamera);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddStunt([FromBody] SceneStunt sceneStunt)
        {
            try
            {


                _context.sceneStunts.Add(sceneStunt);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddOther([FromBody] SceneOther sceneOther)
        {
            try
            {


                _context.sceneOthers.Add(sceneOther);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetAgencyNameByActorId(int id)
        {
            try
            {


                var agency = _context.Actors.Where(x => x.Id == id).Join(_context.Agencies, actor => actor.AgencyId, agency => agency.Id, (actor, agency) => new { agency.Name, actor }).FirstOrDefault();
                if (agency is null)
                {
                    return Ok("-");
                }
                return Ok(agency.Name);
            }
            catch(Exception ex)
            {
                throw;
                }
        }
        [HttpGet]
        public async Task<IActionResult> GetAgencyNameByTalentId(int id)
        {
            try
            {


                var agency = _context.Talents.Where(x => x.Id == id).Join(_context.Agencies, actor => actor.AgencyId, agency => agency.Id, (actor, agency) => new { agency.Name, actor }).FirstOrDefault();
                if(agency is null)
                {
                    return Ok("-");
                }
                return Ok(agency.Name);
            }
            catch(Exception ex)
            {
                throw;
                }
        }
            [HttpGet]
        public async Task<IActionResult> DuplicateScene(int id)
        {
            try
            {
                var Scene = _context.Scenes.Where(x => x.Id == id).FirstOrDefault();
                Scene.Id = 0;
                Scene.Index = Scene.Index + "b";
                _context.Scenes.Add(Scene);
                await _context.SaveChangesAsync();

                var _Char = _context.sceneCharacters.Where(x => x.SceneId == id).ToList();
                foreach (var c in _Char)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneCharacters.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _Extra = _context.scenesExtras.Where(x => x.SceneId == id).ToList();
                foreach (var c in _Extra)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.scenesExtras.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _Construction = _context.sceneConstructions.Where(x => x.SceneId == id).ToList();
                foreach (var c in _Construction)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneConstructions.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _dressings = _context.sceneDressings.Where(x => x.SceneId == id).ToList();
                foreach (var c in _dressings)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneDressings.Add(c);
                    await _context.SaveChangesAsync();
                }
                var _Prop = _context.sceneProps.Where(x => x.SceneId == id).ToList();
                foreach (var c in _Prop)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneProps.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _graphics = _context.sceneGraphics.Where(x => x.SceneId == id).ToList();
                foreach (var c in _graphics)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneGraphics.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _vehicles = _context.sceneVehicles.Where(x => x.SceneId == id).ToList();
                foreach (var c in _vehicles)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneVehicles.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _animals = _context.sceneAnimals.Where(x => x.SceneId == id).ToList();
                foreach (var c in _animals)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneAnimals.Add(c);
                    await _context.SaveChangesAsync();
                }


                var _visualEffects = _context.sceneVisuals.Where(x => x.SceneId == id).ToList();
                foreach (var c in _visualEffects)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneVisuals.Add(c);
                    await _context.SaveChangesAsync();
                }



                var _specialEffects = _context.sceneSpecials.Where(x => x.SceneId == id).ToList();
                foreach (var c in _specialEffects)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneSpecials.Add(c);
                    await _context.SaveChangesAsync();
                }


                var _sound = _context.sceneSounds.Where(x => x.SceneId == id).ToList();
                foreach (var c in _sound)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneSounds.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _cameras = _context.sceneCameras.Where(x => x.SceneId == id).ToList();
                foreach (var c in _cameras)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneCameras.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _stunts = _context.sceneStunts.Where(x => x.SceneId == id).ToList();
                foreach (var c in _stunts)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneStunts.Add(c);
                    await _context.SaveChangesAsync();
                }


                var _others = _context.sceneOthers.Where(x => x.SceneId == id).ToList();
                foreach (var c in _others)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneOthers.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _cosutmes = _context.SceneCostumes.Where(x => x.SceneId == id).ToList();
                foreach (var c in _cosutmes)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.SceneCostumes.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _makeup = _context.sceneMakeups.Where(x => x.SceneId == id).ToList();
                foreach (var c in _makeup)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneMakeups.Add(c);
                    await _context.SaveChangesAsync();
                }



                var dbList = _context.DocumentFiles.Where(x => x.SceneId == id).ToList();

                foreach (var db in dbList)
                {
                    db.Id = 0;
                    db.SceneId = Scene.Id;
                    await _context.DocumentFiles.AddAsync(db);
                    await _context.SaveChangesAsync();

                }

                var task = _context.ProjectTasks.Where(x => x.SceneId == id).ToList();

                foreach (var db in task)
                {
                    db.Id = 0;
                    db.SceneId = Scene.Id;
                    var assignedTo = _context.ProjectTaskAssignedTo.Where(x => x.ProjectTaskId == db.Id).ToList();
                    await _context.ProjectTasks.AddAsync(db);

                    await _context.SaveChangesAsync();

                    foreach (var a in assignedTo)
                    {
                        a.ProjectTaskId = db.Id;
                        a.Id = 0;
                        await _context.ProjectTaskAssignedTo.AddAsync(a);

                        await _context.SaveChangesAsync();
                    }

                }


                var comments = _context.Comments.Where(x => x.SceneId == id).ToList();
                foreach (var c in comments)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.Comments.Add(c);
                    await _context.SaveChangesAsync();
                }

                var shots = _context.Shots.Where(x => x.SceneId == id).ToList();
                foreach (var s in shots)
                {
                    var _shotDetails = _context.ShotDetails.Where(x => x.ShotId == s.Id).ToList();

                    s.Id = 0;
                    s.SceneId = Scene.Id;
                    _context.Shots.Add(s);
                    await _context.SaveChangesAsync();
                    foreach (var sd in _shotDetails)
                    {
                        sd.Id = 0;
                        sd.ShotId = s.Id;
                        _context.ShotDetails.Add(sd);
                        await _context.SaveChangesAsync();

                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> SplitScene(int id)
        {
            try
            {
                var Scene = _context.Scenes.Where(x => x.Id == id).FirstOrDefault();
                var index = Scene.Index;
                Scene.Index += 'a';
                _context.Entry(Scene).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                Scene.Id = 0;
                Scene.Index = index + 'b';
                _context.Scenes.Add(Scene);
                await _context.SaveChangesAsync();

                var _Char = _context.sceneCharacters.Where(x => x.SceneId == id).ToList();
                foreach (var c in _Char)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneCharacters.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _Extra = _context.scenesExtras.Where(x => x.SceneId == id).ToList();
                foreach (var c in _Extra)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.scenesExtras.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _Construction = _context.sceneConstructions.Where(x => x.SceneId == id).ToList();
                foreach (var c in _Construction)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneConstructions.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _dressings = _context.sceneDressings.Where(x => x.SceneId == id).ToList();
                foreach (var c in _dressings)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneDressings.Add(c);
                    await _context.SaveChangesAsync();
                }
                var _Prop = _context.sceneProps.Where(x => x.SceneId == id).ToList();
                foreach (var c in _Prop)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneProps.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _graphics = _context.sceneGraphics.Where(x => x.SceneId == id).ToList();
                foreach (var c in _graphics)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneGraphics.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _vehicles = _context.sceneVehicles.Where(x => x.SceneId == id).ToList();
                foreach (var c in _vehicles)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneVehicles.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _animals = _context.sceneAnimals.Where(x => x.SceneId == id).ToList();
                foreach (var c in _animals)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneAnimals.Add(c);
                    await _context.SaveChangesAsync();
                }


                var _visualEffects = _context.sceneVisuals.Where(x => x.SceneId == id).ToList();
                foreach (var c in _visualEffects)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneVisuals.Add(c);
                    await _context.SaveChangesAsync();
                }



                var _specialEffects = _context.sceneSpecials.Where(x => x.SceneId == id).ToList();
                foreach (var c in _specialEffects)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneSpecials.Add(c);
                    await _context.SaveChangesAsync();
                }


                var _sound = _context.sceneSounds.Where(x => x.SceneId == id).ToList();
                foreach (var c in _sound)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneSounds.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _cameras = _context.sceneCameras.Where(x => x.SceneId == id).ToList();
                foreach (var c in _cameras)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneCameras.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _stunts = _context.sceneStunts.Where(x => x.SceneId == id).ToList();
                foreach (var c in _stunts)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneStunts.Add(c);
                    await _context.SaveChangesAsync();
                }


                var _others = _context.sceneOthers.Where(x => x.SceneId == id).ToList();
                foreach (var c in _others)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneOthers.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _cosutmes = _context.SceneCostumes.Where(x => x.SceneId == id).ToList();
                foreach (var c in _cosutmes)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.SceneCostumes.Add(c);
                    await _context.SaveChangesAsync();
                }

                var _makeup = _context.sceneMakeups.Where(x => x.SceneId == id).ToList();
                foreach (var c in _makeup)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneMakeups.Add(c);
                    await _context.SaveChangesAsync();
                }



                var dbList = _context.DocumentFiles.Where(x => x.SceneId == id).ToList();

                foreach (var db in dbList)
                {
                    db.Id = 0;
                    db.SceneId = Scene.Id;
                    await _context.DocumentFiles.AddAsync(db);
                    await _context.SaveChangesAsync();

                }

                var task = _context.ProjectTasks.Where(x => x.SceneId == id).ToList();

                foreach (var db in task)
                {
                    db.Id = 0;
                    db.SceneId = Scene.Id;
                    var assignedTo = _context.ProjectTaskAssignedTo.Where(x => x.ProjectTaskId == db.Id).ToList();
                    await _context.ProjectTasks.AddAsync(db);

                    await _context.SaveChangesAsync();

                    foreach (var a in assignedTo)
                    {
                        a.ProjectTaskId = db.Id;
                        a.Id = 0;
                        await _context.ProjectTaskAssignedTo.AddAsync(a);

                        await _context.SaveChangesAsync();
                    }

                }


                var comments = _context.Comments.Where(x => x.SceneId == id).ToList();
                foreach (var c in comments)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.Comments.Add(c);
                    await _context.SaveChangesAsync();
                }



                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> MergeScene([FromBody] List<int> id)
        {
            try
            {
                var Scene = _context.Scenes.Where(x => x.Id == id[1]).FirstOrDefault();


                var _Char1 = _context.sceneCharacters.Where(x => x.SceneId == id[0]).AsNoTracking().ToList();
                var _Char2 = _context.sceneCharacters.Where(x => x.SceneId == id[1]).AsNoTracking().ToList();

                var _Char = _Char1.Union(_Char2).ToList();
                _Char = _Char.GroupBy(x => new { x.CharacterId }).Select(a => new SceneCharacter() { CharacterId = a.Key.CharacterId }).ToList();

                foreach (var c in _Char)
                {


                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneCharacters.Add(c);
                    await _context.SaveChangesAsync();
                }

                foreach (var c in _Char2)
                {
                    _context.sceneCharacters.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _Extra1 = _context.scenesExtras.Where(x => x.SceneId == id[0]).ToList();
                var _Extra2 = _context.scenesExtras.Where(x => x.SceneId == id[1]).ToList();
                var _Extra = _Extra1.Union(_Extra2).ToList();
                _Extra = _Extra.GroupBy(x => new { x.ExtraId }).Select(a => new ScenesExtra() { ExtraId = a.Key.ExtraId }).ToList();
                foreach (var c in _Extra)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.scenesExtras.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _Extra2)
                {
                    _context.scenesExtras.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _Construction1 = _context.sceneConstructions.Where(x => x.SceneId == id[0]).ToList();
                var _Construction2 = _context.sceneConstructions.Where(x => x.SceneId == id[1]).ToList();
                var _Construction = _Construction1.Union(_Construction2).ToList();
                _Construction = _Construction.GroupBy(x => new { x.ConstructionId }).Select(a => new SceneConstruction() { ConstructionId = a.Key.ConstructionId }).ToList();
                foreach (var c in _Construction)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneConstructions.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _Construction2)
                {
                    _context.sceneConstructions.Remove(c);
                    await _context.SaveChangesAsync();
                }

                var _dressings1 = _context.sceneDressings.Where(x => x.SceneId == id[0]).ToList();
                var _dressings2 = _context.sceneDressings.Where(x => x.SceneId == id[0]).ToList();
                var _dressings = _dressings1.Union(_dressings2).ToList();
                _dressings = _dressings.GroupBy(x => new { x.DressingId }).Select(a => new SceneDressing() { DressingId = a.Key.DressingId }).ToList();

                foreach (var c in _dressings)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneDressings.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _dressings2)
                {
                    _context.sceneDressings.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _Prop1 = _context.sceneProps.Where(x => x.SceneId == id[0]).ToList();
                var _Prop2 = _context.sceneProps.Where(x => x.SceneId == id[1]).ToList();
                var _Prop = _Prop1.Union(_Prop2).ToList();
                _Prop = _Prop.GroupBy(x => new { x.PropsId }).Select(a => new SceneProps() { PropsId = a.Key.PropsId }).ToList();

                foreach (var c in _Prop)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneProps.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _Prop2)
                {
                    _context.sceneProps.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _graphics1 = _context.sceneGraphics.Where(x => x.SceneId == id[0]).ToList();
                var _graphics2 = _context.sceneGraphics.Where(x => x.SceneId == id[1]).ToList();
                var _graphics = _graphics1.Union(_graphics2).ToList();
                _graphics = _graphics.GroupBy(x => new { x.GraphicId }).Select(a => new SceneGraphic() { GraphicId = a.Key.GraphicId }).ToList();

                foreach (var c in _graphics)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneGraphics.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _graphics2)
                {
                    _context.sceneGraphics.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _vehicles1 = _context.sceneVehicles.Where(x => x.SceneId == id[0]).ToList();
                var _vehicles2 = _context.sceneVehicles.Where(x => x.SceneId == id[1]).ToList();
                var _vehicles = _vehicles1.Union(_vehicles2).ToList();
                _vehicles = _vehicles.GroupBy(x => new { x.VehicleId }).Select(a => new SceneVehicle() { VehicleId = a.Key.VehicleId }).ToList();

                foreach (var c in _vehicles)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneVehicles.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _vehicles2)
                {
                    _context.sceneVehicles.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _animals1 = _context.sceneAnimals.Where(x => x.SceneId == id[0]).ToList();
                var _animals2 = _context.sceneAnimals.Where(x => x.SceneId == id[1]).ToList();
                var _animals = _animals1.Union(_animals2).ToList();
                _animals = _animals.GroupBy(x => new { x.AnimalId }).Select(a => new SceneAnimal() { AnimalId = a.Key.AnimalId }).ToList();

                foreach (var c in _animals)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneAnimals.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _animals2)
                {
                    _context.sceneAnimals.Remove(c);
                    await _context.SaveChangesAsync();
                }

                var _visualEffects1 = _context.sceneVisuals.Where(x => x.SceneId == id[0]).ToList();
                var _visualEffects2 = _context.sceneVisuals.Where(x => x.SceneId == id[1]).ToList();
                var _visualEffects = _visualEffects1.Union(_visualEffects2).ToList();
                _visualEffects = _visualEffects.GroupBy(x => new { x.VisualId }).Select(a => new SceneVisual() { VisualId = a.Key.VisualId }).ToList();

                foreach (var c in _visualEffects)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneVisuals.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _visualEffects2)
                {
                    _context.sceneVisuals.Remove(c);
                    await _context.SaveChangesAsync();
                }


                var _specialEffects1 = _context.sceneSpecials.Where(x => x.SceneId == id[0]).ToList();
                var _specialEffects2 = _context.sceneSpecials.Where(x => x.SceneId == id[1]).ToList();
                var _specialEffects = _specialEffects1.Union(_specialEffects2).ToList();
                _specialEffects = _specialEffects.GroupBy(x => new { x.SpecialId }).Select(a => new SceneSpecial() { SpecialId = a.Key.SpecialId }).ToList();

                foreach (var c in _specialEffects)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneSpecials.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _specialEffects2)
                {
                    _context.sceneSpecials.Remove(c);
                    await _context.SaveChangesAsync();
                }


                var _sound1 = _context.sceneSounds.Where(x => x.SceneId == id[0]).ToList();
                var _sound2 = _context.sceneSounds.Where(x => x.SceneId == id[1]).ToList();
                var _sound = _sound1.Union(_sound2).ToList();
                _sound = _sound.GroupBy(x => new { x.SoundId }).Select(a => new SceneSound() { SoundId = a.Key.SoundId }).ToList();

                foreach (var c in _sound)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneSounds.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _sound2)
                {
                    _context.sceneSounds.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _cameras1 = _context.sceneCameras.Where(x => x.SceneId == id[0]).ToList();
                var _cameras2 = _context.sceneCameras.Where(x => x.SceneId == id[1]).ToList();
                var _cameras = _cameras1.Union(_cameras2).ToList();
                _cameras = _cameras.GroupBy(x => new { x.CameraId }).Select(a => new SceneCamera() { CameraId = a.Key.CameraId }).ToList();

                foreach (var c in _cameras)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneCameras.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _cameras2)
                {
                    _context.sceneCameras.Remove(c);
                    await _context.SaveChangesAsync();
                }

                var _stunts1 = _context.sceneStunts.Where(x => x.SceneId == id[0]).ToList();
                var _stunts2 = _context.sceneStunts.Where(x => x.SceneId == id[1]).ToList();
                var _stunts = _stunts1.Union(_stunts2).ToList();
                _stunts = _stunts.GroupBy(x => new { x.StuntId }).Select(a => new SceneStunt() { StuntId = a.Key.StuntId }).ToList();

                foreach (var c in _stunts)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneStunts.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _stunts2)
                {
                    _context.sceneStunts.Remove(c);
                    await _context.SaveChangesAsync();
                }

                var _others1 = _context.sceneOthers.Where(x => x.SceneId == id[0]).ToList();
                var _others2 = _context.sceneOthers.Where(x => x.SceneId == id[1]).ToList();
                var _others = _others1.Union(_others2).ToList();
                _others = _others.GroupBy(x => new { x.OtherId }).Select(a => new SceneOther() { OtherId = a.Key.OtherId }).ToList();

                foreach (var c in _others)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneOthers.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _others2)
                {
                    _context.sceneOthers.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _cosutmes1 = _context.SceneCostumes.Where(x => x.SceneId == id[0]).ToList();
                var _cosutmes2 = _context.SceneCostumes.Where(x => x.SceneId == id[1]).ToList();
                var _cosutmes = _cosutmes1.Union(_cosutmes2).ToList();
                _cosutmes = _cosutmes.GroupBy(x => new { x.CostumeId, x }).Select(a => a.Key.x).ToList();

                foreach (var c in _cosutmes)
                {

                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.SceneCostumes.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _cosutmes2)
                {
                    _context.SceneCostumes.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _makeup1 = _context.sceneMakeups.Where(x => x.SceneId == id[0]).ToList();
                var _makeup2 = _context.sceneMakeups.Where(x => x.SceneId == id[1]).ToList();
                var _makeup = _makeup1.Union(_makeup2).ToList();
                _makeup = _makeup.GroupBy(x => new { x.MakeupId, x }).Select(a => a.Key.x).ToList();

                foreach (var c in _makeup)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.sceneMakeups.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _makeup2)
                {
                    _context.sceneMakeups.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var _shot1 = _context.Shots.Where(x => x.SceneId == id[0]).ToList();
                var _shot2 = _context.Shots.Where(x => x.SceneId == id[1]).ToList();
                var _shot = _shot1.Union(_shot2).ToList();
                var Shot = _shot.GroupBy(x => new { x.SceneId, x }).Select(a => a.Key.x).ToList();

                foreach (var c in Shot)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.Shots.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in _shot2)
                {
                    _context.Shots.Remove(c);
                    await _context.SaveChangesAsync();
                }



                var dbList1 = _context.DocumentFiles.Where(x => x.SceneId == id[0]).ToList();
                var dbList2 = _context.DocumentFiles.Where(x => x.SceneId == id[1]).ToList();
                var dbList = dbList1.Union(dbList2).ToList();
                dbList = dbList.GroupBy(x => new { x.FileId, x }).Select(a => a.Key.x).ToList();
                foreach (var db in dbList)
                {
                    db.Id = 0;
                    db.SceneId = Scene.Id;
                    await _context.DocumentFiles.AddAsync(db);
                    await _context.SaveChangesAsync();

                }
                foreach (var c in dbList2)
                {

                    _context.DocumentFiles.Remove(c);
                    await _context.SaveChangesAsync();
                }

                var task1 = _context.ProjectTasks.Where(x => x.SceneId == id[0]).ToList();
                var task2 = _context.ProjectTasks.Where(x => x.SceneId == id[1]).ToList();
                var task = task1.Union(task2).ToList();
                //var _task = task.GroupBy(x =>  x.Id ).ToList();
                var _task =
     from t in task
     group t by new
     {
         Id = t.SceneId,
         X = t,
     } into nt
     select nt;
                foreach (var db in _task)
                {
                    db.Key.X.Id = 0;
                    db.Key.X.SceneId = Scene.Id;
                    var assignedTo = _context.ProjectTaskAssignedTo.Where(x => x.ProjectTaskId == db.Key.X.Id).ToList();
                    await _context.ProjectTasks.AddAsync(db.Key.X);

                    await _context.SaveChangesAsync();

                    foreach (var a in assignedTo)
                    {
                        a.ProjectTaskId = db.Key.X.Id;
                        a.Id = 0;
                        await _context.ProjectTaskAssignedTo.AddAsync(a);

                        await _context.SaveChangesAsync();
                    }

                }
                foreach (var c in task2)
                {

                    _context.ProjectTasks.Remove(c);
                    await _context.SaveChangesAsync();
                }


                var comments1 = _context.Comments.Where(x => x.SceneId == id[0]).ToList();
                var comments2 = _context.Comments.Where(x => x.SceneId == id[1]).ToList();
                var comments = comments1.Union(comments2).ToList();
                comments = comments.GroupBy(x => new { x.Id, x }).Select(a => a.Key.x).ToList();

                foreach (var c in comments)
                {
                    c.Id = 0;
                    c.SceneId = Scene.Id;
                    _context.Comments.Add(c);
                    await _context.SaveChangesAsync();
                }
                foreach (var c in comments2)
                {

                    _context.Comments.Remove(c);
                    await _context.SaveChangesAsync();
                }
                var Scene1 = _context.Scenes.Where(x => x.Id == id[0]).FirstOrDefault();
                Scene1.isDeleted = true;
                _context.Entry(Scene1).State = EntityState.Modified;
                await _context.SaveChangesAsync();



                return Ok(Scene.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetLinksByActorId(int id)
        {
            try
            {
                var links = _context.link.Where(x => x.ActorId == id && x.Is_Deleted == false).ToList();
                return Ok(links);

                
            }catch(Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetLinksByTalentId(int id)
        {
            try
            {
                var links = _context.link.Where(x => x.TalentId == id && x.Is_Deleted == false).ToList();
                return Ok(links);


            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpGet("{id}")]
        public async Task<ActionResult> GetPeriodsByActorId(int id)
        {
            try
            {
                var periods = _context.offperiods.Where(x => x.ActorId == id && x.Is_Deleted == false).ToList();
                return Ok(periods);


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPeriodsByTalentId(int id)
        {
            try
            {
                var periods = _context.offperiods.Where(x => x.TalentId == id && x.Is_Deleted == false).ToList();
                return Ok(periods);


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreateLink([FromBody] link link)
        {
            try
            {
                if (link.Id == 0)
                {
                    _context.link.Add(link);
                    await _context.SaveChangesAsync();
                    return Ok(link);
                }
                else
                {
                    _context.Entry(link).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(link);
                }
            }catch(Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreatePeriod([FromBody] offperiod period)
        {
            try
            {
                if (period.Id == 0)
                {
                    _context.offperiods.Add(period);
                    await _context.SaveChangesAsync();
                    return Ok(period);
                }
                else
                {
                    _context.Entry(period).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(period);
                }
            }catch(Exception ex)
            {
                throw;
            }
        }

        [HttpGet("{chid}/{chid2}")]
        public async Task<ActionResult> MergeCharacter(int chid,int chid2)
        {
            try
            {
                var scenechr = _context.sceneCharacters.Where(x => x.CharacterId == chid).ToList();
                foreach (var _chr in scenechr)
                {
                    var check = _context.sceneCharacters.Where(x => x.CharacterId == chid2 && x.SceneId==_chr.SceneId).ToList();
                    if (check.Count == 0)
                    {
                        _chr.CharacterId = chid2;

                        _context.Entry(_chr).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }

                var Costumechr = _context.SceneCostumes.Where(x => x.CharacterId == chid).ToList();
                foreach (var _chr in Costumechr)
                {
                    var check = _context.SceneCostumes.Where(x => x.CharacterId == chid2 && x.SceneId == _chr.SceneId).ToList();
                    if (check.Count == 0)
                    {
                        _chr.CharacterId = chid2;

                        _context.Entry(_chr).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
                var Makeupchr = _context.sceneMakeups.Where(x => x.CharacterId == chid).ToList();
                foreach (var _chr in Makeupchr)
                {
                    var check = _context.sceneMakeups.Where(x => x.CharacterId == chid2 && x.SceneId == _chr.SceneId).ToList();
                    if (check.Count == 0)
                    {
                        _chr.CharacterId = chid2;

                        _context.Entry(_chr).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
                var charT = _context.CharactersTalents.Where(x => x.CharId == chid && x.Is_CastFixed == true).ToList();
                var TargetActors = _context.CharactersTalents.Where(x => x.CharId == chid2 && x.Is_CastFixed == true).ToList();
                if (TargetActors.Count == 0)
                {
                    foreach (var _chr in charT)
                    {
                     
                            _chr.CharId = chid2;

                            _context.Entry(_chr).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        
                    }
                }
                var tasks = _context.ProjectTasks.Where(x => x.CharId == chid).ToList();
                foreach (var _chr in tasks)
                {
                    _chr.CharId = chid2;

                    _context.Entry(_chr).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                var comments = _context.Comments.Where(x => x.CharId == chid).ToList();
                foreach (var _chr in comments)
                {
                    _chr.CharId = chid2;

                    _context.Entry(_chr).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                var doc = _context.DocumentFiles.Where(x => x.CharId == chid).ToList();
                foreach (var _chr in doc)
                {
                    _chr.CharId = chid2;

                    _context.Entry(_chr).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }catch(Exception ex)
            {
                throw;
            }
        }



        [HttpGet("{chid}/{chid2}")]
        public async Task<ActionResult> MergeExtra(int chid, int chid2)
        {
            try
            {
                var scenechr = _context.scenesExtras.Where(x => x.ExtraId == chid).ToList();
                foreach (var _chr in scenechr)
                {
                    var check = _context.scenesExtras.Where(x => x.ExtraId == chid2 && x.SceneId == _chr.SceneId).ToList();
                    if (check.Count == 0)
                    {
                        _chr.ExtraId = chid2;

                        _context.Entry(_chr).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }

                var Costumechr = _context.SceneCostumes.Where(x => x.ExtraId == chid).ToList();
                foreach (var _chr in Costumechr)
                {
                    var check = _context.SceneCostumes.Where(x => x.ExtraId == chid2 && x.SceneId == _chr.SceneId).ToList();
                    if (check.Count == 0)
                    {
                        _chr.ExtraId = chid2;

                        _context.Entry(_chr).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
                var Makeupchr = _context.sceneMakeups.Where(x => x.ExtraId == chid).ToList();
                foreach (var _chr in Makeupchr)
                {
                    var check = _context.sceneMakeups.Where(x => x.ExtraId == chid2 && x.SceneId == _chr.SceneId).ToList();
                    if (check.Count == 0)
                    {
                        _chr.ExtraId = chid2;

                        _context.Entry(_chr).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
                var charT = _context.CharactersTalents.Where(x => x.ExtraId == chid && x.Is_CastFixed == true).ToList();
                var TargetActors = _context.CharactersTalents.Where(x => x.ExtraId == chid2 && x.Is_CastFixed == true).ToList();
                if (TargetActors.Count == 0)
                {
                    foreach (var _chr in charT)
                    {
                       
                            _chr.ExtraId = chid2;

                            _context.Entry(_chr).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        
                    }
                }

                var tasks = _context.ProjectTasks.Where(x => x.ExtraId == chid).ToList();
                foreach (var _chr in tasks)
                {
                    _chr.ExtraId = chid2;

                    _context.Entry(_chr).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                var comments = _context.Comments.Where(x => x.ExtraId == chid).ToList();
                foreach (var _chr in comments)
                {
                    _chr.ExtraId = chid2;

                    _context.Entry(_chr).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                var doc = _context.DocumentFiles.Where(x => x.ExtraId == chid).ToList();
                foreach (var _chr in doc)
                {
                    _chr.ExtraId = chid2;

                    _context.Entry(_chr).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{SceneId}/{page}/{size}")]
        public async Task<ActionResult> GetSceneFiles(int SceneId, int page, int size)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                where d.SceneId == SceneId
                                select new DocumentFilesDto()
                                {
                                    ContentType = d.ContentType,
                                    DocumentCategoryId = d.DocumentCategoryId.Value,
                                    Extension = d.Extension,
                                    FileId = d.FileId,
                                    Id = d.Id,
                                    MimeType = d.MimeType,
                                    Name = d.Name,
                                    Type = d.Type,
                                    UserFriendlySize = d.UserFriendlySize,
                                    UserId = d.UserId,
                                    UserName = user.GetUserName(),
                                    RelativeTime = d.Created.GetRelativeTime(),
                                    UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                    Default = d.Default,

                                }).GetPaged(page, size);

            var defaultImageId = await _context.DocumentFiles.Where(a => a.Default && a.SceneId == SceneId).FirstOrDefaultAsync();

            var list = from d in dbList
                       let latestVersion = (from v in _context.VersionFiles
                                            where d.Id == v.DocumentFileId
                                            //orderby d.CreateAtTicks ascending

                                            select v

                                                   ).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()


                       select new DocumentFilesDto()
                       {
                           ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                           DocumentCategoryId = d.DocumentCategoryId,
                           Extension = latestVersion == null ? d.Extension : latestVersion.Extension,
                           FileId = latestVersion == null ? d.FileId : latestVersion.FileId,
                           Id = d.Id,
                           MimeType = latestVersion == null ? d.MimeType : latestVersion.MimeType,
                           Name = latestVersion == null ? d.Name : latestVersion.Name,
                           Type = latestVersion == null ? d.Type : latestVersion.Type,
                           UserFriendlySize = latestVersion == null ? d.UserFriendlySize : latestVersion.UserFriendlySize,
                           UserId = latestVersion == null ? d.UserId : latestVersion.UserId,
                           UserName = latestVersion == null ? d.UserName : d.UserName,
                           RelativeTime = latestVersion == null ? d.RelativeTime : latestVersion.Created.GetRelativeTime(),
                           UserFriendlyTime = latestVersion == null ? d.UserFriendlyTime : latestVersion.Created.ToUserFriendlyTime(),
                           Default = d.Default,
                           LatestVersion = latestVersion == null ? string.Empty : latestVersion.Version
                       };

            return Ok(new { list, defaultImageId?.FileId });
        }
        [HttpGet("{TalentId}/{page}/{size}")]
        public async Task<ActionResult> GetTalentFiles(int TalentId, int page, int size)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                where d.TalentId == TalentId
                                select new DocumentFilesDto()
                                {
                                    ContentType = d.ContentType,
                                    DocumentCategoryId = d.DocumentCategoryId.Value,
                                    Extension = d.Extension,
                                    FileId = d.FileId,
                                    Id = d.Id,
                                    MimeType = d.MimeType,
                                    Name = d.Name,
                                    Type = d.Type,
                                    UserFriendlySize = d.UserFriendlySize,
                                    UserId = d.UserId,
                                    UserName = user.GetUserName(),
                                    RelativeTime = d.Created.GetRelativeTime(),
                                    UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                    Default = d.Default,

                                }).GetPaged(page, size);

            var defaultImageId = await _context.DocumentFiles.Where(a => a.Default && a.TalentId == TalentId).FirstOrDefaultAsync();

            var list = from d in dbList
                       let latestVersion = (from v in _context.VersionFiles
                                            where d.Id == v.DocumentFileId
                                            //orderby d.CreateAtTicks ascending

                                            select v

                                                   ).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()


                       select new DocumentFilesDto()
                       {
                           ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                           DocumentCategoryId = d.DocumentCategoryId,
                           Extension = latestVersion == null ? d.Extension : latestVersion.Extension,
                           FileId = latestVersion == null ? d.FileId : latestVersion.FileId,
                           Id = d.Id,
                           MimeType = latestVersion == null ? d.MimeType : latestVersion.MimeType,
                           Name = latestVersion == null ? d.Name : latestVersion.Name,
                           Type = latestVersion == null ? d.Type : latestVersion.Type,
                           UserFriendlySize = latestVersion == null ? d.UserFriendlySize : latestVersion.UserFriendlySize,
                           UserId = latestVersion == null ? d.UserId : latestVersion.UserId,
                           UserName = latestVersion == null ? d.UserName : d.UserName,
                           RelativeTime = latestVersion == null ? d.RelativeTime : latestVersion.Created.GetRelativeTime(),
                           UserFriendlyTime = latestVersion == null ? d.UserFriendlyTime : latestVersion.Created.ToUserFriendlyTime(),
                           Default = d.Default,
                           LatestVersion = latestVersion == null ? string.Empty : latestVersion.Version
                       };

            return Ok(new { list, defaultImageId?.FileId });
        }
        [HttpGet("{ActorId}/{page}/{size}")]
        public async Task<ActionResult> GetActorFiles(int ActorId, int page, int size)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                where d.ActorId == ActorId
                                select new DocumentFilesDto()
                                {
                                    ContentType = d.ContentType,
                                    DocumentCategoryId = d.DocumentCategoryId.Value,
                                    Extension = d.Extension,
                                    FileId = d.FileId,
                                    Id = d.Id,
                                    MimeType = d.MimeType,
                                    Name = d.Name,
                                    Type = d.Type,
                                    UserFriendlySize = d.UserFriendlySize,
                                    UserId = d.UserId,
                                    UserName = user.GetUserName(),
                                    RelativeTime = d.Created.GetRelativeTime(),
                                    UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                    Default = d.Default,

                                }).GetPaged(page, size);

            var defaultImageId = await _context.DocumentFiles.Where(a => a.Default && a.ActorId == ActorId).FirstOrDefaultAsync();

            var list = from d in dbList
                       let latestVersion = (from v in _context.VersionFiles
                                            where d.Id == v.DocumentFileId
                                            //orderby d.CreateAtTicks ascending

                                            select v

                                                   ).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()


                       select new DocumentFilesDto()
                       {
                           ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                           DocumentCategoryId = d.DocumentCategoryId,
                           Extension = latestVersion == null ? d.Extension : latestVersion.Extension,
                           FileId = latestVersion == null ? d.FileId : latestVersion.FileId,
                           Id = d.Id,
                           MimeType = latestVersion == null ? d.MimeType : latestVersion.MimeType,
                           Name = latestVersion == null ? d.Name : latestVersion.Name,
                           Type = latestVersion == null ? d.Type : latestVersion.Type,
                           UserFriendlySize = latestVersion == null ? d.UserFriendlySize : latestVersion.UserFriendlySize,
                           UserId = latestVersion == null ? d.UserId : latestVersion.UserId,
                           UserName = latestVersion == null ? d.UserName : d.UserName,
                           RelativeTime = latestVersion == null ? d.RelativeTime : latestVersion.Created.GetRelativeTime(),
                           UserFriendlyTime = latestVersion == null ? d.UserFriendlyTime : latestVersion.Created.ToUserFriendlyTime(),
                           Default = d.Default,
                           LatestVersion = latestVersion == null ? string.Empty : latestVersion.Version
                       };

            return Ok(new { list, defaultImageId?.FileId });
        }
        [HttpGet("{CharId}/{page}/{size}")]
        public async Task<ActionResult> GetCharFiles(int CharId, int page, int size)
        {
            try
            {
                var dbList = await (from d in _context.DocumentFiles
                                    join user in _context.Users
                                    on d.UserId equals user.Id
                                    where d.CharId == CharId
                                    select new DocumentFilesDto()
                                    {
                                        ContentType = d.ContentType,
                                        DocumentCategoryId = d.DocumentCategoryId.Value,
                                        Extension = d.Extension,
                                        FileId = d.FileId,
                                        Id = d.Id,
                                        MimeType = d.MimeType,
                                        Name = d.Name,
                                        Type = d.Type,
                                        UserFriendlySize = d.UserFriendlySize,
                                        UserId = d.UserId,
                                        UserName = user.GetUserName(),
                                        RelativeTime = d.Created.GetRelativeTime(),
                                        UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                        Default = d.Default,

                                    }).GetPaged(page, size);

                var defaultImageId = await _context.DocumentFiles.Where(a => a.Default && a.CharId == CharId).FirstOrDefaultAsync();

                var list = from d in dbList
                           let latestVersion = (from v in _context.VersionFiles
                                                where d.Id == v.DocumentFileId
                                                //orderby d.CreateAtTicks ascending

                                                select v

                                                       ).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()


                           select new DocumentFilesDto()
                           {
                               ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                               DocumentCategoryId = d.DocumentCategoryId,
                               Extension = latestVersion == null ? d.Extension : latestVersion.Extension,
                               FileId = latestVersion == null ? d.FileId : latestVersion.FileId,
                               Id = d.Id,
                               MimeType = latestVersion == null ? d.MimeType : latestVersion.MimeType,
                               Name = latestVersion == null ? d.Name : latestVersion.Name,
                               Type = latestVersion == null ? d.Type : latestVersion.Type,
                               UserFriendlySize = latestVersion == null ? d.UserFriendlySize : latestVersion.UserFriendlySize,
                               UserId = latestVersion == null ? d.UserId : latestVersion.UserId,
                               UserName = latestVersion == null ? d.UserName : d.UserName,
                               RelativeTime = latestVersion == null ? d.RelativeTime : latestVersion.Created.GetRelativeTime(),
                               UserFriendlyTime = latestVersion == null ? d.UserFriendlyTime : latestVersion.Created.ToUserFriendlyTime(),
                               Default = d.Default,
                               LatestVersion = latestVersion == null ? string.Empty : latestVersion.Version
                           };

                return Ok(new { list, defaultImageId?.FileId });
            }catch(Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{CharId}/{page}/{size}")]
        public async Task<ActionResult> GetExtraFiles(int CharId, int page, int size)
        {
            try
            {
                var dbList = await (from d in _context.DocumentFiles
                                    join user in _context.Users
                                    on d.UserId equals user.Id
                                    where d.ExtraId == CharId
                                    select new DocumentFilesDto()
                                    {
                                        ContentType = d.ContentType,
                                        DocumentCategoryId = d.DocumentCategoryId.Value,
                                        Extension = d.Extension,
                                        FileId = d.FileId,
                                        Id = d.Id,
                                        MimeType = d.MimeType,
                                        Name = d.Name,
                                        Type = d.Type,
                                        UserFriendlySize = d.UserFriendlySize,
                                        UserId = d.UserId,
                                        UserName = user.GetUserName(),
                                        RelativeTime = d.Created.GetRelativeTime(),
                                        UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                        Default = d.Default,

                                    }).GetPaged(page, size);

                var defaultImageId = await _context.DocumentFiles.Where(a => a.Default && a.ExtraId == CharId).FirstOrDefaultAsync();

                var list = from d in dbList
                           let latestVersion = (from v in _context.VersionFiles
                                                where d.Id == v.DocumentFileId
                                                //orderby d.CreateAtTicks ascending

                                                select v

                                                       ).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()


                           select new DocumentFilesDto()
                           {
                               ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                               DocumentCategoryId = d.DocumentCategoryId,
                               Extension = latestVersion == null ? d.Extension : latestVersion.Extension,
                               FileId = latestVersion == null ? d.FileId : latestVersion.FileId,
                               Id = d.Id,
                               MimeType = latestVersion == null ? d.MimeType : latestVersion.MimeType,
                               Name = latestVersion == null ? d.Name : latestVersion.Name,
                               Type = latestVersion == null ? d.Type : latestVersion.Type,
                               UserFriendlySize = latestVersion == null ? d.UserFriendlySize : latestVersion.UserFriendlySize,
                               UserId = latestVersion == null ? d.UserId : latestVersion.UserId,
                               UserName = latestVersion == null ? d.UserName : d.UserName,
                               RelativeTime = latestVersion == null ? d.RelativeTime : latestVersion.Created.GetRelativeTime(),
                               UserFriendlyTime = latestVersion == null ? d.UserFriendlyTime : latestVersion.Created.ToUserFriendlyTime(),
                               Default = d.Default,
                               LatestVersion = latestVersion == null ? string.Empty : latestVersion.Version
                           };

                return Ok(new { list, defaultImageId?.FileId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{AgencyId}/{page}/{size}")]
        public async Task<ActionResult> GetAgencyFiles(int AgencyId, int page, int size)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                where d.AgencyID == AgencyId
                                select new DocumentFilesDto()
                                {
                                    ContentType = d.ContentType,
                                    DocumentCategoryId = d.DocumentCategoryId.Value,
                                    Extension = d.Extension,
                                    FileId = d.FileId,
                                    Id = d.Id,
                                    MimeType = d.MimeType,
                                    Name = d.Name,
                                    Type = d.Type,
                                    UserFriendlySize = d.UserFriendlySize,
                                    UserId = d.UserId,
                                    UserName = user.GetUserName(),
                                    RelativeTime = d.Created.GetRelativeTime(),
                                    UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                    Default = d.Default,

                                }).GetPaged(page, size);

            var defaultImageId = await _context.DocumentFiles.Where(a => a.Default && a.AgencyID == AgencyId).FirstOrDefaultAsync();

            var list = from d in dbList
                       let latestVersion = (from v in _context.VersionFiles
                                            where d.Id == v.DocumentFileId
                                            //orderby d.CreateAtTicks ascending

                                            select v

                                                   ).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()


                       select new DocumentFilesDto()
                       {
                           ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                           DocumentCategoryId = d.DocumentCategoryId,
                           Extension = latestVersion == null ? d.Extension : latestVersion.Extension,
                           FileId = latestVersion == null ? d.FileId : latestVersion.FileId,
                           Id = d.Id,
                           MimeType = latestVersion == null ? d.MimeType : latestVersion.MimeType,
                           Name = latestVersion == null ? d.Name : latestVersion.Name,
                           Type = latestVersion == null ? d.Type : latestVersion.Type,
                           UserFriendlySize = latestVersion == null ? d.UserFriendlySize : latestVersion.UserFriendlySize,
                           UserId = latestVersion == null ? d.UserId : latestVersion.UserId,
                           UserName = latestVersion == null ? d.UserName : d.UserName,
                           RelativeTime = latestVersion == null ? d.RelativeTime : latestVersion.Created.GetRelativeTime(),
                           UserFriendlyTime = latestVersion == null ? d.UserFriendlyTime : latestVersion.Created.ToUserFriendlyTime(),
                           Default = d.Default,
                           LatestVersion = latestVersion == null ? string.Empty : latestVersion.Version
                       };

            return Ok(new { list, defaultImageId?.FileId });
        }
        [HttpGet("{SceneId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetSceneComments(int SceneId)
        {
            //var comments = await _context.Comments.Where(a => a.AnnouncementId == id).ToListAsync();

            var commentsDto = await (from k in _context.Comments
                                     join u in _context.Users
                                     on k.ApplicationUserId equals u.Id
                                     where k.SceneId == SceneId
                                     select new CommentDto
                                     {
                                         Id = k.Id,
                                         MarkupText = k.MarkupText,
                                         Text = k.Text,
                                         AnnouncementId = k.AnnouncementId,
                                         UserId = k.ApplicationUserId,
                                         Created = k.Created.GetRelativeTime(),
                                         UserName = u.FirstName + " " + u.LastName,
                                         SceneId = k.SceneId

                                     }).ToListAsync();
            if (commentsDto.Count == 0)
            {
                return NotFound();
            }

            return commentsDto;
        }
        [HttpGet("{CharId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCharComments(int CharId)
        {
            //var comments = await _context.Comments.Where(a => a.AnnouncementId == id).ToListAsync();
            try
            {
                var commentsDto = await (from k in _context.Comments
                                         join u in _context.Users
                                         on k.ApplicationUserId equals u.Id
                                         where k.CharId == CharId
                                         select new CommentDto
                                         {
                                             Id = k.Id,
                                             MarkupText = k.MarkupText,
                                             Text = k.Text,
                                             AnnouncementId = k.AnnouncementId,
                                             UserId = k.ApplicationUserId,
                                             Created = k.Created.GetRelativeTime(),
                                             UserName = u.FirstName + " " + u.LastName,
                                             CharId = k.CharId

                                         }).ToListAsync();
                if (commentsDto.Count == 0)
                {
                    return NotFound();
                }

                return commentsDto;
            }
            catch(Exception ex)
            {
                throw;
            }
        }  
        [HttpGet("{CharId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetActorComments(int CharId)
        {
            //var comments = await _context.Comments.Where(a => a.AnnouncementId == id).ToListAsync();
            try
            {
                var commentsDto = await (from k in _context.Comments
                                         join u in _context.Users
                                         on k.ApplicationUserId equals u.Id
                                         where k.ActorId == CharId
                                         select new CommentDto
                                         {
                                             Id = k.Id,
                                             MarkupText = k.MarkupText,
                                             Text = k.Text,
                                             AnnouncementId = k.AnnouncementId,
                                             UserId = k.ApplicationUserId,
                                             Created = k.Created.GetRelativeTime(),
                                             UserName = u.FirstName + " " + u.LastName,
                                             ActorId = k.ActorId

                                         }).ToListAsync();
                if (commentsDto.Count == 0)
                {
                    return NotFound();
                }

                return commentsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{CharId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetTalentComments(int CharId)
        {
            //var comments = await _context.Comments.Where(a => a.AnnouncementId == id).ToListAsync();
            try
            {
                var commentsDto = await (from k in _context.Comments
                                         join u in _context.Users
                                         on k.ApplicationUserId equals u.Id
                                         where k.TalentId == CharId
                                         select new CommentDto
                                         {
                                             Id = k.Id,
                                             MarkupText = k.MarkupText,
                                             Text = k.Text,
                                             AnnouncementId = k.AnnouncementId,
                                             UserId = k.ApplicationUserId,
                                             Created = k.Created.GetRelativeTime(),
                                             UserName = u.FirstName + " " + u.LastName,
                                             TalentId = k.TalentId

                                         }).ToListAsync();
                if (commentsDto.Count == 0)
                {
                    return NotFound();
                }

                return commentsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{CharId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetExtraComments(int CharId)
        {
            //var comments = await _context.Comments.Where(a => a.AnnouncementId == id).ToListAsync();
            try
            {
                var commentsDto = await (from k in _context.Comments
                                         join u in _context.Users
                                         on k.ApplicationUserId equals u.Id
                                         where k.ExtraId == CharId
                                         select new CommentDto
                                         {
                                             Id = k.Id,
                                             MarkupText = k.MarkupText,
                                             Text = k.Text,
                                             AnnouncementId = k.AnnouncementId,
                                             UserId = k.ApplicationUserId,
                                             Created = k.Created.GetRelativeTime(),
                                             UserName = u.FirstName + " " + u.LastName,
                                             ExtraId = k.ExtraId

                                         }).ToListAsync();
                if (commentsDto.Count == 0)
                {
                    return NotFound();
                }

                return commentsDto;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        private async Task<int> GetNextIndex(int projectId)
        {
            return await _context.Database.ExecuteSqlRawAsync(@"select isnull( max(""Index"")+1,2000) from (

select ""Index"", projectId from Constructions
union
select ""Index"", projectId from dressings
union
select ""Index"", projectId from Props
union
select ""Index"", projectId from Graphics
union
select ""Index"", projectId from vehicles )
as t

where t.ProjectId = {0}", projectId);
        }
    }
}

