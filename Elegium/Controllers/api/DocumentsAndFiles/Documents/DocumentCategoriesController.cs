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
using Elegium.Dtos;
using System.Text;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using tusdotnet.Stores;
using Microsoft.AspNetCore.Hosting;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        IWebHostEnvironment _env;
        public DocumentCategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: api/DocumentCategories
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentCategoryDto>> GetDocumentCategoryObj(int id)
        {
            var documentCategory = await (from k in _context.DocumentCategory
                                          join d in _context.Documents
                                          on k.DocumentId equals d.Id
                                          join p in _context.Project
                                          on d.ProjectId equals p.Id
                                          where k.Id == id && !k.Deleted && !d.Deleted
                                          select new
                                          {
                                              Name = k.Name,
                                              Id = k.Id,
                                              CreatedDate = k.CreatedDate.GetRelativeTime(),
                                              CreatedBy = k.CreatedBy.FirstName + " " + k.CreatedBy.LastName,
                                              CanEdit = k.CanEdit,
                                              CanView = k.CanView,
                                              ProjectId = d.ProjectId,
                                              DocumentId = d.Id,
                                              DocumentName = d.Name
                                          }).FirstOrDefaultAsync();

            return Ok(documentCategory);
        }

        // GET: api/DocumentCategories/5
        [HttpGet("{docId}")]
        public async Task<ActionResult> GetDocumentCategory(int docId)
        {
            var documentCategory = await (from k in _context.DocumentCategory
                                          join d in _context.Documents
                                          on k.DocumentId equals d.Id
                                          join p in _context.Project
                                          on d.ProjectId equals p.Id
                                          where k.DocumentId == docId && !k.Deleted && !d.Deleted
                                          select new DocumentCategoryDto()
                                          {
                                              Name = k.Name,
                                              Id = k.Id,
                                              CreatedDate = k.CreatedDate.GetRelativeTime(),
                                              CreatedBy = k.CreatedBy.FirstName + " " + k.CreatedBy.LastName,
                                              CanEdit = k.CanEdit,
                                              CanView = k.CanView,
                                              ProjectId = d.ProjectId,
                                              DocumentId = d.Id,
                                              FileCount = _context.DocumentFiles.Count(a => a.DocumentCategoryId == k.Id),
                                              FileDto = _context.DocumentFiles.Where(a => a.DocumentCategoryId == k.Id).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()
                                          }).ToListAsync();


            var list = (from k in documentCategory
                        let LatestVersion = (

                        from t in _context.VersionFiles
                        where t.DocumentFileId == (k.FileDto == null ? -1 : k.FileDto.Id)

                        select t

                        ).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()

                        select new DocumentCategoryDto()
                        {
                            Name = k.Name,
                            Id = k.Id,
                            CreatedDate = k.CreatedDate,
                            CreatedBy = k.CreatedBy,
                            CanEdit = k.CanEdit,
                            CanView = k.CanView,
                            ProjectId = k.ProjectId,
                            DocumentId = k.Id,
                            FileCount = k.FileCount,
                            HasFile = k.FileDto != null || LatestVersion != null,
                            FileType = LatestVersion == null ? (k.FileDto == null ? string.Empty : k.FileDto.Type) : LatestVersion.Type,
                            FileExtension = LatestVersion == null ? (k.FileDto == null ? string.Empty : k.FileDto.Extension) : LatestVersion.Extension,
                            FileId = LatestVersion == null ? (k.FileDto == null ? string.Empty : k.FileDto.FileId) : LatestVersion.FileId
                        });


            return Ok(list);
        }

        // PUT: api/DocumentCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumentCategory(int id, DocumentCategory documentCategory)
        {
            if (id != documentCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(documentCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentCategoryExists(id))
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

        // POST: api/DocumentCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DocumentCategoryDto>> PostDocumentCategory(DocumentCategoryDto documentCategory)
        {
            var documentCat = await _context.DocumentCategory.FindAsync(documentCategory.Id);
            var user = await _userManager.GetUserAsync(User);
            if (documentCat == null)
            {
                documentCat = new DocumentCategory()
                {
                    Name = documentCategory.Name,
                    Icon = documentCategory.Icon,
                    CreatedById = user.Id,
                    DocumentId = documentCategory.DocumentId
                };
                _context.DocumentCategory.Add(documentCat);
            }
            else
            {
                documentCat.Name = documentCategory.Name;
                _context.Entry(documentCat).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            documentCategory.Id = documentCat.Id;

            return Ok(documentCategory);
        }

        // DELETE: api/DocumentCategories/5
        [HttpPost("{id}")]
        public async Task<ActionResult<DocumentCategory>> DeleteDocumentCategory(int id)
        {
            var documentCategory = await _context.DocumentCategory.FindAsync(id);
            if (documentCategory == null)
            {
                return NotFound();
            }

            documentCategory.Deleted = true;

            _context.Entry(documentCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return documentCategory;
        }

        private bool DocumentCategoryExists(int id)
        {
            return _context.DocumentCategory.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> PostFileThumbnail(FileThumbnail thumb)
        {
            var thumbObj = await _context.FileThumbnails.Where(a => a.FileId == thumb.FileId).FirstOrDefaultAsync();
            if (thumbObj == null)
            {
                thumbObj = new FileThumbnail() { FileId = thumb.FileId, Thumbnail = Convert.FromBase64String(thumb.FileArray.Replace("data:image/png;base64,", "")) };
                await _context.FileThumbnails.AddAsync(thumbObj);
            }
            else
            {
                thumbObj.Thumbnail = Convert.FromBase64String(thumb.FileArray.Replace("data:image/png;base64,", ""));
                _context.Entry(thumbObj).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/{width}/{height}")]
        public async Task<IActionResult> GetFileThumbnail(string id, int width = 348, int height = 218)
        {
            Guid isItGuid = new Guid();
            if (!string.IsNullOrEmpty(id) && Guid.TryParseExact(id, "N", out isItGuid))
            {
                try
                {
                    var thumbObj = await _context.FileThumbnails.Where(a => a.FileId == id).FirstOrDefaultAsync();
                    if (thumbObj != null)
                    {
                        return File(thumbObj.Thumbnail, "image/png");
                    }

                    var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                    var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

                    if (file == null)
                    {
                        HttpContext.Response.StatusCode = 404;
                        await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
                        return NotFound();
                    }
                    var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
                    var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

                    // The tus protocol does not specify any required metadata.
                    // "filetype" is metadata that is specific to this domain and is not required.
                    var type = metadata.ContainsKey("filetype")
                              ? metadata["filetype"].GetString(Encoding.UTF8)
                              : "application/octet-stream";
                    using (Image imgPhoto = Image.Load(fileStream))
                    {

                        ResizeOptions resizeOpt = new ResizeOptions()
                        {
                            // Mode = ResizeMode.Min,
                            Size = new Size() { Height = height, Width = width }
                        };
                        imgPhoto.Mutate(x => x
                             .Resize(resizeOpt)
                         );
                        MemoryStream ms = new MemoryStream();
                        await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                        fileStream.Close();
                        return File(ms.ToArray(), "image/png");
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                // }
            }
            return NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileThumbnail(string id)
        {
            Guid isItGuid = new Guid();
            if (!string.IsNullOrEmpty(id) && Guid.TryParseExact(id, "N", out isItGuid))
            {
                try
                {
                    var thumbObj = await _context.FileThumbnails.Where(a => a.FileId == id).FirstOrDefaultAsync();
                    if (thumbObj != null)
                    {
                        return File(thumbObj.Thumbnail, "image/png");
                    }

                    var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                    var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

                    if (file == null)
                    {
                        HttpContext.Response.StatusCode = 404;
                        await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
                        return NotFound();
                    }
                    var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
                    var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

                    // The tus protocol does not specify any required metadata.
                    // "filetype" is metadata that is specific to this domain and is not required.
                    var type = metadata.ContainsKey("filetype")
                              ? metadata["filetype"].GetString(Encoding.UTF8)
                              : "application/octet-stream";
                    using (Image imgPhoto = Image.Load(fileStream))
                    {

                       
                        MemoryStream ms = new MemoryStream();
                        await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                        fileStream.Close();
                        return File(ms.ToArray(), "image/png");
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                // }
            }
            return NotFound();
        }
    }
}
