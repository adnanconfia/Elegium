using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Voting;
using Elegium.Dtos.Voting;

namespace Elegium.Controllers.api.Voting
{
    [Route("api/[controller]")]
    [ApiController]
    public class NominationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NominationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Nominations
        [HttpGet]
        public async Task<ActionResult> GetNominations()
        {
            var nominations =  await _context.Nominations.Select(n => new NominationDto()
            {
                Id = n.Id,
                Name = n.Name,
                Description = n.Description,
                ProductionType = n.ProductionType,
                ProductionTypeId = n.ProductionTypeId,
                Country = n.Country,
                CountryId = n.CountryId,
                TermsAndConditions = n.TermsAndConditions,
                Currency = n.Currency,
                CurrencyId = n.CurrencyId,
                Price = n.Price,
                IsVotingStarted = n.IsVotingStarted,
                IsVotingFinished = n.IsVotingFinished,
                IsResultApproved = n.IsResultApproved,
                StartDate = n.StartDate,
                EndDate = n.EndDate,
                CreatedDate = n.CreatedDate,
                ProjectsAppliedCount = _context.NominationDetails.Where(nd => nd.NominationId == n.Id).Count()
            }).ToListAsync();

            return Ok(nominations);
        }

        // GET: api/Nominations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Nomination>> GetNomination(int id)
        {
            var nomination = await _context.Nominations.FindAsync(id);

            if (nomination == null)
            {
                return NotFound();
            }

            return nomination;
        }

        // PUT: api/Nominations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNomination(int id, Nomination nomination)
        {
            if (id != nomination.Id)
            {
                return BadRequest();
            }

            //var nominationsCount = await _context.Nominations
            //    .Where(n => n.Id != nomination.Id &&
            //        ((n.StartDate <= nomination.StartDate
            //        && n.EndDate >= nomination.EndDate) ||
            //        (n.StartDate >= nomination.StartDate
            //        && n.EndDate <= nomination.EndDate))).CountAsync();

            //if (nominationsCount > 0)
            //    return BadRequest("Nomination is already exists on these dates.");

            _context.Entry(nomination).State = EntityState.Modified;


            if (nomination.IsVotingFinished && !nomination.IsResultApproved)
            {
                var nominationDetailList = await _context.NominationDetails
                    .Where(nd => nd.NominationId == nomination.Id)
                    .Select(nd => nd.Id)
                    .ToListAsync();

                var nominationVote = await _context.FinalVotes
                    .Where(v => nominationDetailList.Contains(v.NominationDetailId))
                    .OrderByDescending(v => v.TotalScore)
                    .FirstOrDefaultAsync();


                var singleNominationDetail = await _context.NominationDetails
                    .Where(nd => nd.Id == nominationVote.NominationDetailId)
                    .FirstOrDefaultAsync();

                singleNominationDetail.IsWinner = true;
                singleNominationDetail.IsSystemSuggested = true;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NominationExists(id))
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

        // POST: api/Nominations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Nomination>> PostNomination(Nomination nomination)
        {
            //var nominationsCount = await _context.Nominations
            //    .Where(n => 
            //        (n.StartDate <= nomination.StartDate
            //        && n.EndDate >= nomination.EndDate) || 
            //        (n.StartDate >= nomination.StartDate
            //        && n.EndDate <= nomination.EndDate)).CountAsync();

            //if (nominationsCount > 0)
            //    return BadRequest("Nomination is already exists on these dates.");

            _context.Nominations.Add(nomination);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNomination", new { id = nomination.Id }, nomination);
        }

        // DELETE: api/Nominations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Nomination>> DeleteNomination(int id)
        {
            var nomination = await _context.Nominations.FindAsync(id);
            if (nomination == null)
            {
                return NotFound();
            }

            _context.Nominations.Remove(nomination);
            await _context.SaveChangesAsync();

            return nomination;
        }

        private bool NominationExists(int id)
        {
            return _context.Nominations.Any(e => e.Id == id);
        }
    }
}
