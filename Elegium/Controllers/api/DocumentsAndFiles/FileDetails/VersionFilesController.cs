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
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace Elegium.Controllers.api.DocumentsAndFiles.FileDetails
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class VersionFilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VersionFilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{fileId}/{page}/{size}")]
        public async Task<ActionResult> GetFileVersions(int fileId, int page, int size)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                join v in _context.VersionFiles
                                on d.Id equals v.DocumentFileId
                                where d.Id == fileId
                                select new FileVersionsDto()
                                {
                                    ContentType = v.ContentType,
                                    DocumentFileId = d.Id,
                                    Extension = v.Extension,
                                    FileId = v.FileId,
                                    Id = v.Id,
                                    MimeType = d.MimeType,
                                    Name = v.Name,
                                    Type = v.Type,
                                    UserFriendlySize = v.UserFriendlySize,
                                    UserId = v.UserId,
                                    UserName = user.GetUserName(),
                                    RelativeTime = v.Created.GetRelativeTime(),
                                    UserFriendlyTime = v.Created.ToUserFriendlyTime(),
                                    Version = v.Version,
                                    CreateAtTicks = v.CreateAtTicks
                                }).GetPaged(page, size);

            var defaultImageId = await _context.VersionFiles.Where(a => a.DocumentFileId == fileId).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();

            return Ok(new { dbList, defaultImageId?.FileId });
        }

        // GET: api/VersionFiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VersionFiles>> GetVersionFiles(int id)
        {
            var versionFiles = await _context.VersionFiles.FindAsync(id);

            if (versionFiles == null)
            {
                return NotFound();
            }

            return versionFiles;
        }

        // PUT: api/VersionFiles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVersionFiles(int id, VersionFiles versionFiles)
        {
            if (id != versionFiles.Id)
            {
                return BadRequest();
            }

            _context.Entry(versionFiles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VersionFilesExists(id))
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

        // POST: api/VersionFiles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<IEnumerable<VersionFiles>>> PostDocumentFiles(List<VersionFiles> documentFiles)
        {
            var user = await _userManager.GetUserAsync(User);
            var fileId = documentFiles.Select(a => a.DocumentFileId).FirstOrDefault();
            //var alreadyDefault = await _context.DocumentFiles.Where(a => a.Default && a.DocumentCategoryId == documentFiles.Select(a => a.DocumentCategoryId).FirstOrDefault()).FirstOrDefaultAsync();
            //if (alreadyDefault != null && documentFiles.Count(a => a.Default) > 0)
            //    alreadyDefault.Default = false;
            foreach (var d in documentFiles)
            {
                d.UserFriendlySize = d.Size.GetBytesReadable();
                d.MimeType = MimeMapping.MimeUtility.GetMimeMapping(d.Name);
                FileInfo fi = new FileInfo(d.Name);
                d.Type = fi.Extension.GetFileType();
                d.UserId = user.Id;
                d.Extension = fi.Extension;
            }
            await _context.VersionFiles.AddRangeAsync(documentFiles);
            await _context.SaveChangesAsync();

            //reshuffle version sequence
            var list = _context.VersionFiles.Where(a => a.DocumentFileId == fileId).OrderBy(a => a.CreateAtTicks);
            int i = 1;
            foreach (var t in list)
            {
                t.Version = $"V{i}";
                i++;
            }
            await _context.SaveChangesAsync();

            return Ok(new { list, DefaultImage = list.Select(a => a.FileId).FirstOrDefault() });
        }

        // DELETE: api/VersionFiles/5
        [HttpPost("{id}")]
        public async Task<ActionResult<VersionFiles>> DeleteVersionFiles(int id)
        {
            var documentFiles = await _context.VersionFiles.FindAsync(id);
            if (documentFiles == null)
            {
                return NotFound();
            }

            _context.VersionFiles.Remove(documentFiles);
            await _context.SaveChangesAsync();

            return documentFiles;
        }

        private bool VersionFilesExists(int id)
        {
            return _context.VersionFiles.Any(e => e.Id == id);
        }
    }
}
