using Elegium.Data;
using Elegium.Dtos;
using Elegium.Dtos.ProjectDtos;
using Elegium.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OnboardingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public OnboardingController(ApplicationDbContext context
            , UserManager<ApplicationUser> userManager
            , IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetOnBoarding()
        {
            var user = await _userManager.GetUserAsync(User);

            var list = new List<OnBoardingDto>();

            var userProject = await
                _context.Project
                .Where(a => !a.OnBoardingCompleted && a.UserId == user.Id && !a.Deleted)
                .OrderByDescending(a => a.Id).Select(a => new ProjectDto()
                {
                    Name = a.Name,
                    Id = a.Id
                })
                .FirstOrDefaultAsync();

            var projectExists = await _context.Project.CountAsync(a => a.UserId == user.Id && !a.Deleted);

            if(projectExists==0)
                return Ok(new { createNew = true });


            if (userProject == null)
                return Ok(new { complete = true });

            var ProjectCrew = await _context.ProjectCrews.CountAsync(a => a.IsActive && a.ProjectId == userProject.Id);
            var characters = await _context.characters.CountAsync(a => a.Project_Id == userProject.Id);
            var scene = await _context.Scenes.CountAsync(a => !a.isDeleted && a.project_id == userProject.Id);
            var set = await _context.sets.CountAsync(a => a.ProjectId == userProject.Id);
            var announcements = await _context.Announcements.CountAsync(a => !a.Deleted && a.ProjectId == userProject.Id);
            var tasks = await _context.ProjectTasks.CountAsync(a => !a.Deleted && a.ProjectId == userProject.Id);
            var calendar = await _context.Events.CountAsync(a => !a.Deleted && a.ProjectId == userProject.Id);
            var documents = await _context.Documents.CountAsync(a => !a.Deleted && a.ProjectId == userProject.Id);
            #region onboarding
            list.Add(new OnBoardingDto()
            {
                description = "Create Project",
                completed = true,
                font_icon = "CAST",
                name = "casting_scene",
                ignore_as_first_step = false,
                url_hash = "#/onboarding",
                projectId = userProject.Id,
                parent_description = "Onboarding"
            });
            list.Add(new OnBoardingDto()
            {
                description = "Setup Budget and upload Financial Plan",
                completed = true,
                font_icon = "CAST",
                name = "casting_scene",
                ignore_as_first_step = false,
                url_hash = "#/onboarding",
                parent_description = "Onboarding"
            });
            list.Add(new OnBoardingDto()
            {
                description = "Upload Synopsis & Script",
                completed = true,
                font_icon = "SCENESSCRIPT",
                name = "location_scene",
                ignore_as_first_step = false,
                url_hash = "#/onboarding",
                parent_description = "Onboarding"
            });
            #endregion

            #region Team and crew
            list.Add(new OnBoardingDto()
            {
                description = "Assing Production Team",
                completed = true,
                font_icon = "CAST",
                name = "casting",
                ignore_as_first_step = false,
                url_hash = $"#/{userProject.Id}/crew",
                parent_description = "Team and crew"
            });
            list.Add(new OnBoardingDto()
            {
                description = "Assign the Crew",
                completed = ProjectCrew > 0,
                font_icon = "SHOOTINGSCHEDULING",
                name = "shooting_schedule",
                ignore_as_first_step = false,
                url_hash = $"#/{userProject.Id}/crew",
                parent_description = "Team and crew"
            });
            #endregion

            #region Characters
            list.Add(new OnBoardingDto()
            {
                description = "Cast the Characters",
                completed = characters > 0,
                font_icon = "SHOOTINGSCHEDULING",
                name = "shooting_schedule",
                ignore_as_first_step = false,
                url_hash = $"#/{userProject.Id}/cast",
                parent_description = "Characters"
            });
            #endregion

            #region Locations
            list.Add(new OnBoardingDto()
            {
                description = "Assign a location",
                completed = true,
                font_icon = "SHOOTINGSCHEDULING",
                name = "shooting_schedule",
                ignore_as_first_step = false,
                url_hash = "#/onboarding",
                parent_description = "Locations"
            });
            #endregion

            //#region Shooting day
            //list.Add(new OnBoardingDto()
            //{
            //    description = "Draft a shooting day",
            //    completed = true,
            //    font_icon = "SHOOTINGSCHEDULING",
            //    name = "shooting_schedule",
            //    ignore_as_first_step = false,
            //    url_hash = "#/",
            //    parent_description = "Shooting day"
            //});
            //#endregion

            var newList = list
                .GroupBy(a => a.parent_description)
                .Select(g => new { g.Key, total_percentage = Convert.ToInt32(Math.Round(((decimal)g.Count(a => a.completed) / g.Count()) * 100, 0)) })
                .ToArray();

            list.ForEach(a => a.parent_percentage = newList.Where(n => n.Key == a.parent_description).Select(a => a.total_percentage).FirstOrDefault());


            userProject.CompletedVsInProgress = $"{list.Count(a => a.completed)}/{list.Count}";
            userProject.OnboardPercentage = Convert.ToInt32(Math.Round(((decimal)list.Count(a => a.completed) / list.Count) * 100, 0));


            //update the project onnboarding status in db
            var dbProject = await _context.Project.Where(p => p.Id == userProject.Id && !p.Deleted).FirstOrDefaultAsync();

            if (userProject.OnboardPercentage == 100)
            {
                dbProject.OnBoardingCompleted = true;
            }
            else
            {
                dbProject.OnBoardingCompleted = false;
            }

            _context.SaveChanges();

            var lastCompleted = list.OrderBy(a => a.completed ? 0 : 1).Where(a => a.completed).TakeLast(1).SingleOrDefault();

            if (lastCompleted != null)
                lastCompleted.is_last = true;

            var last_step = list.OrderBy(a => a.completed ? 0 : 1).Where(a => !a.completed).Take(1).SingleOrDefault();

            if (last_step != null)
                last_step.next_step = true;



            return
                  Ok(new OnBoardOverlayDto() { Project = userProject, OnBoardingDtoList = list.OrderBy(a => a.completed ? 0 : 1).ToList() });
        }

        [HttpGet("{id}")]
        public async Task<OnBoardOverlayDto> GetAllOnBoarding(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var list = new List<OnBoardingDto>();

            var userProject = _context.Project.Where(a => a.Id == id && !a.Deleted).Select(a => new ProjectDto()
            {
                Name = a.Name,
                Id = a.Id,
                OnboardPercentage = a.OnBoardingPercentage ?? 0,
                OnBoardingCompleted = a.OnBoardingCompleted
            }).FirstOrDefault();

            var ProjectCrew = await _context.ProjectCrews.CountAsync(a => a.IsActive && a.ProjectId == userProject.Id);
            var characters = await _context.characters.CountAsync(a => a.Project_Id == userProject.Id);
            var scene = await _context.Scenes.CountAsync(a => !a.isDeleted && a.project_id == userProject.Id);
            var set = await _context.sets.CountAsync(a => a.ProjectId == userProject.Id);
            var announcements = await _context.Announcements.CountAsync(a => !a.Deleted && a.ProjectId == id);
            var tasks = await _context.ProjectTasks.CountAsync(a => !a.Deleted && a.ProjectId == id);
            var calendar = await _context.Events.CountAsync(a => !a.Deleted && a.ProjectId == id);
            var documents = await _context.Documents.CountAsync(a => !a.Deleted && a.ProjectId == id);


            var productionTeamGroup = await (from k in _context.ProjectUserGroups 
                                             join l in _context.ProjectCrewGroups
                                             on k.Id equals l.GroupId
                                             where k.ProjectId == id
                                             && k.Name == "Production Team"
                                             select new {
                                                 k = k.Id
                                             }
                                             ).CountAsync() ; // await _context.ProjectUserGroups.Where(a => a.ProjectId == id && a.Name == "Production Team").CountAsync();
            #region onboarding
            list.Add(new OnBoardingDto()
            {
                description = "Create Project",
                completed = true,
                font_icon = "CAST",
                name = "casting_scene",
                ignore_as_first_step = false,
                url_hash = $"#/publicprojectdetail/{userProject.Id}",
                projectId = userProject.Id,
                parent_description = "Onboarding"
            });
            list.Add(new OnBoardingDto()
            {
                description = "Setup Budget and upload Financial Plan",
                completed = true,
                font_icon = "CAST",
                name = "casting_scene",
                ignore_as_first_step = false,
                url_hash = $"#/publicprojectdetail/{userProject.Id}",
                parent_description = "Onboarding"
            });
            list.Add(new OnBoardingDto()
            {
                description = "Upload Synopsis & Script",
                completed = true,
                font_icon = "SCENESSCRIPT",
                name = "location_scene",
                ignore_as_first_step = false,
                url_hash = $"#/publicprojectdetail/{userProject.Id}",
                parent_description = "Onboarding"
            });
            #endregion

            #region Team and crew
            list.Add(new OnBoardingDto()
            {
                description = "Assing Production Team",
                completed = productionTeamGroup >= 8,
                font_icon = "CAST",
                name = "casting",
                ignore_as_first_step = false,
                url_hash = $"#/{userProject.Id}/crew",
                parent_description = "Team and crew"
            });
            list.Add(new OnBoardingDto()
            {
                description = "Assign the Crew",
                completed = ProjectCrew >= 8,
                font_icon = "SHOOTINGSCHEDULING",
                name = "shooting_schedule",
                ignore_as_first_step = false,
                url_hash = $"#/{userProject.Id}/crew",
                parent_description = "Team and crew"
            });
            #endregion

            #region Characters
            list.Add(new OnBoardingDto()
            {
                description = "Cast the Characters",
                completed = characters > 0,
                font_icon = "SHOOTINGSCHEDULING",
                name = "shooting_schedule",
                ignore_as_first_step = false,
                url_hash = $"#/{userProject.Id}/cast",
                parent_description = "Characters"
            });
            #endregion

            #region Locations
            list.Add(new OnBoardingDto()
            {
                description = "Assign a location",
                completed = true,
                font_icon = "SHOOTINGSCHEDULING",
                name = "shooting_schedule",
                ignore_as_first_step = false,
                url_hash = $"#/onboarding/{userProject.Id}",
                parent_description = "Locations"
            });
            #endregion

            //#region Shooting day
            //list.Add(new OnBoardingDto()
            //{
            //    description = "Draft a shooting day",
            //    completed = true,
            //    font_icon = "SHOOTINGSCHEDULING",
            //    name = "shooting_schedule",
            //    ignore_as_first_step = false,
            //    url_hash = "#/",
            //    parent_description = "Shooting day"
            //});
            //#endregion

            var newList = list
                .GroupBy(a => a.parent_description)
                .Select(g => new { g.Key, total_percentage = Convert.ToInt32(Math.Round(((decimal)g.Count(a => a.completed) / g.Count()) * 100, 0)) })
                .ToArray();

            list.ForEach(a => a.parent_percentage = newList.Where(n => n.Key == a.parent_description).Select(a => a.total_percentage).FirstOrDefault());


            userProject.CompletedVsInProgress = $"{list.Count(a => a.completed)}/{list.Count}";
            userProject.OnboardPercentage = Convert.ToInt32(Math.Round(((decimal)list.Count(a => a.completed) / list.Count) * 100, 0));

            //update the project onnboarding status in db
            var dbProject = await _context.Project.Where(p => p.Id == userProject.Id && !p.Deleted).FirstOrDefaultAsync();

            if (userProject.OnboardPercentage == 100)
            {
                dbProject.OnBoardingCompleted = true;
            }
            else
            {
                dbProject.OnBoardingCompleted = false;
            }

            _context.SaveChanges();

            var lastCompleted = list.OrderBy(a => a.completed ? 0 : 1).Where(a => a.completed).TakeLast(1).SingleOrDefault();

            if (lastCompleted != null)
                lastCompleted.is_last = true;

            var last_step = list.OrderBy(a => a.completed ? 0 : 1).Where(a => !a.completed).Take(1).SingleOrDefault();

            if (last_step != null)
                last_step.next_step = true;

            return
                  new OnBoardOverlayDto() { Project = userProject, OnBoardingDtoList = list.OrderBy(a => a.completed ? 0 : 1).ToList() };
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> SubmitForOnboarding(ProjectDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            var project = await _context.Project.Where(a => a.Id == dto.Id && a.UserId == user.Id && !a.Deleted).FirstOrDefaultAsync();
            project.OnBoardingCompleted = true;
            project.OnBoardingPercentage = 100;
            await _context.SaveChangesAsync();
            return dto;
        }
    }
}
