using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Microsoft.AspNetCore.Identity;
using Elegium.Dtos;
using Elegium.ExtensionMethods;

namespace Elegium.Controllers.api.DocumentsAndFiles.FileDetails
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FileTasksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/FileTasks
        // GET: api/ProjectTasks
        [HttpGet("{fileId}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetFileTasks(int fileId)
        {
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              where k.DocumentFilesId == fileId && !k.Deleted
                              select new ProjectTaskDto()
                              {
                                  Completed = k.Completed,
                                  Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                  Id = k.Id,
                                  Description = k.Description,
                                  DocumentFilesId = k.DocumentFilesId.Value,
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

        // GET: api/FileTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTask>> GetFileTask(int id)
        {
            var fileTask = await _context.ProjectTasks.FindAsync(id);

            if (fileTask == null)
            {
                return NotFound();
            }

            return fileTask;
        }

        // PUT: api/FileTasks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileTask(int id, FileTask fileTask)
        {
            if (id != fileTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(fileTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileTaskExists(id))
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

        // POST: api/FileTasks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProjectTaskDto>> PostFileTask(ProjectTaskDto fileTask)
        {
            var user = await _userManager.GetUserAsync(User);
            var taskObj = new ProjectTask()
            {
                Description = fileTask.Description,
                Deadline = fileTask.HasDeadline ? DateTime.Parse(fileTask.Deadline) : (DateTime?)null,
                DocumentFilesId = fileTask.DocumentFilesId,
                Completed = fileTask.Completed,
                HasDeadline = fileTask.HasDeadline,
                Title = fileTask.Title,
                User = user,
                ProjectId = fileTask.ProjectId
            };
            await _context.ProjectTasks.AddAsync(taskObj);
            await _context.SaveChangesAsync();
            if (fileTask.AssignedTo.Count > 0)
            {

                var list = (from t in fileTask.AssignedTo
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
            fileTask.Id = taskObj.Id;
            fileTask.UserId = user.Id;
            fileTask.UserName = user.GetUserName();
            fileTask.Deadline = fileTask.HasDeadline ? taskObj.Deadline.Value.ToString("dd/MM/yyyy") : "";
            fileTask.ClassName = taskObj.GetTaskStatus();
            return Ok(fileTask);
        }


        [HttpPost]
        public async Task<ActionResult<ProjectTaskDto>> ChangeFileTaskStatus(ProjectTaskDto fileTask)
        {
            var taskObj = await _context.ProjectTasks.FindAsync(fileTask.Id);
            taskObj.Completed = !taskObj.Completed;
            _context.Entry(taskObj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            fileTask.ClassName = taskObj.GetTaskStatus();
            fileTask.Completed = taskObj.Completed;
            return Ok(fileTask);
        }

        // DELETE: api/ProjectTasks/5
        [HttpPost("{id}")]
        public async Task<ActionResult<ProjectTask>> DeleteProjectTask(int id)
        {
            var projectTask = await _context.ProjectTasks.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }

            _context.ProjectTasks.Remove(projectTask);
            await _context.SaveChangesAsync();

            return projectTask;
        }

        private bool FileTaskExists(int id)
        {
            return _context.FileTask.Any(e => e.Id == id);
        }
    }
}
