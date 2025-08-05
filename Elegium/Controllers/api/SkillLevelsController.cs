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
    public class SkillLevelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SkillLevelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SkillLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillLevel>>> GetSkillLevel()
        {
            return await _context.SkillLevel.ToListAsync();
        }

        // GET: api/SkillLevels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SkillLevel>> GetSkillLevel(int id)
        {
            var skillLevel = await _context.SkillLevel.FindAsync(id);

            if (skillLevel == null)
            {
                return NotFound();
            }

            return skillLevel;
        }

        // PUT: api/SkillLevels/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkillLevel(int id, SkillLevel skillLevel)
        {
            if (id != skillLevel.Id)
            {
                return BadRequest();
            }

            _context.Entry(skillLevel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillLevelExists(id))
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

        // POST: api/SkillLevels
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SkillLevel>> PostSkillLevel(SkillLevel skillLevel)
        {
            _context.SkillLevel.Add(skillLevel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSkillLevel", new { id = skillLevel.Id }, skillLevel);
        }

        // DELETE: api/SkillLevels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SkillLevel>> DeleteSkillLevel(int id)
        {
            var skillLevel = await _context.SkillLevel.FindAsync(id);
            if (skillLevel == null)
            {
                return NotFound();
            }

            _context.SkillLevel.Remove(skillLevel);
            await _context.SaveChangesAsync();

            return skillLevel;
        }

        private bool SkillLevelExists(int id)
        {
            return _context.SkillLevel.Any(e => e.Id == id);
        }
    }
}
