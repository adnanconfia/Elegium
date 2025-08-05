using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.Models.ProjectCrews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.IO;
using Elegium.ExtensionMethods;
using Elegium.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExternalUserFilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ExternalUserFilesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/DocumentFiles
        [HttpGet("{userId}/{page}/{size}")]
        public async Task<ActionResult> GetExternalUserFiles(int userId, int page, int size)
        {
            var dbList = await (from d in _context.ExternalUserFile.Where(a => a.ExternalUserId == userId)
                                select new
                                {
                                    d.ContentType,
                                    d.Extension,
                                    d.FileId,
                                    d.Id,
                                    d.MimeType,
                                    d.Name,
                                    d.Type,
                                    d.UserFriendlySize,
                                    RelativeTime = d.Created.GetRelativeTime(),
                                    UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                    d.Default
                                }).GetPaged(page, size);



            var defaultImageId = await _context.ExternalUserFile.Where(a => a.Default && a.ExternalUserId == userId).FirstOrDefaultAsync();

            return Ok(new { dbList, defaultImageId?.FileId });
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ExternalUserFile>>> PostExternalUserFiles(List<ExternalUserFile> documentFiles)
        {
            var alreadyDefault = await _context.ExternalUserFile.Where(a => a.Default && a.ExternalUserId == documentFiles.Select(a => a.ExternalUserId).FirstOrDefault()).FirstOrDefaultAsync();
            if (alreadyDefault != null && documentFiles.Count(a => a.Default) > 0)
                alreadyDefault.Default = false;

            foreach (var d in documentFiles)
            {
                d.UserFriendlySize = d.Size.GetBytesReadable();
                d.MimeType = MimeMapping.MimeUtility.GetMimeMapping(d.Name);
                FileInfo fi = new FileInfo(d.Name);
                d.Type = fi.Extension.GetFileType();
                d.Extension = fi.Extension;
            }
            await _context.ExternalUserFile.AddRangeAsync(documentFiles);
            await _context.SaveChangesAsync();
            return Ok(documentFiles);
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<ExternalUserFile>> DeleteExternalUserFile(int id)
        {
            var documentFiles = await _context.ExternalUserFile.FindAsync(id);
            if (documentFiles == null)
            {
                return NotFound();
            }

            _context.ExternalUserFile.Remove(documentFiles);
            await _context.SaveChangesAsync();

            return documentFiles;
        }

    }
}
