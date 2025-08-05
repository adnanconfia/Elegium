using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Dtos.ProjectDtos;
using Elegium.Models.Projects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectDisputesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectDisputesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetDisputes()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var disputes = await _context.ProjectDisputes
                .Include(r => r.Project)
                .Include(r => r.User)
                .Where(d => d.Project.UserId == appUser.Id)
                .Select(d => new ProjectDisputeDto()
                {
                    Project = d.Project,
                    ProjectId = d.ProjectId,
                    DisputeDate = d.DisputeDate,
                    ProjectDisputeDetails = _context.ProjectDisputeDetails.Where(pd => pd.ProjectDisputeId == d.Id).OrderByDescending(pd => pd.EnteryDate).ToList(),
                    Status = d.Status,
                    UserId = d.UserId,
                    User = d.User
                })
                .OrderBy(d => new { d.Status, d.DisputeDate})
                .ToListAsync();

            return Ok(disputes);
        }

        public async Task<IActionResult> GetMyDisputes()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var disputes = await _context.ProjectDisputes
                .Include(r => r.Project)
                .Include(r => r.User)
                .Where(d => d.UserId == appUser.Id)
                .Select(d => new ProjectDisputeDto()
                {
                    Project = d.Project,
                    ProjectId = d.ProjectId,
                    DisputeDate = d.DisputeDate,
                    ProjectDisputeDetails = _context.ProjectDisputeDetails.Where(pd => pd.ProjectDisputeId == d.Id).OrderByDescending(pd => pd.EnteryDate).ToList(),
                    Status = d.Status,
                    UserId = d.UserId,
                    User = d.User
                })
                .OrderBy(d => new { d.Status, d.DisputeDate })
                .ToListAsync();

            return Ok(disputes);
        }

        [HttpPost]
        public async Task<ActionResult> RejectDispute([FromBody] ProjectDisputeDto disputeDto)
        {
            var appUser = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (disputeDto == null)
                return BadRequest();

            //var request = await _context.ProjectCrews.Where(c => c.ProjectId == disputeDto.ProjectId && c.UserId == appUser.Id).FirstOrDefaultAsync();
            //if (request == null)
            //    return BadRequest("User is not authorized for this action");

            var dispute = await _context.ProjectDisputes.Where(d => d.ProjectId == disputeDto.ProjectId && d.Project.UserId == appUser.Id).FirstOrDefaultAsync();
            if (dispute == null)
            {
                return BadRequest("User is not authorized for this action.");
            }

            dispute.Status = 3;
            
            _context.ProjectDisputeDetails.Add(new ProjectDisputeDetail()
            {
                ProjectDisputeId = dispute.Id,
                Description = disputeDto.Description,
                UserId = appUser.Id
            });

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> ApproveDispute([FromBody] ProjectDisputeDto disputeDto)
        {
            var appUser = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (disputeDto == null)
                return BadRequest();

            
            var dispute = await _context.ProjectDisputes.Where(d => d.ProjectId == disputeDto.ProjectId && d.Project.UserId == appUser.Id).FirstOrDefaultAsync();
            if (dispute == null)
            {
                return BadRequest("User is not authorized for this action.");
            }

            dispute.Status = 2;

            _context.ProjectDisputeDetails.Add(new ProjectDisputeDetail()
            {
                ProjectDisputeId = dispute.Id,
                Description = disputeDto.Description,
                UserId = appUser.Id
            });


            var projectCrew = await _context.ProjectCrews.Where(c => c.ProjectId == disputeDto.ProjectId && c.UserId == dispute.UserId).FirstOrDefaultAsync();
            if (projectCrew == null)
                return BadRequest("User is not found in the crew list");

            projectCrew.IsActive = false;
            projectCrew.SeperationDate = DateTime.UtcNow;


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> DisputeAgain([FromBody] ProjectDisputeDto disputeDto)
        {
            var appUser = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (disputeDto == null)
                return BadRequest();

            //var request = await _context.ProjectCrews.Where(c => c.ProjectId == disputeDto.ProjectId && c.UserId == appUser.Id).FirstOrDefaultAsync();
            //if (request == null)
            //    return BadRequest("User is not authorized for this action");

            var dispute = await _context.ProjectDisputes.Where(d => d.ProjectId == disputeDto.ProjectId && d.UserId == appUser.Id).FirstOrDefaultAsync();
            if (dispute == null)
            {
                return BadRequest("User is not authorized for this action.");
            }

            dispute.Status = 1;

            _context.ProjectDisputeDetails.Add(new ProjectDisputeDetail()
            {
                ProjectDisputeId = dispute.Id,
                Description = disputeDto.Description,
                UserId = appUser.Id
            });

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
