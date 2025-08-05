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
using Microsoft.AspNetCore.Identity;
using Elegium.ExtensionMethods;
using System.IO;
using Elegium.Middleware;
using Microsoft.AspNetCore.Authorization;

namespace Elegium.Controllers.api.Tasks
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        public TasksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        // GET: api/Tasks
        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetMyTasks(int projectId)
        {
            var user = await _userManager.GetUserAsync(User);

            var projectAssignedToMe = await _context.ProjectTaskAssignedTo.Where(a => a.UserId == user.Id).Select(a => a.ProjectTaskId).ToListAsync();
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              join p in _context.Project
                              on k.ProjectId equals p.Id
                              where !p.Deleted && k.ParentTaskId == null && projectAssignedToMe.Contains(k.Id) && k.ProjectId != null && (projectId == 0 || k.ProjectId == projectId) && !k.Deleted 
                              select new ProjectTaskDto()
                              {
                                  Completed = k.Completed,
                                  Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                  Id = k.Id,
                                  ProjectId = k.ProjectId,
                                  Description = k.Description,
                                  DocumentFilesId = k.DocumentFilesId.Value,
                                  Title = k.Title,
                                  HasDeadline = k.HasDeadline,
                                  UserId = k.UserId,
                                  SceneId = k.SceneId,
                                  SubTasks = (from t in k.SubTasks //     
                                              where !t.Deleted
                                              select new ProjectTaskDto()
                                              {
                                                  Id = t.Id,
                                                  Title = t.Title,
                                                  //AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                                  //              where asi.ProjectTaskId == t.Id
                                                  //              select new MentionDto()
                                                  //              {
                                                  //                  id = asi.UserId,
                                                  //                  name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                  //                  type = asi.Type,
                                                  //                  icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                  //              }).ToList(),
                                                  ClassName = t.GetTaskStatus(),
                                                  Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                                  Completed = t.Completed
                                              }
                                               ).ToList(),
                                  //SubTasks = (from t in _context.ProjectTasks
                                  //            where t.ParentTaskId == k.Id

                                  //            select new ProjectTaskDto()
                                  //            {
                                  //                Id = t.Id,
                                  //                Title = t.Title,
                                  //                AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                  //                              where asi.ProjectTaskId == t.Id
                                  //                              select new MentionDto()
                                  //                              {
                                  //                                  id = asi.UserId,
                                  //                                  name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                  //                                  type = asi.Type,
                                  //                                  icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                  //                              }).ToList(),
                                  //                ClassName = t.GetTaskStatus(),
                                  //                Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                  //            }
                                  //             ).ToList(),
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

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetMyCompletedTasks(int projectId)
        {
            var user = await _userManager.GetUserAsync(User);

            var projectAssignedToMe = await _context.ProjectTaskAssignedTo.Where(a => a.UserId == user.Id).Select(a => a.ProjectTaskId).ToListAsync();
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              join p in _context.Project
                              on k.ProjectId equals p.Id
                              where !p.Deleted && k.ParentTaskId == null && k.Completed && k.ProjectId == projectId && !k.Deleted
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
                                  SubTasks = (from t in k.SubTasks //  
                                              where !t.Deleted
                                              select new ProjectTaskDto()
                                              {
                                                  Id = t.Id,
                                                  Title = t.Title,
                                                  //AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                                  //              where asi.ProjectTaskId == t.Id
                                                  //              select new MentionDto()
                                                  //              {
                                                  //                  id = asi.UserId,
                                                  //                  name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                  //                  type = asi.Type,
                                                  //                  icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                  //              }).ToList(),
                                                  ClassName = t.GetTaskStatus(),
                                                  Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                                  Completed = t.Completed
                                              }
                                               ).ToList(),
                                  //SubTasks = (from t in _context.ProjectTasks
                                  //            where t.ParentTaskId == k.Id

                                  //            select new ProjectTaskDto()
                                  //            {
                                  //                Id = t.Id,
                                  //                Title = t.Title,
                                  //                AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                  //                              where asi.ProjectTaskId == t.Id
                                  //                              select new MentionDto()
                                  //                              {
                                  //                                  id = asi.UserId,
                                  //                                  name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                  //                                  type = asi.Type,
                                  //                                  icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                  //                              }).ToList(),
                                  //                ClassName = t.GetTaskStatus(),
                                  //                Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                  //            }
                                  //             ).ToList(),
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

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetMyCreatedTasks(int projectId)
        {
            var user = await _userManager.GetUserAsync(User);

            var projectAssignedToMe = await _context.ProjectTaskAssignedTo.Where(a => a.UserId == user.Id).Select(a => a.ProjectTaskId).ToListAsync();
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id

                              join p in _context.Project
                              on k.ProjectId equals p.Id
                              where !p.Deleted && k.ParentTaskId == null && k.UserId == user.Id && k.ProjectId != null && (projectId == 0 || k.ProjectId == projectId) && !k.Deleted
                              select new ProjectTaskDto()
                              {
                                  Completed = k.Completed,
                                  Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                  Id = k.Id,
                                  ProjectId = k.ProjectId,
                                  Description = k.Description,
                                  DocumentFilesId = k.DocumentFilesId.Value,
                                  Title = k.Title,
                                  HasDeadline = k.HasDeadline,
                                  UserId = k.UserId,
                                  SubTasks = (from t in k.SubTasks //
                                              where !t.Deleted
                                              select new ProjectTaskDto()
                                              {
                                                  Id = t.Id,
                                                  Title = t.Title,
                                                  //AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                                  //              where asi.ProjectTaskId == t.Id
                                                  //              select new MentionDto()
                                                  //              {
                                                  //                  id = asi.UserId,
                                                  //                  name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                  //                  type = asi.Type,
                                                  //                  icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                  //              }).ToList(),
                                                  ClassName = t.GetTaskStatus(),
                                                  Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                                  Completed = t.Completed
                                              }
                                               ).ToList(),
                                  //SubTasks = (from t in _context.ProjectTasks
                                  //            where t.ParentTaskId == k.Id

                                  //            select new ProjectTaskDto()
                                  //            {
                                  //                Id = t.Id,
                                  //                Title = t.Title,
                                  //                AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                  //                              where asi.ProjectTaskId == t.Id
                                  //                              select new MentionDto()
                                  //                              {
                                  //                                  id = asi.UserId,
                                  //                                  name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                  //                                  type = asi.Type,
                                  //                                  icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                  //                              }).ToList(),
                                  //                ClassName = t.GetTaskStatus(),
                                  //                Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                  //            }
                                  //             ).ToList(),
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

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetAllTasks(int projectId)
        {
            var user = await _userManager.GetUserAsync(User);

            var projectAssignedToMe = await _context.ProjectTaskAssignedTo.Where(a => a.UserId == user.Id).Select(a => a.ProjectTaskId).ToListAsync();
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              join p in _context.Project
                              on k.ProjectId equals p.Id
                              where !p.Deleted && k.ParentTaskId == null && k.ProjectId == projectId && !k.Deleted
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
                                  SceneId = k.SceneId,
                                  CharId = k.CharId,
                                  ExtraId = k.ExtraId,
                                  ActorID = k.ActorId,
                                  TalentId = k.TalentID,
                                  AgencyId=k.AgencyId,
                                  SubTasks = (from t in k.SubTasks //   
                                              where !t.Deleted
                                              select new ProjectTaskDto()
                                              {
                                                  Id = t.Id,
                                                  Title = t.Title,
                                                  //AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                                  //              where asi.ProjectTaskId == t.Id
                                                  //              select new MentionDto()
                                                  //              {
                                                  //                  id = asi.UserId,
                                                  //                  name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                  //                  type = asi.Type,
                                                  //                  icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                  //              }).ToList(),
                                                  ClassName = t.GetTaskStatus(),
                                                  Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                                  Completed = t.Completed
                                              }
                                               ).ToList(),
                                  //SubTasks = (from t in _context.ProjectTasks
                                  //            where t.ParentTaskId == k.Id

                                  //            select new ProjectTaskDto()
                                  //            {
                                  //                Id = t.Id,
                                  //                Title = t.Title,
                                  //                AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                  //                              where asi.ProjectTaskId == t.Id
                                  //                              select new MentionDto()
                                  //                              {
                                  //                                  id = asi.UserId,
                                  //                                  name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                  //                                  type = asi.Type,
                                  //                                  icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                  //                              }).ToList(),
                                  //                ClassName = t.GetTaskStatus(),
                                  //                Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                  //            }
                                  //             ).ToList(),
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

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetAllPendingTasks(int projectId)
        {
            var user = await _userManager.GetUserAsync(User);

            var projectAssignedToMe = await _context.ProjectTaskAssignedTo.Where(a => a.UserId == user.Id).Select(a => a.ProjectTaskId).ToListAsync();
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              join p in _context.Project
                              on k.ProjectId equals p.Id
                              where !p.Deleted && k.ParentTaskId == null 
                              && k.ProjectId == projectId 
                              && !k.Completed
                              && k.HasDeadline
                              && !k.Deleted
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
                                  SubTasks = (from t in k.SubTasks //   
                                              where !t.Deleted && !t.Completed && t.HasDeadline
                                              select new ProjectTaskDto()
                                              {
                                                  Id = t.Id,
                                                  Title = t.Title,
                                                  ClassName = t.GetTaskStatus(),
                                                  Deadline = k.HasDeadline ? t.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                                  Completed = t.Completed,
                                                  AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                                                where asi.ProjectTaskId == t.Id
                                                                select new MentionDto()
                                                                {
                                                                    id = asi.UserId,
                                                                    name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                                    type = asi.Type,
                                                                    icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                                }).ToList()
                                              }
                                               ).ToList(),
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetAllProjectsPendingTasks()
        {
            var user = await _userManager.GetUserAsync(User);

            var projectAssignedToMe = await _context.ProjectTaskAssignedTo.Where(a => a.UserId == user.Id).Select(a => a.ProjectTaskId).ToListAsync();
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              join p in _context.Project
                              on k.ProjectId equals p.Id
                              where !p.Deleted && k.ParentTaskId == null
                              //&& k.ProjectId == projectId
                              && !k.Completed
                              && k.HasDeadline
                              && !k.Deleted
                              select new ProjectTaskDto()
                              {
                                  Completed = k.Completed,
                                  Deadline = k.HasDeadline ? k.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                  Id = k.Id,
                                  ProjectId = k.ProjectId,
                                  Project = new Models.Projects.Project() { Name =  k.Project.Name, Id = k.Project.Id },
                                  Description = k.Description,
                                  DocumentFilesId = k.DocumentFilesId.Value,
                                  Title = k.Title,
                                  HasDeadline = k.HasDeadline,
                                  UserId = k.UserId,
                                  SubTasks = (from t in k.SubTasks //   
                                              where !t.Deleted && !t.Completed && t.HasDeadline
                                              select new ProjectTaskDto()
                                              {
                                                  Id = t.Id,
                                                  Title = t.Title,
                                                  ClassName = t.GetTaskStatus(),
                                                  Deadline = k.HasDeadline ? t.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                                  Completed = t.Completed,
                                                  AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                                                where asi.ProjectTaskId == t.Id
                                                                select new MentionDto()
                                                                {
                                                                    id = asi.UserId,
                                                                    name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                                    type = asi.Type,
                                                                    icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                                }).ToList()
                                              }
                                               ).ToList(),
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTaskDto>> GetTask(int id)
        {
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              where k.Id == id && !k.Deleted
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
                                  //SubTasks = k.SubTasks.ToList(),
                                  ClassName = k.GetTaskStatus(),
                                  UserName = l.GetUserName(),
                                  TaskDeadlineDay = k.HasDeadline ? k.Deadline.Value.ToString("dd") : "",
                                  TaskMonth = k.HasDeadline ? k.Deadline.Value.ToString("MMMM") : "",
                                  ParentTask = _context.ProjectTasks.Where(a => a.Id == k.ParentTaskId).FirstOrDefault(),
                                  AssignedTo = (from asi in _context.ProjectTaskAssignedTo
                                                where asi.ProjectTaskId == k.Id
                                                select new MentionDto()
                                                {
                                                    id = asi.UserId,
                                                    name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                    type = asi.Type,
                                                    icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                }).ToList(),
                                  DocumentCategoryId = k.DocumentCategoryId,
                                  DocumentCategory = _context.DocumentCategory.Where(a => a.Id == k.DocumentCategoryId).FirstOrDefault(),
                                  ProjectId = k.ProjectId,
                                  Section = !string.IsNullOrEmpty(k.Section) ? _context.MenuActivity.Where(a => a.Url == k.Section).FirstOrDefault().Name : "",
                                  SectionUrl = k.Section,

                              }).FirstOrDefaultAsync();

            var commentsCount = await _context.Comments.CountAsync(a => a.ProjectTaskId == id);
            var subTasksCount = await _context.ProjectTasks.CountAsync(a => a.ParentTaskId == id && !a.Deleted);
            var filesCount = await _context.DocumentFiles.CountAsync(a => a.ProjectTaskId == id);
            return Ok(new
            {
                list,
                tabContentLength = new
                {
                    commentsCount,
                    filesCount,
                    subTasksCount

                }
            });
        }

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
                ExtraId =projectTask.ExtraId

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
        public async Task<ActionResult<ProjectTask>> UpdateProjectTask(ProjectTaskDto projectTask)
        {
            var user = await _userManager.GetUserAsync(User);
            var task = await _context.ProjectTasks.FindAsync(projectTask.Id);

            task.Description = projectTask.Description;
            task.Deadline = projectTask.HasDeadline ? DateTime.Parse(projectTask.Deadline) : (DateTime?)null;
            //DocumentCategoryId = projectTask.DocumentCategoryId,
            //Completed = projectTask.Completed,
            task.HasDeadline = projectTask.HasDeadline;
            task.Title = projectTask.Title;
            //User = user,
            //ProjectId = projectTask.ProjectId,
            //ParentTaskId = projectTask.ParentTaskId

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            if (projectTask.AssignedTo.Count > 0)
            {
                var assignedTo = _context.ProjectTaskAssignedTo.Where(a => a.ProjectTaskId == task.Id);
                _context.ProjectTaskAssignedTo.RemoveRange(assignedTo);
                await _context.SaveChangesAsync();
                var list = (from t in projectTask.AssignedTo
                                //where t.type == "user"
                            select new ProjectTaskAssignedTo()
                            {
                                Type = t.type,
                                ProjectTaskId = task.Id,
                                UserId = t.id,
                                Name = t.name,
                                Icon = string.IsNullOrEmpty(t.avatar) ? t.icon : t.avatar
                            }
                            );
                await _context.ProjectTaskAssignedTo.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
            projectTask.Id = task.Id;
            projectTask.UserId = user.Id;
            projectTask.UserName = user.GetUserName();
            projectTask.Deadline = projectTask.HasDeadline ? task.Deadline.Value.ToString("dd/MM/yyyy") : "";
            projectTask.ClassName = task.GetTaskStatus();
            return Ok(projectTask);
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetSubTasks(int taskId)
        {
            var user = await _userManager.GetUserAsync(User);

            var projectAssignedToMe = await _context.ProjectTaskAssignedTo.Where(a => a.UserId == user.Id).Select(a => a.ProjectTaskId).ToListAsync();
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              where k.ParentTaskId == taskId && !k.Deleted
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


        [HttpPost]
        public async Task<ActionResult<IEnumerable<DocumentFiles>>> PostTaskFiles(List<DocumentFiles> documentFiles)
        {
            var user = await _userManager.GetUserAsync(User);
            foreach (var d in documentFiles)
            {
                d.UserFriendlySize = d.Size.GetBytesReadable();
                d.MimeType = MimeMapping.MimeUtility.GetMimeMapping(d.Name);
                FileInfo fi = new FileInfo(d.Name);
                d.Type = fi.Extension.GetFileType();
                d.UserId = user.Id;
                d.Extension = fi.Extension;
            }
            await _context.DocumentFiles.AddRangeAsync(documentFiles);
            await _context.SaveChangesAsync();
            return Ok(documentFiles);
        }

        // GET: api/DocumentFiles
        [HttpGet("{taskId}/{page}/{size}")]
        public async Task<ActionResult> GetTaskFiles(int taskId, int page, int size)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                where d.ProjectTaskId == taskId
                                select new DocumentFilesDto()
                                {
                                    ContentType = d.ContentType,
                                    ProjectTaskId = d.ProjectTaskId,
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
                                    Default = d.Default
                                }).GetPaged(page, size);

            var list = from d in dbList
                       let latestVersion = (from v in _context.VersionFiles
                                            where d.Id == v.DocumentFileId
                                            //orderby d.CreateAtTicks ascending

                                            select v

                                                   ).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()


                       select new DocumentFilesDto()
                       {
                           ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                           ProjectTaskId = d.ProjectTaskId,
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

            return Ok(new { list });
        }

        // DELETE: api/DocumentFiles/5
        [HttpPost("{id}")]
        public async Task<ActionResult<DocumentFiles>> DeleteTaskFiles(int id)
        {
            var documentFiles = await _context.DocumentFiles.FindAsync(id);
            if (documentFiles == null)
            {
                return NotFound();
            }

            _context.DocumentFiles.Remove(documentFiles);
            await _context.SaveChangesAsync();

            return documentFiles;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int id)
        {
            //var comments = await _context.Comments.Where(a => a.ProjectTaskId == id).ToListAsync();

            var commentsDto = await (from k in _context.Comments
                                     join u in _context.Users
                                     on k.ApplicationUserId equals u.Id
                                     where k.ProjectTaskId == id
                                     select new CommentDto
                                     {
                                         Id = k.Id,
                                         MarkupText = k.MarkupText,
                                         Text = k.Text,
                                         DocumentCategoryId = k.DocumentCategoryId,
                                         UserId = k.ApplicationUserId,
                                         Created = k.Created.GetRelativeTime(),
                                         UserName = u.FirstName + " " + u.LastName
                                     }).ToListAsync();
            if (commentsDto.Count == 0)
            {
                return NotFound();
            }

            return commentsDto;
        }

        [HttpPost]
        public async Task<ActionResult> PostComment(CommentDto dto)
        {
            var usr = await _userManager.GetUserAsync(User);
            var comment = new Comment()
            {
                ProjectTaskId = dto.ProjectTaskId,
                ApplicationUserId = usr.Id,
                MarkupText = dto.MarkupText,
                Text = dto.Text
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            dto.Id = comment.Id;
            dto.UserId = usr.Id;
            dto.Created = comment.Created.GetRelativeTime();

            //var obj = await (from docCat in _context.DocumentCategory
            //                 join doc in _context.Documents
            //                 on docCat.DocumentId equals doc.Id
            //                 where docCat.Id == dto.DocumentCategoryId
            //                 select new
            //                 {
            //                     ProjectId = doc.ProjectId,
            //                     DocumentId = doc.Id,
            //                     CategoryId = docCat.Id

            //                 }
            //                ).FirstOrDefaultAsync();

            //var _url = string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";

            //foreach (var e in dto.MentionUsers)
            //{
            //    if (e.type == "user")
            //    {
            //        var appUsr = await _userManager.FindByIdAsync(e.id);
            //        await _context.Entry(appUsr)
            //        .Collection(u => u.Connections)
            //        .Query()
            //        .Where(c => c.Connected == true)
            //        .LoadAsync();
            //        foreach (var con in appUsr.Connections)
            //        {
            //            await _notificationService.GenerateNotificationAsync(usr, appUsr, NotificationKind.MentionedComment, $"{_url}/ProjectManagement/#{obj.ProjectId}/documents/documentcategory/{obj.DocumentId}/files/{obj.CategoryId}");
            //        }
            //    }
            //}
            return Ok(dto);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        [HttpGet("{id}/{flag}/{taskId}")]
        public async Task<DocumentFilesDto> NextPrevFileId(int id, string flag, int taskId)
        {
            var currentFileObj = await _context.DocumentFiles.Where(a => a.Id == id && a.ProjectTaskId == taskId).FirstOrDefaultAsync();
            var currentFileUploadTime = currentFileObj.CreateAtTicks;
            var user = await _userManager.GetUserAsync(User);
            DocumentFiles nextPrevFileId = null;

            if (flag.Equals("P"))
            {
                var nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.ProjectTaskId == taskId && a.CreateAtTicks > currentFileUploadTime).OrderBy(a => a.CreateAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.ProjectTaskId == taskId).OrderBy(a => a.CreateAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }
            else
            {
                var nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.ProjectTaskId == taskId && a.CreateAtTicks < currentFileUploadTime).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.ProjectTaskId == taskId).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }

            var d = new DocumentFilesDto()
            {
                ContentType = nextPrevFileId.ContentType,
                ProjectTaskId = nextPrevFileId.ProjectTaskId,
                Extension = nextPrevFileId.Extension,
                FileId = nextPrevFileId.FileId,
                Id = nextPrevFileId.Id,
                MimeType = nextPrevFileId.MimeType,
                Name = nextPrevFileId.Name,
                Type = nextPrevFileId.Type,
                UserFriendlySize = nextPrevFileId.UserFriendlySize,
                UserId = nextPrevFileId.UserId,
                UserName = _context.Users.Find(nextPrevFileId.UserId).GetUserName(),
                RelativeTime = nextPrevFileId.Created.GetRelativeTime(),
                UserFriendlyTime = nextPrevFileId.Created.ToUserFriendlyTime()
            };

            var latestVersion = await _context.VersionFiles.Where(a => a.DocumentFileId == d.Id).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();

            var dto = new DocumentFilesDto()
            {
                ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                ProjectTaskId = d.ProjectTaskId,
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

            return dto;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectObjects(int projectId)
        {
            var list = await (from p in _context.Project
                              where p.Id == projectId && !p.Deleted
                              join d in _context.Documents
                              on p.Id equals d.ProjectId
                              join c in _context.DocumentCategory
                              on d.Id equals c.DocumentId
                              select new
                              {
                                  ProjectId = p.Id,
                                  c.Name,
                                  c.Id
                              }
                              ).ToListAsync();

            return Ok(list);
        }

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
    }
}
