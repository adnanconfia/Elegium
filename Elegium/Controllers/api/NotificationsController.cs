using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Dtos.Notification;
using Elegium.ExtensionMethods;
using Elegium.Models;
using Elegium.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{pageIndex}")]
        public async Task<ActionResult> GetNotifications(int pageIndex)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = _context.Notification.Where(a => a.ReceipientId == user.Id)
                .Select(a => new NotificationDto()
                {
                    Title = a.NotificationType.Title,
                    Created = a.Created,
                    When = a.Created.GetRelativeTime(),
                    Id = a.Id,
                    NotificationText = a.NotificationText,
                    Read = a.Read,
                    SenderId = a.Sender.Id,
                    Url = a.Url
                }).OrderByDescending(a => a.Created);

            var pagedData = result.GetPaged(pageIndex, 15).Result;
            return Ok(new { records = pagedData, unreadCount = result.Count(a => !a.Read) });
        }

        [HttpPost]
        public async Task<ActionResult> MarkRead(NotificationDto item)
        {
            var notificationObj = await _context.Notification.FindAsync(item.Id);
            notificationObj.Read = true;
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost]
        public async Task<ActionResult> MarkAllRead()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var notifications = _context.Notification.Where(a => a.ReceipientId == loggedInUser.Id && !a.Read);
            foreach (var n in notifications)
                n.Read = true;
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteNotification(NotificationDto item)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var notification = await _context.Notification.Where(a => a.ReceipientId == loggedInUser.Id && a.Id == item.Id).FirstOrDefaultAsync();
            _context.Remove(notification);
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAllNotifications()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var notifications = _context.Notification.Where(a => a.ReceipientId == loggedInUser.Id);
            _context.Notification.RemoveRange(notifications);
            await _context.SaveChangesAsync();
            return Ok(true);
        }

    }
}
