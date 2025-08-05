using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Projects;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisibilityAreasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VisibilityAreasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VisibilityAreas
        [HttpGet]
        public async Task<ActionResult<VisibilityAreas[]>> GetVisibilityAreas()
        {
            return await _context.VisibilityAreas.ToArrayAsync();
        }

        // GET: api/VisibilityAreas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VisibilityAreas>> GetVisibilityAreas(Guid id)
        {
            var visibilityAreas = await _context.VisibilityAreas.FindAsync(id);

            if (visibilityAreas == null)
            {
                return NotFound();
            }

            return visibilityAreas;
        }

        // PUT: api/VisibilityAreas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisibilityAreas(Guid id, VisibilityAreas visibilityAreas)
        {
            if (id != visibilityAreas.Id)
            {
                return BadRequest();
            }

            _context.Entry(visibilityAreas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisibilityAreasExists(id))
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

        // POST: api/VisibilityAreas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<VisibilityAreas>> PostVisibilityAreas(VisibilityAreas visibilityAreas)
        {
            _context.VisibilityAreas.Add(visibilityAreas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVisibilityAreas", new { id = visibilityAreas.Id }, visibilityAreas);
        }

        // DELETE: api/VisibilityAreas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VisibilityAreas>> DeleteVisibilityAreas(Guid id)
        {
            var visibilityAreas = await _context.VisibilityAreas.FindAsync(id);
            if (visibilityAreas == null)
            {
                return NotFound();
            }

            _context.VisibilityAreas.Remove(visibilityAreas);
            await _context.SaveChangesAsync();

            return visibilityAreas;
        }

        private bool VisibilityAreasExists(Guid id)
        {
            return _context.VisibilityAreas.Any(e => e.Id == id);
        }
    }
}
