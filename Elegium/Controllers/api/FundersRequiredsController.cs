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
    public class FundersRequiredsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FundersRequiredsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FundersRequireds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FundersRequired>>> GetFundersRequired()
        {
            return await _context.FundersRequired.OrderBy(a => a.OrderCol).ToListAsync();
        }

        // GET: api/FundersRequireds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FundersRequired>> GetFundersRequired(Guid id)
        {
            var fundersRequired = await _context.FundersRequired.FindAsync(id);

            if (fundersRequired == null)
            {
                return NotFound();
            }

            return fundersRequired;
        }

        // PUT: api/FundersRequireds/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFundersRequired(Guid id, FundersRequired fundersRequired)
        {
            if (id != fundersRequired.Id)
            {
                return BadRequest();
            }

            _context.Entry(fundersRequired).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FundersRequiredExists(id))
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

        // POST: api/FundersRequireds
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FundersRequired>> PostFundersRequired(FundersRequired fundersRequired)
        {
            _context.FundersRequired.Add(fundersRequired);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFundersRequired", new { id = fundersRequired.Id }, fundersRequired);
        }

        // DELETE: api/FundersRequireds/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FundersRequired>> DeleteFundersRequired(Guid id)
        {
            var fundersRequired = await _context.FundersRequired.FindAsync(id);
            if (fundersRequired == null)
            {
                return NotFound();
            }

            _context.FundersRequired.Remove(fundersRequired);
            await _context.SaveChangesAsync();

            return fundersRequired;
        }

        private bool FundersRequiredExists(Guid id)
        {
            return _context.FundersRequired.Any(e => e.Id == id);
        }
    }
}
