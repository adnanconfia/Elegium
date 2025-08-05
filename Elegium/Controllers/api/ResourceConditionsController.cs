using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Resources;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceConditionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResourceConditionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ResourceConditions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceCondition>>> GetResourceCondition()
        {
            return await _context.ResourceCondition.ToListAsync();
        }

        // GET: api/ResourceConditions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceCondition>> GetResourceCondition(int id)
        {
            var resourceCondition = await _context.ResourceCondition.FindAsync(id);

            if (resourceCondition == null)
            {
                return NotFound();
            }

            return resourceCondition;
        }

        // PUT: api/ResourceConditions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResourceCondition(int id, ResourceCondition resourceCondition)
        {
            if (id != resourceCondition.Id)
            {
                return BadRequest();
            }

            _context.Entry(resourceCondition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceConditionExists(id))
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

        // POST: api/ResourceConditions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ResourceCondition>> PostResourceCondition(ResourceCondition resourceCondition)
        {
            _context.ResourceCondition.Add(resourceCondition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResourceCondition", new { id = resourceCondition.Id }, resourceCondition);
        }

        // DELETE: api/ResourceConditions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResourceCondition>> DeleteResourceCondition(int id)
        {
            var resourceCondition = await _context.ResourceCondition.FindAsync(id);
            if (resourceCondition == null)
            {
                return NotFound();
            }

            _context.ResourceCondition.Remove(resourceCondition);
            await _context.SaveChangesAsync();

            return resourceCondition;
        }

        private bool ResourceConditionExists(int id)
        {
            return _context.ResourceCondition.Any(e => e.Id == id);
        }
    }
}
