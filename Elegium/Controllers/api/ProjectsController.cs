using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Projects;
using Elegium.ViewModels.ProjectViewModels;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Hosting;
using Elegium.Dtos.ProjectDtos;
using Elegium.Models.ProjectCrews;
using Microsoft.AspNetCore.Identity;
using Elegium.Models;
using Microsoft.AspNetCore.Authorization;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetProjectsName()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var projects = await _context.Project.Where(p => p.UserId == appUser.Id && !p.Deleted)
                .Include(p => p.ProductionType)
                .Include(p => p.Currency)
                .Include(p => p.ProductionLanguage)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    ProjectStatus = p.IsStarted ? "Running" : ""
                })
                .ToListAsync();

            return Ok(projects);
        }

        [HttpGet]
        public async Task<ActionResult> GetProjectsForNomination()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var projects = await _context.Project.Where(p => p.UserId == appUser.Id && !p.Deleted)
                .Include(p => p.ProductionType)
                .Include(p => p.Currency)
                .Include(p => p.ProductionLanguage)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.OnBoardingCompleted,
                    IsVoteFundingApplied = _context.NominationDetails.Where(n => n.ProjectId == p.Id).Count() > 0 ? true : false
                })
                .ToListAsync();

            return Ok(projects);
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModelDto>>> GetProjects()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            try
            {
                List<ProjectViewModelDto> viewModel = new List<ProjectViewModelDto>();

                var projects = await _context.Project.Where(p => p.UserId == appUser.Id && !p.Deleted)
                    .Include(p => p.ProductionType)
                    .Include(p => p.Currency)
                    .Include(p => p.ProductionLanguage)
                    .Select(p => new ProjectDto()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Detail = p.Detail,
                        CreatedDateTime = p.CreatedDateTime,
                        ProductionType = p.ProductionType,
                        ProductionLength = p.ProductionLength,
                        ProductionLengthMM = p.ProductionLengthMM,
                        ProductionLengthSS = p.ProductionLengthSS,
                        ProductionRecordingMethod = p.ProductionRecordingMethod,
                        ProductionAspectRatio = p.ProductionAspectRatio,
                        ProductionMode = p.ProductionMode,
                        ProductionColor = p.ProductionColor,
                        ProductionLanguage = p.ProductionLanguage,
                        Currency = p.Currency,
                        Color = p.Color,
                        IsStarted = p.IsStarted,
                        IsFinished = p.IsFinished,
                        StartDate = p.StartDate,
                        FinishDate = p.FinishDate,
                        ContactName = p.ContactName,
                        ContactEmail = p.ContactEmail,
                        ContactPhone = p.ContactPhone,
                        ContactFax = p.ContactFax,
                        UserId = p.UserId,
                        OnBoardingCompleted = p.OnBoardingCompleted,
                        IsVoteFundingApplied = _context.NominationDetails.Where(n => n.ProjectId == p.Id).Count() > 0 ? true : false,
                        ProjectStatus = p.IsStarted ? "Running" : ""
                    })
                    .ToListAsync();

                foreach (ProjectDto project in projects)
                {
                    var projectPartnerRequirement = await _context.ProjectPartnerRequirement
                        .Select(p => new ProjectPartnerRequirementDto()
                        {
                            Id = p.Id,
                            Budget = p.Budget,
                            YourFinancialShare = p.YourFinancialShare,
                            ProjectManagementPhaseId = p.ProjectManagementPhaseId,
                            ProjectPartnersCount = p.ProjectPartnersCount,
                            SynopsisCompleted = p.SynopsisCompleted,
                            PlotCompleted = p.PlotCompleted,
                            ScreenplayCompleted = p.ScreenplayCompleted,
                            ScreenplayWorkRequired = p.ScreenplayWorkRequired,
                            NeedFinancialParticipation = p.NeedFinancialParticipation,
                            ProjectId = p.ProjectId
                        })
                        .FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    var projectFunding = await _context.ProjectFunding
                        .Select(p => new ProjectFundingDto()
                        {
                            Id = p.Id,
                            Amount = p.Amount,
                            Currency = p.Currency,
                            CurrencyId = p.CurrencyId,
                            ProjectManagementPhaseId = p.ProjectManagementPhaseId,
                            Requirements = p.Requirements,
                            FundersRequired = p.FundersRequired,
                            BenefitsOffer = p.BenefitsOffer,
                            ProjectId = p.ProjectId
                        })
                        .FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    var projectFinancialParticipation = await _context.ProjectFinancialParticipation
                        .Select(p => new ProjectFinancialParticipationDto()
                        {
                            Id = p.Id,
                            Amount = p.Amount,
                            Currency = p.Currency,
                            CurrencyId = p.CurrencyId,
                            ProjectManagementPhaseId = p.ProjectManagementPhaseId,
                            FundersRequired = p.FundersRequired,
                            FundersRequiredId = p.FundersRequiredId,
                            BenefitsOffer = p.BenefitsOffer,
                            Requirements = p.Requirements,
                            OtherRequirements = p.OtherRequirements,
                            ProjectId = p.ProjectId
                        })
                        .FirstOrDefaultAsync(p => p.ProjectId == project.Id);

                    ProjectViewModelDto vm = new ProjectViewModelDto()
                    {
                        Project = project,
                        ProjectPartnerRequirement = projectPartnerRequirement,
                        ProjectPartners = await _context.ProjectPartner
                        .Where(p => p.ProjectId == project.Id)
                        .Select(p => new ProjectPartnerDto()
                        {
                            Id = p.Id,
                            ProjectPartnerRole = p.ProjectPartnerRole,
                            FinancialParticipationRequired = p.FinancialParticipationRequired,
                            FinancialShare = p.FinancialShare,
                            ProjectId = p.ProjectId
                        })
                        .ToListAsync(),
                        ProjectFunding = projectFunding,
                        ProjectFinancialParticipation = projectFinancialParticipation,
                    };
                    viewModel.Add(vm);
                }

                return viewModel;
            }catch(Exception ex)
            {
                throw;
            }
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectViewModel>> GetProject(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var project = await _context.Project
                .Where(p => p.UserId == appUser.Id && p.Id == id && !p.Deleted)
                .Include(p => p.ProductionType)
                .Include(p => p.Currency)
                .Include(p => p.ProductionLanguage)
                .FirstOrDefaultAsync();

            if (project == null) return NotFound("Project not found for this user!");
            var projectPartnerRequirement = await _context.ProjectPartnerRequirement.Where(p => p.ProjectId == project.Id).FirstOrDefaultAsync();
            var projectFunding = await _context.ProjectFunding.Where(p => p.ProjectId == project.Id).FirstOrDefaultAsync();
            var projectFinancialParticipation = await _context.ProjectFinancialParticipation.Where(p => p.ProjectId == project.Id).FirstOrDefaultAsync();

            ProjectViewModel viewModel = new ProjectViewModel()
            {
                Project = project,
                ProjectPartnerRequirement = projectPartnerRequirement,
                ProjectPartners = await _context.ProjectPartner.Where(p => p.ProjectId == project.Id).ToListAsync(),
                ProjectFunding = projectFunding,
                ProjectFinancialParticipation = projectFinancialParticipation,
                ProjectVisibilityAreas = await _context.ProjectVisibilityArea.Where(p => p.ProjectId == project.Id).Select(v => v.VisibilityAreaId).ToArrayAsync(),
                //ProjectOwner = _context.UserProfiles.Include(u => u.CompanyPosition).Include(u => u.CompanyType).Where(u => u.UserId == project.UserId).FirstOrDefault()
            };

            return viewModel;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> StartProject(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var project = await _context.Project.Where(p => p.Id == id && p.UserId == appUser.Id && !p.Deleted).FirstOrDefaultAsync();

            if (project == null)
                return BadRequest("Unauthorized action taken. No change could be made");

            project.IsStarted = true;
            project.StartDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> FinishProject(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var project = await _context.Project.Where(p => p.Id == id && p.UserId == appUser.Id && !p.Deleted).FirstOrDefaultAsync();

            if (project == null)
                return BadRequest("Unauthorized action taken. No change could be made");

            project.IsFinished = true;
            project.FinishDate = DateTime.UtcNow;

            //var projectCrew = await _context.ProjectCrews.Where(c => c.ProjectId == project.Id).ToListAsync();
            //foreach(ProjectCrew crew in projectCrew)
            //{
            //    crew.IsActive = false;
            //}

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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


        [HttpPost]
        public async Task<ActionResult<ProjectViewModel>> SaveOrUpdateProject([FromBody] ProjectViewModel projectVM)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            Project project = projectVM.Project;

            if (project.Id != 0)
            {
                var projectInDb = _context.Project.Where(p => p.Id == projectVM.Project.Id && !p.Deleted).Select(p => new { p.Id, p.UserId }).FirstOrDefault();
                if (projectInDb == null)
                    return NotFound("Project is not valid.");
                if (projectInDb.UserId != appUser.Id)
                    return BadRequest("Project is unauthorized for this user.");
            }


            ProjectPartnerRequirement projectPartnerReq = projectVM.ProjectPartnerRequirement;
            List<ProjectPartner> projectPartners = projectVM.ProjectPartners;
            ProjectFunding projectFunding = projectVM.ProjectFunding;
            ProjectFinancialParticipation projectFinancialParticipation = projectVM.ProjectFinancialParticipation;


            if (appUser == null || project == null || projectPartnerReq == null || projectPartners == null)
            {
                return BadRequest();
            }

            //Save Project in db
            if (!string.IsNullOrEmpty(projectVM.ProjectLogo))
            {
                project.Logo = Convert.FromBase64String(projectVM.ProjectLogo.Split(',')[1]); ;
            }

            if (!string.IsNullOrEmpty(projectVM.ProjectOriginalLogo))
            {
                project.BackgroundImage = Convert.FromBase64String(projectVM.ProjectOriginalLogo.Split(',')[1]); ;
            }


            project.UserId = appUser.Id;

            if (project.Id != 0) //existing record
            {
                project.ModifiedDateTime = DateTime.Now;
                if (project.CreatedDateTime == default(DateTime)) project.CreatedDateTime = DateTime.Now;
                _context.Entry(project).State = EntityState.Modified;
            }
            else //new record
            {
                project.CreatedDateTime = DateTime.Now;
                _context.Project.Add(project);

                try
                {
                    await _context.SaveChangesAsync();


                    var projectCrew = new ProjectCrew();
                    projectCrew.ProjectId = project.Id;
                    projectCrew.UserId = appUser.Id;
                    projectCrew.IsFromDiscovery = true;

                    _context.ProjectCrews.Add(projectCrew);
                    await _context.SaveChangesAsync();

                    var position = await _context.WorkingPositions.Where(w => w.Id == 2).FirstOrDefaultAsync();
                    if (position != null)
                    {
                        var crewPos = new ProjectCrewPosition();
                        crewPos.ProjectCrewId = projectCrew.Id;
                        crewPos.PositionId = position.Id;
                        _context.ProjectCrewPositions.Add(crewPos);
                    }

                    _context.ProjectUserGroups.Add(new ProjectUserGroup() { Name = "Production Team", ProjectId = project.Id});
                    await _context.SaveChangesAsync();

                    await _context.Database.ExecuteSqlRawAsync(
                        string.Format(@"
                                        insert into UserProjectMenu(ApplicationUserId,MenuActivityId,ProjectId)

                                        select k.Id, m.Id,k.projectId from (select p.Id as projectId, u.Id from project p
                                        join AspNetUsers u
                                        on p.UserId = u.Id ) k
                                        join MenuActivity m
                                        on 1=1

                                        where k.Id='{0}' and k.projectId={1}", appUser.Id, project.Id));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            //Save Project partner req in db
            projectPartnerReq.ProjectId = project.Id;
            if (projectPartnerReq.Id != 0) //existing record
                _context.Entry(projectPartnerReq).State = EntityState.Modified;
            else //new record
            {
                _context.ProjectPartnerRequirement.Add(projectPartnerReq);

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }


            //Save Project partners in db
            foreach (ProjectPartner projPartner in projectPartners)
            {
                projPartner.ProjectId = project.Id;
                if (projPartner.Id != 0) //existing record
                    _context.Entry(projPartner).State = EntityState.Modified;
                else //new record
                    _context.ProjectPartner.Add(projPartner);
            }



            //Save Project Funding
            if (!projectPartnerReq.NeedFinancialParticipation && projectFunding != null)
            {
                projectFunding.ProjectId = project.Id;
                if (projectFunding.Id != 0) //existing record
                    _context.Entry(projectFunding).State = EntityState.Modified;
                else //new record
                {
                    _context.ProjectFunding.Add(projectFunding);
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectExists(project.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }



            //Save Project Financial Participation
            if (projectPartnerReq.NeedFinancialParticipation && projectFinancialParticipation != null)
            {
                projectFinancialParticipation.ProjectId = project.Id;
                if (projectFinancialParticipation.Id != 0) // existing record
                    _context.Entry(projectFinancialParticipation).State = EntityState.Modified;
                else //new record
                {
                    _context.ProjectFinancialParticipation.Add(projectFinancialParticipation);
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectExists(project.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }


            //Save Project Visibility areas
            if (projectVM.ProjectVisibilityAreas != null)
            {
                var alldata = _context.ProjectVisibilityArea.Where(v => v.ProjectId == project.Id);
                _context.ProjectVisibilityArea.RemoveRange(alldata);

                foreach (string visibilityArea in projectVM.ProjectVisibilityAreas)
                {
                    ProjectVisibilityArea projectVisibilityArea = new ProjectVisibilityArea()
                    {
                        ProjectId = project.Id,
                        VisibilityAreaId = visibilityArea
                    };
                    _context.ProjectVisibilityArea.Add(projectVisibilityArea);
                }
            }

            //Save Project Partner req management phases
            //if (projectVM.ProjectPartnerRequirementManagementPhases != null)
            //{
            //    var alldata = _context.ProjectPartnerRequirementManagementPhase.Where(v => v.ProjectPartnerRequirementId == projectPartnerReq.Id);
            //    _context.ProjectPartnerRequirementManagementPhase.RemoveRange(alldata);

            //    foreach (string managePhase in projectVM.ProjectPartnerRequirementManagementPhases)
            //    {
            //        ProjectPartnerRequirementManagementPhase managementPhase = new ProjectPartnerRequirementManagementPhase()
            //        {
            //            ProjectPartnerRequirementId = projectPartnerReq.Id,
            //            ProjectManagementPhasesId = managePhase
            //        };
            //        _context.ProjectPartnerRequirementManagementPhase.Add(managementPhase);
            //    }
            //}

            //Save Project Funding management phases

            //if (projectVM.ProjectFundingManagementPhases != null && projectFunding != null && !projectPartnerReq.NeedFinancialParticipation)
            //{
            //    var alldata = _context.ProjectFundingManagementPhase.Where(v => v.ProjectFundingId == projectFunding.Id);
            //    _context.ProjectFundingManagementPhase.RemoveRange(alldata);

            //    foreach (string managePhase in projectVM.ProjectFundingManagementPhases)
            //    {
            //        ProjectFundingManagementPhase managementPhase = new ProjectFundingManagementPhase()
            //        {
            //            ProjectFundingId = projectFunding.Id,
            //            ProjectManagementPhasesId = managePhase
            //        };
            //        _context.ProjectFundingManagementPhase.Add(managementPhase);
            //    }
            //}

            //Save Project Financial participation management phases
            //if (projectVM.ProjectFinancialParticipationManagementPhases != null && projectFinancialParticipation != null && projectPartnerReq.NeedFinancialParticipation)
            //{
            //    var alldata = _context.ProjectFinancialParticipationManagementPhase.Where(v => v.ProjectFinancialParticipationId == projectFinancialParticipation.Id);
            //    _context.ProjectFinancialParticipationManagementPhase.RemoveRange(alldata);

            //    foreach (string managePhase in projectVM.ProjectFinancialParticipationManagementPhases)
            //    {
            //        ProjectFinancialParticipationManagementPhase managementPhase = new ProjectFinancialParticipationManagementPhase()
            //        {
            //            ProjectFinancialParticipationId = projectFinancialParticipation.Id,
            //            ProjectManagementPhasesId = managePhase
            //        };
            //        _context.ProjectFinancialParticipationManagementPhase.Add(managementPhase);
            //    }
            //}


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }


        // POST: api/Projects
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            _context.Project.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            project.Deleted = true;
            await _context.SaveChangesAsync();
            //if (project == null)
            //{
            //    return NotFound();
            //}

            //var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            //if (project.UserId != appUser.Id)
            //{
            //    return BadRequest("This User is not authorized to delete this project!");
            //}


            //var visibility = await _context.ProjectVisibilityArea.Where(r => r.ProjectId == project.Id).ToListAsync();
            //var partnerReq = await _context.ProjectPartnerRequirement.Where(r => r.ProjectId == project.Id).FirstOrDefaultAsync();
            //var partners = await _context.ProjectPartner.Where(r => r.ProjectId == project.Id).ToListAsync();
            //var funding = await _context.ProjectFunding.Where(r => r.ProjectId == project.Id).FirstOrDefaultAsync();
            //var financialParticipation = await _context.ProjectFinancialParticipation.Where(r => r.ProjectId == project.Id).FirstOrDefaultAsync();

            //var partReqPhase = await _context.ProjectPartnerRequirementManagementPhase
            //    .Where(r => r.ProjectPartnerRequirementId == partnerReq.Id).ToListAsync();
            ////List<ProjectFundingManagementPhase> fundingPhase = new List<ProjectFundingManagementPhase>();
            ////if(funding != null)
            ////    fundingPhase = await _context.ProjectFundingManagementPhase
            ////        .Where(r => r.ProjectFundingId == funding.Id).ToListAsync();

            ////List<ProjectFinancialParticipationManagementPhase> financialPhase = new List<ProjectFinancialParticipationManagementPhase>();
            ////if(financialParticipation != null)
            ////    financialPhase = await _context.ProjectFinancialParticipationManagementPhase
            ////        .Where(r => r.ProjectFinancialParticipationId == financialParticipation.Id).ToListAsync();

            ////if(financialPhase != null)
            ////{ //financial phase
            ////    foreach(ProjectFinancialParticipationManagementPhase phase in financialPhase)
            ////    {
            ////        _context.ProjectFinancialParticipationManagementPhase.Remove(phase);
            ////    }
            ////}

            ////if (fundingPhase != null)
            ////{
            ////    foreach(ProjectFundingManagementPhase phase in fundingPhase)
            ////    {
            ////        _context.ProjectFundingManagementPhase.Remove(phase);
            ////    }
            ////}

            ////if (partReqPhase != null)
            ////{
            ////    foreach (ProjectPartnerRequirementManagementPhase phase in partReqPhase)
            ////    {
            ////        _context.ProjectPartnerRequirementManagementPhase.Remove(phase);
            ////    }
            ////}

            ////if(financialParticipation != null)
            ////{
            ////    _context.ProjectFinancialParticipation.Remove(financialParticipation);
            ////}

            //if (funding != null)
            //{
            //    _context.ProjectFunding.Remove(funding);
            //}

            //if (partners != null)
            //{
            //    foreach (ProjectPartner partner in partners)
            //    {
            //        _context.ProjectPartner.Remove(partner);
            //    }
            //}

            //if (partnerReq != null)
            //{
            //    _context.ProjectPartnerRequirement.Remove(partnerReq);
            //}

            //if (visibility != null)
            //{
            //    foreach (ProjectVisibilityArea visi in visibility)
            //    {
            //        _context.ProjectVisibilityArea.Remove(visi);
            //    }
            //}
            //_context.Project.Remove(project);
            //await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.Id == id);
        }

        #region Project photo related request handlers

        [HttpGet("{id}/{width}/{height}")]
        public async Task<IActionResult> GetProjectCover(int id, int width = 348, int height = 218)
        {
            if (id != 0)
            {
                var file = await _context.Project
                    .Where(p => p.Id == id && !p.Deleted)
                    .Select(p => p.Logo)
                    .FirstOrDefaultAsync();

                if (file == null)
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string path = Path.Combine(webRootPath, "img", "no-image.png");
                    using (Image imgPhoto = Image.Load(path))
                    {
                        ResizeOptions options = new ResizeOptions()
                        {
                            Size = new Size() { Height = height, Width = width },
                            Mode = ResizeMode.BoxPad
                        };
                        imgPhoto.Mutate(x => x
                             .Resize(options)
                         );
                        MemoryStream ms = new MemoryStream();
                        await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                        return File(ms.ToArray(), "image/png");
                    }
                }

                using (Image imgPhoto = Image.Load(file))
                {
                    imgPhoto.Mutate(x => x
                         .Resize(width, height)
                     );
                    MemoryStream ms = new MemoryStream();
                    await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                    return File(ms.ToArray(), "image/png");
                }
                // }
            }
            return NotFound();
        }

        #endregion
    }
}
