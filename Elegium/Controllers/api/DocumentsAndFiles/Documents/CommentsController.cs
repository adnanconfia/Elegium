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
using Microsoft.AspNetCore.Authorization;
using Elegium.Dtos;
using Elegium.ExtensionMethods;
using Elegium.Middleware;
using Newtonsoft.Json.Schema;

namespace Elegium.Controllers.api.DocumentsAndFiles.Documents
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/Comments/GetMentions/projectId
        [HttpGet("{projectId}/{query}")]
        public async Task<ActionResult> GetMentions(int projectId, string query)
        {
            var _projectId = await _context.Project.FindAsync(projectId);
            var userMentions = await (

                from u in _context.ProjectCrews
                join k in _context.Project
                on u.ProjectId equals k.Id
                join usrs in _context.Users
                on u.UserId equals usrs.Id
                join t in _context.UserProfiles
                on usrs.Id equals t.UserId

                where k.Id == _projectId.Id && u.IsActive
                && (t.FirstName + " " + t.LastName).Contains(query)

                select new MentionDto()
                {
                    id = usrs.Id,
                    name = t.FirstName + " " + t.LastName,
                    avatar = $"/api/UserProfiles/GetUserPhoto/{usrs.Id}/70/70",
                    icon = $"/api/UserProfiles/GetUserPhoto/{usrs.Id}/30/30",
                    type = "user"
                }
                ).ToListAsync();

            var units = await (

                from u in _context.ProjectUnits
                join k in _context.Project
                on u.ProjectId equals k.Id
                where k.Id == _projectId.Id //&& u.IsActive

                select new MentionDto()
                {
                    id = u.Id.ToString(),
                    name = u.Name,
                    avatar = $"/icons/CREW.svg",
                    icon = $"/icons/CREW.svg",
                    type = "units"
                }
                ).ToListAsync();

            var groups = await (

                from u in _context.ProjectUserGroups
                join k in _context.Project
                on u.ProjectId equals k.Id
                where k.Id == _projectId.Id //&& u.IsActive

                select new MentionDto()
                {
                    id = u.Id.ToString(),
                    name = u.Name,
                    avatar = $"/icons/CREW.svg",
                    icon = $"/icons/CREW.svg",
                    type = "groups"
                }
                ).ToListAsync();

            foreach (var i in units)
                userMentions.Add(i);

            foreach (var i in groups)
                userMentions.Add(i);

            return Ok(userMentions);
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult> GetProjectUsersAndGroups(int projectId)
        {
            var project = await _context.Project.FindAsync(projectId);

            var userMentions = await (

                from u in _context.ProjectCrews
                join k in _context.Project
                on u.ProjectId equals k.Id
                join usrs in _context.Users
                on u.UserId equals usrs.Id
                join t in _context.UserProfiles
                on usrs.Id equals t.UserId

                where k.Id == project.Id && u.IsActive

                select new MentionDto()
                {
                    id = usrs.Id,
                    name = t.FirstName + " " + t.LastName,
                    avatar = $"/api/UserProfiles/GetUserPhoto/{usrs.Id}/70/70",
                    icon = $"/api/UserProfiles/GetUserPhoto/{usrs.Id}/30/30",
                    type = "user"
                }
                ).ToListAsync();


            var units = await (

                from u in _context.ProjectUnits
                join k in _context.Project
                on u.ProjectId equals k.Id
                where k.Id == project.Id //&& u.IsActive

                select new MentionDto()
                {
                    id = u.Id.ToString(),
                    name = u.Name,
                    avatar = $"/icons/CREW.svg",
                    icon = $"/icons/CREW.svg",
                    type = "units"
                }
                ).ToListAsync();

            var groups = await (

                from u in _context.ProjectUserGroups
                join k in _context.Project
                on u.ProjectId equals k.Id
                where k.Id == project.Id //&& u.IsActive

                select new MentionDto()
                {
                    id = u.Id.ToString(),
                    name = u.Name,
                    avatar = $"/icons/CREW.svg",
                    icon = $"/icons/CREW.svg",
                    type = "groups"
                }
                ).ToListAsync();

            foreach (var i in units)
                userMentions.Add(i);

            foreach (var i in groups)
                userMentions.Add(i);

            return Ok(userMentions);
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int id)
        {
            //var comments = await _context.Comments.Where(a => a.DocumentCategoryId == id).ToListAsync();

            var commentsDto = await (from k in _context.Comments
                                     join u in _context.Users
                                     on k.ApplicationUserId equals u.Id
                                     where k.DocumentCategoryId == id
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

        // PUT: api/Comments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostComment(CommentDto dto)
        {
            var usr = await _userManager.GetUserAsync(User);
            var comment = new Comment()
            {
                DocumentCategoryId = dto.DocumentCategoryId,
                ApplicationUserId = usr.Id,
                MarkupText = dto.MarkupText,
                Text = dto.Text
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            dto.Id = comment.Id;
            dto.UserId = usr.Id;
            dto.Created = comment.Created.GetRelativeTime();

            var obj = await (from docCat in _context.DocumentCategory
                             join doc in _context.Documents
                             on docCat.DocumentId equals doc.Id
                             where docCat.Id == dto.DocumentCategoryId
                             select new
                             {
                                 ProjectId = doc.ProjectId,
                                 DocumentId = doc.Id,
                                 CategoryId = docCat.Id

                             }
                            ).FirstOrDefaultAsync();

            var _url = string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";

            foreach (var e in dto.MentionUsers)
            {
                if (e.type == "user")
                {
                    var appUsr = await _userManager.FindByIdAsync(e.id);
                    await _context.Entry(appUsr)
                    .Collection(u => u.Connections)
                    .Query()
                    .Where(c => c.Connected == true)
                    .LoadAsync();
                    foreach (var con in appUsr.Connections)
                    {
                        await _notificationService.GenerateNotificationAsync(usr, appUsr, NotificationKind.MentionedComment, $"{_url}/#/{obj.ProjectId}/documents/documentcategory/{obj.DocumentId}/files/{obj.CategoryId}");
                    }
                }
            }
            return Ok(dto);
        }

        // DELETE: api/Comments/5
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

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
