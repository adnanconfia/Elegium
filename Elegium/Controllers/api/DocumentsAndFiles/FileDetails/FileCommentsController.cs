using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.Middleware;
using Microsoft.AspNetCore.Identity;
using Elegium.Dtos;
using Elegium.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;

namespace Elegium.Controllers.api.DocumentsAndFiles.FileDetails
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class FileCommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public FileCommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        // GET: api/FileComments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetFileComment()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/FileComments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int id)
        {
            //var comments = await _context.Comments.Where(a => a.DocumentCategoryId == id).ToListAsync();

            var commentsDto = await (from k in _context.Comments
                                     join u in _context.Users
                                     on k.ApplicationUserId equals u.Id
                                     where k.DocumentFileId == id
                                     select new CommentDto
                                     {
                                         Id = k.Id,
                                         MarkupText = k.MarkupText,
                                         Text = k.Text,
                                         DocumentFileId = k.DocumentFileId,
                                         UserId = k.ApplicationUserId,
                                         Created = k.Created.GetRelativeTime(),
                                         UserName = u.GetUserName()
                                     }).ToListAsync();
            if (commentsDto.Count == 0)
            {
                return NotFound();
            }

            return commentsDto;
        }

        // PUT: api/FileComments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileComment(int id, FileComment fileComment)
        {
            if (id != fileComment.Id)
            {
                return BadRequest();
            }

            _context.Entry(fileComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileCommentExists(id))
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

        // POST: api/FileComments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostComment(CommentDto dto)
        {
            var usr = await _userManager.GetUserAsync(User);
            var comment = new Comment()
            {
                DocumentFileId = dto.DocumentFileId,
                ApplicationUserId = usr.Id,
                MarkupText = dto.MarkupText,
                Text = dto.Text
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            dto.Id = comment.Id;
            dto.UserId = usr.Id;
            dto.Created = comment.Created.GetRelativeTime();

            var _url = string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";
            var test = HttpContext.Request.GetDisplayUrl();
            foreach (var e in dto.MentionUsers)
            {
                if (e.type == "user")
                {
                    var appUsr = await _userManager.FindByIdAsync(e.id);
                    //await _context.Entry(appUsr)
                    //.Collection(u => u.Connections)
                    //.Query()
                    //.Where(c => c.Connected == true)
                    //.LoadAsync();
                    //foreach (var con in appUsr.Connections)
                    //{
                    await _notificationService.GenerateNotificationAsync(usr, appUsr, NotificationKind.MentionedComment, $"{_url}/#/{dto.ProjectId}/dashboard/fileProfile/{dto.DocumentFileId}");
                    //}
                }
            }

            //var obj = await (from docCat in _context.DocumentCategory
            //                 join doc in _context.Documents
            //                 on docCat.DocumentId equals doc.Id
            //                 join f in _context.DocumentFiles
            //                 on docCat.Id equals f.DocumentCategoryId
            //                 where f.Id == dto.DocumentFileId
            //                 select new
            //                 {
            //                     ProjectId = doc.ProjectId,
            //                     DocumentId = doc.Id,
            //                     CategoryId = docCat.Id,
            //                     FileId = f.Id
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
            //            await _notificationService.GenerateNotificationAsync(usr, appUsr, NotificationKind.MentionedComment, $"{_url}/ProjectManagement/#{obj.ProjectId}/documents/documentcategory/{obj.DocumentId}/files/{obj.CategoryId}/fileProfile/{obj.FileId}");
            //        }
            //    }
            //}
            return Ok(dto);
        }

        // DELETE: api/FileComments/5
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

        private bool FileCommentExists(int id)
        {
            return _context.FileComment.Any(e => e.Id == id);
        }
    }
}
