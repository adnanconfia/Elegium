using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.Dtos;
using Elegium.ViewModels;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Identity;
using Elegium.Models.Professionals;
using System.Security.Claims;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfessionalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfessionalsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<List<ProfessionalSearchViewModel>>> GetProfessionals([FromBody] ProfessionalSearchQuery searchQuery)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userIds = await (from u in _context.UserProfiles
                           join a in _context.Users
                           on u.UserId equals a.Id
                           where !a.Trash &&
                           (searchQuery.PromotionCategory == null || searchQuery.PromotionCategory == 1 || _context.UserPromotionCategory.Where(p => p.UserId == u.UserId && p.PromotionCategoryId == searchQuery.PromotionCategory).Select(p => p.PromotionCategoryId).FirstOrDefault() == searchQuery.PromotionCategory) &&

                (searchQuery.CountryId == null || u.CountryId == searchQuery.CountryId) &&
                (searchQuery.CompanyPositionId == null || u.CompanyPositionId == searchQuery.CompanyPositionId) &&
                (searchQuery.SkillId == null || u.SkillId == searchQuery.SkillId) &&
                (searchQuery.SkillLevelId == null || u.SkillLevelId == searchQuery.SkillLevelId) &&
                (string.IsNullOrEmpty(searchQuery.Name) || u.FirstName.ToLower().Contains(searchQuery.Name.ToLower()) || u.LastName.ToLower().Contains(searchQuery.Name.ToLower())) &&
                (string.IsNullOrEmpty(searchQuery.Keyword) || u.FirstName.ToLower().Contains(searchQuery.Keyword.ToLower()) || u.LastName.ToLower().Contains(searchQuery.Keyword.ToLower())) &&
                (searchQuery.CityId == null || u.CityId == searchQuery.CityId)

                           select new UserProfileDto()
                           {
                              UserId = u.UserId
                           } ).ToListAsync();

            //var userIds = uIds.Union(_context.UserPromotionCategory.Where(
            //    u =>
            //    (searchQuery.PromotionCategory == null || searchQuery.PromotionCategory == 1 || u.PromotionCategoryId == searchQuery.PromotionCategory)
            //).Select(u => u.UserId).ToList()).ToList();

            List<ProfessionalSearchViewModel> viewModel = new List<ProfessionalSearchViewModel>();

            foreach (var userid in userIds)
            {
                ProfessionalSearchViewModel vm = new ProfessionalSearchViewModel()
                {
                    UserProfile = _context.UserProfiles.Where(u => u.UserId == userid.UserId)
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.Country)
                    .Include(u => u.City)
                    .Select(u => new UserProfileDto()
                    {
                        UserId = u.UserId,
                        City = u.City,
                        Country = u.Country,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        IntroText = u.IntroText,
                        CompanyPosition = u.CompanyPosition
                    })
                    .FirstOrDefault(),
                    UserCredits = _context.UserCredit.Where(u => u.UserId == userid.UserId).ToList(),
                    UserEquipments = _context.UserEquipment.Where(u => u.UserId == userid.UserId).ToList(),
                    UserPromotionCategory = _context.UserPromotionCategory.Where(u => u.UserId == userid.UserId).Include(u => u.PromotionCategory).ToList(),
                    UserOtherLanguages = _context.UserOtherLanguages.Where(u => u.UserId == userid.UserId).Include(u => u.Language).ToList(),
                    Online = _context.Connection.Where(a => a.UserId == userid.UserId).OrderByDescending(a => a.ConnectionTime).FirstOrDefault()?.Connected,
                    FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == userid.UserId).Count(),
                    FollowingCount = _context.UserFollowing.Where(f => f.UserId == userid.UserId).Count(),
                    IsSaved = _context.SavedProfessionals.Where(sr => sr.UserId == appUserId && sr.ProfessionalId == userid.UserId).Count() == 0 ? false : true,
                    IsFavorite = _context.FavoriteProfessionals.Where(fr => fr.UserId == appUserId && fr.ProfessionalId == userid.UserId).Count() == 0 ? false : true,
                };

                viewModel.Add(vm);
            }


            return Ok(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfessionalSearchViewModel>>> GetSavedProfessionals()
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profIds = await (from sr in _context.SavedProfessionals
                                 join u in _context.Users
                                 on sr.ProfessionalId equals u.Id
                                 where !u.Trash && sr.UserId == appUserId
                                 select new UserProfileDto()
                                 {
                                     UserId = u.Id
                                 }).ToListAsync();

            List<ProfessionalSearchViewModel> viewModel = new List<ProfessionalSearchViewModel>();

            foreach (var userid in profIds.Select(a => a.UserId))
            {
                ProfessionalSearchViewModel vm = new ProfessionalSearchViewModel()
                {
                    UserProfile = _context.UserProfiles.Where(u => u.UserId == userid)
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.Country)
                    .Include(u => u.City)
                    .Select(u => new UserProfileDto()
                    {
                        UserId = u.UserId,
                        City = u.City,
                        Country = u.Country,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        IntroText = u.IntroText,
                        CompanyPosition = u.CompanyPosition
                    })
                    .FirstOrDefault(),
                    UserCredits = _context.UserCredit.Where(u => u.UserId == userid).ToList(),
                    UserEquipments = _context.UserEquipment.Where(u => u.UserId == userid).ToList(),
                    UserPromotionCategory = _context.UserPromotionCategory.Where(u => u.UserId == userid).Include(u => u.PromotionCategory).ToList(),
                    UserOtherLanguages = _context.UserOtherLanguages.Where(u => u.UserId == userid).Include(u => u.Language).ToList(),
                    Online = _context.Connection.Where(a => a.UserId == userid).OrderByDescending(a => a.ConnectionTime).FirstOrDefault()?.Connected,
                    FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == userid).Count(),
                    FollowingCount = _context.UserFollowing.Where(f => f.UserId == userid).Count(),
                    IsSaved = _context.SavedProfessionals.Where(sr => sr.UserId == appUserId && sr.ProfessionalId == userid).Count() == 0 ? false : true,
                    IsFavorite = _context.FavoriteProfessionals.Where(fr => fr.UserId == appUserId && fr.ProfessionalId == userid).Count() == 0 ? false : true,
                };

                viewModel.Add(vm);
            }


            return Ok(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfessionalSearchViewModel>>> GetFavoriteProfessionals()
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profIds = await
                (from sr in _context.FavoriteProfessionals
                 join u in _context.Users
                 on sr.ProfessionalId equals u.Id
                 where !u.Trash && sr.UserId == appUserId
                 select new UserProfileDto()
                 {
                     UserId = sr.ProfessionalId
                 }).ToListAsync();

            List<ProfessionalSearchViewModel> viewModel = new List<ProfessionalSearchViewModel>();

            foreach (var userid in profIds.Select(a => a.UserId))
            {
                ProfessionalSearchViewModel vm = new ProfessionalSearchViewModel()
                {
                    UserProfile = _context.UserProfiles.Where(u => u.UserId == userid)
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.Country)
                    .Include(u => u.City)
                    .Select(u => new UserProfileDto()
                    {
                        UserId = u.UserId,
                        City = u.City,
                        Country = u.Country,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        IntroText = u.IntroText,
                        CompanyPosition = u.CompanyPosition
                    })
                    .FirstOrDefault(),
                    UserCredits = _context.UserCredit.Where(u => u.UserId == userid).ToList(),
                    UserEquipments = _context.UserEquipment.Where(u => u.UserId == userid).ToList(),
                    UserPromotionCategory = _context.UserPromotionCategory.Where(u => u.UserId == userid).Include(u => u.PromotionCategory).ToList(),
                    UserOtherLanguages = _context.UserOtherLanguages.Where(u => u.UserId == userid).Include(u => u.Language).ToList(),
                    Online = _context.Connection.Where(a => a.UserId == userid).OrderByDescending(a => a.ConnectionTime).FirstOrDefault()?.Connected,
                    FollowersCount = _context.UserFollowing.Where(f => f.FollowingToId == userid).Count(),
                    FollowingCount = _context.UserFollowing.Where(f => f.UserId == userid).Count(),
                    IsSaved = _context.SavedProfessionals.Where(sr => sr.UserId == appUserId && sr.ProfessionalId == userid).Count() == 0 ? false : true,
                    IsFavorite = _context.FavoriteProfessionals.Where(fr => fr.UserId == appUserId && fr.ProfessionalId == userid).Count() == 0 ? false : true,
                };

                viewModel.Add(vm);
            }


            return Ok(viewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleSavedProfessional(string id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var message = "";
            var savedProfessional = await _context.SavedProfessionals.Where(r => r.UserId == appUser.Id && r.ProfessionalId == id).FirstOrDefaultAsync();
            if (savedProfessional == null)
            {
                savedProfessional = new SavedProfessional()
                {
                    UserId = appUser.Id,
                    ProfessionalId = id
                };
                _context.SavedProfessionals.Add(savedProfessional);
                message = "saved";
            }
            else
            {
                _context.SavedProfessionals.Remove(savedProfessional);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleFavoriteProfessional(string id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var message = "";
            var favoriteProfessional = await _context.FavoriteProfessionals.Where(r => r.UserId == appUser.Id && r.ProfessionalId == id).FirstOrDefaultAsync();
            if (favoriteProfessional == null)
            {
                favoriteProfessional = new FavoriteProfessional()
                {
                    UserId = appUser.Id,
                    ProfessionalId = id
                };
                _context.FavoriteProfessionals.Add(favoriteProfessional);
                message = "saved";
            }
            else
            {
                _context.FavoriteProfessionals.Remove(favoriteProfessional);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }
    }
}