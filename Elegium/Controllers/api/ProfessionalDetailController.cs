using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Microsoft.AspNetCore.SignalR;
using Elegium.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using tusdotnet.Stores;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Elegium.Configuration;
using Elegium.Dtos;
using Elegium.Services;
using Elegium.ExtensionMethods;
using Elegium.Dtos.ResourceDtos;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfessionalDetailController : ControllerBase
    {
        private const string Format = " Dear {0} <br /> You have received a message from {1} <br /> {2}";

        private const string ForwardMsg = "{0} has forwarded you profile of <a href='{1}'>{2}</a> <br /> Addditional Notes: {2}";
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        IWebHostEnvironment _env;
        private readonly IEmailSender _emailSender;
        public ProfessionalDetailController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
            _emailSender = emailSender;
        }

        //cde75b94-0c08-42e3-a27c-aa0816b25e47
        [Route("[controller]/[action]/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserData(string id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var applicationUser = await _userManager.FindByIdAsync(id);

            var userCredits = await _context.UserCredit.Where(a => a.UserId == applicationUser.Id).ToListAsync();
            var userEquipment = await _context.UserEquipment.Where(a => a.UserId == applicationUser.Id)
                .Include(a => a.EquipmentCategory)
                .ToListAsync();
            var userProfile = await _context.UserProfiles.Where(a => a.UserId == applicationUser.Id)
                .Include(a => a.Country)
                .Include(a => a.City)
                .Include(a => a.CompanyPosition)
                .Include(a => a.CompanyCountry)
                .Include(a => a.CompanyCity)
                .Include(a => a.CompanyStudioCity)
                .Include(a => a.CompanyStudioCountry)
                .Include(a => a.CompanyType)
                .Include(a => a.Skill)
                .Include(a => a.SkillLevel)
                .FirstOrDefaultAsync();

            userProfile.Following = await _context.UserFollowing
                .Where(f => f.UserId == appUser.Id && f.FollowingToId == applicationUser.Id)
                .CountAsync()>0?true:false;

            var audioAlbums = await _context.Album.Where(a => a.UserId == applicationUser.Id && a.Type == "A" && a.AccessRight == true).Select(a => new
            {
                Id = a.Id.ToString(),
                RelativeTime = a.Created.GetRelativeTime(),
                a.Name,
                a.Type,
                a.User,
                a.UserId,
                FileIds = _context.MediaFiles.Where(t => t.AlbumId == a.Id).Take(5).Select(t => t.FileId).ToList(),
                FilesCount = _context.MediaFiles.Where(t => t.AlbumId == a.Id).Count() - 5,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                a.Created
            }).OrderByDescending(a => a.Created).ToListAsync();

            var videoAlbums = await _context.Album.Where(a => a.UserId == applicationUser.Id && a.Type == "V" && a.AccessRight == true).Select(a => new
            {
                Id = a.Id.ToString(),
                RelativeTime = a.Created.GetRelativeTime(),
                a.Name,
                a.Type,
                a.User,
                a.UserId,
                FileIds = _context.MediaFiles.Where(t => t.AlbumId == a.Id).Take(5).Select(t => t.FileId).ToList(),
                FilesCount = _context.MediaFiles.Where(t => t.AlbumId == a.Id).Count() - 5,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                a.Created
            }).OrderByDescending(a => a.Created).ToListAsync();

            var photoAlbums = await _context.Album.Where(a => a.UserId == applicationUser.Id && a.Type == "P" && a.AccessRight == true).Select(a => new
            {
                Id = a.Id.ToString(),
                RelativeTime = a.Created.GetRelativeTime(),
                a.Name,
                a.Type,
                a.User,
                a.UserId,
                FileIds = _context.MediaFiles.Where(t => t.AlbumId == a.Id).Take(5).Select(t => t.FileId).ToList(),
                FilesCount = _context.MediaFiles.Where(t => t.AlbumId == a.Id).Count() - 5,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                a.Created
            }).OrderByDescending(a => a.Created).ToListAsync();

            var audios = await _context.MediaFiles.Where(a => a.UserId == applicationUser.Id && a.Type == "A" && a.AccessRight == true).Select(a => new
            {
                RelativeTime = a.CreateAt.GetRelativeTime(),
                a.ContentType,
                a.FileId,
                a.Name,
                a.Size,
                a.Id,
                a.Type,
                a.AlbumId,
                a.User,
                a.UserId,
                a.Album,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                UserName = applicationUser.FirstName + " " + applicationUser.LastName,
                a.CreateAt,
                a.CreatedAtTicks
            }).OrderByDescending(a => a.CreatedAtTicks).ToListAsync();

            var videos = await _context.MediaFiles.Where(a => a.UserId == applicationUser.Id && a.Type == "V" && a.AccessRight == true).Select(a => new
            {
                RelativeTime = a.CreateAt.GetRelativeTime(),
                a.ContentType,
                a.FileId,
                a.Name,
                a.Size,
                a.Id,
                a.Type,
                a.AlbumId,
                a.User,
                a.UserId,
                a.Album,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                UserName = applicationUser.FirstName + " " + applicationUser.LastName,
                a.CreateAt,
                a.CreatedAtTicks
            }).OrderByDescending(a => a.CreatedAtTicks).ToListAsync();

            var photos = await _context.MediaFiles.Where(a => a.UserId == applicationUser.Id && a.Type == "P" && a.AccessRight == true).Select(a => new
            {
                RelativeTime = a.CreateAt.GetRelativeTime(),
                a.ContentType,
                a.FileId,
                a.Name,
                a.Size,
                a.Id,
                a.Type,
                a.AlbumId,
                a.User,
                a.UserId,
                a.Album,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                UserName = applicationUser.FirstName + " " + applicationUser.LastName,
                a.CreateAt,
                a.CreatedAtTicks
            }).OrderByDescending(a => a.CreatedAtTicks).ToListAsync();

            var otherLanguages = await _context.UserOtherLanguages
                .Where(a => a.UserId == applicationUser.Id)
                .Include(a => a.Language)
                .ToListAsync();

            var promotionCategories = await _context.UserPromotionCategory
                .Where(a => a.UserId == applicationUser.Id)
                .Include(a => a.PromotionCategory)
                .ToListAsync();

            var userSkills = await _context.UserAdditionalSkills
                .Where(a => a.UserId == applicationUser.Id)
                .ToListAsync();

            var userCategories = await _context.UserPromotionCategory
                .Where(a => a.UserId == applicationUser.Id)
                .Include(a => a.PromotionCategory)
                .ToListAsync();


            var userLoggedIn = User.Identity.IsAuthenticated;
            var loggedInUser = await _userManager.GetUserAsync(User);

            //foreach (var i in userProfile)
            //    i.Online = _context.Connection.Where(a => a.UserId == applicationUser.Id).OrderByDescending(a => a.ConnectionTime).FirstOrDefault()?.Connected;

            return Ok(new
            {
                photos,
                videos,
                audios,
                photoAlbums,
                videoAlbums,
                audioAlbums,
                userProfile,
                userEquipment,
                userCredits,
                otherLanguages,
                promotionCategories,
                userSkills,
                userCategories,
                userLoggedIn,
                loggedInUser
            });
        }

        [Route("[controller]/[action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetUserEquipments(string id)
        {
            var appUser = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (appUser == null)
                return NotFound();

            var resources = await _context.Resource
                .Include(r => r.EquipmentCategory)
                .Include(r => r.Currency)
                .Where(r => r.UserId == appUser.Id && r.IsEquipment == true)
                .Select(r => new ResourceDto()
                {
                    Id = r.Id,
                    EquipmentCategory = r.EquipmentCategory,
                    EquipmentCategoryId = r.EquipmentCategoryId,
                    Name = r.Name,
                    Condition = r.Condition,
                    ConditionId = r.ConditionId,
                    Description = r.Description,
                    HireOrSale = r.HireOrSale,
                    MinRentalPeriod = r.MinRentalPeriod,
                    MaxRentalPeriod = r.MaxRentalPeriod,
                    RentalPrice = r.RentalPrice,
                    Currency = r.Currency,
                    CurrencyId = r.CurrencyId,
                    IsEquipment = r.IsEquipment,
                    LendingType = r.LendingType,
                    Insured = r.Insured,
                    RentalTerms = r.RentalTerms,
                    SalePrice = r.SalePrice,
                    Website = r.Website,
                    YoutubeVideoLink = r.YoutubeVideoLink,
                    VimeoVideoLink = r.VimeoVideoLink,
                    OtherTerms = r.OtherTerms,
                    CreateDateTime = r.CreateDateTime,
                    ModifiedDateTime = r.ModifiedDateTime,
                    MediaFileIds = _context.ResourceMediaFile.Where(t => t.ResourceId == r.Id).Take(3).Select(t => t.FileId).ToList(),
                    UserId = r.UserId,
                    ResourceOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => r.UserId == r.UserId)
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
                    }).FirstOrDefault(),
                }).ToListAsync();

            return resources;
        }

        [HttpGet("{id}/{flag}/{type}/{userId}")]
        public async Task<IActionResult> NextPrevFileId(string id, string flag, string type, string userId)
        {
            var currentFileObj = await _context.MediaFiles.Where(a => a.FileId.Equals(id) && a.Type == type && a.AccessRight == true).FirstOrDefaultAsync();
            var currentFileUploadTime = currentFileObj.CreatedAtTicks;
            var user = await _userManager.FindByIdAsync(userId);
            MediaFile nextPrevFileId = null;

            if (flag.Equals("P"))
            {
                var nextFileObj = await _context.MediaFiles.Where(a => a.UserId.Equals(user.Id) && a.Type == type && a.CreatedAtTicks > currentFileUploadTime && a.AccessRight == true).OrderBy(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.MediaFiles.Where(a => a.UserId.Equals(user.Id) && a.Type == type && a.AccessRight == true).OrderBy(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }
            else
            {
                var nextFileObj = await _context.MediaFiles.Where(a => a.UserId.Equals(user.Id) && a.Type == type && a.CreatedAtTicks < currentFileUploadTime && a.AccessRight == true).OrderByDescending(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.MediaFiles.Where(a => a.UserId.Equals(user.Id) && a.Type == type && a.AccessRight == true).OrderByDescending(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }

            return Ok(new { Record = nextPrevFileId });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPhotos(int? albumId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            List<MediaFile> list = null;

            if (albumId == null)
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "P" && a.AccessRight == true).ToListAsync();
            else
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "P" && a.AlbumId == albumId && a.AccessRight == true).ToListAsync();
            var q = list.Select(a => new
            {
                RelativeTime = a.CreateAt.GetRelativeTime(),
                a.ContentType,
                a.FileId,
                a.Name,
                a.Size,
                a.Id,
                a.AlbumId,
                a.User,
                a.Type,
                a.UserId,
                a.Album,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                UserName = user.FirstName + " " + user.LastName,
                a.CreateAt,
                a.CreatedAtTicks
            }).OrderByDescending(a => a.CreatedAtTicks);
            return Ok(new { Records = q });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserVideos(int? albumId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            List<MediaFile> list = null;

            if (albumId == null)
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "V" && a.AccessRight == true).ToListAsync();
            else
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "V" && a.AlbumId == albumId && a.AccessRight == true).ToListAsync();
            var q = list.Select(a => new
            {
                RelativeTime = a.CreateAt.GetRelativeTime(),
                a.ContentType,
                a.FileId,
                a.Name,
                a.Size,
                a.Id,
                a.Type,
                a.AlbumId,
                a.User,
                a.UserId,
                a.Album,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                UserName = user.FirstName + " " + user.LastName,
                a.CreateAt,
                a.CreatedAtTicks
            }).OrderByDescending(a => a.CreatedAtTicks);
            return Ok(new { Records = q });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAudios(int? albumId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            List<MediaFile> list = null;

            if (albumId == null)
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "A" && a.AccessRight == true).ToListAsync();
            else
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "A" && a.AlbumId == albumId && a.AccessRight == true).ToListAsync();
            var q = list.Select(a => new
            {
                RelativeTime = a.CreateAt.GetRelativeTime(),
                a.ContentType,
                a.FileId,
                a.Name,
                a.Size,
                a.Id,
                a.AlbumId,
                a.User,
                a.Type,
                a.UserId,
                a.Album,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                UserName = user.FirstName + " " + user.LastName,
                a.CreateAt,
                a.CreatedAtTicks
            }).OrderByDescending(a => a.CreatedAtTicks);
            return Ok(new { Records = q });
        }

        [HttpPost]
        public async Task<IActionResult> SaveMessage([FromBody] UserMessageDTO message)
        {
            try
            {
                var isLoggedIn = User.Identity.IsAuthenticated;
                var newMessage = message.UserMessages;
                var userProfile = message.UserProfile;
                if (isLoggedIn)
                    newMessage.FromUser = await _userManager.GetUserAsync(User);
                newMessage.ToUser = await _userManager.FindByIdAsync(userProfile.UserId);
                await _context.UserMessages.AddAsync(newMessage);
                await _context.SaveChangesAsync();

                var messageBody = string.Format(Format
                    , userProfile.FirstName + " " + userProfile.LastName,
                    newMessage.FirstName + " " + newMessage.LastName,
                    newMessage.Message
                    );

                await _emailSender.SendEmailAsync(userProfile.Email, newMessage.Subject, messageBody);
                return Ok(new { success = true, suc = "Success", Message = "Message sent successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, suc = "Failure", Message = ex.Message + "--" + (ex.InnerException == null ? string.Empty : ex.InnerException.Message) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForwardProfile([FromBody] ForwardMessageDTO message)
        {
            try
            {
                var isLoggedIn = User.Identity.IsAuthenticated;
                var forwardMsg = message.ForwardMessage;
                var userProfile = message.UserProfile;
                //if (isLoggedIn)
                //    newMessage.FromUser = await _userManager.GetUserAsync(User);
                //newMessage.ToUser = await _userManager.FindByIdAsync(userProfile.UserId);
                //await _context.UserMessages.AddAsync(newMessage);
                //await _context.SaveChangesAsync();

                var messageBody = string.Format(Format
                    , forwardMsg.FromName,
                    message.Url,
                    userProfile.FirstName + " " + userProfile.LastName,
                    forwardMsg.Message
                    );

                await _emailSender.SendEmailAsync(forwardMsg.ToEmail, userProfile.FirstName + " " + userProfile.LastName + " Profile | Elegium Network", messageBody);
                return Ok(new { success = true, suc = "Success", Message = "Profile forwarded successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, suc = "Failure", Message = ex.Message + "--" + (ex.InnerException == null ? string.Empty : ex.InnerException.Message) });
            }
        }
    }
}
