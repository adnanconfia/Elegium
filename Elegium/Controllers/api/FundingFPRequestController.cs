using Elegium.Data;
using Elegium.Dtos;
using Elegium.Middleware;
using Elegium.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.ExtensionMethods;
using Elegium.Models.ProjectCrews;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FundingFPRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        public FundingFPRequestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<ActionResult<FundingFPRequests>> CreateRequest(FundingFPRequests data)
        {
            var user = await _userManager.GetUserAsync(User);

            var alreadySentRequest = await _context.FundingFPRequests.CountAsync(a => a.ProjectId == data.ProjectId && (a.Status == "A" || a.Status == "P") && a.SenderId == user.Id && a.FundingOrFP == data.FundingOrFP);
            if (alreadySentRequest > 0)
                return NotFound("You have already sent a request.");
            data.SenderId = user.Id;

            await _context.FundingFPRequests.AddAsync(data);
            await _context.SaveChangesAsync();
            var _url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";// string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";

            await _notificationService.GenerateNotificationAsync(
                user,
                _context.Users.Find(data.OwnerId),
               NotificationKind.FundingRequestReceived,
                $"{_url}/#/fundingRequests");
            return data;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FundingFPRequestsDto>>> GetMyOffers()
        {
            var user = await _userManager.GetUserAsync(User);
            var myOffers = await (from k in _context.FundingFPRequests
                                  join u1 in _context.Users
                                  on k.OwnerId equals u1.Id
                                  join u2 in _context.Users
                                  on k.SenderId equals u2.Id
                                  join p in _context.Project
                                  on k.ProjectId equals p.Id
                                  where k.OwnerId == user.Id
                                  select new FundingFPRequestsDto()
                                  {
                                      Id = k.Id,
                                      Status = k.Status,
                                      Description = k.Description,
                                      Offer = k.Offer,
                                      FundingOrFP = k.FundingOrFP,
                                      Sender = u2.GetUserName(),
                                      Owner = u1.GetUserName(),
                                      ProjectId = p.Id,
                                      Created = k.Created.ToUserFriendlyTime(),
                                      OwnerId = u1.Id,
                                      SenderId = u2.Id,
                                      OfferOrLooking = k.OfferOrLooking,
                                      ProjectName = p.Name,
                                      Action = string.Empty
                                  }
                                  ).ToListAsync();

            return myOffers;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FundingFPRequestsDto>>> GetMyRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            var myOffers = await (from k in _context.FundingFPRequests
                                  join u1 in _context.Users
                                  on k.OwnerId equals u1.Id
                                  join u2 in _context.Users
                                  on k.SenderId equals u2.Id
                                  join p in _context.Project
                                  on k.ProjectId equals p.Id
                                  where k.SenderId == user.Id
                                  select new FundingFPRequestsDto()
                                  {
                                      Id = k.Id,
                                      Status = k.Status,
                                      Description = k.Description,
                                      Offer = k.Offer,
                                      FundingOrFP = k.FundingOrFP,
                                      Sender = u2.GetUserName(),
                                      Owner = u1.GetUserName(),
                                      ProjectId = p.Id,
                                      Created = k.Created.ToUserFriendlyTime(),
                                      OwnerId = u1.Id,
                                      SenderId = u2.Id,
                                      OfferOrLooking = k.OfferOrLooking,
                                      ProjectName = p.Name,
                                      Action = string.Empty
                                  }
                                  ).ToListAsync();

            return myOffers;
        }

        [HttpPost]
        public async Task<ActionResult<FundingFPRequestsDto>> TakeFundingRequestAction(FundingFPRequestsDto offer)
        {
            var user = await _userManager.GetUserAsync(User);
            var resProject = await _context.FundingFPRequests.Where(a => a.Id == offer.Id && a.Status == "P").FirstOrDefaultAsync();
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
                    UserId = offer.OfferOrLooking == "L" ? offer.OwnerId : offer.SenderId,
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
               offer.Action == "A" ? NotificationKind.FundingRequestApproved : offer.Action == "R" ? NotificationKind.FundingRequestRejected : "",
                $"{_url}/#/fundingRequests");

            return offer;
        }
    }
}
