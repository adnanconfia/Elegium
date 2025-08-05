using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Data;
using Elegium.Dtos;
using Elegium.Dtos.Voting;
using Elegium.Models;
using Elegium.Models.Voting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Elegium.Controllers.api.Voting
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VoteFundingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VoteFundingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> ApplyVoteFunding([FromBody] ProjectNominationDto voteFunding)
        {
            try
            {
                var project = _context.Project.Where(p => p.Id == voteFunding.ProjectId && !p.Deleted).FirstOrDefault();
                if (project == null)
                    return BadRequest("Project is not found.");

                var currentUser = await _userManager.GetUserAsync(User);

                if (project.UserId != currentUser.Id)
                    return BadRequest("User is not authorized for this operation.");

                var discoveryVisibilityId = _context.VisibilityAreas.Where(v => v.Code == "discovery").Select(v => v.Id).FirstOrDefault();
                var discoveryProjectId = _context.ProjectVisibilityArea
                    .Where(v => v.VisibilityAreaId == discoveryVisibilityId.ToString() && v.ProjectId == project.Id)
                    .Select(v => v.ProjectId).FirstOrDefault();

                if (discoveryProjectId == 0) return NotFound("Project is not allowed to be visible for public. Please make it visible from project settings.");

                if (!project.OnBoardingCompleted)
                    return BadRequest("Please complete the onbarding for this project to apply vote funding.");

                var currentNomination = _context.Nominations
                    .Where(n => n.StartDate <= DateTime.Now && n.EndDate >= DateTime.Now
                    && n.Id == voteFunding.Id).FirstOrDefault();

                if (currentNomination == null)
                    return BadRequest("We are not accepting nomination applications right now. Please try again later");

                
                var nominationDetailCount = _context.NominationDetails
                    .Where(nd => nd.NominationId == currentNomination.Id
                        && nd.ProjectId == project.Id).Count();

                if (nominationDetailCount > 0)
                {
                    return BadRequest("You have already applied for this project.");
                }

                var newDetail = new NominationDetail()
                {
                    ProjectId = project.Id,
                    NominationId = currentNomination.Id
                };

                _context.NominationDetails.Add(newDetail);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpPost]
        public async Task<IActionResult> GetAllNominations([FromBody] ProjectSearchQuery searchQuery)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var nominations = _context.Nominations
                    .Where(n => n.StartDate <= DateTime.Now && n.EndDate >= DateTime.Now
                    && !n.IsVotingStarted && !n.IsVotingFinished
                    && (searchQuery.ProductionTypeId == null || n.ProductionTypeId == searchQuery.ProductionTypeId)
                    && (searchQuery.CountryId == null || n.CountryId == searchQuery.CountryId)
                    ).Select(n => new NominationDto()
                    {
                        Id = n.Id,
                        Name = n.Name,
                        Description = n.Description,
                        ProductionType = n.ProductionType,
                        ProductionTypeId = n.ProductionTypeId,
                        Country = n.Country,
                        CountryId = n.CountryId,
                        TermsAndConditions = n.TermsAndConditions,
                        Currency = n.Currency,
                        CurrencyId = n.CurrencyId,
                        Price = n.Price,
                        IsVotingStarted = n.IsVotingStarted,
                        IsVotingFinished = n.IsVotingFinished,
                        IsResultApproved = n.IsResultApproved,
                        StartDate = n.StartDate,
                        EndDate = n.EndDate,
                        CreatedDate = n.CreatedDate,
                    })
                    .ToList();

                return Ok(nominations);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpPost]
        public async Task<IActionResult> GetAllNominationApplications([FromBody] ProjectSearchQuery searchQuery)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var nominationsList = _context.Nominations
                    .Where(n => n.StartDate <= DateTime.Now && n.EndDate >= DateTime.Now
                        && !n.IsVotingStarted && !n.IsVotingFinished)
                    .Select(n => n.Id)
                    .ToList();

                if (nominationsList.Count() == 0)
                    return BadRequest("There are no projects available for nomination right now.");


                var nominationDetail = _context.NominationDetails
                    .Where(nd => nominationsList.Contains(nd.NominationId)
                    && (searchQuery.ProductionTypeId == null || nd.Project.ProductionTypeId == searchQuery.ProductionTypeId)
                    && (searchQuery.LanguageId == null || nd.Project.ProductionLanguageId == searchQuery.LanguageId)
                    && (searchQuery.CountryId == null || _context.UserProfiles
                                                            .Where(p => p.UserId == nd.Project.UserId)
                                                            .FirstOrDefault().CountryId == searchQuery.CountryId)
                    )
                    .Select(nd => new ProjectNominationDetailDto()
                    {
                        Id = nd.Id,
                        AppliedDateTime = nd.CreatedDate,
                        NominationId = nd.NominationId,
                        NominationName = nd.Nomination.Name,
                        ProjectId = nd.ProjectId,
                        ProjectName = nd.Project.Name,
                        AverageScore = _context.NominationVotes
                                        .Where(v => v.NominationDetailId == nd.Id)
                                        .Select(v => v.TotalScore)
                                        .Average(),
                        UsersVotedCount = _context.NominationVotes
                                        .Where(v => v.NominationDetailId == nd.Id).Count(),
                        UserId = nd.Project.UserId,
                        Location = _context.UserProfiles.Where(p => p.UserId == nd.Project.UserId).FirstOrDefault().Country.Name,
                        UserName = nd.Project.User.FirstName + " " + nd.Project.User.LastName
                    })
                    .ToList();

                return Ok(nominationDetail);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllNominationApplications(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var currentNomination = _context.Nominations
                    .Where(n => n.Id == id)
                    .FirstOrDefault();

                if (currentNomination == null)
                    return BadRequest("Invalid nomination");


                var nominationDetail = _context.NominationDetails
                    .Where(nd => nd.NominationId == id)
                    .Select(nd => new ProjectNominationDetailDto()
                    {
                        Id = nd.Id,
                        AppliedDateTime = nd.CreatedDate,
                        NominationId = nd.NominationId,
                        NominationName = nd.Nomination.Name,
                        ProjectId = nd.ProjectId,
                        ProjectName = nd.Project.Name,
                        AverageScore = _context.NominationVotes
                                        .Where(v => v.NominationDetailId == nd.Id)
                                        .Select(v => v.TotalScore)
                                        .Average(),
                        UsersVotedCount = _context.NominationVotes
                                        .Where(v => v.NominationDetailId == nd.Id).Count(),
                        IsSelectedForFinalVoting = nd.IsSelectedForFinalVoting,
                        UserId = nd.Project.UserId,
                        Location = _context.UserProfiles.Where(p => p.UserId == nd.Project.UserId).FirstOrDefault().Country.Name,
                        UserName = nd.Project.User.FirstName + " " + nd.Project.User.LastName
                    }).OrderByDescending(d => d.AverageScore).ToList();

                return Ok(nominationDetail);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNominationDetail(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var nominationDetail = _context.NominationDetails
                    .Where(nd => nd.Id == id)
                    .Select(nd => new ProjectNominationDetailDto()
                    {
                        Id = nd.Id,
                        AppliedDateTime = nd.CreatedDate,
                        ProjectId = nd.ProjectId,
                        ProjectName = nd.Project.Name,
                        ProjectDetail = nd.Project.Detail,
                        EndDateTime = nd.Nomination.EndDate,
                        AlreadyVoted = _context.NominationVotes.Where(v => v.UserVotedId == currentUser.Id && v.NominationDetailId == nd.Id).Count() > 0,
                        VotingParameters = _context.VotingParameters
                                            .Where(v => v.IsActive)
                                            .Select(v => new VotingParameterDto()
                                            {
                                                Id = v.Id,
                                                Name = v.Name
                                            })
                                            .ToList(),
                        AverageScore = _context.NominationVotes
                                        .Where(v => v.NominationDetailId == nd.Id)
                                        .Select(v => v.TotalScore)
                                        .Average(),
                        UsersVotedCount = _context.NominationVotes
                                        .Where(v => v.NominationDetailId == nd.Id).Count(),
                        UserId = nd.Project.UserId,
                        Location = _context.UserProfiles.Where(p => p.UserId == nd.Project.UserId).FirstOrDefault().Country.Name,
                        UserName = nd.Project.User.FirstName + " " + nd.Project.User.LastName
                    })
                    .FirstOrDefault();

                return Ok(nominationDetail);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> NominateToFinalVoting(int id)
        {
            try
            {
                if (User.IsInRole(RoleName.Admin))
                {
                    var nomination = _context.NominationDetails
                        .Where(nd => nd.Id == id).FirstOrDefault();

                    if (nomination == null)
                        return BadRequest("Nomination is not found.");

                    var nominatedProjectsCount = _context.NominationDetails
                        .Where(nd => nd.NominationId == nomination.NominationId && nd.IsSelectedForFinalVoting)
                        .Count();

                    if (nominatedProjectsCount == 5)
                        return BadRequest("5 projects are already selected for final voting");

                    nomination.IsSelectedForFinalVoting = true;
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else { return BadRequest("User is not authorized for this action"); }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RemoveFromFinalVoting(int id)
        {
            try
            {
                if (User.IsInRole(RoleName.Admin))
                {
                    var nomination = _context.NominationDetails
                        .Where(nd => nd.Id == id).FirstOrDefault();

                    if (nomination == null)
                        return BadRequest("Nomination is not found.");

                    var votes = _context.FinalVotes
                                .Where(v => v.NominationDetailId == nomination.Id)
                                .Count();

                    if (votes > 0)
                        return BadRequest("Cannot remove this project as users are already voted for it.");

                    nomination.IsSelectedForFinalVoting = false;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else { return BadRequest("User is not authorized for this action"); }


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public IActionResult GetRecentVotes(int id)
        {
            try
            {
                var nomination = _context.NominationDetails
                    .Where(n => n.Id == id)
                    .FirstOrDefault();

                if (nomination == null)
                    return BadRequest("Nomination is not found.");

                var recentVotes = _context.NominationVotes
                    .Where(v => v.NominationDetailId == id)
                    .OrderByDescending(v => v.CreatedDateTime)
                    .Select(v => new NominationVoteDto()
                    {
                        TotalScore = v.TotalScore,
                        UserVotedId = v.UserVotedId,
                        UserVotedName = _context.UserProfiles
                            .Where(p => p.UserId == v.UserVotedId)
                            .FirstOrDefault().FirstName,
                        UserVotedLocation = _context.UserProfiles
                            .Where(p => p.UserId == v.UserVotedId)
                            .FirstOrDefault().Country.Name,
                        NominationVoteDetails = _context.NominationVoteDetails
                                                .Where(vd => vd.NominationVoteId == v.Id)
                                                .Select(vd => new NominationVoteDetailDto()
                                                {
                                                    VotingParameter = vd.VotingParameter,
                                                    Score = vd.Score
                                                }).ToList()
                    }).ToList();




                return Ok(recentVotes);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveNominationVoting([FromBody] ProjectNominationDetailDto nominationVoting)
        {
            try
            {
                var project = _context.Project.Where(p => p.Id == nominationVoting.ProjectId && !p.Deleted).FirstOrDefault();
                if (project == null)
                    return BadRequest("Project is not found.");

                var currentUser = await _userManager.GetUserAsync(User);

                var nominationVote = _context.NominationVotes
                    .Where(n => n.UserVotedId == currentUser.Id && n.NominationDetailId == nominationVoting.Id)
                    .Count();

                if (nominationVote > 0)
                    return BadRequest("This user is already voted for this project.");

                var userVote = new NominationVote()
                {
                    NominationDetailId = nominationVoting.Id,
                    TotalScore = nominationVoting.TotalScore,
                    UserVotedId = currentUser.Id
                };

                _context.NominationVotes.Add(userVote);
                _context.SaveChanges();

                foreach (var item in nominationVoting.VotingParameters)
                {
                    var userVoteDetail = new NominationVoteDetail()
                    {
                        NominationVoteId = userVote.Id,
                        VotingParameterId = item.Id,
                        Score = item.Score
                    };
                    _context.NominationVoteDetails.Add(userVoteDetail);
                }

                _context.SaveChanges();

                var averageScore = _context.NominationVotes
                                        .Where(v => v.NominationDetailId == nominationVoting.Id)
                                        .Select(v => v.TotalScore)
                                        .Average();

                return Ok(averageScore);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }








        #region Final voting apis here

        [HttpPost]
        public async Task<IActionResult> GetAllFinalVotingProjects([FromBody] ProjectSearchQuery searchQuery)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var nominationsList = _context.Nominations
                    .Where(n => n.IsVotingStarted && !n.IsVotingFinished)
                    .Select(n => n.Id)
                    .ToList();

                if (nominationsList.Count() == 0)
                    return BadRequest("There are no projects available for voting right now.");


                var nominationDetail = _context.NominationDetails
                    .Where(nd => nominationsList.Contains(nd.NominationId) && nd.IsSelectedForFinalVoting
                    && (searchQuery.ProductionTypeId == null || nd.Project.ProductionTypeId == searchQuery.ProductionTypeId)
                    && (searchQuery.LanguageId == null || nd.Project.ProductionLanguageId == searchQuery.LanguageId)
                    && (searchQuery.CountryId == null || _context.UserProfiles
                                                            .Where(p => p.UserId == nd.Project.UserId)
                                                            .FirstOrDefault().CountryId == searchQuery.CountryId)
                    )
                    .Select(nd => new ProjectNominationDetailDto()
                    {
                        Id = nd.Id,
                        AppliedDateTime = nd.CreatedDate,
                        NominationId = nd.NominationId,
                        NominationName = nd.Nomination.Name,
                        ProjectId = nd.ProjectId,
                        ProjectName = nd.Project.Name,
                        AverageScore = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id)
                                        .Select(v => v.TotalScore)
                                        .Average(),
                        UsersVotedCount = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id).Count(),
                        UserId = nd.Project.UserId,
                        Location = _context.UserProfiles.Where(p => p.UserId == nd.Project.UserId).FirstOrDefault().Country.Name,
                        UserName = nd.Project.User.FirstName + " " + nd.Project.User.LastName
                    })
                    .ToList();

                if (nominationDetail.Count() == 0)
                    return BadRequest("There are no projects available for voting right now.");

                return Ok(nominationDetail);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllFinalVotingProjects(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var currentNomination = _context.Nominations
                    .Where(n => n.Id == id)
                    .FirstOrDefault();

                if (currentNomination == null)
                    return BadRequest("Invalid nomination");


                var nominationDetail = _context.NominationDetails
                    .Where(nd => nd.NominationId == id && nd.IsSelectedForFinalVoting)
                    .Select(nd => new ProjectNominationDetailDto()
                    {
                        Id = nd.Id,
                        AppliedDateTime = nd.CreatedDate,
                        NominationId = nd.NominationId,
                        NominationName = nd.Nomination.Name,
                        ProjectId = nd.ProjectId,
                        ProjectName = nd.Project.Name,
                        IsWinner = nd.IsWinner,
                        IsSystemSuggested = nd.IsSystemSuggested,
                        AverageScore = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id)
                                        .Select(v => v.TotalScore)
                                        .Average(),
                        UsersVotedCount = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id).Count(),
                        UserId = nd.Project.UserId,
                        Location = _context.UserProfiles.Where(p => p.UserId == nd.Project.UserId).FirstOrDefault().Country.Name,
                        UserName = nd.Project.User.FirstName + " " + nd.Project.User.LastName
                    })
                    .OrderByDescending(d => d.IsWinner ? 1 : 0)
                    .ThenByDescending(d => d.AverageScore)
                    .ToList();

                if (nominationDetail.Count() == 0)
                    return BadRequest("There are no projects available for voting right now.");

                return Ok(nominationDetail);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFinalNominationDetail(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var nominationDetail = _context.NominationDetails
                    .Where(nd => nd.Id == id)
                    .Select(nd => new ProjectNominationDetailDto()
                    {
                        Id = nd.Id,
                        AppliedDateTime = nd.CreatedDate,
                        ProjectId = nd.ProjectId,
                        ProjectName = nd.Project.Name,
                        ProjectDetail = nd.Project.Detail,
                        EndDateTime = nd.Nomination.EndDate,
                        AlreadyVoted = _context.FinalVotes
                                        .Where(v => v.UserVotedId == currentUser.Id && v.NominationDetailId == nd.Id)
                                        .Count() > 0,
                        VotingParameters = _context.VotingParameters
                                            .Where(v => v.IsActive)
                                            .Select(v => new VotingParameterDto()
                                            {
                                                Id = v.Id,
                                                Name = v.Name
                                            })
                                            .ToList(),
                        AverageScore = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id)
                                        .Select(v => v.TotalScore)
                                        .Average(),
                        UsersVotedCount = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id).Count(),
                        UserId = nd.Project.UserId,
                        Location = _context.UserProfiles.Where(p => p.UserId == nd.Project.UserId).FirstOrDefault().Country.Name,
                        UserName = nd.Project.User.FirstName + " " + nd.Project.User.LastName
                    })
                    .FirstOrDefault();

                return Ok(nominationDetail);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> MakeWinner(int id)
        {
            try
            {
                if (User.IsInRole(RoleName.Admin))
                {
                    var nomination = _context.NominationDetails
                        .Where(nd => nd.Id == id).FirstOrDefault();

                    if (nomination == null)
                        return BadRequest("Nomination is not found.");

                    var nominationMaster = _context.Nominations
                        .Where(n => n.Id == nomination.NominationId)
                        .FirstOrDefault();

                    if (nominationMaster.IsResultApproved)
                        return BadRequest("Results are already approved for this nomination.");

                    var previousWinner = _context.NominationDetails
                        .Where(nd => nd.IsWinner).FirstOrDefault();

                    previousWinner.IsWinner = false;
                    previousWinner.IsSystemSuggested = false;

                    nomination.IsWinner = true;
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else { return BadRequest("User is not authorized for this action"); }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public IActionResult GetRecentFinalVotes(int id)
        {
            try
            {
                var nomination = _context.NominationDetails
                    .Where(n => n.Id == id)
                    .FirstOrDefault();

                if (nomination == null)
                    return BadRequest("Nomination is not found.");

                var recentVotes = _context.FinalVotes
                    .Where(v => v.NominationDetailId == id)
                    .OrderByDescending(v => v.CreatedDateTime)
                    .Select(v => new FinalVoteDto()
                    {
                        TotalScore = v.TotalScore,
                        UserVotedId = v.UserVotedId,
                        UserVotedName = _context.UserProfiles
                            .Where(p => p.UserId == v.UserVotedId)
                            .FirstOrDefault().FirstName,
                        UserVotedLocation = _context.UserProfiles
                            .Where(p => p.UserId == v.UserVotedId)
                            .FirstOrDefault().Country.Name,
                        FinalVoteDetails = _context.FinalVoteDetails
                                                .Where(vd => vd.FinalVoteId == v.Id)
                                                .Select(vd => new FinalVoteDetailDto()
                                                {
                                                    VotingParameter = vd.VotingParameter,
                                                    Score = vd.Score
                                                }).ToList()
                    }).ToList();




                return Ok(recentVotes);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveNominationFinalVoting([FromBody] ProjectNominationDetailDto nominationVoting)
        {
            try
            {
                var project = _context.Project
                                .Where(p => p.Id == nominationVoting.ProjectId)
                                .FirstOrDefault();
                if (project == null)
                    return BadRequest("Project is not found.");

                var currentUser = await _userManager.GetUserAsync(User);

                var finalVote = _context.FinalVotes
                    .Where(n => n.UserVotedId == currentUser.Id && n.NominationDetailId == nominationVoting.Id)
                    .Count();

                if (finalVote > 0)
                    return BadRequest("This user is already voted for this project.");

                var userVote = new FinalVote()
                {
                    NominationDetailId = nominationVoting.Id,
                    TotalScore = nominationVoting.TotalScore,
                    UserVotedId = currentUser.Id
                };

                _context.FinalVotes.Add(userVote);
                _context.SaveChanges();

                foreach (var item in nominationVoting.VotingParameters)
                {
                    var userVoteDetail = new FinalVoteDetail()
                    {
                        FinalVoteId = userVote.Id,
                        VotingParameterId = item.Id,
                        Score = item.Score
                    };
                    _context.FinalVoteDetails.Add(userVoteDetail);
                }

                _context.SaveChanges();

                var averageScore = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nominationVoting.Id)
                                        .Select(v => v.TotalScore)
                                        .Average();

                var userDetails = new
                {
                    UserVotedId = currentUser.Id,
                    UserVotedName = _context.UserProfiles
                            .Where(p => p.UserId == currentUser.Id)
                            .FirstOrDefault().FirstName,
                    UserVotedLocation = _context.UserProfiles
                            .Where(p => p.UserId == currentUser.Id)
                            .FirstOrDefault().Country?.Name
                };

                return Ok(new { averageScore, userDetails });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        #endregion


        #region Voting result apis here

        [HttpGet]
        public async Task<IActionResult> GetAllProjectsResult()
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var nominationsList = _context.Nominations
                    .Where(n => n.IsVotingStarted && n.IsVotingFinished && n.IsResultApproved)
                    .Select(n => n.Id)
                    .ToList();

                if (nominationsList.Count() == 0)
                    return BadRequest("There are no results right now.");


                var nominationDetail1 = _context.NominationDetails
                    .Where(nd => nominationsList.Contains(nd.NominationId)
                                && nd.IsSelectedForFinalVoting)
                    .Select(nd => new ProjectNominationDetailDto()
                    {
                        Id = nd.Id,
                        AppliedDateTime = nd.CreatedDate,
                        NominationId = nd.NominationId,
                        NominationName = nd.Nomination.Name,
                        ProjectId = nd.ProjectId,
                        ProjectName = nd.Project.Name,
                        IsWinner = nd.IsWinner,
                        //FinalVoteDetails =  (from v in _context.FinalVotes
                        //                    join d in _context.FinalVoteDetails
                        //                    on v.Id equals d.FinalVoteId
                        //                    where v.NominationDetailId == nd.Id

                        //                    //group new { d.Score}
                        //                    //by new { v.NominationDetailId, d.VotingParameterId, d.VotingParameter} into grp
                        //                    select new
                        //                    {
                        //                        VotingParameterId = d.VotingParameterId,
                        //                        //VotingParameter = d.VotingParameter,
                        //                        NominationDetailId = v.NominationDetailId,
                        //                        Score = d.Score
                        //                    })
                        //                    .AsEnumerable()
                        //                    .ToList()
                        //                    .GroupBy(a => new { a.VotingParameterId, a.NominationDetailId })
                        //                    .Select(a => new FinalVoteDetailDto()
                        //                    {
                        //                        Score = a.Average(a => a.Score),
                        //                        VotingParameterId = a.Key.VotingParameterId
                        //                    }).ToList(),
                        //_context.FinalVoteDetails
                        //.Where(v => _context.FinalVotes
                        //                    .Where(f => f.NominationDetailId == nd.Id)
                        //                    .Select(f => f.Id)
                        //                    .ToList().Contains(v.FinalVoteId))
                        //.GroupBy(v => v.VotingParameter)
                        //.Select(v => new FinalVoteDetailDto()
                        //{
                        //    VotingParameter = v.Key,
                        //    Score = v.Average(a => a.Score)
                        //}).ToList(),


                        AverageScore = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id)
                                        .Select(v => v.TotalScore)
                                        .Average(),
                        UsersVotedCount = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id).Count(),
                        UserId = nd.Project.UserId,
                        Location = _context.UserProfiles.Where(p => p.UserId == nd.Project.UserId).FirstOrDefault().Country.Name,
                        UserName = nd.Project.User.FirstName + " " + nd.Project.User.LastName
                    })
                    .OrderByDescending(nd => nd.IsWinner ? 1 : 0)
                    .ThenByDescending(nd => nd.AverageScore)
                    .ToList();

                var nominationDetail = (from t in nominationDetail1

                                        select new ProjectNominationDetailDto()
                                        {

                                            Id = t.Id,
                                            AppliedDateTime = t.AppliedDateTime,
                                            NominationId = t.NominationId,
                                            NominationName = t.NominationName,
                                            ProjectId = t.ProjectId,
                                            ProjectName = t.ProjectName,
                                            IsWinner = t.IsWinner,
                                            AverageScore = t.AverageScore,
                                            UsersVotedCount = t.UsersVotedCount,
                                            UserId = t.UserId,
                                            Location = t.Location,
                                            UserName = t.UserName,
                                            FinalVoteDetails = (from v in _context.FinalVotes
                                                                join d in _context.FinalVoteDetails
                                                                on v.Id equals d.FinalVoteId
                                                                where v.NominationDetailId == t.Id

                                                                //group new { d.Score}
                                                                //by new { v.NominationDetailId, d.VotingParameterId, d.VotingParameter} into grp
                                                                select new
                                                                {
                                                                    VotingParameterId = d.VotingParameterId,
                                                                    //VotingParameter = d.VotingParameter,
                                                                    NominationDetailId = v.NominationDetailId,
                                                                    Score = d.Score
                                                                })
                                                                .AsEnumerable()
                                                                .ToList()
                                                                .GroupBy(a => new { a.VotingParameterId, a.NominationDetailId })
                                                                .Select(a => new FinalVoteDetailDto()
                                                                {
                                                                    Score = a.Average(a => a.Score),
                                                                    VotingParameterId = a.Key.VotingParameterId
                                                                }).ToList()
                                        }).ToList();

                if (nominationDetail.Count() == 0)
                    return BadRequest("There are no results right now.");

                return Ok(nominationDetail);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectResultDetail(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var nominationDetail1 = _context.NominationDetails
                    .Where(nd => nd.Id == id)
                    .Select(nd => new ProjectNominationDetailDto()
                    {
                        Id = nd.Id,
                        AppliedDateTime = nd.CreatedDate,
                        ProjectId = nd.ProjectId,
                        ProjectName = nd.Project.Name,
                        ProjectDetail = nd.Project.Detail,
                        IsWinner = nd.IsWinner,
                        EndDateTime = nd.Nomination.EndDate,
                        AverageScore = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id)
                                        .Select(v => v.TotalScore)
                                        .Average(),
                        UsersVotedCount = _context.FinalVotes
                                        .Where(v => v.NominationDetailId == nd.Id).Count(),
                        UserId = nd.Project.UserId,
                        Location = _context.UserProfiles.Where(p => p.UserId == nd.Project.UserId).FirstOrDefault().Country.Name,
                        UserName = nd.Project.User.FirstName + " " + nd.Project.User.LastName
                    })
                    .ToList();

                var nominationDetail = (from t in nominationDetail1

                                        select new ProjectNominationDetailDto()
                                        {

                                            Id = t.Id,
                                            AppliedDateTime = t.AppliedDateTime,
                                            NominationId = t.NominationId,
                                            NominationName = t.NominationName,
                                            ProjectId = t.ProjectId,
                                            ProjectName = t.ProjectName,
                                            IsWinner = t.IsWinner,
                                            AverageScore = t.AverageScore,
                                            UsersVotedCount = t.UsersVotedCount,
                                            UserId = t.UserId,
                                            Location = t.Location,
                                            UserName = t.UserName,
                                            FinalVoteDetails = (from v in _context.FinalVotes
                                                                join d in _context.FinalVoteDetails
                                                                on v.Id equals d.FinalVoteId
                                                                where v.NominationDetailId == t.Id

                                                                //group new { d.Score}
                                                                //by new { v.NominationDetailId, d.VotingParameterId, d.VotingParameter} into grp
                                                                select new
                                                                {
                                                                    d.VotingParameterId,
                                                                    d.VotingParameter,
                                                                    v.NominationDetailId,
                                                                    d.Score
                                                                })
                                                                .AsEnumerable()
                                                                .ToList()
                                                                .GroupBy(a => new { a.VotingParameterId, a.NominationDetailId, a.VotingParameter })
                                                                .Select(a => new FinalVoteDetailDto()
                                                                {
                                                                    Score = a.Average(a => a.Score),
                                                                    VotingParameter = a.Key.VotingParameter,
                                                                    VotingParameterId = a.Key.VotingParameterId
                                                                }).ToList()
                                        }).FirstOrDefault();

                return Ok(nominationDetail);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        #endregion
    }
}
