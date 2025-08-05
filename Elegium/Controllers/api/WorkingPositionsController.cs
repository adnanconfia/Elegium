using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingPositionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkingPositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkingPositions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkingPosition>>> GetWorkingPositions()
        {
            return await _context.WorkingPositions.ToListAsync();
        }

        // GET: api/WorkingPositions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkingPosition>> GetWorkingPosition(int id)
        {
            var workingPosition = await _context.WorkingPositions.FindAsync(id);

            if (workingPosition == null)
            {
                return NotFound();
            }

            return workingPosition;
        }

        // PUT: api/WorkingPositions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkingPosition(int id, WorkingPosition workingPosition)
        {
            if (id != workingPosition.Id)
            {
                return BadRequest();
            }

            _context.Entry(workingPosition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkingPositionExists(id))
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

        // POST: api/WorkingPositions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WorkingPosition>> PostWorkingPosition(WorkingPosition workingPosition)
        {
            _context.WorkingPositions.Add(workingPosition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkingPosition", new { id = workingPosition.Id }, workingPosition);
        }

        // DELETE: api/WorkingPositions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WorkingPosition>> DeleteWorkingPosition(int id)
        {
            var workingPosition = await _context.WorkingPositions.FindAsync(id);
            if (workingPosition == null)
            {
                return NotFound();
            }

            _context.WorkingPositions.Remove(workingPosition);
            await _context.SaveChangesAsync();

            return workingPosition;
        }

        private bool WorkingPositionExists(int id)
        {
            return _context.WorkingPositions.Any(e => e.Id == id);
        }
    }
}
