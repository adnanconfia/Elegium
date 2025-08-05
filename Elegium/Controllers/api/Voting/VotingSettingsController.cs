using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Voting;

namespace Elegium.Controllers.api.Voting
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingSettingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VotingSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VotingSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VotingSetting>>> GetVotingSettings()
        {
            return await _context.VotingSettings.ToListAsync();
        }

        // GET: api/VotingSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VotingSetting>> GetVotingSetting(int id)
        {
            var votingSetting = await _context.VotingSettings.FindAsync(id);

            if (votingSetting == null)
            {
                return NotFound();
            }

            return votingSetting;
        }

        // PUT: api/VotingSettings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVotingSetting(int id, VotingSetting votingSetting)
        {
            if (id != votingSetting.Id)
            {
                return BadRequest();
            }

            _context.Entry(votingSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VotingSettingExists(id))
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

        // POST: api/VotingSettings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<VotingSetting>> PostVotingSetting(VotingSetting votingSetting)
        {
            _context.VotingSettings.Add(votingSetting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVotingSetting", new { id = votingSetting.Id }, votingSetting);
        }

        // DELETE: api/VotingSettings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VotingSetting>> DeleteVotingSetting(int id)
        {
            var votingSetting = await _context.VotingSettings.FindAsync(id);
            if (votingSetting == null)
            {
                return NotFound();
            }

            _context.VotingSettings.Remove(votingSetting);
            await _context.SaveChangesAsync();

            return votingSetting;
        }

        private bool VotingSettingExists(int id)
        {
            return _context.VotingSettings.Any(e => e.Id == id);
        }
    }
}
