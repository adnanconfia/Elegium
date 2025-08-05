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
    public class ExternalUserContractFileController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public ExternalUserContractFileController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/DocumentFiles
        [HttpGet("{userId}/{page}/{size}")]
        public async Task<ActionResult> GetExternalUserContractFiles(int userId, int page, int size)
        {
            var dbList = await (from d in _context.ExternalUserContractFile.Where(a => a.ExternalUserId == userId)
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



            return Ok(dbList);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ExternalUserContractFile>>> PostExternalUserContractFiles(List<ExternalUserContractFile> documentFiles)
        {
            foreach (var d in documentFiles)
            {
                d.UserFriendlySize = d.Size.GetBytesReadable();
                d.MimeType = MimeMapping.MimeUtility.GetMimeMapping(d.Name);
                FileInfo fi = new FileInfo(d.Name);
                d.Type = fi.Extension.GetFileType();
                d.Extension = fi.Extension;
            }
            await _context.ExternalUserContractFile.AddRangeAsync(documentFiles);
            await _context.SaveChangesAsync();
            return Ok(documentFiles);
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<ExternalUserContractFile>> DeleteExternalUserContractFile(int id)
        {
            var documentFiles = await _context.ExternalUserContractFile.FindAsync(id);
            if (documentFiles == null)
            {
                return NotFound();
            }

            _context.ExternalUserContractFile.Remove(documentFiles);
            await _context.SaveChangesAsync();

            return documentFiles;
        }

    }
}
