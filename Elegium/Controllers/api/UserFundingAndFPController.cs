using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.FundingAndFP;
using Elegium.ViewModels.FundingAndFP;
using SixLabors.ImageSharp.ColorSpaces;
using Elegium.Dtos;
using Elegium.Dtos.FundingAndFPDtos;
using System.Security.Claims;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserFundingAndFPController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserFundingAndFPController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserFundingAndFP
        [HttpPost]
        public async Task<ActionResult<IEnumerable<UserFundingAndFPDto>>> GetFundingAndFP(FundingAndFPSearchQuery searchQuery)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _context.UserFundingAndFP
                .Include(u => u.ProductionType)
                .Include(u => u.City)
                .Include(u => u.Currency)
                .Include(u => u.ProjectPhase)
                .Where(u =>
                    (string.IsNullOrEmpty(searchQuery.Type) || u.Type == searchQuery.Type) &&
                    (searchQuery.ProductionTypeId == null || u.ProductionTypeId == searchQuery.ProductionTypeId) &&
                    (searchQuery.CountryId == null || u.CountryId == searchQuery.CountryId) &&
                    (searchQuery.CurrencyId == null || u.CurrencyId == searchQuery.CurrencyId) &&
                    (string.IsNullOrEmpty(searchQuery.ProjectPhaseId) || u.ProjectPhaseId.ToString() == searchQuery.ProjectPhaseId) &&
                    (searchQuery.BudgetUpto == null || u.BudgetUpto <= searchQuery.BudgetUpto))
                .Select(u => new UserFundingAndFPDto()
                {
                    Id = u.Id,
                    Type = u.Type,
                    BudgetUpto = u.BudgetUpto,
                    ProductionType = u.ProductionType,
                    Currency = u.Currency,
                    City = u.City,
                    Country = u.Country,
                    Offer = u.Offer,
                    OfferShare = u.OfferShare,
                    ProjectPhase = u.ProjectPhase,
                    SupportDetail = u.SupportDetail,
                    OtherRequirements = u.OtherRequirements,
                    CreatedDateTime = u.CreatedDateTime,
                    ModifiedDateTime = u.ModifiedDateTime,
                    IsSaved = _context.SavedFundingAndFPs.Where(sp => sp.UserId == appUserId && sp.UserFundingAndFPId == u.Id).Count() != 0,
                    IsFavorite = _context.FavoriteFundingAndFPs.Where(fp => fp.UserId == appUserId && fp.UserFundingAndFPId == u.Id).Count() != 0,
                    User = _context.UserProfiles
                    .Include(up => up.CompanyPosition)
                    .Include(up => up.CompanyType)
                    .Include(up => up.CompanyCountry)
                    .Include(up => up.CompanyCity)
                    .Where(up => up.UserId == u.UserId)
                    .Select(up => new UserProfileDto()
                    {
                        Id = up.Id,
                        FirstName = up.FirstName,
                        MiddleName = up.MiddleName,
                        LastName = up.LastName,
                        Country = up.Country,
                        CityName = up.CityName,
                        IntroText = up.IntroText,
                        CompanyName = up.CompanyName,
                        CompanyType = up.CompanyType,
                        CompanyPosition = up.CompanyPosition,
                        CompanyCountry = up.CompanyCountry,
                        CompanyCity = up.CompanyCity,
                        CompanyWeb = up.CompanyWeb,
                        UserId = up.UserId,
                        FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == up.UserId).Count(),
                        FollowingCount = _context.UserFollowing.Where(f => f.UserId == up.UserId).Count()

                    })
                    .FirstOrDefault()
                })
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFundingAndFPDto>>> GetSavedFundingAndFP()
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fundingIds = _context.SavedFundingAndFPs.Where(sp => sp.UserId == appUserId).Select(sp => sp.UserFundingAndFPId).ToList();

            return await _context.UserFundingAndFP
                .Include(u => u.ProductionType)
                .Include(u => u.City)
                .Include(u => u.Currency)
                .Include(u => u.ProjectPhase)
                .Where(u => fundingIds.Contains(u.Id))
                .Select(u => new UserFundingAndFPDto()
                {
                    Id = u.Id,
                    Type = u.Type,
                    BudgetUpto = u.BudgetUpto,
                    ProductionType = u.ProductionType,
                    Currency = u.Currency,
                    City = u.City,
                    Offer = u.Offer,
                    OfferShare = u.OfferShare,
                    ProjectPhase = u.ProjectPhase,
                    SupportDetail = u.SupportDetail,
                    OtherRequirements = u.OtherRequirements,
                    CreatedDateTime = u.CreatedDateTime,
                    ModifiedDateTime = u.ModifiedDateTime,
                    IsSaved = _context.SavedFundingAndFPs.Where(sp => sp.UserId == appUserId && sp.UserFundingAndFPId == u.Id).Count() != 0,
                    IsFavorite = _context.FavoriteFundingAndFPs.Where(fp => fp.UserId == appUserId && fp.UserFundingAndFPId == u.Id).Count() != 0,
                    User = _context.UserProfiles
                    .Include(up => up.CompanyPosition)
                    .Include(up => up.CompanyType)
                    .Include(up => up.CompanyCountry)
                    .Include(up => up.CompanyCity)
                    .Where(up => up.UserId == u.UserId)
                    .Select(up => new UserProfileDto()
                    {
                        Id = up.Id,
                        FirstName = up.FirstName,
                        MiddleName = up.MiddleName,
                        LastName = up.LastName,
                        Country = up.Country,
                        City = up.City,
                        IntroText = up.IntroText,
                        CompanyName = up.CompanyName,
                        CompanyType = up.CompanyType,
                        CompanyPosition = up.CompanyPosition,
                        CompanyCountry = up.CompanyCountry,
                        CompanyCity = up.CompanyCity,
                        CompanyWeb = up.CompanyWeb,
                        UserId = up.UserId,
                        FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == up.UserId).Count(),
                        FollowingCount = _context.UserFollowing.Where(f => f.UserId == up.UserId).Count()

                    })
                    .FirstOrDefault()
                })
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFundingAndFPDto>>> GetFavoriteFundingAndFP()
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fundingIds = _context.FavoriteFundingAndFPs.Where(sp => sp.UserId == appUserId).Select(sp => sp.UserFundingAndFPId).ToList();

            return await _context.UserFundingAndFP
                .Include(u => u.ProductionType)
                .Include(u => u.City)
                .Include(u => u.Currency)
                .Include(u => u.ProjectPhase)
                .Where(u => fundingIds.Contains(u.Id))
                .Select(u => new UserFundingAndFPDto()
                {
                    Id = u.Id,
                    Type = u.Type,
                    BudgetUpto = u.BudgetUpto,
                    ProductionType = u.ProductionType,
                    Currency = u.Currency,
                    City = u.City,
                    Offer = u.Offer,
                    OfferShare = u.OfferShare,
                    ProjectPhase = u.ProjectPhase,
                    SupportDetail = u.SupportDetail,
                    OtherRequirements = u.OtherRequirements,
                    CreatedDateTime = u.CreatedDateTime,
                    ModifiedDateTime = u.ModifiedDateTime,
                    IsSaved = _context.SavedFundingAndFPs.Where(sp => sp.UserId == appUserId && sp.UserFundingAndFPId == u.Id).Count() != 0,
                    IsFavorite = _context.FavoriteFundingAndFPs.Where(fp => fp.UserId == appUserId && fp.UserFundingAndFPId == u.Id).Count() != 0,
                    User = _context.UserProfiles
                    .Include(up => up.CompanyPosition)
                    .Include(up => up.CompanyType)
                    .Include(up => up.CompanyCountry)
                    .Include(up => up.CompanyCity)
                    .Where(up => up.UserId == u.UserId)
                    .Select(up => new UserProfileDto()
                    {
                        Id = up.Id,
                        FirstName = up.FirstName,
                        MiddleName = up.MiddleName,
                        LastName = up.LastName,
                        Country = up.Country,
                        City = up.City,
                        IntroText = up.IntroText,
                        CompanyName = up.CompanyName,
                        CompanyType = up.CompanyType,
                        CompanyPosition = up.CompanyPosition,
                        CompanyCountry = up.CompanyCountry,
                        CompanyCity = up.CompanyCity,
                        CompanyWeb = up.CompanyWeb,
                        UserId = up.UserId,
                        FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == up.UserId).Count(),
                        FollowingCount = _context.UserFollowing.Where(f => f.UserId == up.UserId).Count()

                    })
                    .FirstOrDefault()
                })
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<UserFundingAndFPDto>> GetCurrentUserFundingAndFP()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var userFundingAndFp = await _context.UserFundingAndFP
                .Where(u => u.UserId == appUser.Id)
                .Select(u => new UserFundingAndFPDto()
                {
                    Type = u.Type,
                    ProductionTypeId = u.ProductionTypeId,
                    BudgetUpto = u.BudgetUpto,
                    CurrencyId = u.CurrencyId,
                    CountryId = u.CountryId,
                    Offer = u.Offer,
                    OfferShare = u.OfferShare,
                    ProjectPhaseId = u.ProjectPhaseId,
                    SupportDetail = u.SupportDetail,
                    OtherRequirements = u.OtherRequirements,
                })
                .FirstOrDefaultAsync();
            if (userFundingAndFp == null) return NotFound();

            return userFundingAndFp;
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdateUserFundingAndFP([FromBody] UserFundingAndFP userFundingAndFpVM)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var userFundingAndFp = await _context.UserFundingAndFP
                .Where(u => u.UserId == appUser.Id)
                .FirstOrDefaultAsync();

            if (userFundingAndFp == null)
            {
                userFundingAndFp = new UserFundingAndFP()
                {
                    Type = userFundingAndFpVM.Type,
                    BudgetUpto = userFundingAndFpVM.BudgetUpto,
                    CurrencyId = userFundingAndFpVM.CurrencyId,
                    ProductionTypeId = userFundingAndFpVM.ProductionTypeId,
                    CountryId = userFundingAndFpVM.CountryId,
                    Offer = userFundingAndFpVM.Type == "F" ? userFundingAndFpVM.Offer : 0,
                    OfferShare = userFundingAndFpVM.Type == "FP" ? userFundingAndFpVM.OfferShare : "",
                    ProjectPhaseId = userFundingAndFpVM.ProjectPhaseId,
                    SupportDetail = userFundingAndFpVM.SupportDetail,
                    OtherRequirements = userFundingAndFpVM.OtherRequirements,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    UserId = appUser.Id
                };
                _context.UserFundingAndFP.Add(userFundingAndFp);
            }
            else
            {
                userFundingAndFp.Type = userFundingAndFpVM.Type;
                userFundingAndFp.ProductionTypeId = userFundingAndFpVM.ProductionTypeId;
                userFundingAndFp.BudgetUpto = userFundingAndFpVM.BudgetUpto;
                userFundingAndFp.CurrencyId = userFundingAndFpVM.CurrencyId;
                userFundingAndFp.CountryId = userFundingAndFpVM.CountryId;
                userFundingAndFp.Offer = userFundingAndFpVM.Type == "F" ? userFundingAndFpVM.Offer : 0;
                userFundingAndFp.OfferShare = userFundingAndFpVM.Type == "FP" ? userFundingAndFpVM.OfferShare : "";
                userFundingAndFp.ProjectPhaseId = userFundingAndFpVM.ProjectPhaseId;
                userFundingAndFp.SupportDetail = userFundingAndFpVM.SupportDetail;
                userFundingAndFp.OtherRequirements = userFundingAndFpVM.OtherRequirements;
                userFundingAndFp.ModifiedDateTime = DateTime.Now;
            }

            await _context.SaveChangesAsync();


            return Ok("Successfully saved!");
        }

        // GET: api/UserFundingAndFP/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserFundingAndFP>> GetUserFundingAndFP(int id)
        {
            var userFundingAndFP = await _context.UserFundingAndFP.FindAsync(id);

            if (userFundingAndFP == null)
            {
                return NotFound();
            }

            return userFundingAndFP;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleSavedFundingAndFP(int id)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var message = "";
            var savedFundingAndFP = await _context.SavedFundingAndFPs.Where(r => r.UserId == appUserId && r.UserFundingAndFPId == id).FirstOrDefaultAsync();
            if (savedFundingAndFP == null)
            {
                savedFundingAndFP = new SavedFundingAndFP()
                {
                    UserId = appUserId,
                    UserFundingAndFPId = id
                };
                _context.SavedFundingAndFPs.Add(savedFundingAndFP);
                message = "saved";
            }
            else
            {
                _context.SavedFundingAndFPs.Remove(savedFundingAndFP);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleFavoriteFundingAndFP(int id)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var message = "";
            var favoriteFundingAndFP = await _context.FavoriteFundingAndFPs.Where(r => r.UserId == appUserId && r.UserFundingAndFPId == id).FirstOrDefaultAsync();
            if (favoriteFundingAndFP == null)
            {
                favoriteFundingAndFP = new FavoriteFundingAndFP()
                {
                    UserId = appUserId,
                    UserFundingAndFPId = id
                };
                _context.FavoriteFundingAndFPs.Add(favoriteFundingAndFP);
                message = "saved";
            }
            else
            {
                _context.FavoriteFundingAndFPs.Remove(favoriteFundingAndFP);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }



        // PUT: api/UserFundingAndFP/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserFundingAndFP(int id, UserFundingAndFP userFundingAndFP)
        {
            if (id != userFundingAndFP.Id)
            {
                return BadRequest();
            }

            _context.Entry(userFundingAndFP).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFundingAndFPExists(id))
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

        // POST: api/UserFundingAndFP
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserFundingAndFP>> PostUserFundingAndFP(UserFundingAndFP userFundingAndFP)
        {
            _context.UserFundingAndFP.Add(userFundingAndFP);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserFundingAndFP", new { id = userFundingAndFP.Id }, userFundingAndFP);
        }

        // DELETE: api/UserFundingAndFP/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserFundingAndFP>> DeleteUserFundingAndFP(int id)
        {
            var userFundingAndFP = await _context.UserFundingAndFP.FindAsync(id);
            if (userFundingAndFP == null)
            {
                return NotFound();
            }

            _context.UserFundingAndFP.Remove(userFundingAndFP);
            await _context.SaveChangesAsync();

            return userFundingAndFP;
        }

        private bool UserFundingAndFPExists(int id)
        {
            return _context.UserFundingAndFP.Any(e => e.Id == id);
        }
    }
}
