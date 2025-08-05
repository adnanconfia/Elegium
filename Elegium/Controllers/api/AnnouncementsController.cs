using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Dtos;
using Elegium.ExtensionMethods;
using Elegium.Middleware;
using Elegium.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AnnouncementsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public AnnouncementsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<ActionResult<AnnouncementDto>> PostAnnouncement(AnnouncementDto announcement)
        {
            var user = await _userManager.GetUserAsync(User);
            var announcementObj = new Announcement()
            {
                Message = announcement.Message,
                Deadline = announcement.HasDeadline ? DateTime.Parse(announcement.Deadline) : (DateTime?)null,
                HasDeadline = announcement.HasDeadline,
                Title = announcement.Title,
                User = user,
                ProjectId = announcement.ProjectId,
                PinTop = announcement.PinTop,
                IncludeExternal = announcement.IncludeExternal
            };
            await _context.Announcements.AddAsync(announcementObj);
            await _context.SaveChangesAsync();
            if (announcement.AssignedTo.Count > 0)
            {
                var list = (from t in announcement.AssignedTo
                                //where t.type == "user"
                            select new AnnouncementsAssignedTo()
                            {
                                Type = t.type,
                                AnnouncementId = announcementObj.Id,
                                UserId = t.id,
                                Name = t.name,
                                Icon = t.avatar
                            }
                            );
                await _context.AnnouncementsAssignedTo.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
            announcement.Id = announcementObj.Id;
            announcement.UserId = user.Id;
            announcement.UserName = user.GetUserName();
            announcement.Deadline = announcementObj.HasDeadline ? announcementObj.Deadline.Value.ToString("dd/MM/yyyy") : "";
            announcement.Created = announcementObj.Created.ToString("MM/dd/yyyy hh:mm");
            return announcement;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<AnnouncementDto>>> GetAnnouncements(int id)
        {
            var list = await (from a in _context.Announcements
                              where a.ProjectId == id && !a.Deleted
                              join u in _context.Users
                              on a.UserId equals u.Id
                              select new AnnouncementDto()
                              {
                                  Created = a.Created.ToString("MM/dd/yyyy hh:mm"),
                                  Id = a.Id,
                                  ProjectId = a.ProjectId,
                                  Deadline = a.HasDeadline ? a.Deadline.Value.ToString("MM/dd/yyyy hh:mm") : string.Empty,
                                  Message = a.Message,
                                  Title = a.Title,
                                  UserId = a.UserId,
                                  PinTop = a.PinTop,
                                  UserName = u.GetUserName(),
                                  AssignedTo = (from asi in _context.AnnouncementsAssignedTo
                                                where asi.AnnouncementId == a.Id
                                                select new MentionDto()
                                                {
                                                    id = asi.UserId,
                                                    name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                    type = asi.Type,
                                                    icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                }).ToList(),
                                  HasDeadline = a.HasDeadline

                              }).ToListAsync();

            return list;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Announcement>> DeleteAnnouncement(int id)
        {
            var annObj = await _context.Announcements.FindAsync(id);
            if (annObj == null)
                return NotFound();

            annObj.Deleted = true;
            await _context.SaveChangesAsync();
            return annObj;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(int id)
        {
            var list = await (from a in _context.Announcements
                              where !a.Deleted && a.Id == id
                              join u in _context.Users
                              on a.UserId equals u.Id
                              select new AnnouncementDto()
                              {
                                  Created = a.Created.ToString("MM/dd/yyyy hh:mm"),
                                  Id = a.Id,
                                  ProjectId = a.ProjectId,
                                  Deadline = a.HasDeadline ? a.Deadline.Value.ToString("MM/dd/yyyy hh:mm") : string.Empty,
                                  Message = a.Message,
                                  Title = a.Title,
                                  UserId = a.UserId,
                                  PinTop = a.PinTop,
                                  UserName = u.GetUserName(),
                                  AssignedTo = (from asi in _context.AnnouncementsAssignedTo
                                                where asi.AnnouncementId == a.Id
                                                select new MentionDto()
                                                {
                                                    id = asi.UserId,
                                                    name = asi.Type == "user" || string.IsNullOrEmpty(asi.Type) ? _context.Users.Where(a => a.Id == asi.UserId).FirstOrDefault().GetUserName() : asi.Name,
                                                    type = asi.Type,
                                                    icon = asi.Type == "user" ? $"/api/UserProfiles/GetUserPhoto/{asi.UserId}/30/30" : asi.Icon
                                                }).ToList(),
                                  HasDeadline = a.HasDeadline

                              }).FirstOrDefaultAsync();

            var commentsCount = await _context.Comments.CountAsync(a => a.AnnouncementId == id);
            var filesCount = await _context.DocumentFiles.CountAsync(a => a.AnnouncementId == id);
            return Ok(new
            {
                list,
                tabContentLength = new
                {
                    commentsCount,
                    filesCount
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult<AnnouncementDto>> UpdateAnnouncement(AnnouncementDto dto)
        {
            var obj = await _context.Announcements.FindAsync(dto.Id);
            if (obj == null)
                return NotFound();
            obj.Title = dto.Title;
            obj.Message = dto.Message;
            obj.PinTop = dto.PinTop;
            obj.HasDeadline = dto.HasDeadline;
            obj.Deadline = dto.HasDeadline ? DateTime.Parse(dto.Deadline) : (DateTime?)null;

            await _context.SaveChangesAsync();
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<DocumentFiles>>> PostAnnouncementFiles(List<DocumentFiles> documentFiles)
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
        [HttpGet("{announcementId}/{page}/{size}")]
        public async Task<ActionResult> GetAnnouncementFiles(int announcementId, int page, int size)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                where d.AnnouncementId == announcementId
                                select new DocumentFilesDto()
                                {
                                    ContentType = d.ContentType,
                                    AnnouncementId = d.AnnouncementId,
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
                           AnnouncementId = d.AnnouncementId,
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
        public async Task<ActionResult<DocumentFiles>> DeleteAnnouncementFiles(int id)
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
            //var comments = await _context.Comments.Where(a => a.AnnouncementId == id).ToListAsync();

            var commentsDto = await (from k in _context.Comments
                                     join u in _context.Users
                                     on k.ApplicationUserId equals u.Id
                                     where k.AnnouncementId == id
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
      
        [HttpPost]
        public async Task<ActionResult> PostComment(CommentDto dto)
        {
            var usr = await _userManager.GetUserAsync(User);
            var comment = new Comment()
            {
                AnnouncementId = dto.AnnouncementId,
                ApplicationUserId = usr.Id,
                MarkupText = dto.MarkupText,
                Text = dto.Text,
               SceneId=dto.SceneId,
               CharId = dto.CharId,
               ExtraId = dto.ExtraId,
               ActorId = dto.ActorId,
               TalentId = dto.TalentId
               
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

        [HttpGet("{id}/{flag}/{announcementId}")]
        public async Task<DocumentFilesDto> NextPrevFileId(int id, string flag, int announcementId)
        {
            var currentFileObj = await _context.DocumentFiles.Where(a => a.Id == id && a.AnnouncementId == announcementId).FirstOrDefaultAsync();
            var currentFileUploadTime = currentFileObj.CreateAtTicks;
            var user = await _userManager.GetUserAsync(User);
            DocumentFiles nextPrevFileId = null;

            if (flag.Equals("P"))
            {
                var nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.AnnouncementId == announcementId && a.CreateAtTicks > currentFileUploadTime).OrderBy(a => a.CreateAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.AnnouncementId == announcementId).OrderBy(a => a.CreateAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }
            else
            {
                var nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.AnnouncementId == announcementId && a.CreateAtTicks < currentFileUploadTime).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.AnnouncementId == announcementId).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }

            var d = new DocumentFilesDto()
            {
                ContentType = nextPrevFileId.ContentType,
                AnnouncementId = nextPrevFileId.AnnouncementId,
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
                AnnouncementId = d.AnnouncementId,
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

        [HttpGet]
        public async Task<ActionResult<List<AnnouncementDto>>> GetUserWiseAnnouncements()
        {
            var user = await _userManager.GetUserAsync(User);

            //getting all the groups of which i am part of in any project
            var crewUserGroups = await (from p in _context.Project
                                        join g in _context.ProjectUserGroups
                                        on p.Id equals g.ProjectId
                                        join u in _context.ProjectCrewGroups
                                        on g.Id equals u.GroupId
                                        join c in _context.ProjectCrews
                                        on new { ProjectId = p.Id, CrewId = u.ProjectCrewId } equals
                                        new { c.ProjectId, CrewId = c.Id }
                                        where c.IsActive && c.UserId == user.Id
                                        select g.Id.ToString()
                                        ).ToListAsync();

            var announcementsForMe = await (from t in _context.AnnouncementsAssignedTo
                                            join p in _context.Announcements
                                            on t.AnnouncementId equals p.Id
                                            where !p.Deleted
                                            && crewUserGroups.Contains(t.UserId)
                                            select p.Id

                                            ).ToListAsync();

            var announcements = await (from a in _context.Announcements
                                       where !a.Deleted

                                       && announcementsForMe.Contains(a.Id) && (DateTime.UtcNow >= a.Deadline || a.PinTop)
                                       join p in _context.Project
                                       on a.ProjectId equals p.Id
                                       join u in _context.Users
                                       on a.UserId equals u.Id
                                       select new AnnouncementDto()
                                       {
                                           ProjectId = a.ProjectId,
                                           ProjectName = p.Name,
                                           Title = a.Title,
                                           Message = a.Message,
                                           Id = a.Id,
                                           UserName = u.GetUserName(),
                                           Deadline = a.HasDeadline ? a.Deadline.Value.ToString("MM/dd/yyyy hh:mm UTCzz") : string.Empty,
                                           ProjectColor = p.Color
                                       }
                                       ).ToListAsync();

            return announcements;
        }
    }
}
