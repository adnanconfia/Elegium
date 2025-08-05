using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.Models.ProjectCrews;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectUserGroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectUserGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<ProjectUnitsController>
        public async Task<ActionResult<IEnumerable<ProjectUserGroup>>> GetUserGroups(int projectId)
        {
            return await _context.ProjectUserGroups.Where(a=>a.ProjectId== projectId).ToListAsync();
        }

        // GET api/<ProjectUnitsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> SaveorUpdateProjectUserGroup([FromBody] ProjectUserGroup projectUserGroup)
        {
            //projectUserGroup.ProjectId = 4;
            _context.ProjectUserGroups.Add(projectUserGroup);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProjectUserGroup([FromBody] ProjectUserGroup projectUserGroup)
        {
            

            try
            {
                _context.ProjectUserGroups.Update(projectUserGroup);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectUserGroups(int id)
        {
            var projectUserGroups = await _context.ProjectUserGroups.FindAsync(id);
            if (projectUserGroups == null)
            {
                return NotFound();
            }

            _context.ProjectUserGroups.Remove(projectUserGroups);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectUserGroup>> GetProjectGroup(int id)
        {
            var ProjectUserGroup = await _context.ProjectUserGroups.FindAsync(id);

            if (ProjectUserGroup == null)
            {
                return NotFound();
            }

            return ProjectUserGroup;
        }
    }
}
