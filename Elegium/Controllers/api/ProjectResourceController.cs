using Elegium.Data;
using Elegium.Dtos;
using Elegium.Dtos.ProjectDtos;
using Elegium.Dtos.ResourceDtos;
using Elegium.ExtensionMethods;
using Elegium.Middleware;
using Elegium.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProjectResourceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        public ProjectResourceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResourceDto>> CreateResourceRequest(ProjectResourceDto request)
        {
            var user = await _userManager.GetUserAsync(User);

            var hireDateConflict = await (from k in _context.Resource
                                          join l in _context.ProjectResources
                                          on k.Id equals l.OriginalResourceId
                                          where l.Status == "A" &&

                                          (l.HireOrSale == "H" && (DateTime.Parse(request.FromTime).Date >= l.FromTime.Value.Date && DateTime.Parse(request.FromTime).Date <= l.ToTime.Value.Date)

                                          || (DateTime.Parse(request.ToTime).Date >= l.FromTime.Value.Date && DateTime.Parse(request.ToTime).Date <= l.ToTime.Value.Date)
                                          )

                                          select new
                                          {
                                              abc = "124"
                                          }).CountAsync();

            if (hireDateConflict > 0)
                return Ok(new { success = true, msg = "Resource is not available in given dates!" });



            var projectResourceObj = new ProjectResource()
            {
                Description = request.Description,
                FromTime = request.ResourceDto.HireOrSale == "H" ? DateTime.Parse(request.FromTime) : (DateTime?)null,
                ToTime = request.ResourceDto.HireOrSale == "H" ? DateTime.Parse(request.ToTime) : (DateTime?)null,
                OriginalResourceId = request.ResourceDto.Id,
                OwnerId = request.ResourceDto.ResourceOwner.UserId,
                Price = request.Price,
                ProjectId = request.ProjectId,
                HireOrSale = request.ResourceDto.HireOrSale,
                SenderId = user.Id,
                PurchaseOn = request.ResourceDto.HireOrSale == "S" ? DateTime.Parse(request.PurchaseOn) : (DateTime?)null,
                Status = "P"//,
                //ResourceId = request.ResourceDto.Id
            };

            await _context.ProjectResources.AddAsync(projectResourceObj);
            await _context.SaveChangesAsync();

            var _url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";// string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";

            await _notificationService.GenerateNotificationAsync(_context.Users.Find(projectResourceObj.SenderId), _context.Users.Find(projectResourceObj.OwnerId), "ResourceRequestSent", $"{_url}/#/myresourcerequests");

            request.Id = projectResourceObj.Id;


            return request;
        }

        public async Task<ActionResult<IEnumerable<DocumentFiles>>> PostResourceRequestFiles(List<DocumentFiles> documentFiles)
        {
            var user = await _userManager.GetUserAsync(User);
            foreach (var d in documentFiles)
            {
                d.UserFriendlySize = d.Size.GetBytesReadable();
                d.MimeType = MimeMapping.MimeUtility.GetMimeMapping(d.Name);
                FileInfo fi = new FileInfo(d.Name);
                d.Type = fi.Extension.GetFileType();
                d.UserId = user.Id;
                d.Extension = fi.Extension;
            }
            await _context.DocumentFiles.AddRangeAsync(documentFiles);
            await _context.SaveChangesAsync();
            return Ok(documentFiles);
        }

        public async Task<ActionResult<IEnumerable<ProjectResourceDto>>> GetMyOffers()
        {
            var user = await _userManager.GetUserAsync(User);
            var requests = (from k in _context.Resource
                            join l in _context.ProjectResources
                            on k.Id equals l.OriginalResourceId
                            where l.OwnerId == user.Id
                            select new ProjectResourceDto()
                            {
                                Id = l.Id,
                                FromTime = l.HireOrSale == "H" ? l.FromTime.Value.ToString("MM/dd/yyyy") : string.Empty,
                                ToTime = l.HireOrSale == "H" ? l.ToTime.Value.ToString("MM/dd/yyyy") : string.Empty,
                                Status = l.Status,
                                HireOrSale = l.HireOrSale,
                                Description = l.Description,
                                Price = l.Price,
                                ProjectId = l.ProjectId.Value,
                                OwnerId = l.OwnerId,
                                SenderId = l.SenderId,
                                SenderName = _context.Users.Where(a => a.Id == l.SenderId).FirstOrDefault().GetUserName(),
                                PurchaseOn = l.HireOrSale == "S" ? l.PurchaseOn.Value.ToString("MM/dd/yyyy") : string.Empty,
                                ResourceDto = _context.Resource
                 .Include(r => r.EquipmentCategory)
                 .Include(r => r.Currency)
                 .Where(r => r.Id == k.Id)
                 .Select(r => new ResourceDto()
                 {
                     Id = r.Id,
                     EquipmentCategory = r.EquipmentCategory,
                     //EquipmentCategoryId = r.EquipmentCategoryId,
                     Name = r.Name,
                     //Condition = r.Condition,
                     //ConditionId = r.ConditionId,
                     //Description = r.Description,
                     //HireOrSale = r.HireOrSale,
                     //MinRentalPeriod = r.MinRentalPeriod,
                     //MaxRentalPeriod = r.MaxRentalPeriod,
                     //RentalPrice = r.RentalPrice,
                     //Currency = r.Currency,
                     //CurrencyId = r.CurrencyId,
                     //IsEquipment = r.IsEquipment,
                     //LendingType = r.LendingType,
                     //Insured = r.Insured,
                     //RentalTerms = r.RentalTerms,
                     //SalePrice = r.SalePrice,
                     //Website = r.Website,
                     //YoutubeVideoLink = r.YoutubeVideoLink,
                     //VimeoVideoLink = r.VimeoVideoLink,
                     //OtherTerms = r.OtherTerms,
                     //CreateDateTime = r.CreateDateTime,
                     //ModifiedDateTime = r.ModifiedDateTime,
                     //MediaFileIds = _context.ResourceMediaFile.Where(t => t.ResourceId == r.Id).Take(3).Select(t => t.FileId).ToList(),
                     //UserId = r.UserId,
                     ResourceOwner = _context.UserProfiles
                     .Where(u => u.UserId == r.UserId)
                     .Select(u => new UserProfileDto()
                     {
                         Id = u.Id,
                         FirstName = u.FirstName,
                         MiddleName = u.MiddleName,
                         LastName = u.LastName,
                         UserId = u.UserId
                     }).FirstOrDefault()
                 }).FirstOrDefault(),
                                ProjectDto = _context.Project.Where(a => a.Id == l.ProjectId && !a.Deleted).Select(a => new ProjectDto()
                                {
                                    Name = a.Name,
                                    Id = a.Id
                                }).FirstOrDefault(),
                                Action = string.Empty

                            }
                                 ).ToList();
            return requests;
        }

        public async Task<ActionResult<IEnumerable<ProjectResourceDto>>> GetMyRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            var requests = (from k in _context.Resource
                            join l in _context.ProjectResources
                            on k.Id equals l.OriginalResourceId
                            where l.SenderId == user.Id
                            select new ProjectResourceDto()
                            {
                                Id = l.Id,
                                FromTime = l.HireOrSale == "H" ? l.FromTime.Value.ToString("MM/dd/yyyy") : string.Empty,
                                ToTime = l.HireOrSale == "H" ? l.ToTime.Value.ToString("MM/dd/yyyy") : string.Empty,
                                Status = l.Status,
                                HireOrSale = l.HireOrSale,
                                Description = l.Description,
                                Price = l.Price,
                                ProjectId = l.ProjectId.Value,
                                OwnerId = l.OwnerId,
                                SenderId = l.SenderId,
                                SenderName = _context.Users.Where(a => a.Id == l.SenderId).FirstOrDefault().GetUserName(),
                                PurchaseOn = l.HireOrSale == "S" ? l.PurchaseOn.Value.ToString("MM/dd/yyyy") : string.Empty,
                                ResourceDto = _context.Resource
                 .Include(r => r.EquipmentCategory)
                 .Include(r => r.Currency)
                 .Where(r => r.Id == k.Id)
                 .Select(r => new ResourceDto()
                 {
                     Id = r.Id,
                     EquipmentCategory = r.EquipmentCategory,
                     //EquipmentCategoryId = r.EquipmentCategoryId,
                     Name = r.Name,
                     //Condition = r.Condition,
                     //ConditionId = r.ConditionId,
                     //Description = r.Description,
                     //HireOrSale = r.HireOrSale,
                     //MinRentalPeriod = r.MinRentalPeriod,
                     //MaxRentalPeriod = r.MaxRentalPeriod,
                     //RentalPrice = r.RentalPrice,
                     //Currency = r.Currency,
                     //CurrencyId = r.CurrencyId,
                     //IsEquipment = r.IsEquipment,
                     //LendingType = r.LendingType,
                     //Insured = r.Insured,
                     //RentalTerms = r.RentalTerms,
                     //SalePrice = r.SalePrice,
                     //Website = r.Website,
                     //YoutubeVideoLink = r.YoutubeVideoLink,
                     //VimeoVideoLink = r.VimeoVideoLink,
                     //OtherTerms = r.OtherTerms,
                     //CreateDateTime = r.CreateDateTime,
                     //ModifiedDateTime = r.ModifiedDateTime,
                     //MediaFileIds = _context.ResourceMediaFile.Where(t => t.ResourceId == r.Id).Take(3).Select(t => t.FileId).ToList(),
                     //UserId = r.UserId,
                     ResourceOwner = _context.UserProfiles
                     .Where(u => u.UserId == r.UserId)
                     .Select(u => new UserProfileDto()
                     {
                         Id = u.Id,
                         FirstName = u.FirstName,
                         MiddleName = u.MiddleName,
                         LastName = u.LastName,
                         UserId = u.UserId
                     }).FirstOrDefault()
                 }).FirstOrDefault(),
                                ProjectDto = _context.Project.Where(a => a.Id == l.ProjectId && !a.Deleted).Select(a => new ProjectDto()
                                {
                                    Name = a.Name,
                                    Id = a.Id
                                }).FirstOrDefault(),
                                Action = string.Empty

                            }
                                 ).ToList();
            return requests;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResourceDto>> TakeResourceRequestAction(ProjectResourceDto offer)
        {
            var user = await _userManager.GetUserAsync(User);
            var resProject = await _context.ProjectResources.Where(a => a.Id == offer.Id && a.Status == "P").FirstOrDefaultAsync();
            if (resProject == null)
                return NotFound();

            if (offer.Action == "C" || (offer.Action == "R" && (offer.HireOrSale == "H" || offer.HireOrSale == "S")) || (offer.Action == "A" && offer.HireOrSale == "H"))
            {
                resProject.Status = offer.Action;
                _context.Entry(resProject).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else if (offer.Action == "A" && offer.HireOrSale == "S")
            {
                var resource = await _context.Resource.FindAsync(offer.ResourceDto.Id);
                var projectResource = await _context.ProjectResources.FindAsync(offer.Id);

                if (resource == null || projectResource == null)
                    return NotFound();

                resource.UserId = user.Id;
                var resourceMediaFiles = await _context.ResourceMediaFile.Where(a => a.ResourceId == resource.Id).ToListAsync();

                await _context.Resource.AddAsync(resource);

                await _context.SaveChangesAsync();

                projectResource.ResourceId = resource.Id;
                _context.Entry(projectResource).State = EntityState.Modified;

                foreach (var mf in resourceMediaFiles)
                {
                    mf.ResourceId = resource.Id;
                }
                await _context.ResourceMediaFile.AddRangeAsync(resourceMediaFiles);
                await _context.SaveChangesAsync();

                offer.ResourceDto.Id = resource.Id;
                offer.ResourceDto.UserId = resource.UserId;
            }

            var _url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";// string.Format(@"{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);///{0}/{1}";

            await _notificationService.GenerateNotificationAsync(
                user,
                _context.Users.Find(offer.ResourceDto.ResourceOwner.UserId),
               offer.Action == "A" ? NotificationKind.ResourceRequestApproved : offer.Action == "R" ? NotificationKind.ResourceRequestRejected : "",
                $"{_url}/#/myresourcerequests");

            return offer;
        }
    }
}
