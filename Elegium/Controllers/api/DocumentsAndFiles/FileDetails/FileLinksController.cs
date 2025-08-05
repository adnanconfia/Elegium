using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;

namespace Elegium.Controllers.api.DocumentsAndFiles.FileDetails
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileLinksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FileLinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FileLinks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileLink>>> GetFileLink()
        {
            return await _context.FileLink.ToListAsync();
        }

        // GET: api/FileLinks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FileLink>> GetFileLink(int id)
        {
            var fileLink = await _context.FileLink.FindAsync(id);

            if (fileLink == null)
            {
                return NotFound();
            }

            return fileLink;
        }

        // PUT: api/FileLinks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileLink(int id, FileLink fileLink)
        {
            if (id != fileLink.Id)
            {
                return BadRequest();
            }

            _context.Entry(fileLink).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileLinkExists(id))
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

        // POST: api/FileLinks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FileLink>> PostFileLink(FileLink fileLink)
        {
            _context.FileLink.Add(fileLink);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFileLink", new { id = fileLink.Id }, fileLink);
        }

        // DELETE: api/FileLinks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FileLink>> DeleteFileLink(int id)
        {
            var fileLink = await _context.FileLink.FindAsync(id);
            if (fileLink == null)
            {
                return NotFound();
            }

            _context.FileLink.Remove(fileLink);
            await _context.SaveChangesAsync();

            return fileLink;
        }

        private bool FileLinkExists(int id)
        {
            return _context.FileLink.Any(e => e.Id == id);
        }
    }
}
