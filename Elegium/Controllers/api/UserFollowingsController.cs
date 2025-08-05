using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Microsoft.AspNetCore.SignalR;
using Elegium.Hubs;
using Elegium.ExtensionMethods;
using Microsoft.AspNetCore.Identity;
using Elegium.Middleware;
using System.Security.Permissions;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserFollowingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private string _url;
        public UserFollowingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
            _userManager = userManager;
            
        }

        // GET: api/UserFollowings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFollowing>>> GetUserFollowing()
        {
            return await _context.UserFollowing.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleUserFollowing(string id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var message = "";
            var isFollowBack = false;
            var receiver = await _userManager.FindByIdAsync(id);
            _url = string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";
            var userFollowing = await _context.UserFollowing.Where(f => f.UserId == appUser.Id && f.FollowingToId == id).FirstOrDefaultAsync();
            if (userFollowing == null)
            {
                userFollowing = new UserFollowing()
                {
                    UserId = appUser.Id,
                    FollowingToId = id
                };
                _context.UserFollowing.Add(userFollowing);
                message = "following";

                var userFollowingMe = await _context.UserFollowing.Where(f => f.UserId == id && f.FollowingToId == appUser.Id).FirstOrDefaultAsync();
                if (userFollowingMe != null)
                    isFollowBack = true;

                if (isFollowBack)
                    await _notificationService.GenerateNotificationAsync(appUser, receiver, NotificationKind.FollowBack, _url + "#/professionaldetails/" + appUser.Id);
                else
                    await _notificationService.GenerateNotificationAsync(appUser, receiver, NotificationKind.Follow, _url + "#/professionaldetails/" + appUser.Id);
            }
            else
            {
                _context.UserFollowing.Remove(userFollowing);
                message = "unfollow";
            }
            //_url
            await _context.SaveChangesAsync();

            return Ok(message);
        }

        // GET: api/UserFollowings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserFollowing>> GetUserFollowing(int id)
        {
            var userFollowing = await _context.UserFollowing.FindAsync(id);

            if (userFollowing == null)
            {
                return NotFound();
            }

            return userFollowing;
        }

        // PUT: api/UserFollowings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserFollowing(int id, UserFollowing userFollowing)
        {
            if (id != userFollowing.Id)
            {
                return BadRequest();
            }

            _context.Entry(userFollowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFollowingExists(id))
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

        // POST: api/UserFollowings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserFollowing>> PostUserFollowing(UserFollowing userFollowing)
        {
            _context.UserFollowing.Add(userFollowing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserFollowing", new { id = userFollowing.Id }, userFollowing);
        }

        // DELETE: api/UserFollowings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserFollowing>> DeleteUserFollowing(int id)
        {
            var userFollowing = await _context.UserFollowing.FindAsync(id);
            if (userFollowing == null)
            {
                return NotFound();
            }

            _context.UserFollowing.Remove(userFollowing);
            await _context.SaveChangesAsync();
            return userFollowing;
        }

        private bool UserFollowingExists(int id)
        {
            return _context.UserFollowing.Any(e => e.Id == id);
        }
    }
}
