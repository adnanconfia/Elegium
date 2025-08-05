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
using Elegium.ExtensionMethods;
using Microsoft.AspNetCore.WebSockets;
using Elegium.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DocumentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Documents
        [HttpGet("{projectId}")]
        public async Task<ActionResult> GetDocuments(int projectId)
        {
            var list = await (from k in _context.Project
                              join t in _context.Documents on k.Id equals t.ProjectId
                              where k.Id == projectId && !t.Deleted
                              select new DocumentsDto()
                              {
                                  Name = t.Name,
                                  Icon = t.Icon,
                                  CreatedBy = t.CreatedBy.FirstName + t.CreatedBy.LastName,
                                  CreatedDate = t.CreatedDate.GetRelativeTime(),
                                  Id = t.Id,
                                  CanEdit = true,
                                  CanView = true,
                                  Description = t.Description
                              }).ToListAsync();
            return Ok(list);
        }

        public ActionResult GetMaterialIcons()
        {
            var list = ExtensionMethods.ExtensionMethods.GetIcons();
            return Ok(list);
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentsDto>> GetDocument(int id)
        {
            var documents = await _context.Documents.Where(a => a.Id == id && !a.Deleted).FirstOrDefaultAsync();


            if (documents == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(documents.CreatedById);
            var document = new DocumentsDto()
            {
                Name = documents.Name,
                Icon = documents.Icon,
                Description = documents.Description,
                CreatedBy = user.FirstName + user.LastName,
                CreatedDate = documents.CreatedDate.GetRelativeTime(),
                Id = documents.Id,
                CanEdit = true,
                CanView = true,
                ProjectId = documents.ProjectId
            };

            return document;
        }

        //// PUT: api/Documents/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDocuments(int id, Documents documents)
        //{
        //    if (id != documents.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(documents).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DocumentsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        [HttpPost]
        public async Task<ActionResult<DocumentsDto>> ChangeIcon(DocumentsDto document)
        {
            var doc = await _context.Documents.FindAsync(document.Id);
            if (doc == null)
                return NotFound();
            doc.Icon = document.Icon;
            await _context.SaveChangesAsync();
            return Ok(document);
        }

        // POST: api/Documents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DocumentsDto>> PostDocuments(DocumentsDto documents)
        {
            var document = await _context.Documents.FindAsync(documents.Id);
            var user = await _userManager.GetUserAsync(User);
            if (document == null)
            {
                document = new Documents()
                {
                    Name = documents.Name,
                    Description = documents.Description,
                    Icon = documents.Icon,
                    CreatedById = user.Id,
                    ProjectId = documents.ProjectId
                };
                _context.Documents.Add(document);
            }
            else
            {
                document.Name = documents.Name;
                document.Description = documents.Description;
                document.Icon = documents.Icon;
                _context.Entry(document).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            documents.Id = document.Id;

            return Ok(documents);
        }

        // DELETE: api/Documents/5
        [HttpPost("{id}")]
        public async Task<ActionResult> DeleteDocuments(int id)
        {
            var documents = await _context.Documents.FindAsync(id);
            if (documents == null)
            {
                return NotFound();
            }

            documents.Deleted = true;

            _context.Entry(documents).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(true);
        }

        //private bool DocumentsExists(int id)
        //{
        //    return _context.Documents.Any(e => e.Id == id);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDocumentCategorySummary(int id)
        {
            var linksCount = await _context.Links.CountAsync(a => a.DocumentCategoryId == id);
            var commentsCount = await _context.Comments.CountAsync(a => a.DocumentCategoryId == id);
            var tasksCount = await _context.ProjectTasks.CountAsync(a => a.DocumentCategoryId == id && !a.Deleted);
            var filesCount = await _context.DocumentFiles.CountAsync(a => a.DocumentCategoryId == id);

            var projectId = await (from k in _context.DocumentCategory
                                   where k.Id == id
                                   join doc in _context.Documents
                                   on k.DocumentId equals doc.Id
                                   join p in _context.Project
                                   on doc.ProjectId equals p.Id
                                   select new
                                   {
                                       p.Id
                                   }).FirstOrDefaultAsync();
            return Ok(new
            {
                linksCount,
                commentsCount,
                tasksCount,
                filesCount,
                projectId = projectId.Id
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProjectWiseUserMenu(int id)
        {
            var userId = await _userManager.GetUserAsync(User);
            var project = await _context.Project.Where(a => a.Id == id && !a.Deleted).FirstOrDefaultAsync();

            var list = await (from u in _context.Users
                              join k in _context.UserProjectMenu
                              on u.Id equals k.ApplicationUserId
                              join p in _context.Project
                              on k.ProjectId equals p.Id
                              join m in _context.MenuActivity
                              on k.MenuActivityId equals m.Id
                              where p.Id == id && u.Id == userId.Id && !p.Deleted
                              select new
                              {
                                  m.Name,
                                  m.Url,
                                  ProjectId = p.Id,
                                  m.Icon,
                                  m.Type,
                                  m.Id
                              }).ToListAsync();

            return Ok(new { projectUserMenu = list, projectName = project?.Name, IsStarted = project?.IsStarted });
        }
    }
}
