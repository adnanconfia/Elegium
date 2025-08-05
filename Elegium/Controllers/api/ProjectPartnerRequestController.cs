using Elegium.Data;
using Elegium.Dtos;
using Elegium.ExtensionMethods;
using Elegium.Middleware;
using Elegium.Models;
using Elegium.Models.ProjectCrews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProjectPartnerRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        public ProjectPartnerRequestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }
        [HttpPost]
        public async Task<ActionResult<ProjectPartnerRequest>> CreateRequest(ProjectPartnerRequest data)
        {
            var user = await _userManager.GetUserAsync(User);

            var alreadySentRequest = await _context.ProjectPartnerRequests.CountAsync(a => a.ProjectId == data.ProjectId && (a.Status == "A" || a.Status == "P") && a.SenderId == user.Id);
            if (alreadySentRequest > 0)
                return NotFound("You have already sent a request.");
            data.SenderId = user.Id;

            await _context.ProjectPartnerRequests.AddAsync(data);
            await _context.SaveChangesAsync();
            var _url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";// string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";

            await _notificationService.GenerateNotificationAsync(
                user,
                _context.Users.Find(data.OwnerId),
               NotificationKind.ProjectPartnerRequestRequested,
                $"{_url}/#/projectpartnerrequests");
            return data;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectPartnerRequestDto>>> GetMyOffers()
        {
            var user = await _userManager.GetUserAsync(User);
            var myOffers = await (from k in _context.ProjectPartnerRequests
                                  join u1 in _context.Users
                                  on k.OwnerId equals u1.Id
                                  join u2 in _context.Users
                                  on k.SenderId equals u2.Id
                                  join p in _context.Project
                                  on k.ProjectId equals p.Id
                                  where k.OwnerId == user.Id
                                  select new ProjectPartnerRequestDto()
                                  {
                                      Id = k.Id,
                                      Status = k.Status,
                                      Description = k.Description,
                                      Sender = u2.GetUserName(),
                                      Owner = u1.GetUserName(),
                                      ProjectId = p.Id,
                                      Created = k.Created.ToUserFriendlyTime(),
                                      OwnerId = u1.Id,
                                      SenderId = u2.Id,
                                      ProjectName = p.Name,
                                      Action = string.Empty
                                  }
                                  ).ToListAsync();

            return myOffers;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectPartnerRequestDto>>> GetMyRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            var myOffers = await (from k in _context.ProjectPartnerRequests
                                  join u1 in _context.Users
                                  on k.OwnerId equals u1.Id
                                  join u2 in _context.Users
                                  on k.SenderId equals u2.Id
                                  join p in _context.Project
                                  on k.ProjectId equals p.Id
                                  where k.SenderId == user.Id
                                  select new ProjectPartnerRequestDto()
                                  {
                                      Id = k.Id,
                                      Status = k.Status,
                                      Description = k.Description,
                                      Sender = u2.GetUserName(),
                                      Owner = u1.GetUserName(),
                                      ProjectId = p.Id,
                                      Created = k.Created.ToUserFriendlyTime(),
                                      OwnerId = u1.Id,
                                      SenderId = u2.Id,
                                      ProjectName = p.Name,
                                      Action = string.Empty
                                  }
                                  ).ToListAsync();

            return myOffers;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectPartnerRequestDto>> TakeRequestAction(ProjectPartnerRequestDto offer)
        {
            var user = await _userManager.GetUserAsync(User);
            var resProject = await _context.ProjectPartnerRequests.Where(a => a.Id == offer.Id && a.Status == "P").FirstOrDefaultAsync();
            if (resProject == null)
                return NotFound();

            if (offer.Action == "C" || offer.Action == "R")
            {
                resProject.Status = offer.Action;
                _context.Entry(resProject).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                resProject.Status = offer.Action;
                var projectCrew = new ProjectCrew()
                {
                    ProjectId = offer.ProjectId,
                    UserId = offer.OwnerId,
                    IsActive = true,
                    IsFromDiscovery = true
                };
                await _context.ProjectCrews.AddAsync(projectCrew);
                await _context.SaveChangesAsync();
                ProjectCrewPosition posi = new ProjectCrewPosition()
                {
                    ProjectCrewId = projectCrew.Id,
                    PositionId = 10
                };

                await _context.ProjectCrewPositions.AddAsync(posi);
                await _context.SaveChangesAsync();
            }

            var _url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";// string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";

            await _notificationService.GenerateNotificationAsync(
                user,
                _context.Users.Find(offer.OwnerId),
               offer.Action == "A" ? NotificationKind.ProjectPartnerRequestAppproved : offer.Action == "R" ? NotificationKind.ProjectPartnerRequestRejected : "",
                $"{_url}/#/projectpartnerrequests");

            return offer;
        }
    }
}
