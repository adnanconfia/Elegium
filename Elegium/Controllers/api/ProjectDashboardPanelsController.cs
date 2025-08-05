using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
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
    public class ProjectDashboardPanelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectDashboardPanelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetDashboardPanels(int projectId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var selectedPanelIds = _context.ProjectDashboardSelectedPanels
                                    .Where(s => s.UserId == currentUser.Id && s.ProjectId == projectId)
                                    .Select(s => s.PanelId)
                                    .ToList();

            if (selectedPanelIds.Count == 0)
            {
                var defaultDashboardPanels = _context.ProjectDashboardPanels
                                        .Where(s => s.PanelId != "myProjects"
                                    && s.PanelId != "myCreatedTasks"
                                    && s.PanelId != "myTasks").ToList();

                return Ok(defaultDashboardPanels);
            }

            var dashboardPanels = _context.ProjectDashboardPanels.Where(d => !selectedPanelIds.Contains(d.PanelId)).ToList();

            return Ok(dashboardPanels);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetUserDashboardPanels(int projectId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var selectedPanels = _context.ProjectDashboardSelectedPanels
                                    .Where(s => s.UserId == currentUser.Id && s.ProjectId == projectId)
                                    .OrderBy(s => s.Sort)
                                    .ToList();

            if (selectedPanels.Count == 0)
            {
                var defaultPanels = _context.ProjectDashboardPanels
                                    .Where(s => s.PanelId == "myProjects"
                                    || s.PanelId == "myCreatedTasks"
                                    || s.PanelId == "myTasks")
                                    .ToList();

                return Ok(defaultPanels);
            }

            return Ok(selectedPanels);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetDashboardStats(int projectId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var activeProjectsIds = _context.Project
                                    .Where(p => p.UserId == currentUser.Id 
                                            && !p.IsFinished)
                                    .Select(p => p.Id).ToList();

            var activeProjectsCount = activeProjectsIds.Count();


            var overDueTasks = _context.ProjectTasks
                                .Where(t => t.ParentTaskId == null && t.ProjectId == projectId
                                        && !t.Deleted
                                        && !t.Completed
                                        && t.Deadline <= DateTime.Now)
                                .Count();

            var completedTasks = _context.ProjectTasks
                                .Where(t => t.ParentTaskId == null && t.ProjectId == projectId
                                        && !t.Deleted
                                        && t.Completed)
                                .Count();

            var activeTasks = _context.ProjectTasks
                                .Where(t => t.ParentTaskId == null && t.ProjectId == projectId
                                        && !t.Deleted
                                        && !t.Completed)
                                .Count();

            var activeCrew = _context.ProjectCrews
                                .Where(c => c.IsActive 
                                    && c.ProjectId == projectId)
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

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetAllCrews(int projectId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var usersList = await _context.ProjectCrews
                                    .Where(p => p.IsActive
                                        && p.ProjectId == projectId)
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

        [HttpPost("{projectId}")]
        public async Task<IActionResult> UpdateUserDashboardPanels(int projectId, [FromBody] List<ProjectDashboardSelectedPanel> dashboardSelectedPanels)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var selectedPanels = _context.ProjectDashboardSelectedPanels
                                        .Where(s => s.UserId == currentUser.Id && s.ProjectId == projectId)
                                        .ToList();

                _context.ProjectDashboardSelectedPanels.RemoveRange(selectedPanels);

                foreach (ProjectDashboardSelectedPanel panel in dashboardSelectedPanels)
                {
                    panel.UserId = currentUser.Id;
                    _context.ProjectDashboardSelectedPanels.Add(panel);
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
