using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Permissions;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Dtos;
using Elegium.Dtos.ProjectDtos;
using Elegium.Models.Projects;
using Elegium.ViewModels.ProjectViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.ColorSpaces;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublicProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PublicProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModel>>> GetPublicProjects()
        {
            List<ProjectViewModel> viewModel = new List<ProjectViewModel>();
            var discoveryVisibilityId = await _context.VisibilityAreas.Where(v => v.Code == "discovery").Select(v => v.Id).FirstOrDefaultAsync();
            var discoveryProjectIds = await _context.ProjectVisibilityArea.Where(v => v.VisibilityAreaId == discoveryVisibilityId.ToString()).Select(v => v.ProjectId).ToListAsync();

            var projects = await (from p in _context.Project
                                  where p.isVisible && !p.Deleted
                                  && discoveryProjectIds.Contains(p.Id)
                                  join ppr in _context.ProjectPartnerRequirement
                            on p.Id equals ppr.ProjectId into projectPartnerReqList
                                  from ppr in projectPartnerReqList.DefaultIfEmpty()
                                  select p
                        )
                        .Include(p => p.ProductionType)
                        .Include(p => p.Currency)
                        .Include(p => p.ProductionLanguage)
                        .ToListAsync();


            foreach (Project project in projects)
            {
                var projectPartnerRequirement = await _context.ProjectPartnerRequirement
                    .Where(p => p.ProjectId == project.Id)
                    .Include(p => p.ProjectManagementPhase)
                    .FirstOrDefaultAsync();
                var projectFunding = await _context.ProjectFunding
                    .Where(p => p.ProjectId == project.Id)
                    .Include(p => p.ProjectManagementPhase)
                    .FirstOrDefaultAsync();
                var projectFinancialParticipation = await _context.ProjectFinancialParticipation
                    .Where(p => p.ProjectId == project.Id)
                    .Include(p => p.ProjectManagementPhase)
                    .FirstOrDefaultAsync();

                ProjectViewModel vm = new ProjectViewModel()
                {
                    Project = project,
                    ProjectPartnerRequirement = projectPartnerRequirement,
                    ProjectPartners = await _context.ProjectPartner.Where(p => p.ProjectId == project.Id).ToListAsync(),
                    ProjectFunding = projectFunding,
                    ProjectFinancialParticipation = projectFinancialParticipation,
                    ProjectVisibilityAreas = await _context.ProjectVisibilityArea.Where(p => p.ProjectId == project.Id).Select(v => v.VisibilityAreaId).ToArrayAsync(),
                    //ProjectPartnerRequirementManagementPhases = await _context.ProjectPartnerRequirementManagementPhase.Where(p => p.ProjectPartnerRequirementId == projectPartnerRequirement.Id).Select(v => v.ProjectManagementPhasesId).ToA,rrayAsync(),
                    //ProjectFundingManagementPhases = projectFunding != null ? await _context.ProjectFundingManagementPhase.Where(p => p.ProjectFundingId == projectFunding.Id).Select(v => v.ProjectManagementPhasesId).ToArrayAsync() : null,
                    //ProjectFinancialParticipationManagementPhases = projectFinancialParticipation != null ? await _context.ProjectFinancialParticipationManagementPhase.Where(p => p.ProjectFinancialParticipationId == projectFinancialParticipation.Id).Select(v => v.ProjectManagementPhasesId).ToArrayAsync() : null
                    ProjectOwner = _context.UserProfiles.Include(u => u.CompanyPosition).Include(u => u.CompanyType).Where(u => u.UserId == project.UserId).FirstOrDefault()
                };
                viewModel.Add(vm);
            }

            return viewModel;
        }

        [HttpPost]
        public async Task<ActionResult<List<ProjectViewModelDto>>> SearchPublicProjects([FromBody] ProjectSearchQuery searchQuery)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            List<ProjectViewModelDto> viewModel = new List<ProjectViewModelDto>();
            var discoveryVisibilityId = await _context.VisibilityAreas.Where(v => v.Code == "discovery").Select(v => v.Id).FirstOrDefaultAsync();
            var discoveryProjectIds = await _context.ProjectVisibilityArea.Where(v => v.VisibilityAreaId == discoveryVisibilityId.ToString()).Select(v => v.ProjectId).ToListAsync();

            var projects = await (from p in _context.Project
                                  where p.isVisible && !p.Deleted
                                  && discoveryProjectIds.Contains(p.Id)
                                  && (searchQuery.ProductionTypeId == null || p.ProductionTypeId == searchQuery.ProductionTypeId)
                                  && (searchQuery.LanguageId == null || p.ProductionLanguageId == searchQuery.LanguageId)
                                  join ppr in _context.ProjectPartnerRequirement
                            on p.Id equals ppr.ProjectId into projectPartnerReqList
                                  from ppr in projectPartnerReqList.DefaultIfEmpty()
                                  where (string.IsNullOrEmpty(searchQuery.BudgetMin) || ppr.Budget >= int.Parse(searchQuery.BudgetMin))
                                  && (string.IsNullOrEmpty(searchQuery.BudgetMax) || ppr.Budget <= int.Parse(searchQuery.BudgetMax))
                                  join po in _context.UserProfiles on p.UserId equals po.UserId into userProfileList
                                  from po in userProfileList.DefaultIfEmpty()
                                  where (searchQuery.CountryId == null || po.CompanyCountryId == searchQuery.CountryId)
                                  && (searchQuery.CityId == null || po.CompanyCityId == searchQuery.CityId)

                                  select new ProjectDto()
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Detail = p.Detail,
                                      CreatedDateTime = p.CreatedDateTime,
                                      ProductionType = p.ProductionType,
                                      ProductionLength = p.ProductionLength,
                                      ProductionRecordingMethod = p.ProductionRecordingMethod,
                                      ProductionAspectRatio = p.ProductionAspectRatio,
                                      ProductionMode = p.ProductionMode,
                                      ProductionColor = p.ProductionColor,
                                      ProductionLanguage = p.ProductionLanguage,
                                      Currency = p.Currency,
                                      Color = p.Color,
                                      ContactName = p.ContactName,
                                      ContactEmail = p.ContactEmail,
                                      ContactPhone = p.ContactPhone,
                                      ContactFax = p.ContactFax,
                                      UserId = p.UserId,
                                      IsSaved = _context.SavedProjects.Where(sp => sp.UserId == appUser.Id && sp.ProjectId == p.Id).Count() != 0,
                                      IsFavorite = _context.FavoriteProjects.Where(fp => fp.UserId == appUser.Id && fp.ProjectId == p.Id).Count() != 0,
                                      IsSavedPartner = _context.SavedProjectPartners.Where(sp => sp.UserId == appUser.Id && sp.ProjectId == p.Id).Count() != 0,
                                      IsFavoritePartner = _context.FavoriteProjectPartners.Where(fp => fp.UserId == appUser.Id && fp.ProjectId == p.Id).Count() != 0,
                                      IsItMe = appUser.Id == p.UserId || _context.ProjectPartnerRequests.Count(a =>
                                      a.ProjectId == p.Id
                                      && (a.Status == "P" || a.Status == "A")
                                      && a.SenderId == appUser.Id
                                      ) > 0 ? "Y" : "N"
                                  }
                        )
                        //.Include(p => p.Currency)
                        //.Include(p => p.ProductionLanguage)
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
                    ProjectOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == project.UserId)
                    .Select(u => new UserProfileDto()
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        MiddleName = u.MiddleName,
                        LastName = u.LastName,
                        Country = u.Country,
                        City = u.City,
                        IntroText = u.IntroText,
                        CompanyName = u.CompanyName,
                        CompanyType = u.CompanyType,
                        CompanyPosition = u.CompanyPosition,
                        CompanyCountry = u.CompanyCountry,
                        CompanyCity = u.CompanyCity,
                        UserId = u.UserId,
                        FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == u.UserId).Count(),
                        FollowingCount = _context.UserFollowing.Where(f => f.UserId == u.UserId).Count()
                    })
                    .FirstOrDefault()
                };
                viewModel.Add(vm);
            }

            return viewModel;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModelDto>>> GetSavedProjects()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            List<ProjectViewModelDto> viewModel = new List<ProjectViewModelDto>();
            var discoveryVisibilityId = await _context.VisibilityAreas.Where(v => v.Code == "discovery").Select(v => v.Id).FirstOrDefaultAsync();
            var discoveryProjectIds = await _context.ProjectVisibilityArea.Where(v => v.VisibilityAreaId == discoveryVisibilityId.ToString()).Select(v => v.ProjectId).ToListAsync();

            var projIds = _context.SavedProjects.Where(sp => sp.UserId == appUser.Id).Select(sp => sp.ProjectId).ToList();

            var projects = await (from p in _context.Project
                                  where p.isVisible && !p.Deleted
                                  && discoveryProjectIds.Contains(p.Id)
                                  && projIds.Contains(p.Id)

                                  select new ProjectDto()
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Detail = p.Detail,
                                      CreatedDateTime = p.CreatedDateTime,
                                      ProductionType = p.ProductionType,
                                      ProductionLength = p.ProductionLength,
                                      ProductionRecordingMethod = p.ProductionRecordingMethod,
                                      ProductionAspectRatio = p.ProductionAspectRatio,
                                      ProductionMode = p.ProductionMode,
                                      ProductionColor = p.ProductionColor,
                                      ProductionLanguage = p.ProductionLanguage,
                                      Currency = p.Currency,
                                      Color = p.Color,
                                      ContactName = p.ContactName,
                                      ContactEmail = p.ContactEmail,
                                      ContactPhone = p.ContactPhone,
                                      ContactFax = p.ContactFax,
                                      UserId = p.UserId,
                                      IsSaved = _context.SavedProjects.Where(sp => sp.UserId == appUser.Id && sp.ProjectId == p.Id).Count() != 0,
                                      IsFavorite = _context.FavoriteProjects.Where(fp => fp.UserId == appUser.Id && fp.ProjectId == p.Id).Count() != 0,
                                  }
                        )
                        //.Include(p => p.Currency)
                        //.Include(p => p.ProductionLanguage)
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
                    ProjectOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == project.UserId)
                    .Select(u => new UserProfileDto()
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        MiddleName = u.MiddleName,
                        LastName = u.LastName,
                        Country = u.Country,
                        City = u.City,
                        IntroText = u.IntroText,
                        CompanyName = u.CompanyName,
                        CompanyType = u.CompanyType,
                        CompanyPosition = u.CompanyPosition,
                        CompanyCountry = u.CompanyCountry,
                        CompanyCity = u.CompanyCity,
                        UserId = u.UserId,
                        FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == u.UserId).Count(),
                        FollowingCount = _context.UserFollowing.Where(f => f.UserId == u.UserId).Count()
                    })
                    .FirstOrDefault()
                };
                viewModel.Add(vm);
            }

            return viewModel;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModelDto>>> GetFavoriteProjects()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            List<ProjectViewModelDto> viewModel = new List<ProjectViewModelDto>();
            var discoveryVisibilityId = await _context.VisibilityAreas.Where(v => v.Code == "discovery").Select(v => v.Id).FirstOrDefaultAsync();
            var discoveryProjectIds = await _context.ProjectVisibilityArea.Where(v => v.VisibilityAreaId == discoveryVisibilityId.ToString()).Select(v => v.ProjectId).ToListAsync();

            var projIds = _context.FavoriteProjects.Where(fp => fp.UserId == appUser.Id).Select(fp => fp.ProjectId).ToList();

            var projects = await (from p in _context.Project
                                  where p.isVisible && !p.Deleted
                                  && discoveryProjectIds.Contains(p.Id)
                                  && projIds.Contains(p.Id)

                                  select new ProjectDto()
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Detail = p.Detail,
                                      CreatedDateTime = p.CreatedDateTime,
                                      ProductionType = p.ProductionType,
                                      ProductionLength = p.ProductionLength,
                                      ProductionRecordingMethod = p.ProductionRecordingMethod,
                                      ProductionAspectRatio = p.ProductionAspectRatio,
                                      ProductionMode = p.ProductionMode,
                                      ProductionColor = p.ProductionColor,
                                      ProductionLanguage = p.ProductionLanguage,
                                      Currency = p.Currency,
                                      Color = p.Color,
                                      ContactName = p.ContactName,
                                      ContactEmail = p.ContactEmail,
                                      ContactPhone = p.ContactPhone,
                                      ContactFax = p.ContactFax,
                                      UserId = p.UserId,
                                      IsSaved = _context.SavedProjects.Where(sp => sp.UserId == appUser.Id && sp.ProjectId == p.Id).Count() != 0,
                                      IsFavorite = _context.FavoriteProjects.Where(fp => fp.UserId == appUser.Id && fp.ProjectId == p.Id).Count() != 0,
                                  }
                        )
                        //.Include(p => p.Currency)
                        //.Include(p => p.ProductionLanguage)
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
                    ProjectOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == project.UserId)
                    .Select(u => new UserProfileDto()
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        MiddleName = u.MiddleName,
                        LastName = u.LastName,
                        Country = u.Country,
                        City = u.City,
                        IntroText = u.IntroText,
                        CompanyName = u.CompanyName,
                        CompanyType = u.CompanyType,
                        CompanyPosition = u.CompanyPosition,
                        CompanyCountry = u.CompanyCountry,
                        CompanyCity = u.CompanyCity,
                        UserId = u.UserId,
                        FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == u.UserId).Count(),
                        FollowingCount = _context.UserFollowing.Where(f => f.UserId == u.UserId).Count()
                    })
                    .FirstOrDefault()
                };
                viewModel.Add(vm);
            }

            return viewModel;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModelDto>>> GetSavedProjectPartners()
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<ProjectViewModelDto> viewModel = new List<ProjectViewModelDto>();
            var discoveryVisibilityId = await _context.VisibilityAreas.Where(v => v.Code == "discovery").Select(v => v.Id).FirstOrDefaultAsync();
            var discoveryProjectIds = await _context.ProjectVisibilityArea.Where(v => v.VisibilityAreaId == discoveryVisibilityId.ToString()).Select(v => v.ProjectId).ToListAsync();

            var projIds = _context.SavedProjectPartners.Where(sp => sp.UserId == appUserId).Select(sp => sp.ProjectId).ToList();

            var projects = await (from p in _context.Project
                                  where p.isVisible && !p.Deleted
                                  && discoveryProjectIds.Contains(p.Id)
                                  && projIds.Contains(p.Id)

                                  select new ProjectDto()
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Detail = p.Detail,
                                      CreatedDateTime = p.CreatedDateTime,
                                      ProductionType = p.ProductionType,
                                      ProductionLength = p.ProductionLength,
                                      ProductionRecordingMethod = p.ProductionRecordingMethod,
                                      ProductionAspectRatio = p.ProductionAspectRatio,
                                      ProductionMode = p.ProductionMode,
                                      ProductionColor = p.ProductionColor,
                                      ProductionLanguage = p.ProductionLanguage,
                                      Currency = p.Currency,
                                      Color = p.Color,
                                      ContactName = p.ContactName,
                                      ContactEmail = p.ContactEmail,
                                      ContactPhone = p.ContactPhone,
                                      ContactFax = p.ContactFax,
                                      UserId = p.UserId,
                                      IsSavedPartner = _context.SavedProjectPartners.Where(sp => sp.UserId == appUserId && sp.ProjectId == p.Id).Count() != 0,
                                      IsFavoritePartner = _context.FavoriteProjectPartners.Where(fp => fp.UserId == appUserId && fp.ProjectId == p.Id).Count() != 0,
                                  }
                        )
                        //.Include(p => p.Currency)
                        //.Include(p => p.ProductionLanguage)
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
                    ProjectOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == project.UserId)
                    .Select(u => new UserProfileDto()
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        MiddleName = u.MiddleName,
                        LastName = u.LastName,
                        Country = u.Country,
                        City = u.City,
                        IntroText = u.IntroText,
                        CompanyName = u.CompanyName,
                        CompanyType = u.CompanyType,
                        CompanyPosition = u.CompanyPosition,
                        CompanyCountry = u.CompanyCountry,
                        CompanyCity = u.CompanyCity,
                        UserId = u.UserId,
                        FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == u.UserId).Count(),
                        FollowingCount = _context.UserFollowing.Where(f => f.UserId == u.UserId).Count()
                    })
                    .FirstOrDefault()
                };
                viewModel.Add(vm);
            }

            return viewModel;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModelDto>>> GetFavoriteProjectPartners()
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<ProjectViewModelDto> viewModel = new List<ProjectViewModelDto>();
            var discoveryVisibilityId = await _context.VisibilityAreas.Where(v => v.Code == "discovery").Select(v => v.Id).FirstOrDefaultAsync();
            var discoveryProjectIds = await _context.ProjectVisibilityArea.Where(v => v.VisibilityAreaId == discoveryVisibilityId.ToString()).Select(v => v.ProjectId).ToListAsync();

            var projIds = _context.FavoriteProjectPartners.Where(sp => sp.UserId == appUserId).Select(sp => sp.ProjectId).ToList();

            var projects = await (from p in _context.Project
                                  where p.isVisible && !p.Deleted
                                  && discoveryProjectIds.Contains(p.Id)
                                  && projIds.Contains(p.Id)

                                  select new ProjectDto()
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Detail = p.Detail,
                                      CreatedDateTime = p.CreatedDateTime,
                                      ProductionType = p.ProductionType,
                                      ProductionLength = p.ProductionLength,
                                      ProductionRecordingMethod = p.ProductionRecordingMethod,
                                      ProductionAspectRatio = p.ProductionAspectRatio,
                                      ProductionMode = p.ProductionMode,
                                      ProductionColor = p.ProductionColor,
                                      ProductionLanguage = p.ProductionLanguage,
                                      Currency = p.Currency,
                                      Color = p.Color,
                                      ContactName = p.ContactName,
                                      ContactEmail = p.ContactEmail,
                                      ContactPhone = p.ContactPhone,
                                      ContactFax = p.ContactFax,
                                      UserId = p.UserId,
                                      IsSavedPartner = _context.SavedProjectPartners.Where(sp => sp.UserId == appUserId && sp.ProjectId == p.Id).Count() != 0,
                                      IsFavoritePartner = _context.FavoriteProjectPartners.Where(fp => fp.UserId == appUserId && fp.ProjectId == p.Id).Count() != 0,
                                  }
                        )
                        //.Include(p => p.Currency)
                        //.Include(p => p.ProductionLanguage)
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
                    ProjectOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == project.UserId)
                    .Select(u => new UserProfileDto()
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        MiddleName = u.MiddleName,
                        LastName = u.LastName,
                        Country = u.Country,
                        City = u.City,
                        IntroText = u.IntroText,
                        CompanyName = u.CompanyName,
                        CompanyType = u.CompanyType,
                        CompanyPosition = u.CompanyPosition,
                        CompanyCountry = u.CompanyCountry,
                        CompanyCity = u.CompanyCity,
                        UserId = u.UserId,
                        FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == u.UserId).Count(),
                        FollowingCount = _context.UserFollowing.Where(f => f.UserId == u.UserId).Count()
                    })
                    .FirstOrDefault()
                };
                viewModel.Add(vm);
            }

            return viewModel;
        }

        [HttpPost]
        public async Task<ActionResult<List<ProjectViewModelDto>>> GetPublicProjectsForFundingAndFP()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var userPreferences = await _context.UserFundingAndFP.Where(f => f.UserId == appUser.Id).FirstOrDefaultAsync();

            if (userPreferences == null) return NotFound("Please set preferences to show the projects");


            List<ProjectViewModelDto> viewModel = new List<ProjectViewModelDto>();
            var discoveryVisibilityId = await _context.VisibilityAreas.Where(v => v.Code == "discovery").Select(v => v.Id).FirstOrDefaultAsync();
            var discoveryProjectIds = await _context.ProjectVisibilityArea.Where(v => v.VisibilityAreaId == discoveryVisibilityId.ToString()).Select(v => v.ProjectId).ToListAsync();


            var projects = await (from p in _context.Project
                                  where p.isVisible && !p.Deleted
                                  && discoveryProjectIds.Contains(p.Id)
                                  && (userPreferences.ProductionTypeId == null || p.ProductionTypeId == userPreferences.ProductionTypeId)
                                  && (userPreferences.CurrencyId == null || p.CurrencyId == userPreferences.CurrencyId)
                                  //&& (userPreferences.CurrencyId == null || p. == userPreferences.CurrencyId)
                                  join ppr in _context.ProjectPartnerRequirement
                            on p.Id equals ppr.ProjectId into projectPartnerReqList
                                  from ppr in projectPartnerReqList.DefaultIfEmpty()
                                  where (ppr.Budget <= userPreferences.BudgetUpto)
                                  && (userPreferences.ProjectPhaseId == null || ppr.ProjectManagementPhaseId == userPreferences.ProjectPhaseId.ToString())
                                  && ((userPreferences.Type == "F" && !ppr.NeedFinancialParticipation) || (userPreferences.Type == "FP" && ppr.NeedFinancialParticipation))
                                  join po in _context.UserProfiles on p.UserId equals po.UserId into userProfileList
                                  from po in userProfileList.DefaultIfEmpty()
                                  where (userPreferences.CityId == null || po.CompanyCityId == userPreferences.CityId)
                                  //&& (searchQuery.CityId == null || po.CompanyCityId == searchQuery.CityId)

                                  select new ProjectDto()
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Detail = p.Detail,
                                      CreatedDateTime = p.CreatedDateTime,
                                      ProductionType = p.ProductionType,
                                      ProductionLength = p.ProductionLength,
                                      ProductionRecordingMethod = p.ProductionRecordingMethod,
                                      ProductionAspectRatio = p.ProductionAspectRatio,
                                      ProductionMode = p.ProductionMode,
                                      ProductionColor = p.ProductionColor,
                                      ProductionLanguage = p.ProductionLanguage,
                                      Currency = p.Currency,
                                      Color = p.Color,
                                      ContactName = p.ContactName,
                                      ContactEmail = p.ContactEmail,
                                      ContactPhone = p.ContactPhone,
                                      ContactFax = p.ContactFax,
                                      UserId = p.UserId,
                                      IsItMe = p.UserId == appUser.Id || _context.FundingFPRequests.Where(a =>
                                      a.ProjectId == p.Id &&
                                      (a.Status == "P" || a.Status == "A") &&
                                      a.SenderId == appUser.Id &&
                                      a.OfferOrLooking == "O").Count() > 0 ? "Y" : "N"
                                  }
                        )
                        //.Include(p => p.Currency)
                        //.Include(p => p.ProductionLanguage)
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
                    ProjectOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == project.UserId)
                    .Select(u => new UserProfileDto()
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        MiddleName = u.MiddleName,
                        LastName = u.LastName,
                        Country = u.Country,
                        City = u.City,
                        IntroText = u.IntroText,
                        CompanyName = u.CompanyName,
                        CompanyType = u.CompanyType,
                        CompanyPosition = u.CompanyPosition,
                        CompanyCountry = u.CompanyCountry,
                        CompanyCity = u.CompanyCity,
                        UserId = u.UserId
                    })
                    .FirstOrDefault()
                };
                viewModel.Add(vm);
            }

            return viewModel;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectViewModelDto>> GetPublicProject(int id)
        {

            if (id == 0) return NotFound("Project not found");

            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var discoveryVisibilityId = await _context.VisibilityAreas.Where(v => v.Code == "discovery").Select(v => v.Id).FirstOrDefaultAsync();
            var discoveryProjectId = await _context.ProjectVisibilityArea
                .Where(v => v.VisibilityAreaId == discoveryVisibilityId.ToString() && v.ProjectId == id)
                .Select(v => v.ProjectId).FirstOrDefaultAsync();

            if (discoveryProjectId == 0) return NotFound("Project not found");

            var project = await _context.Project.Where(p => p.isVisible == true && p.Id == discoveryProjectId && !p.Deleted) //only get if project is public for discovery module
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
                    ProductionRecordingMethod = p.ProductionRecordingMethod,
                    ProductionAspectRatio = p.ProductionAspectRatio,
                    ProductionMode = p.ProductionMode,
                    ProductionColor = p.ProductionColor,
                    ProductionLanguage = p.ProductionLanguage,
                    Currency = p.Currency,
                    Color = p.Color,
                    ContactName = p.ContactName,
                    ContactEmail = p.ContactEmail,
                    ContactPhone = p.ContactPhone,
                    ContactFax = p.ContactFax,
                    UserId = p.UserId,
                    IsHired = _context.ProjectCrews.Where(c => c.ProjectId == p.Id && c.UserId == appUser.Id).FirstOrDefault() == null ? false : true
                })
                .FirstOrDefaultAsync();

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
            var projectOwner = await _context.UserProfiles
                .Include(u => u.CompanyPosition)
                .Include(u => u.CompanyType)
                .Include(u => u.CompanyStudioCountry)
                .Include(u => u.CompanyStudioCity)
                .Where(u => u.UserId == project.UserId)
                .FirstOrDefaultAsync();

            ProjectViewModelDto viewModel = new ProjectViewModelDto()
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
                ProjectOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == project.UserId)
                    .Select(u => new UserProfileDto()
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        MiddleName = u.MiddleName,
                        LastName = u.LastName,
                        Country = u.Country,
                        City = u.City,
                        IntroText = u.IntroText,
                        CompanyName = u.CompanyName,
                        CompanyType = u.CompanyType,
                        CompanyPosition = u.CompanyPosition,
                        CompanyCountry = u.CompanyCountry,
                        CompanyCity = u.CompanyCity,
                        UserId = u.UserId
                    })
                    .FirstOrDefault()
            };

            return viewModel;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleSavedProject(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var message = "";
            var savedProject = await _context.SavedProjects.Where(r => r.UserId == appUser.Id && r.ProjectId == id).FirstOrDefaultAsync();
            if (savedProject == null)
            {
                savedProject = new SavedProject()
                {
                    UserId = appUser.Id,
                    ProjectId = id
                };
                _context.SavedProjects.Add(savedProject);
                message = "saved";
            }
            else
            {
                _context.SavedProjects.Remove(savedProject);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleFavoriteProject(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var message = "";
            var favoriteProject = await _context.FavoriteProjects.Where(r => r.UserId == appUser.Id && r.ProjectId == id).FirstOrDefaultAsync();
            if (favoriteProject == null)
            {
                favoriteProject = new FavoriteProject()
                {
                    UserId = appUser.Id,
                    ProjectId = id
                };
                _context.FavoriteProjects.Add(favoriteProject);
                message = "saved";
            }
            else
            {
                _context.FavoriteProjects.Remove(favoriteProject);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleSavedProjectPartner(int id)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var message = "";
            var savedProject = await _context.SavedProjectPartners.Where(r => r.UserId == appUserId && r.ProjectId == id).FirstOrDefaultAsync();
            if (savedProject == null)
            {
                savedProject = new SavedProjectPartner()
                {
                    UserId = appUserId,
                    ProjectId = id
                };
                _context.SavedProjectPartners.Add(savedProject);
                message = "saved";
            }
            else
            {
                _context.SavedProjectPartners.Remove(savedProject);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleFavoriteProjectPartner(int id)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var message = "";
            var favoriteProject = await _context.FavoriteProjectPartners.Where(r => r.UserId == appUserId && r.ProjectId == id).FirstOrDefaultAsync();
            if (favoriteProject == null)
            {
                favoriteProject = new FavoriteProjectPartner()
                {
                    UserId = appUserId,
                    ProjectId = id
                };
                _context.FavoriteProjectPartners.Add(favoriteProject);
                message = "saved";
            }
            else
            {
                _context.FavoriteProjectPartners.Remove(favoriteProject);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult> LeaveProject([FromBody] ProjectDisputeDto disputeDto)
        {
            var appUser = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (disputeDto == null)
                return BadRequest();

            //var project = _context.Project.Where(p => p.Id == disputeDto.ProjectId)
            var request = await _context.ProjectCrews.Where(c => c.ProjectId == disputeDto.ProjectId && c.UserId == appUser.Id).FirstOrDefaultAsync();
            if (request == null)
                return BadRequest("User is not authorized for this action");

            var dispute = await _context.ProjectDisputes.Where(d => d.ProjectId == request.ProjectId && d.UserId == appUser.Id).FirstOrDefaultAsync();
            if (dispute == null)
            {
                dispute = new ProjectDispute()
                {
                    ProjectId = disputeDto.ProjectId,
                    UserId = appUser.Id,
                    Status = 1
                };
                _context.ProjectDisputes.Add(dispute);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("You have already a dispute on this project.");
            }

            _context.ProjectDisputeDetails.Add(new ProjectDisputeDetail()
            {
                ProjectDisputeId = dispute.Id,
                Description = disputeDto.Description,
                UserId = appUser.Id
            });

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetFollowers(int id)
        {
            var projectOwnerId = await _context.Project.Where(p => p.Id == id && !p.Deleted).Select(p => p.UserId).FirstOrDefaultAsync();
            var followers = await _context.UserFollowing.Where(u => u.FollowingToId == projectOwnerId).OrderByDescending(u => u.CreateDateTime).Select(u => u.UserId).Take(5).ToListAsync();
            return Ok(followers);
        }
    }
}
