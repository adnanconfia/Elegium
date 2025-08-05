using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Dtos;
using Elegium.Middleware;
using Elegium.Models;
using Elegium.Models.Professionals;
using Elegium.Models.ProjectCrews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfessionalHireRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private string _url;

        public ProfessionalHireRequestsController(ApplicationDbContext context
            , UserManager<ApplicationUser> userManager
            , INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetProfessionalHireRequests()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var hireRequests = await _context.ProfessionalHireRequests
                .Include(r => r.Project)
                .Where(r => r.ProfessionalId == appUser.Id)
                .Select(r => new ProfessionalHireRequestDto()
                {
                    Id = r.Id,
                    Professional = r.Professional,
                    ProfessionalId = r.ProfessionalId,
                    Project = r.Project,
                    ProjectId = r.ProjectId,
                    WorkingPosition = r.WorkingPosition,
                    WorkingPositionId = r.WorkingPositionId,
                    Message = r.Message,
                    MediaFiles = _context.ProfessionalHireRequestMediaFiles.Where(t => t.ProfessionalHireRequestId == r.Id).Take(3).ToList(),
                    RequestDateTime = r.RequestDateTime,
                    Status = r.Status,
                    StatusDateTime = r.StatusDateTime
                })
                .OrderBy(r => r.Status).ToListAsync();

            return Ok(hireRequests);
        }

        public async Task<IActionResult> GetMyProfessionalHireRequests()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var hireRequests = await _context.ProfessionalHireRequests
                .Include(r => r.Project)
                .Where(r => r.Project.UserId == appUser.Id)
                .OrderBy(r => r.Status)
                .Select(r => new ProfessionalHireRequestDto()
                {
                    Id = r.Id,
                    Professional = r.Professional,
                    ProfessionalId = r.ProfessionalId,
                    Project = r.Project,
                    ProjectId = r.ProjectId,
                    WorkingPosition = r.WorkingPosition,
                    WorkingPositionId = r.WorkingPositionId,
                    Message = r.Message,
                    MediaFiles = _context.ProfessionalHireRequestMediaFiles.Where(t => t.ProfessionalHireRequestId == r.Id).Take(3).ToList(),
                    RequestDateTime = r.RequestDateTime,
                    Status = r.Status,
                    StatusDateTime = r.StatusDateTime
                })
                .ToListAsync();

            return Ok(hireRequests);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfessionalHireRequest([FromBody] ProfessionalHireRequest request)
        {
            try
            {
                var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                var project = _context.Project.Where(p => p.Id == request.ProjectId && p.UserId == appUser.Id && !p.Deleted).FirstOrDefault();
                if (project == null)
                    return BadRequest("User is not authorized for this project");
                var hireRequest = await _context.ProfessionalHireRequests.Where(r => r.ProfessionalId == request.ProfessionalId && r.ProjectId == request.ProjectId && r.Status != 3).FirstOrDefaultAsync();
                if (hireRequest != null)
                {
                    if(hireRequest.Status == 2)
                    {
                        var projectCrew = await _context.ProjectCrews.Where(c => c.IsActive == true && c.ProjectId == hireRequest.ProjectId && c.UserId == hireRequest.ProfessionalId).FirstOrDefaultAsync();
                        if (projectCrew != null)
                            return BadRequest("Request is already sent for this project");
                    }
                    else
                        return BadRequest("Request is already sent for this project");
                }

                request.Status = 1;
                request.StatusDateTime = DateTime.UtcNow;

                _context.ProfessionalHireRequests.Add(request);
                await _context.SaveChangesAsync();


                var receiver = await _userManager.FindByIdAsync(request.ProfessionalId);

                _url = string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";
                
                await _notificationService.GenerateNotificationAsync(appUser, receiver, NotificationKind.HireRequestSent, _url + "/ProfessionalHireRequests");

                return Ok(new { success = true, suc = "Success", Message = "Request is sent successfully!", RequestId=request.Id });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, suc = "Failure", Message = ex.Message + "--" + (ex.InnerException == null ? string.Empty : ex.InnerException.Message) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProfessionalHireRequestMediaFiles([FromBody] ProfessionalHireRequestMediaFile[] mediaFiles)
        {
            try
            {
                if (mediaFiles != null && mediaFiles.Length > 0)
                {
                    await _context.ProfessionalHireRequestMediaFiles.AddRangeAsync(mediaFiles);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("[controller]/[action]/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> RejectProfessionalHireRequest(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var hireRequest = await _context.ProfessionalHireRequests.Where(r => r.ProfessionalId == appUser.Id && r.Id == id).FirstOrDefaultAsync();
            if(hireRequest == null)
                return BadRequest("User is not authorized for this action");

            hireRequest.Status = 3;
            hireRequest.StatusDateTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Route("[controller]/[action]/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> AcceptProfessionalHireRequest(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var hireRequest = await _context.ProfessionalHireRequests.Where(r => r.ProfessionalId == appUser.Id && r.Id == id).FirstOrDefaultAsync();
            if (hireRequest == null)
                return BadRequest("User is not authorized for this action");

            hireRequest.Status = 2;
            hireRequest.StatusDateTime = DateTime.UtcNow;

            var crew = await _context.ProjectCrews.Where(c => c.UserId == appUser.Id && c.ProjectId == hireRequest.ProjectId).FirstOrDefaultAsync();
            if(crew == null)
            {
                crew = new ProjectCrew()
                {
                    ProjectId = hireRequest.ProjectId,
                    IsFromDiscovery = true,
                    UserId = appUser.Id,
                    IsActive = true,
                    ProfessionalHireRequestId = hireRequest.Id
                };
                await _context.ProjectCrews.AddAsync(crew);
                await _context.SaveChangesAsync();

                var crewPos = new ProjectCrewPosition();
                crewPos.ProjectCrewId = crew.Id;
                crewPos.PositionId = hireRequest.WorkingPositionId;
                await _context.ProjectCrewPositions.AddAsync(crewPos);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
