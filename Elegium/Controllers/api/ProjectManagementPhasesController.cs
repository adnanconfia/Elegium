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
    public class ProjectManagementPhasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectManagementPhasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectManagementPhases
        [HttpGet]
        public async Task<ActionResult<ProjectManagementPhases[]>> GetProjectManagementPhase()
        {
            return await _context.ProjectManagementPhase.ToArrayAsync();
        }

        // GET: api/ProjectManagementPhases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectManagementPhases>> GetProjectManagementPhases(Guid id)
        {
            var projectManagementPhases = await _context.ProjectManagementPhase.FindAsync(id);

            if (projectManagementPhases == null)
            {
                return NotFound();
            }

            return projectManagementPhases;
        }

        // PUT: api/ProjectManagementPhases/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectManagementPhases(Guid id, ProjectManagementPhases projectManagementPhases)
        {
            if (id != projectManagementPhases.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectManagementPhases).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectManagementPhasesExists(id))
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

        // POST: api/ProjectManagementPhases
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProjectManagementPhases>> PostProjectManagementPhases(ProjectManagementPhases projectManagementPhases)
        {
            _context.ProjectManagementPhase.Add(projectManagementPhases);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectManagementPhases", new { id = projectManagementPhases.Id }, projectManagementPhases);
        }

        // DELETE: api/ProjectManagementPhases/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProjectManagementPhases>> DeleteProjectManagementPhases(Guid id)
        {
            var projectManagementPhases = await _context.ProjectManagementPhase.FindAsync(id);
            if (projectManagementPhases == null)
            {
                return NotFound();
            }

            _context.ProjectManagementPhase.Remove(projectManagementPhases);
            await _context.SaveChangesAsync();

            return projectManagementPhases;
        }

        private bool ProjectManagementPhasesExists(Guid id)
        {
            return _context.ProjectManagementPhase.Any(e => e.Id == id);
        }
    }
}
