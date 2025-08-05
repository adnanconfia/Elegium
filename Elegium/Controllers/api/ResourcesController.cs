using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Resources;
using Elegium.Dtos.ResourceDtos;
using Elegium.Dtos;
using tusdotnet.Stores;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Elegium.ExtensionMethods;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment _env;

        public ResourcesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Resources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetUserResources()
        {
            var appUser = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            var resources = await _context.Resource
                .Include(r => r.EquipmentCategory)
                .Include(r => r.Currency)
                .Where(r => r.UserId == appUser.Id)
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
                    .Where(u => u.UserId == r.UserId)
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

        // GET: api/Resources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetUserEquipments()
        {
            var appUser = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

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
                    .Where(u => u.UserId == r.UserId)
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
                    }).FirstOrDefault(),
                }).ToListAsync();

            return resources;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetAllResources([FromBody] ResourceSearchQuery searchQuery)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var resources = await _context.Resource
                .Include(r => r.EquipmentCategory)
                .Include(r => r.Currency)
                .Where(r => (searchQuery.ConditionId == 0 || r.ConditionId == searchQuery.ConditionId)
                && (string.IsNullOrEmpty(searchQuery.HireOrSale) || r.HireOrSale == searchQuery.HireOrSale)

                    && (searchQuery.MinRentalPeriod == 0 || r.MinRentalPeriod >= searchQuery.MinRentalPeriod)
                    && (searchQuery.MaxRentalPeriod == 0 || r.MaxRentalPeriod <= searchQuery.MaxRentalPeriod)
                    && (searchQuery.RentalPrice == 0 || r.RentalPrice <= searchQuery.RentalPrice)
                    && (searchQuery.Insured == false || r.Insured == searchQuery.Insured)
                    && (searchQuery.EquipmentCategoryId == 0 || r.EquipmentCategoryId == searchQuery.EquipmentCategoryId)
                    && (searchQuery.CurrencyId == 0 || r.CurrencyId == searchQuery.CurrencyId)
                    && (searchQuery.SalePrice == 0 || r.SalePrice <= searchQuery.SalePrice)

                )
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
                    IsSaved = _context.SavedResource.Where(sr => sr.UserId == appUser.Id && sr.ResourceId == r.Id).Count() == 0 ? false : true,
                    IsFavorite = _context.FavoriteResource.Where(fr => fr.UserId == appUser.Id && fr.ResourceId == r.Id).Count() == 0 ? false : true,
                    ResourceOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == r.UserId)
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
                    }).FirstOrDefault(),
                })
                .Where(list => (searchQuery.CountryId == 0 || list.ResourceOwner.Country.Id == searchQuery.CountryId))
                .ToListAsync();

            return resources;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetSavedResources()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var resIds = _context.SavedResource.Where(sr => sr.UserId == appUser.Id).Select(sr => sr.ResourceId).ToList();

            var resources = await _context.Resource
                .Include(r => r.EquipmentCategory)
                .Include(r => r.Currency)
                .Where(r => resIds.Contains(r.Id))
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
                    IsSaved = _context.SavedResource.Where(sr => sr.UserId == appUser.Id && sr.ResourceId == r.Id).Count() == 0 ? false : true,
                    IsFavorite = _context.FavoriteResource.Where(fr => fr.UserId == appUser.Id && fr.ResourceId == r.Id).Count() == 0 ? false : true,
                    ResourceOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == r.UserId)
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
                    }).FirstOrDefault(),
                }).ToListAsync();

            return resources;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetFavoriteResources()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var resIds = _context.FavoriteResource.Where(sr => sr.UserId == appUser.Id).Select(sr => sr.ResourceId).ToList();

            var resources = await _context.Resource
                .Include(r => r.EquipmentCategory)
                .Include(r => r.Currency)
                .Where(r => resIds.Contains(r.Id))
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
                    IsSaved = _context.SavedResource.Where(sr => sr.UserId == appUser.Id && sr.ResourceId == r.Id).Count() == 0 ? false : true,
                    IsFavorite = _context.FavoriteResource.Where(fr => fr.UserId == appUser.Id && fr.ResourceId == r.Id).Count() == 0 ? false : true,
                    ResourceOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == r.UserId)
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
                    }).FirstOrDefault(),
                }).ToListAsync();

            return resources;
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceDto>> GetResource(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var resource = await _context.Resource
                .Include(r => r.EquipmentCategory)
                .Include(r => r.Currency)
                .Where(r => r.Id == id)
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
                    IsSaved = _context.SavedResource.Where(sr => sr.UserId == appUser.Id && sr.ResourceId == r.Id).Count() == 0 ? false : true,
                    IsFavorite = _context.FavoriteResource.Where(fr => fr.UserId == appUser.Id && fr.ResourceId == r.Id).Count() == 0 ? false : true,
                    ResourceOwner = _context.UserProfiles
                    .Include(u => u.CompanyPosition)
                    .Include(u => u.CompanyType)
                    .Include(u => u.CompanyCountry)
                    .Include(u => u.CompanyCity)
                    .Where(u => u.UserId == r.UserId)
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
                    ProjectResourceDto = (from k in _context.ProjectResources
                                          where k.OriginalResourceId == r.Id
                                          && k.Status == "A"
                                          && (
                                          (

                                          k.HireOrSale == "H" && DateTime.UtcNow >= k.FromTime.Value.Date && DateTime.UtcNow <= k.ToTime.Value.Date) || (

                                          k.HireOrSale == "S"

                                          ))
                                          select new ProjectResourceDto()
                                          {
                                              Available = false,
                                              FromTime = k.FromTime != null ? k.FromTime.Value.ToUserFriendlyTime() : string.Empty,
                                              ToTime = k.ToTime != null ? k.FromTime.Value.ToUserFriendlyTime() : string.Empty,
                                              Id = k.Id,
                                              OwnerId = k.OwnerId,
                                              SenderId = k.SenderId,
                                              Status = k.SenderId == appUser.Id ? "ME" : k.Status
                                          }
                                 ).FirstOrDefault(),
                    Status = (from l in _context.ProjectResources
                              where l.OriginalResourceId == id && l.Status == "P" && l.SenderId == appUser.Id
                              select new
                              {
                                  Status = "ME"
                              }

                              ).Count() == 0 ? "" : "ME",
                    Action = string.Empty

                }).FirstOrDefaultAsync();

            if (resource == null)
            {
                return NotFound();
            }

            return resource;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleSavedResource(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var message = "";
            var savedResource = await _context.SavedResource.Where(r => r.UserId == appUser.Id && r.ResourceId == id).FirstOrDefaultAsync();
            if (savedResource == null)
            {
                savedResource = new SavedResource()
                {
                    UserId = appUser.Id,
                    ResourceId = id
                };
                _context.SavedResource.Add(savedResource);
                message = "saved";
            }
            else
            {
                _context.SavedResource.Remove(savedResource);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ToggleFavoriteResource(int id)
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var message = "";
            var favoriteResource = await _context.FavoriteResource.Where(r => r.UserId == appUser.Id && r.ResourceId == id).FirstOrDefaultAsync();
            if (favoriteResource == null)
            {
                favoriteResource = new FavoriteResource()
                {
                    UserId = appUser.Id,
                    ResourceId = id
                };
                _context.FavoriteResource.Add(favoriteResource);
                message = "saved";
            }
            else
            {
                _context.FavoriteResource.Remove(favoriteResource);
                message = "removed";
            }

            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult<Resource>> SaveOrUpdateResource([FromBody] Resource resource)
        {
            var appUser = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (resource.Id != 0)
            {
                var resourceInDb = await _context.Resource.Where(r => r.Id == resource.Id).Select(p => new { p.Id, p.UserId }).FirstOrDefaultAsync();
                if (resourceInDb == null)
                    return NotFound("Resource is not valid.");
                if (resourceInDb.UserId != appUser.Id)
                    return NotFound("Resource is unauthorized for this user.");
            }
            if (resource.HireOrSale == "S")
            {
                resource.Insured = false;
                resource.MaxRentalPeriod = null;
                resource.MinRentalPeriod = null;
                resource.RentalPrice = null;
                resource.RentalTerms = null;
                resource.LendingType = null;
            }
            else if (resource.HireOrSale == "H")
            {
                resource.SalePrice = null;
                resource.ConditionId = null;
            }
            if (resource.Id == 0)
            {
                resource.CreateDateTime = DateTime.Now;
                resource.ModifiedDateTime = DateTime.Now;
                resource.UserId = appUser.Id;
                _context.Resource.Add(resource);
            }
            else
            {
                resource.ModifiedDateTime = DateTime.Now;
                _context.Entry(resource).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok(resource);
        }

        [HttpGet]
        public async Task<IActionResult> GetResourcePhotos(int resourceId)
        {
            List<ResourceMediaFile> list = await _context.ResourceMediaFile.Where(a => a.Type == "P" && a.ResourceId == resourceId).ToListAsync();
            var q = list.Select(a => new
            {
                RelativeTime = a.CreateAt.GetRelativeTime(),
                a.ContentType,
                a.FileId,
                a.Name,
                a.Size,
                a.Id,
                a.ResourceId,
                a.Type,
                a.CreateAt,
                a.CreatedAtTicks
            }).OrderByDescending(a => a.CreatedAtTicks);
            return Ok(new { Records = q });
        }

        [HttpPost]
        public async Task<IActionResult> PostResourceMediaFiles([FromBody] ResourceMediaFile[] mediaFiles)
        {
            try
            {
                if (mediaFiles != null && mediaFiles.Length > 0)
                {
                    await _context.ResourceMediaFile.AddRangeAsync(mediaFiles);
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

        [HttpDelete("{id}/{type}/{fileId}")]
        public async Task<IActionResult> DeleteFile(int id, string type, string fileId)
        {
            try
            {
                var filesList = await _context.ResourceMediaFile.Where(a => a.FileId == fileId && a.Type == type).ToListAsync();
                var fileObj = await _context.ResourceMediaFile.FindAsync(id);
                _context.ResourceMediaFile.Remove(fileObj);
                _context.SaveChanges();

                var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                var file = await store.GetFileAsync(fileId, HttpContext.RequestAborted);
                if (file == null)
                {
                    return NotFound();
                }
                if (filesList.Count() == 1)
                    await store.DeleteFileAsync(fileId, HttpContext.RequestAborted);
                return Ok(new { success = true, Message = "Deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, Message = ex.Message });
            }
        }


        [HttpGet("{id}/{flag}/{type}/{resourceId}")]
        public async Task<IActionResult> NextPrevFileId(string id, string flag, string type, int resourceId)
        {
            var currentFileObj = await _context.ResourceMediaFile.Where(a => a.FileId.Equals(id) && a.ResourceId == resourceId).FirstOrDefaultAsync();
            var currentFileUploadTime = currentFileObj.CreatedAtTicks;
            ResourceMediaFile nextPrevFileId = null;

            if (flag.Equals("P"))
            {
                var nextFileObj = await _context.ResourceMediaFile.Where(a => a.ResourceId.Equals(resourceId) && a.CreatedAtTicks > currentFileUploadTime).OrderBy(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.ResourceMediaFile.Where(a => a.ResourceId.Equals(resourceId)).OrderBy(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }
            else
            {
                var nextFileObj = await _context.ResourceMediaFile.Where(a => a.ResourceId.Equals(resourceId) && a.CreatedAtTicks < currentFileUploadTime).OrderByDescending(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.ResourceMediaFile.Where(a => a.ResourceId.Equals(resourceId)).OrderByDescending(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }

            return Ok(new { Record = nextPrevFileId });
        }

        // PUT: api/Resources/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResource(int id, Resource resource)
        {
            if (id != resource.Id)
            {
                return BadRequest();
            }

            _context.Entry(resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(id))
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

        // POST: api/Resources
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Resource>> PostResource(Resource resource)
        {
            _context.Resource.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResource", new { id = resource.Id }, resource);
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteResource(int id)
        {
            var appUser = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();
            var resource = await _context.Resource.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }
            if (resource.UserId != appUser.Id)
            {
                return NotFound("Resource is unauthorized for this user.");
            }

            //delete media files first
            List<ResourceMediaFile> filesList = await _context.ResourceMediaFile.Where(a => a.ResourceId == resource.Id).ToListAsync();

            foreach (ResourceMediaFile fileObj in filesList)
            {
                var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                var file = await store.GetFileAsync(fileObj.FileId, HttpContext.RequestAborted);
                if (file == null)
                {
                    return NotFound();
                }
                await store.DeleteFileAsync(fileObj.FileId, HttpContext.RequestAborted);

                _context.ResourceMediaFile.Remove(fileObj);
            }





            _context.Resource.Remove(resource);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ResourceExists(int id)
        {
            return _context.Resource.Any(e => e.Id == id);
        }
    }
}
