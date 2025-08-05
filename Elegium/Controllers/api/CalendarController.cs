using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Dtos;
using Elegium.Dtos.Calendar;
using Elegium.ExtensionMethods;
using Elegium.Middleware;
using Elegium.Models;
using Elegium.Models.Calendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public CalendarController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> PostEvent(EventDto calendarEvent)
        {
            var user = await _userManager.GetUserAsync(User);


            if (!string.IsNullOrEmpty(calendarEvent.StartTime))
            {
                calendarEvent.StartDate = DateTime.Parse(calendarEvent.StartDate.Value.ToLocalTime().ToString("dd-MMM-yyyy") + " " + calendarEvent.StartTime).ToUniversalTime();
            }
            else if (calendarEvent.StartDate != null)
            {
                calendarEvent.StartDate = calendarEvent.StartDate.Value.ToLocalTime().ToUniversalTime();
            }

            if (!string.IsNullOrEmpty(calendarEvent.EndTime))
            {
                calendarEvent.EndDate = DateTime.Parse(calendarEvent.EndDate.Value.ToLocalTime().ToString("dd-MMM-yyyy") + " " + calendarEvent.EndTime).ToUniversalTime();
            }
            else if (calendarEvent.EndDate != null)
            {
                calendarEvent.EndDate = calendarEvent.EndDate.Value.ToLocalTime().ToUniversalTime();
            }
            Event eventObj = null;
            if (calendarEvent.Id == 0)
            {
                eventObj = new Event();
                if (string.IsNullOrEmpty(calendarEvent.StartTime))
                    eventObj.AllDay = true;
                eventObj.Title = calendarEvent.Title;
                eventObj.Description = calendarEvent.Description;
                eventObj.StartDate = calendarEvent.StartDate;
                eventObj.EndDate = calendarEvent.EndDate;
                eventObj.StartTime = calendarEvent.StartTime;
                eventObj.EndTime = calendarEvent.EndTime;
                eventObj.ProjectId = calendarEvent.ProjectId;
                eventObj.Color = calendarEvent.Color;
                eventObj.Location = calendarEvent.Location;
                eventObj.CalenderCategoryId = calendarEvent.CalenderCategoryId;
                eventObj.UserId = user.Id;


                await _context.Events.AddAsync(eventObj);
            }
            else
            {
                eventObj = await _context.Events.Where(e => e.Id == calendarEvent.Id).FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(calendarEvent.StartTime))
                    eventObj.AllDay = false;
                eventObj.Title = calendarEvent.Title ?? eventObj.Title;
                eventObj.Description = calendarEvent.Description ?? eventObj.Description;
                eventObj.StartDate = calendarEvent.StartDate ?? eventObj.StartDate;
                eventObj.EndDate = calendarEvent.EndDate ?? eventObj.EndDate;
                eventObj.StartTime = calendarEvent.StartTime ?? eventObj.StartTime;
                eventObj.EndTime = calendarEvent.EndTime ?? eventObj.EndTime;
                eventObj.Color = calendarEvent.Color ?? eventObj.Color;
                eventObj.Location = calendarEvent.Location ?? eventObj.Location;
                eventObj.CalenderCategoryId = calendarEvent.CalenderCategoryId ?? eventObj.CalenderCategoryId;

                _context.Entry(eventObj).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            if (calendarEvent.AssignedTo != null && calendarEvent.AssignedTo.Count > 0)
            {
                var deleteList = _context.EventsAssignedTo.Where(e => e.EventId == eventObj.Id);
                _context.EventsAssignedTo.RemoveRange(deleteList);

                var list = (from t in calendarEvent.AssignedTo
                            select new EventsAssignedTo()
                            {
                                Type = t.type,
                                EventId = eventObj.Id,
                                UserId = t.id,
                                Name = t.name,
                                Icon = t.icon
                            }
                            );
                await _context.EventsAssignedTo.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
            if (calendarEvent.AdditionalViewers != null && calendarEvent.AdditionalViewers.Count > 0)
            {
                var deleteList = _context.EventsAdditionalViewers.Where(e => e.EventId == eventObj.Id);
                _context.EventsAdditionalViewers.RemoveRange(deleteList);

                var list = (from t in calendarEvent.AdditionalViewers
                            select new EventsAdditionalViewer()
                            {
                                Type = t.type,
                                EventId = eventObj.Id,
                                UserId = t.id,
                                Name = t.name,
                                Icon = t.icon
                            }
                            );
                await _context.EventsAdditionalViewers.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }

            calendarEvent.Id = eventObj.Id;
            calendarEvent.UserId = user.Id;
            calendarEvent.UserName = user.GetUserName();
            calendarEvent.Created = eventObj.Created;
            return calendarEvent;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<EventDto>>> GetEvents(int id)
        {
            var list = await (from e in _context.Events
                              where e.ProjectId == id && !e.Deleted
                              join u in _context.Users
                              on e.UserId equals u.Id
                              select new EventDto()
                              {
                                  Created = e.Created,
                                  Id = e.Id,
                                  ProjectId = e.ProjectId,
                                  Title = e.Title,
                                  Description = e.Description,
                                  Color = e.Color,
                                  Location = e.Location,
                                  AllDay = e.AllDay,
                                  CalenderCategoryId = e.CalenderCategoryId,
                                  StartDate = e.StartDate.Value,
                                  EndDate = e.EndDate.Value,
                                  StartTime = e.StartTime,
                                  EndTime = e.EndTime,
                                  UserId = e.UserId,
                                  UserName = u.GetUserName(),
                                  AssignedTo = (from asi in _context.EventsAssignedTo
                                                where asi.EventId == e.Id
                                                select new MentionDto()
                                                {
                                                    id = asi.UserId,
                                                    name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                    type = asi.Type,
                                                    icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                }).ToList(),
                                  AdditionalViewers = (from asi in _context.EventsAdditionalViewers
                                                       where asi.EventId == e.Id
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
        public async Task<ActionResult<List<EventDto>>> GetMyCalenderEvents()
        {
            var user = await _userManager.GetUserAsync(User);

            var projectIds = await _context.Project.Where(p => p.UserId == user.Id && !p.Deleted).Select(p => p.Id).ToListAsync();

            var list = await (from e in _context.Events
                              where projectIds.Contains(e.ProjectId) && !e.Deleted
                              join u in _context.Users
                              on e.UserId equals u.Id
                              select new EventDto()
                              {
                                  Created = e.Created,
                                  Id = e.Id,
                                  ProjectId = e.ProjectId,
                                  Project = new Models.Projects.Project() { Name = e.Project.Name, Id = e.Project.Id},
                                  Title = e.Title,
                                  Description = e.Description,
                                  Color = e.Color,
                                  Location = e.Location,
                                  AllDay = e.AllDay,
                                  CalenderCategoryId = e.CalenderCategoryId,
                                  StartDate = e.StartDate.Value,
                                  EndDate = e.EndDate.Value,
                                  StartTime = e.StartTime,
                                  EndTime = e.EndTime,
                                  UserId = e.UserId,
                                  UserName = u.GetUserName(),
                                  AssignedTo = (from asi in _context.EventsAssignedTo
                                                where asi.EventId == e.Id
                                                select new MentionDto()
                                                {
                                                    id = asi.UserId,
                                                    name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                    type = asi.Type,
                                                    icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                }).ToList(),
                                  AdditionalViewers = (from asi in _context.EventsAdditionalViewers
                                                       where asi.EventId == e.Id
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var eventInDb = await _context.Events.FindAsync(id);
            if (eventInDb == null)
                return BadRequest("Event not found in db");

            //_context.Comments.RemoveRange(await _context.Comments.Where(c => c.EventId == id).ToListAsync());
            //_context.ProjectTasks.RemoveRange(await _context.ProjectTasks.Where(c => c.EventId == id).ToListAsync());
            //await _context.SaveChangesAsync();

            //_context.Events.Remove(eventInDb);
            
            eventInDb.Deleted = true;

            _context.Entry(eventInDb).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok();
        }

        #region Calendar event profile code

        [HttpGet("{id}")]
        public async Task<ActionResult> GetEvent(int id)
        {
            var list = await (from e in _context.Events
                              where e.Id == id && !e.Deleted
                              join u in _context.Users
                              on e.UserId equals u.Id
                              select new EventDto()
                              {
                                  Created = e.Created,
                                  Id = e.Id,
                                  ProjectId = e.ProjectId,
                                  Title = e.Title,
                                  Description = e.Description,
                                  Color = e.Color,
                                  Location = e.Location,
                                  AllDay = e.AllDay,
                                  CalenderCategoryId = e.CalenderCategoryId,
                                  CalenderCategory = _context.CalenderCategories.Where(a => a.Id == e.CalenderCategoryId).FirstOrDefault(),
                                  StartDate = e.StartDate.Value,
                                  EndDate = e.EndDate.Value,
                                  StartTime = e.StartDate.Value.ToString("hh:mm tt"),
                                  EndTime = e.EndDate.Value.ToString("hh:mm tt"),
                                  UserId = e.UserId,
                                  UserName = u.GetUserName(),
                                  AssignedTo = (from asi in _context.EventsAssignedTo
                                                where asi.EventId == e.Id
                                                select new MentionDto()
                                                {
                                                    id = asi.UserId,
                                                    name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                    type = asi.Type,
                                                    icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                }).ToList(),
                                  AdditionalViewers = (from asi in _context.EventsAdditionalViewers
                                                       where asi.EventId == e.Id
                                                       select new MentionDto()
                                                       {
                                                           id = asi.UserId,
                                                           name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                           type = asi.Type,
                                                           icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                       }).ToList()

                              }).FirstOrDefaultAsync();

            var commentsCount = await _context.Comments.CountAsync(a => a.EventId == id);
            var tasksCount = await _context.ProjectTasks.CountAsync(a => a.EventId == id && !a.Deleted);
            var filesCount = await _context.DocumentFiles.CountAsync(a => a.EventId == id);

            return Ok(new
            {
                list,
                tabContentLength = new
                {
                    commentsCount,
                    filesCount,
                    tasksCount
                }
            });
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetTasks(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);

            var projectAssignedToMe = await _context.ProjectTaskAssignedTo.Where(a => a.UserId == user.Id).Select(a => a.ProjectTaskId).ToListAsync();
            var list = await (from k in _context.ProjectTasks
                              join l in _context.Users
                              on k.UserId equals l.Id
                              where k.EventId == eventId && !k.Deleted
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
        public async Task<ActionResult<ProjectTask>> PostTask(ProjectTaskDto projectTask)
        {
            var user = await _userManager.GetUserAsync(User);
            var taskObj = new ProjectTask()
            {
                Description = projectTask.Description,
                Deadline = projectTask.HasDeadline ? DateTime.Parse(projectTask.Deadline) : (DateTime?)null,
                Completed = projectTask.Completed,
                HasDeadline = projectTask.HasDeadline,
                Title = projectTask.Title,
                User = user,
                ProjectId = projectTask.ProjectId,
                EventId = projectTask.EventId,
                Section = projectTask.HasSection ? projectTask.Section : string.Empty,

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
        public async Task<ActionResult<IEnumerable<DocumentFiles>>> PostFiles(List<DocumentFiles> documentFiles)
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
        [HttpGet("{eventId}/{page}/{size}")]
        public async Task<ActionResult> GetFiles(int eventId, int page, int size)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                where d.EventId == eventId
                                select new DocumentFilesDto()
                                {
                                    ContentType = d.ContentType,
                                    EventId = d.EventId,
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
                           EventId = d.EventId,
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
        public async Task<ActionResult<DocumentFiles>> DeleteFiles(int id)
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

        [HttpGet("{id}/{flag}/{eventId}")]
        public async Task<DocumentFilesDto> NextPrevFileId(int id, string flag, int eventId)
        {
            var currentFileObj = await _context.DocumentFiles.Where(a => a.Id == id && a.EventId == eventId).FirstOrDefaultAsync();
            var currentFileUploadTime = currentFileObj.CreateAtTicks;
            var user = await _userManager.GetUserAsync(User);
            DocumentFiles nextPrevFileId = null;

            if (flag.Equals("P"))
            {
                var nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.EventId == eventId && a.CreateAtTicks > currentFileUploadTime).OrderBy(a => a.CreateAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.EventId == eventId).OrderBy(a => a.CreateAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }
            else
            {
                var nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.EventId == eventId && a.CreateAtTicks < currentFileUploadTime).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.EventId == eventId).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }

            var d = new DocumentFilesDto()
            {
                ContentType = nextPrevFileId.ContentType,
                EventId = nextPrevFileId.EventId,
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
                EventId = d.EventId,
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

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int id)
        {
            //var comments = await _context.Comments.Where(a => a.ProjectTaskId == id).ToListAsync();

            var commentsDto = await (from k in _context.Comments
                                     join u in _context.Users
                                     on k.ApplicationUserId equals u.Id
                                     where k.EventId == id
                                     select new CommentDto
                                     {
                                         Id = k.Id,
                                         MarkupText = k.MarkupText,
                                         Text = k.Text,
                                         EventId = k.EventId,
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
                EventId = dto.EventId,
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

        #endregion
    }
}