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
    public class ProjectUnitsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectUnitsController(ApplicationDbContext context)
        {
            _context = context;
        }
        #region for User Unit
        public async Task<ActionResult<IEnumerable<ProjectCrewUnit>>> GetUnitUsers(int unitId)
        {
            //var users = await _context.CrewUnits.Where(a => a.UnitId == unitId).ToListAsync();

            var usersList = await (from user in _context.CrewUnits.Where(a => a.UnitId == unitId).Include(a=>a.ProjectCrew.User)
                          
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitUsers(int id)
        {
            var projectUnit = await _context.CrewUnits.FindAsync(id);
            if (projectUnit == null)
            {
                return NotFound();
            }

            _context.CrewUnits.Remove(projectUnit);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public async Task<ActionResult<IEnumerable<ProjectCrew>>> GetAllUsers(int unitId, int projectId)
        {
            ///await _context.pro.ToListAsync();
            await _context.ProjectCrews.ToListAsync();
            var usersList = await (from user in _context.ProjectCrews.Where(a=>a.ProjectId==projectId).Include(a => a.User)
                                  // where 1=1
                                    //  join _context.
                                        //where user.ProjectId == ProjectId

                                       join units in _context.CrewUnits.Where(a=>a.UnitId== unitId) on user.Id equals units.ProjectCrewId into unitList
                                   from unitCrew in unitList.DefaultIfEmpty()
                                   select new
                                   {
                                       user.Id,
                                       Name = user.User.FirstName + " " + user.User.LastName,
                                       IsCrewUserUnit= unitCrew==null?false:true,
                                       user.UserId,
                                       ProfileImageAvailable = _context.UserProfiles.Where(u => u.UserId == user.UserId).FirstOrDefault().Photo != null ? true : false,
                                   })
                   .ToListAsync();

            return Ok(usersList);
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToUnit1([FromQuery] int userId, [FromQuery] int unitId)
        {

            _context.CrewUnits.Add(new ProjectCrewUnit() { UnitId = unitId, ProjectCrewId = userId });
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

        #region for External User Unit
        public async Task<ActionResult<IEnumerable<ExternalUserUnit>>> GetUnitExternalUsers(int unitId)
        {
            //var users = await _context.CrewUnits.Where(a => a.UnitId == unitId).ToListAsync();

            var usersList = await (from user in _context.ExternalUserUnits.Where(a => a.UnitId == unitId).Include(a => a.ExternalUser)

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
        public async Task<IActionResult> DeleteUnitExternalUsers(int id)
        {
            var projectUnit = await _context.ExternalUserUnits.FindAsync(id);
            if (projectUnit == null)
            {
                return NotFound();
            }

            _context.ExternalUserUnits.Remove(projectUnit);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public async Task<ActionResult<IEnumerable<ProjectExternalUser>>> GetAllExternalUsers(int unitId, int projectId)
        {
            ///await _context.pro.ToListAsync();
            //await _context.ProjectCrews.ToListAsync();
            var usersList = await (from user in _context.ProjectExternalUsers.Where(a=>a.ProjectId== projectId)
                                       // where 1=1
                                       //  join _context.
                                       //where user.ProjectId == ProjectId

                                   join units in _context.ExternalUserUnits.Where(a => a.UnitId == unitId) on user.Id equals units.ExternalUserId into unitList
                                   from unitCrew in unitList.DefaultIfEmpty()
                                   select new
                                   {
                                       user.Id,
                                       user.Name,
                                       IsExternalUserUnit = unitCrew == null ? false : true,
                                       defaultImageId = _context.ExternalUserFile.Where(a => a.Default && a.ExternalUserId == user.Id).FirstOrDefault()
                                   })
                   .ToListAsync();

            return Ok(usersList);
        }
        [HttpPost]
        public async Task<IActionResult> AddExternalUserToUnit([FromQuery] int userId, [FromQuery] int unitId)
        {

            _context.ExternalUserUnits.Add(new ExternalUserUnit() { UnitId = unitId, ExternalUserId = userId });
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


        // GET: api/<ProjectUnitsController>
        public async Task<ActionResult<IEnumerable<ProjectUnit>>> GetProjectUnits(int projectId)
        {
            return await _context.ProjectUnits.Where(a => a.ProjectId == projectId).ToListAsync();
        }

        // GET api/<ProjectUnitsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> SaveorUpdateProjectUnit([FromBody] ProjectUnit projectUnit)
        {
            //projectUnit.ProjectId = 4;
            _context.ProjectUnits.Add(projectUnit);
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
        public async Task<IActionResult> UpdateProjectUnit([FromBody] ProjectUnit projectUnit)
        {


            try
            {
                _context.ProjectUnits.Update(projectUnit);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return Ok();
        }

        // PUT api/<ProjectUnitsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectUnit(int id)
        {
            var projectUnit = await _context.ProjectUnits.FindAsync(id);
            if (projectUnit == null)
            {
                return NotFound();
            }

            _context.ProjectUnits.Remove(projectUnit);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
