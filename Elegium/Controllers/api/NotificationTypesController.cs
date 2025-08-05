using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.ExtensionMethods;
using Elegium.Models;
using Elegium.Models.Notifications;
using Microsoft.AspNetCore.Identity;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationTypesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/NotificationTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationType>>> GetNotificationType()
        {
            return await _context.NotificationType.ToListAsync();
        }

        // GET: api/NotificationTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationType>> GetNotificationType(Guid id)
        {
            var notificationType = await _context.NotificationType.FindAsync(id);

            if (notificationType == null)
            {
                return NotFound();
            }

            return notificationType;
        }

        // PUT: api/NotificationTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotificationType(Guid id, NotificationType notificationType)
        {
            if (id != notificationType.Id)
            {
                return BadRequest();
            }

            _context.Entry(notificationType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationTypeExists(id))
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

        // POST: api/NotificationTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<NotificationType>> PostNotificationType(List<NotificationType> notificationTypes)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            try
            {


                //Save User skills in DB
                foreach (var nType in notificationTypes)
                {
                    var uType = await _context.NotificationType.FindAsync(nType.Id);
                    if (uType == null)
                    {
                        uType = new NotificationType()
                        {
                            ApplicationUser = loggedInUser,
                            Name = nType.Name,
                            Template = nType.Template,
                            Title = nType.Title,
                            Type = nType.Type
                        };

                        await _context.NotificationType.AddAsync(uType);
                    }
                    else
                    {
                        uType.Name = nType.Name;
                        uType.Template = nType.Template;
                        uType.Title = nType.Title;
                        uType.Type = nType.Type;
                        _context.Entry(uType).State = EntityState.Modified;
                    }
                }
                //delete skills from db that are not received from UI
                var nTypeIds = notificationTypes.Select(c => c.Id).ToList();
                var nTypeTobeRemoved = _context.NotificationType.Where(c => !nTypeIds.Contains(c.Id) && c.ApplicationUserId == loggedInUser.Id);
                foreach (var nType in nTypeTobeRemoved)
                {
                    _context.NotificationType.Remove(nType);
                }
                await _context.SaveChangesAsync();
                var list = await _context.NotificationType.ToListAsync();


                return Ok(new { success = true, Msg = "Notification saved successfully!", List = list });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, Msg = ex.Message + (ex.InnerException == null ? string.Empty : " - " + ex.InnerException.Message) });
            }
        }

        // DELETE: api/NotificationTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NotificationType>> DeleteNotificationType(Guid id)
        {
            var notificationType = await _context.NotificationType.FindAsync(id);
            if (notificationType == null)
            {
                return NotFound();
            }

            _context.NotificationType.Remove(notificationType);
            await _context.SaveChangesAsync();

            return notificationType;
        }

        private bool NotificationTypeExists(Guid id)
        {
            return _context.NotificationType.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult GetNotificationKind()
        {
            return Ok(typeof(NotificationKind).GetAllPublicConstantValues<string>());
        }
    }
}
