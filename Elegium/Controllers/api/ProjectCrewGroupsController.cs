using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.ProjectCrews;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectCrewGroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectCrewGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<ProjectCrewGroup>>> GetGroupUsers(int groupId)
        {
            //var users = await _context.CrewUnits.Where(a => a.UnitId == unitId).ToListAsync();

            var usersList = await (from user in _context.ProjectCrewGroups.Where(a => a.GroupId == groupId).Include(a => a.ProjectCrew.User)

                                   select new
                                   {
                                       user.Id,
                                       CrewId = user.ProjectCrew.Id,
                                       Name = user.ProjectCrew.User.FirstName + " " + user.ProjectCrew.User.LastName,
                                       user.ProjectCrew.UserId,
                                       ProfileImageAvailable = _context.UserProfiles.Where(u => u.UserId == user.ProjectCrew.UserId).FirstOrDefault().Photo != null ? true : false,
                                   }
                          ).ToListAsync(); ;
            return Ok(usersList);
        }

        public async Task<ActionResult<IEnumerable<ProjectCrew>>> GetAllUsers(int groupId, int projectId)
        {
            ///await _context.pro.ToListAsync();
            await _context.ProjectCrews.ToListAsync();
            var usersList = await (from user in _context.ProjectCrews.Where(a => a.ProjectId == projectId).Include(a => a.User)
                                       // where 1=1
                                       //  join _context.
                                       //where user.ProjectId == ProjectId

                                   join groups in _context.ProjectCrewGroups.Where(a => a.GroupId == groupId) on user.Id equals groups.ProjectCrewId into groupList
                                   from groupCrew in groupList.DefaultIfEmpty()
                                   select new
                                   {
                                       user.Id,
                                       Name = user.User.FirstName + " " + user.User.LastName,
                                       IsCrewUserGroup = groupCrew == null ? false : true,
                                       user.UserId,
                                       ProfileImageAvailable = _context.UserProfiles.Where(u => u.UserId == user.UserId).FirstOrDefault().Photo != null ? true : false,
                                   })
                   .ToListAsync();

            return Ok(usersList);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToGroup([FromQuery] int userId, [FromQuery] int groupId)
        {

            _context.ProjectCrewGroups.Add(new ProjectCrewGroup() { GroupId = groupId, ProjectCrewId = userId });
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupUser(int id)
        {
            var projectCrewGroup = await _context.ProjectCrewGroups.FindAsync(id);
            if (projectCrewGroup == null)
            {
                return NotFound();
            }

            _context.ProjectCrewGroups.Remove(projectCrewGroup);
            await _context.SaveChangesAsync();

            return Ok();
        }

        #region for External User Unit
        public async Task<ActionResult<IEnumerable<ExternalUserGroup>>> GetGroupExternalUsers(int groupId)
        {
            //var users = await _context.CrewUnits.Where(a => a.UnitId == unitId).ToListAsync();

            var usersList = await (from user in _context.ExternalUserGroups.Where(a => a.GroupId == groupId).Include(a => a.ExternalUser)

                                   select new
                                   {
                                       user.Id,
                                       Name = user.ExternalUser.Name,
                                       defaultImageId = _context.ExternalUserFile.Where(a => a.Default && a.ExternalUserId == user.ExternalUser.Id).FirstOrDefault()
                                   }
                          ).ToListAsync(); ;
            return Ok(usersList);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupExternalUsers(int id)
        {
            var projectUnit = await _context.ExternalUserGroups.FindAsync(id);
            if (projectUnit == null)
            {
                return NotFound();
            }

            _context.ExternalUserGroups.Remove(projectUnit);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public async Task<ActionResult<IEnumerable<ProjectExternalUser>>> GetAllExternalUsers(int groupId, int projectId)
        {
            ///await _context.pro.ToListAsync();
            //await _context.ProjectCrews.ToListAsync();
            var usersList = await (from user in _context.ProjectExternalUsers.Where(a => a.ProjectId == projectId)
                                       // where 1=1
                                       //  join _context.
                                       //where user.ProjectId == ProjectId

                                   join units in _context.ExternalUserGroups.Where(a => a.GroupId == groupId) on user.Id equals units.ExternalUserId into unitList
                                   from unitCrew in unitList.DefaultIfEmpty()
                                   select new
                                   {
                                       user.Id,
                                       user.Name,
                                       IsExternalUserGroup = unitCrew == null ? false : true,
                                       defaultImageId = _context.ExternalUserFile.Where(a => a.Default && a.ExternalUserId == user.Id).FirstOrDefault()
                                   })
                   .ToListAsync();

            return Ok(usersList);
        }
        [HttpPost]
        public async Task<IActionResult> AddExternalUserToGroup([FromQuery] int userId, [FromQuery] int groupId)
        {

            _context.ExternalUserGroups.Add(new ExternalUserGroup() { GroupId = groupId, ExternalUserId = userId });
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
        #endregion

        // GET: api/ProjectCrewGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectCrewGroup>>> GetProjectCrewGroups()
        {
            return await _context.ProjectCrewGroups.ToListAsync();
        }

        // GET: api/ProjectCrewGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectCrewGroup>> GetProjectCrewGroup(int id)
        {
            var projectCrewGroup = await _context.ProjectCrewGroups.FindAsync(id);

            if (projectCrewGroup == null)
            {
                return NotFound();
            }

            return projectCrewGroup;
        }

        // PUT: api/ProjectCrewGroups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectCrewGroup(int id, ProjectCrewGroup projectCrewGroup)
        {
            if (id != projectCrewGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectCrewGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectCrewGroupExists(id))
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

        // POST: api/ProjectCrewGroups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProjectCrewGroup>> PostProjectCrewGroup(ProjectCrewGroup projectCrewGroup)
        {
            _context.ProjectCrewGroups.Add(projectCrewGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectCrewGroup", new { id = projectCrewGroup.Id }, projectCrewGroup);
        }

        // DELETE: api/ProjectCrewGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProjectCrewGroup>> DeleteProjectCrewGroup(int id)
        {
            var projectCrewGroup = await _context.ProjectCrewGroups.FindAsync(id);
            if (projectCrewGroup == null)
            {
                return NotFound();
            }

            _context.ProjectCrewGroups.Remove(projectCrewGroup);
            await _context.SaveChangesAsync();

            return projectCrewGroup;
        }

        private bool ProjectCrewGroupExists(int id)
        {
            return _context.ProjectCrewGroups.Any(e => e.Id == id);
        }
    }
}
