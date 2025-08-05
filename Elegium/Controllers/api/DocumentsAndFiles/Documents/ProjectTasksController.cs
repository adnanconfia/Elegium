using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.Dtos;
using Elegium.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Elegium.Controllers.api.DocumentsAndFiles.Documents
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProjectTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectTasksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/ProjectTasks
        [HttpGet("{docCatId}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetProjectTasks(int docCatId)
        {
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              where k.DocumentCategoryId == docCatId && !k.Deleted
                              select new ProjectTaskDto()
                              {
                                  Completed = k.Completed,
                                  Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                  Id = k.Id,
                                  Description = k.Description,
                                  DocumentCategoryId = k.DocumentCategoryId.Value,
                                  Title = k.Title,
                                  HasDeadline = k.HasDeadline,
                                  UserId = k.UserId,
                                  ClassName = k.GetTaskStatus(),
                                  UserName = l.GetUserName(),
                                  AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                                where asi.ProjectTaskId == k.Id
                                                select new MentionDto()
                                                {
                                                    id = asi.UserId,
                                                    name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                    type = asi.Type,
                                                    icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                }).ToList()
                              }).ToListAsync();
            return list;
        }

        // GET: api/ProjectTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTask>> GetProjectTask(int id)
        {
            var projectTask = await _context.ProjectTasks.FindAsync(id);

            if (projectTask == null)
            {
                return NotFound();
            }

            return projectTask;
        }

        // PUT: api/ProjectTasks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectTask(int id, ProjectTask projectTask)
        {
            if (id != projectTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProjectTasks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProjectTask>> PostProjectTask(ProjectTaskDto projectTask)
        {
            var user = await _userManager.GetUserAsync(User);
            var taskObj = new ProjectTask()
            {
                Description = projectTask.Description,
                Deadline = projectTask.HasDeadline ? DateTime.Parse(projectTask.Deadline) : (DateTime?)null,
                DocumentCategoryId = projectTask.DocumentCategoryId,
                Completed = projectTask.Completed,
                HasDeadline = projectTask.HasDeadline,
                Title = projectTask.Title,
                User = user,
                ProjectId = projectTask.ProjectId,
                ParentTaskId = projectTask.ParentTaskId,
                Section = projectTask.HasSection ? projectTask.Section : string.Empty,
                SceneId = projectTask.SceneId,
                CharId = projectTask.CharId,
                ExtraId = projectTask.ExtraId,
                ActorId = projectTask.ActorID,
                TalentID = projectTask.TalentId,
                AgencyId = projectTask.AgencyId

            };
            await _context.ProjectTasks.AddAsync(taskObj);
            await _context.SaveChangesAsync();
            if (projectTask.AssignedTo.Count > 0)
            {
                var list = (from t in projectTask.AssignedTo
                                //where t.type == "user"
                            select new ProjectTaskAssignedTo()
                            {
                                Type = t.type,
                                ProjectTaskId = taskObj.Id,
                                UserId = t.id,
                                Name = t.name,
                                Icon = t.avatar
                            }
                            );
                await _context.ProjectTaskAssignedTo.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
            projectTask.Id = taskObj.Id;
            projectTask.UserId = user.Id;
            projectTask.UserName = user.GetUserName();
            projectTask.Deadline = projectTask.HasDeadline ? taskObj.Deadline.Value.ToString("dd/MM/yyyy") : "";
            projectTask.ClassName = taskObj.GetTaskStatus();
            return Ok(projectTask);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectTask>> ChangeProjectTaskStatus(ProjectTaskDto projectTask)
        {
            var taskObj = await _context.ProjectTasks.FindAsync(projectTask.Id);
            taskObj.Completed = !taskObj.Completed;
            _context.Entry(taskObj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            projectTask.ClassName = taskObj.GetTaskStatus();
            projectTask.Completed = taskObj.Completed;
            return Ok(projectTask);
        }

        // DELETE: api/ProjectTasks/5
        //[HttpPost("{id}")]
        //public async Task<ActionResult<ProjectTask>> DeleteProjectTask(int id)
        //{
        //    var projectTask = await _context.ProjectTasks.FindAsync(id);
        //    if (projectTask == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ProjectTasks.Remove(projectTask);
        //    await _context.SaveChangesAsync();

        //    return projectTask;
        //}


        [HttpPost("{id}")]
        public async Task<ActionResult<ProjectTask>> DeleteProjectTask(int id)
        {
            var projectTask = await _context.ProjectTasks.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            projectTask.Deleted = true;
            _context.Entry(projectTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return projectTask;
        }

        private bool ProjectTaskExists(int id)
        {
            return _context.ProjectTasks.Any(e => e.Id == id);
        }
    }
}
