using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Dtos;
using Elegium.Models;
using Elegium.Models.Overview;
using Elegium.Models.ProjectCrews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardPanelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardPanelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardPanels()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var selectedPanelIds = _context.DashboardSelectedPanels
                                    .Where(s => s.UserId == currentUser.Id)
                                    .Select(s => s.PanelId)
                                    .ToList();

            if(selectedPanelIds.Count == 0)
            {
                var defaultDashboardPanels = _context.DashboardPanels
                                        .Where(s => s.PanelId != "myProjects"
                                    && s.PanelId != "myCreatedTasks"
                                    && s.PanelId != "myTasks").ToList();

                return Ok(defaultDashboardPanels);
            }
            var dashboardPanels = _context.DashboardPanels.Where(d => !selectedPanelIds.Contains(d.PanelId)).ToList();

            return Ok(dashboardPanels);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDashboardPanels()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var selectedPanels = _context.DashboardSelectedPanels
                                    .Where(s => s.UserId == currentUser.Id)
                                    .OrderBy(s => s.Sort)
                                    .ToList();

            
            if(selectedPanels.Count == 0)
            {
                var defaultPanels = _context.DashboardPanels
                                    .Where(s => s.PanelId == "myProjects" 
                                    || s.PanelId == "myCreatedTasks"
                                    || s.PanelId == "myTasks")
                                    .ToList();

                return Ok(defaultPanels);
            }
            
            return Ok(selectedPanels);
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardStats()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var activeProjectsIds = _context.Project
                                    .Where(p => p.UserId == currentUser.Id && !p.IsFinished).Select(p => p.Id).ToList();

            var activeProjectsCount = activeProjectsIds.Count();


            var overDueTasks = _context.ProjectTasks
                                .Where(t => t.ParentTaskId == null
                                        && !t.Deleted
                                        && !t.Completed
                                        && t.Deadline <= DateTime.Now
                                        && activeProjectsIds.Cast<int?>().Contains(t.ProjectId))
                                .Count();

            var completedTasks = _context.ProjectTasks
                                .Where(t => t.ParentTaskId == null
                                        && !t.Deleted
                                        && t.Completed
                                        && activeProjectsIds.Cast<int?>().Contains(t.ProjectId))
                                .Count();

            var activeTasks = _context.ProjectTasks
                                .Where(t => t.ParentTaskId == null
                                        && !t.Deleted
                                        && !t.Completed
                                        && activeProjectsIds.Cast<int?>().Contains(t.ProjectId))
                                .Count();

            var activeCrew = _context.ProjectCrews
                                .Where(c => c.IsActive
                                    && !c.Project.IsFinished
                                    && activeProjectsIds.Cast<int?>().Contains(c.ProjectId))
                                .Count();

            var dashboardStats = new
            {
                activeProjectsCount,
                overDueTasks,
                completedTasks,
                activeTasks,
                activeCrew
            };

            return Ok(dashboardStats);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCrews()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var activeProjectsIds = _context.Project
                                    .Where(p => p.UserId == currentUser.Id && !p.IsFinished).Select(p => p.Id).ToList();

            var usersList = await _context.ProjectCrews
                                    .Where(p => p.IsActive 
                                        && activeProjectsIds.Cast<int?>().Contains(p.ProjectId))
               .Include(a => a.User)
               .ToListAsync();

            List<ProjectCrewListModel> viewModel = new List<ProjectCrewListModel>();
            List<object> data = new List<object>();
            foreach (ProjectCrew user in usersList)
            {
                ProjectCrewListModel vm = new ProjectCrewListModel()
                {
                    Crew = user,
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    Name = user.User.FirstName + " " + user.User.LastName,
                    Online = _context.Connection.Where(a => a.UserId == user.UserId).OrderByDescending(a => a.ConnectionTime).FirstOrDefault()?.Connected,
                    CrewPostions = await _context.ProjectCrewPositions.Where(a => a.ProjectCrewId == user.Id).Include(a => a.Position).Select(a => a.Position.Name).ToListAsync()
                };
                viewModel.Add(vm);
            }

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserDashboardPanels([FromBody] List<DashboardSelectedPanel> dashboardSelectedPanels)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var selectedPanels = _context.DashboardSelectedPanels
                                        .Where(s => s.UserId == currentUser.Id)
                                        .ToList();

                _context.DashboardSelectedPanels.RemoveRange(selectedPanels);

                foreach (DashboardSelectedPanel panel in dashboardSelectedPanels)
                {
                    panel.UserId = currentUser.Id;
                    _context.DashboardSelectedPanels.Add(panel);
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }
    }
}
