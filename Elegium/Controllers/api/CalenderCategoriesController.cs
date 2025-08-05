using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Models;
using Elegium.Models.Calendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalenderCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CalenderCategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetCalenderCategories(int projectId)
        {
            return Ok(await _context.CalenderCategories.Where(c => c.ProjectId == projectId).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCalenderAllCategories()
        {
            return Ok(await _context.CalenderCategories.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateCalenderCategory([FromBody] CalenderCategory calenderCategory)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (calenderCategory.Id == 0)
                {
                    calenderCategory.UserId = currentUser.Id;
                    _context.CalenderCategories.Add(calenderCategory);
                }
                else
                {
                    var calenderInDb = await _context.CalenderCategories.Where(c => c.Id == calenderCategory.Id).FirstOrDefaultAsync();
                    calenderInDb.Name = calenderCategory.Name;
                    calenderInDb.Color = calenderCategory.Color;
                    _context.Entry(calenderInDb).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CalenderCategory>> DeleteCalenderCategory(int id)
        {
            var calenderCategory = await _context.CalenderCategories.FindAsync(id);
            if (calenderCategory == null)
            {
                return NotFound();
            }

            _context.CalenderCategories.Remove(calenderCategory);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
