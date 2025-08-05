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
using Elegium.Data.Migrations;
using MimeTypes;
using Elegium.ExtensionMethods;
using Elegium.Dtos;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        IWebHostEnvironment _env;
        public UserProfilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: api/UserProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUserProfiles()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        // GET: api/UserProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return userProfile;
        }

        [HttpGet]
        public ActionResult<UserProfileViewModel> GetCurrentUserProfile()
        {
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var userProfile = _context.UserProfiles.Where(u => u.UserId == appUser.Id).FirstOrDefault();

            if (userProfile == null)
            {
                UserProfile profile = new UserProfile()
                {
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    Email = appUser.Email,
                    CompanyName = appUser.Company,
                    CompanyTypeId = int.Parse(appUser.Industry),
                    CompanyEmail = appUser.Email,
                    CompanyStudioEmail = appUser.Email,
                    UserId = appUser.Id
                };

                _context.UserProfiles.Add(profile);
                _context.SaveChanges();
                userProfile = profile;
            }

            var userProfileVM = new UserProfileViewModel()
            {
                UserProfile = userProfile,
                UserCredits = _context.UserCredit.Where(c => c.UserId == appUser.Id).ToList(),
                UserEquipments = _context.UserEquipment.Select(e => new UserEquipment { Id = e.Id, EquipmentName = e.EquipmentName, LendingType = e.LendingType, EquipmentCategoryId = e.EquipmentCategoryId, UserId = e.UserId }).Where(c => c.UserId == appUser.Id).ToList(),
                UserOtherLanguages = _context.UserOtherLanguages.Where(c => c.UserId == appUser.Id).Select(a => a.LanguageId).ToArray(),
                UserPromotionCategory = _context.UserPromotionCategory.Where(c => c.UserId == appUser.Id).Select(a => a.PromotionCategoryId).ToArray(),
                UserAdditionalSkills = _context.UserAdditionalSkills.Where(a => a.UserId == appUser.Id).ToList()
            };

            return userProfileVM;
        }

        // PUT: api/UserProfiles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(int id, UserProfile userProfile)
        {
            if (id != userProfile.Id)
            {
                return BadRequest();
            }

            _context.Entry(userProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
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
        public async Task<IActionResult> SaveUserProfile([FromBody] UserProfileViewModel userProfileVM)
        {
            UserProfile userProfile = userProfileVM.UserProfile;
            List<UserCredit> userCredit = userProfileVM.UserCredits;
            List<UserEquipment> userEquipment = userProfileVM.UserEquipments;
            List<Models.UserAdditionalSkills> userAdditionalSkills = userProfileVM.UserAdditionalSkills;

            var UserOtherLanguages = userProfileVM.UserOtherLanguages;
            var UserPromotionCategory = userProfileVM.UserPromotionCategory;

            if (!string.IsNullOrEmpty(userProfileVM.UserImage))
            {
                userProfile.Photo = Convert.FromBase64String(userProfileVM.UserImage.Split(',')[1]);
            }
            if (!string.IsNullOrEmpty(userProfileVM.CompanyImage))
            {
                userProfile.CompanyLogo = Convert.FromBase64String(userProfileVM.CompanyImage.Split(',')[1]);
            }
            var appUser = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (appUser == null || userProfile == null)
            {
                return BadRequest();
            }
            userProfile.User = null;
            userProfile.UserId = appUser.Id;

            _context.Entry(userProfile).State = EntityState.Modified;

            //Save User credit in DB
            foreach (var ucredit in userCredit)
            {
                var ucreditInDb = _context.UserCredit.Where(c => c.Id == ucredit.Id).FirstOrDefault();
                if (ucreditInDb == null)
                {
                    UserCredit credit = new UserCredit()
                    {
                        Year = ucredit.Year,
                        Workplace = ucredit.Workplace,
                        Job = ucredit.Job,
                        UserId = appUser.Id
                    };

                    _context.UserCredit.Add(credit);
                }
                else
                {
                    ucreditInDb.Year = ucredit.Year;
                    ucreditInDb.Workplace = ucredit.Workplace;
                    ucreditInDb.Job = ucredit.Job;

                    _context.Entry(ucreditInDb).State = EntityState.Modified;
                }
            }
            //delete credits from db that are not received from UI
            var creditIds = userCredit.Select(c => c.Id).ToList();
            var tobeRemoved = _context.UserCredit.Where(c => !creditIds.Contains(c.Id) && c.UserId == appUser.Id);
            foreach (var ucredit in tobeRemoved)
            {
                _context.UserCredit.Remove(ucredit);
            }


            //Save User Equipments in DB
            foreach (var uequip in userEquipment)
            {
                var uequipInDb = _context.UserEquipment.Where(c => c.Id == uequip.Id).FirstOrDefault();
                if (uequipInDb == null)
                {
                    UserEquipment equip = new UserEquipment()
                    {
                        EquipmentName = uequip.EquipmentName,
                        EquipmentCategoryId = uequip.EquipmentCategoryId,
                        LendingType = uequip.LendingType,
                        UserId = appUser.Id
                    };

                    _context.UserEquipment.Add(equip);
                }
                else
                {
                    uequipInDb.EquipmentName = uequip.EquipmentName;
                    uequipInDb.EquipmentCategoryId = uequip.EquipmentCategoryId;
                    uequipInDb.LendingType = uequip.LendingType;

                    _context.Entry(uequipInDb).State = EntityState.Modified;
                }
            }

            //delete equipments from db that are not received from UI
            var equipmentIds = userEquipment.Select(c => c.Id).ToList();
            var equipmentstobeRemoved = _context.UserEquipment.Where(c => !equipmentIds.Contains(c.Id) && c.UserId == appUser.Id);
            foreach (var uequipment in equipmentstobeRemoved)
            {
                _context.UserEquipment.Remove(uequipment);
            }


            //Save User Promotion Category in DB
            if (UserPromotionCategory != null)
            {
                var upromoInDb = _context.UserPromotionCategory.Where(c => c.UserId == appUser.Id);
                _context.UserPromotionCategory.RemoveRange(upromoInDb);
                foreach (var uPromo in UserPromotionCategory)
                {
                    UserPromotionCategory uPromoObj = new UserPromotionCategory()
                    {
                        PromotionCategoryId = uPromo,
                        UserId = appUser.Id
                    };

                    await _context.UserPromotionCategory.AddAsync(uPromoObj);
                }
            }

            //Save User Other languages in DB
            if (UserOtherLanguages != null)
            {
                var uOtherLangInDb = _context.UserOtherLanguages.Where(c => c.UserId == appUser.Id);
                _context.UserOtherLanguages.RemoveRange(uOtherLangInDb);
                foreach (var uOtherLang in UserOtherLanguages)
                {
                    UserOtherLanguages uOtherLangObj = new UserOtherLanguages()
                    {
                        LanguageId = uOtherLang,
                        UserId = appUser.Id
                    };
                    await _context.UserOtherLanguages.AddAsync(uOtherLangObj);
                }
            }

            //add user skills

            //Save User skills in DB
            foreach (var skill in userAdditionalSkills)
            {
                var uSkill = await _context.UserAdditionalSkills.FindAsync(skill.Id);
                if (uSkill == null)
                {
                    uSkill = new Models.UserAdditionalSkills()
                    {
                        Name = skill.Name,
                        Description = skill.Description,
                        UserId = appUser.Id
                    };

                    await _context.UserAdditionalSkills.AddAsync(uSkill);
                }
                else
                {
                    uSkill.Name = skill.Name;
                    uSkill.Description = skill.Description;

                    _context.Entry(uSkill).State = EntityState.Modified;
                }
            }
            //delete skills from db that are not received from UI
            var skillIds = userAdditionalSkills.Select(c => c.Id).ToList();
            var skillsTobeRemoved = _context.UserAdditionalSkills.Where(c => !creditIds.Contains(c.Id) && c.UserId == appUser.Id);
            foreach (var skill in skillsTobeRemoved)
            {
                _context.UserAdditionalSkills.Remove(skill);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(userProfile.Id))
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

        // POST: api/UserProfiles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfile userProfile)
        {
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserProfile", new { id = userProfile.Id }, userProfile);
        }

        // DELETE: api/UserProfiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserProfile>> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();

            return userProfile;
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfiles.Any(e => e.Id == id);
        }

        #region Photos Videos and Audios
        [HttpGet]
        public async Task<IActionResult> GetUserAlbums()
        {
            var user = await _userManager.GetUserAsync(User);
            var allUserAlbums = _context.Album.Where(a => a.UserId == user.Id && a.Type == "P");
            var albumsList = await allUserAlbums.Take(3).ToListAsync();
            var albumsCount = await allUserAlbums.LongCountAsync();

            var q = albumsList.Select(a => new
            {
                Id = a.Id.ToString(),
                RelativeTime = getRelativeDateTime(a.Created),
                a.Name,
                a.Type,
                a.User,
                a.UserId,
                FileIds = _context.MediaFiles.Where(t => t.AlbumId == a.Id).Take(5).Select(t => t.FileId).ToList(),
                FilesCount = _context.MediaFiles.Where(t => t.AlbumId == a.Id).Count() - 5,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                a.Created
            }).OrderByDescending(a => a.Created);
            return Ok(new { Records = q, Count = albumsCount });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPhotos(int? albumId)
        {
            var user = await _userManager.GetUserAsync(User);

            List<MediaFile> list = null;

            if (albumId == null)
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "P").ToListAsync();
            else
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "P" && a.AlbumId == albumId).ToListAsync();
            var q = list.Select(a => new
            {
                RelativeTime = getRelativeDateTime(a.CreateAt),
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
        public async Task<IActionResult> PostMediaFiles([FromBody] MediaFile[] mediaFiles)
        {
            try
            {
                if (mediaFiles != null && mediaFiles.Length > 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    foreach (var item in mediaFiles)
                    {
                        item.User = user;
                        item.Album = await _context.Album.FindAsync(item.AlbumId);
                    }
                    await _context.MediaFiles.AddRangeAsync(mediaFiles);
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

        [HttpGet("{id}/{width}/{height}")]
        public async Task<IActionResult> GetFile(string id, int width = 348, int height = 218)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

                if (file == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
                    return NotFound();
                }
                var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
                var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

                // The tus protocol does not specify any required metadata.
                // "filetype" is metadata that is specific to this domain and is not required.
                var type = metadata.ContainsKey("filetype")
                          ? metadata["filetype"].GetString(Encoding.UTF8)
                          : "application/octet-stream";
                using (Image imgPhoto = Image.Load(fileStream))
                {
                    //int sourceWidth = imgPhoto.Width;
                    //int sourceHeight = imgPhoto.Height;

                    ////if (sourceWidth < width || sourceHeight < height)
                    ////    return File(fileStream, type);

                    //int destX = 0, destY = 0;
                    //float nPercent = 0, nPercentW = 0, nPercentH = 0;

                    //nPercentW = ((float)width / (float)sourceWidth);
                    //nPercentH = ((float)height / (float)sourceHeight);
                    //if (nPercentH < nPercentW)
                    //{
                    //    nPercent = nPercentH;
                    //    destX = System.Convert.ToInt16((width -
                    //              (sourceWidth * nPercent)) / 2);
                    //}
                    //else
                    //{
                    //    nPercent = nPercentW;
                    //    destY = System.Convert.ToInt16((height -
                    //              (sourceHeight * nPercent)) / 2);
                    //}

                    //int destWidth = (int)(sourceWidth * nPercent);
                    //int destHeight = (int)(sourceHeight * nPercent);

                    ResizeOptions resizeOpt = new ResizeOptions()
                    {
                        Mode = ResizeMode.Pad,
                        Size = new Size() { Height = height, Width = width }
                    };

                    imgPhoto.Mutate(x => x
                         .Resize(resizeOpt)
                     );
                    MemoryStream ms = new MemoryStream();
                    await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                    fileStream.Close();
                    return File(ms.ToArray(), "image/png");
                }
                // }
            }
            return NotFound();
        }

        [HttpGet("{id}/{width}/{height}")]
        public async Task<IActionResult> GetFileThumbnail(string id, int width = 348, int height = 218)
        {
            Guid isItGuid = new Guid();
            if (!string.IsNullOrEmpty(id) && Guid.TryParseExact(id, "N", out isItGuid))
            {
                try
                {
                    var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                    var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

                    if (file == null)
                    {
                        HttpContext.Response.StatusCode = 404;
                        await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
                        return NotFound();
                    }
                    var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
                    var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

                    // The tus protocol does not specify any required metadata.
                    // "filetype" is metadata that is specific to this domain and is not required.
                    var type = metadata.ContainsKey("filetype")
                              ? metadata["filetype"].GetString(Encoding.UTF8)
                              : "application/octet-stream";
                    using (Image imgPhoto = Image.Load(fileStream))
                    {

                        ResizeOptions resizeOpt = new ResizeOptions()
                        {
                            // Mode = ResizeMode.Min,
                            Size = new Size() { Height = height, Width = width }
                        };
                        imgPhoto.Mutate(x => x
                             .Resize(resizeOpt)
                         );
                        MemoryStream ms = new MemoryStream();
                        await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                        fileStream.Close();
                        return File(ms.ToArray(), "image/png");
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                // }
            }
            return NotFound();
        } [HttpGet("{id}")]
        public async Task<IActionResult> GetFileThumbnail(string id)
        {
            Guid isItGuid = new Guid();
            if (!string.IsNullOrEmpty(id) && Guid.TryParseExact(id, "N", out isItGuid))
            {
                try
                {
                    var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                    var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

                    if (file == null)
                    {
                        HttpContext.Response.StatusCode = 404;
                        await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
                        return NotFound();
                    }
                    var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
                    var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

                    // The tus protocol does not specify any required metadata.
                    // "filetype" is metadata that is specific to this domain and is not required.
                    var type = metadata.ContainsKey("filetype")
                              ? metadata["filetype"].GetString(Encoding.UTF8)
                              : "application/octet-stream";
                    using (Image imgPhoto = Image.Load(fileStream))
                    {

                      
                        MemoryStream ms = new MemoryStream();
                        await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                        fileStream.Close();
                        return File(ms.ToArray(), "image/png");
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                // }
            }
            return NotFound();
        }


        [HttpDelete("{id}/{type}/{fileId}")]
        public async Task<IActionResult> DeleteFile(int id, string type, string fileId)
        {
            try
            {
                var filesList = await _context.MediaFiles.Where(a => a.FileId == fileId && a.Type == type).ToListAsync();
                var fileObj = await _context.MediaFiles.FindAsync(id);
                _context.MediaFiles.Remove(fileObj);
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

        [HttpDelete("{id}/{type}")]
        public async Task<IActionResult> DeleteAlbum(string id, string type)
        {
            try
            {
                var fileObjs = _context.MediaFiles.Where(a => a.AlbumId == int.Parse(id) && a.Type == type);
                _context.MediaFiles.RemoveRange(fileObjs);

                var albumObj = _context.Album.Find(int.Parse(id));

                _context.Album.Remove(albumObj);
                await _context.SaveChangesAsync();

                var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                foreach (var i in fileObjs)
                {
                    var file = await store.GetFileAsync(i.FileId, HttpContext.RequestAborted);
                    if (file == null)
                    {
                        continue;
                        //return NotFound();
                    }
                    await store.DeleteFileAsync(id, HttpContext.RequestAborted);
                }
                return Ok(new { success = true, Message = "Deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, Message = ex.Message });
            }
        }

        public string getRelativeDateTime(DateTime date)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum(AlbumViewModel albumVM)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var album = await _context.Album.FindAsync(string.IsNullOrEmpty(albumVM.albumId) ? 0 : int.Parse(albumVM.albumId));

                if (album == null)
                {
                    album = new Album()
                    {
                        User = user,
                        Name = albumVM.albumName,
                        Type = albumVM.type
                    };
                    await _context.Album.AddAsync(album);
                }
                else
                {
                    album.Name = albumVM.albumName;
                }
                await _context.SaveChangesAsync();
                return Ok(new { success = true, Message = "Album has been created successfully!", albumId = album.Id });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, Message = ex.Message + " --" + (ex.InnerException == null ? string.Empty : ex.InnerException.Message), albumId = string.Empty });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFirstFileInAlbum(int id)
        {
            var album = await _context.Album.FindAsync(id);
            if (album == null)
                return NotFound();
            var files = await _context.MediaFiles.Where(a => a.AlbumId == album.Id && a.Type == "P").OrderByDescending(a => a.CreatedAtTicks).FirstOrDefaultAsync();
            if (files == null)
                return NotFound();
            var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
            var file = await store.GetFileAsync(files.FileId, HttpContext.RequestAborted);

            if (file == null)
            {
                HttpContext.Response.StatusCode = 404;
                await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
                return NotFound();
            }
            var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
            var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

            // The tus protocol does not specify any required metadata.
            // "filetype" is metadata that is specific to this domain and is not required.
            var type = metadata.ContainsKey("filetype")
                      ? metadata["filetype"].GetString(Encoding.UTF8)
                      : "application/octet-stream";

            using (Image imgPhoto = Image.Load(fileStream))
            {
                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;
                var width = 348;
                var height = 218;

                if (sourceWidth < width || sourceHeight < height)
                    return File(fileStream, type);

                int destX = 0, destY = 0;
                float nPercent = 0, nPercentW = 0, nPercentH = 0;

                nPercentW = ((float)width / (float)sourceWidth);
                nPercentH = ((float)height / (float)sourceHeight);
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                    destX = System.Convert.ToInt16((width -
                              (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = System.Convert.ToInt16((height -
                              (sourceHeight * nPercent)) / 2);
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                imgPhoto.Mutate(x => x
                     .Resize(destWidth, destHeight)
                 );
                MemoryStream ms = new MemoryStream();
                await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                fileStream.Close();
                return File(ms.ToArray(), "image/png");
            }
        }

        [HttpGet("{id}/{flag}/{type}")]
        public async Task<IActionResult> NextPrevFileId(string id, string flag, string type)
        {
            var currentFileObj = await _context.MediaFiles.Where(a => a.FileId.Equals(id) && a.Type == type).FirstOrDefaultAsync();
            var currentFileUploadTime = currentFileObj.CreatedAtTicks;
            var user = await _userManager.GetUserAsync(User);
            MediaFile nextPrevFileId = null;

            if (flag.Equals("P"))
            {
                var nextFileObj = await _context.MediaFiles.Where(a => a.UserId.Equals(user.Id) && a.Type == type && a.CreatedAtTicks > currentFileUploadTime).OrderBy(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.MediaFiles.Where(a => a.UserId.Equals(user.Id) && a.Type == type).OrderBy(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }
            else
            {
                var nextFileObj = await _context.MediaFiles.Where(a => a.UserId.Equals(user.Id) && a.Type == type && a.CreatedAtTicks < currentFileUploadTime).OrderByDescending(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.MediaFiles.Where(a => a.UserId.Equals(user.Id) && a.Type == type).OrderByDescending(a => a.CreatedAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }

            return Ok(new { Record = nextPrevFileId });
        }

        #region videos related code

        [HttpGet]
        public async Task<IActionResult> GetVideoAlbums()
        {
            var user = await _userManager.GetUserAsync(User);
            var allUserAlbums = _context.Album.Where(a => a.UserId == user.Id && a.Type == "V");
            var albumsList = await allUserAlbums.Take(3).ToListAsync();
            var albumsCount = await allUserAlbums.LongCountAsync();
            var q = albumsList.Select(a => new
            {
                Id = a.Id.ToString(),
                RelativeTime = getRelativeDateTime(a.Created),
                a.Name,
                a.Type,
                a.User,
                a.UserId,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                a.Created
            }).OrderByDescending(a => a.Created);
            return Ok(new { Records = q, Count = albumsCount });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserVideos(int? albumId)
        {
            var user = await _userManager.GetUserAsync(User);

            List<MediaFile> list = null;

            if (albumId == null)
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "V").ToListAsync();
            else
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "V" && a.AlbumId == albumId).ToListAsync();
            var q = list.Select(a => new
            {
                RelativeTime = getRelativeDateTime(a.CreateAt),
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetVideoFile(string id)
        {
            Guid isItGuid = new Guid();
            if (!string.IsNullOrEmpty(id) && Guid.TryParseExact(id, "N", out isItGuid))
            {
                var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

                if (file == null)
                {
                    return NotFound();
                }
                var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
                var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

                // The tus protocol does not specify any required metadata.
                // "filetype" is metadata that is specific to this domain and is not required.
                var type = metadata.ContainsKey("filetype")
                          ? metadata["filetype"].GetString(Encoding.UTF8)
                          : "application/octet-stream";
                return File(fileStream, type, enableRangeProcessing: true);
            }
            return NotFound();
        }

        #endregion

        #region audios related code

        [HttpGet]
        public async Task<IActionResult> GetAudioAlbums()
        {
            var user = await _userManager.GetUserAsync(User);
            var allUserAlbums = _context.Album.Where(a => a.UserId == user.Id && a.Type == "A");
            var albumsList = await allUserAlbums.Take(3).ToListAsync();
            var albumsCount = await allUserAlbums.LongCountAsync();
            var q = albumsList.Select(a => new
            {
                Id = a.Id.ToString(),
                RelativeTime = getRelativeDateTime(a.Created),
                a.Name,
                a.Type,
                a.User,
                a.UserId,
                Favorite = a.Favorite == null ? false : a.Favorite,
                AccessRight = a.AccessRight == null ? false : a.AccessRight,
                a.Created
            }).OrderByDescending(a => a.Created);
            return Ok(new { Records = q, Count = albumsCount });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAudios(int? albumId)
        {
            var user = await _userManager.GetUserAsync(User);

            List<MediaFile> list = null;

            if (albumId == null)
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "A").ToListAsync();
            else
                list = await _context.MediaFiles.Where(a => a.UserId == user.Id && a.Type == "A" && a.AlbumId == albumId).ToListAsync();
            var q = list.Select(a => new
            {
                RelativeTime = getRelativeDateTime(a.CreateAt),
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAudioFile(string id)
        {
            Guid isItGuid = new Guid();
            if (!string.IsNullOrEmpty(id) && Guid.TryParseExact(id, "N", out isItGuid))
            {
                var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

                if (file == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
                    return null;
                }
                var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
                var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

                // The tus protocol does not specify any required metadata.
                // "filetype" is metadata that is specific to this domain and is not required.
                var type = metadata.ContainsKey("filetype")
                          ? metadata["filetype"].GetString(Encoding.UTF8)
                          : "application/octet-stream";
                return File(fileStream, type, enableRangeProcessing: true);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> MediaFavorite(MediaFile obj)
        {
            obj.Favorite = !obj.Favorite;
            _context.Entry(obj).State = EntityState.Modified;
            var user = await _userManager.GetUserAsync(User);
            await _context.SaveChangesAsync();
            var q = new
            {
                RelativeTime = getRelativeDateTime(obj.CreateAt),
                obj.ContentType,
                obj.FileId,
                obj.Name,
                obj.Size,
                obj.Id,
                obj.AlbumId,
                obj.User,
                obj.Type,
                obj.UserId,
                obj.Album,
                Favorite = obj.Favorite == null ? false : obj.Favorite,
                AccessRight = obj.AccessRight == null ? false : obj.AccessRight,
                UserName = user.FirstName + " " + user.LastName,
                obj.CreateAt
            };
            return Ok(q);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeMediaPrivacy(MediaFile obj)
        {
            obj.AccessRight = !obj.AccessRight;
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var user = await _userManager.GetUserAsync(User);
            var q = new
            {
                RelativeTime = getRelativeDateTime(obj.CreateAt),
                obj.ContentType,
                obj.FileId,
                obj.Name,
                obj.Size,
                obj.Id,
                obj.AlbumId,
                obj.User,
                obj.Type,
                obj.UserId,
                obj.Album,
                Favorite = obj.Favorite == null ? false : obj.Favorite,
                AccessRight = obj.AccessRight == null ? false : obj.AccessRight,
                UserName = user.FirstName + " " + user.LastName,
                obj.CreateAt
            };
            return Ok(q);
        }

        [HttpPost]
        public async Task<ActionResult> AlbumFavorite(Album obj)
        {
            obj.Favorite = !obj.Favorite;
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var q = new
            {
                Id = obj.Id.ToString(),
                RelativeTime = getRelativeDateTime(obj.Created),
                obj.Name,
                obj.Type,
                obj.User,
                obj.UserId,
                Favorite = obj.Favorite == null ? false : obj.Favorite,
                AccessRight = obj.AccessRight == null ? false : obj.AccessRight,
                obj.Created
            };
            return Ok(q);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeAlbumPrivacy(Album obj)
        {
            obj.AccessRight = !obj.AccessRight;

            var mediaFiles = _context.MediaFiles.Where(m => m.AlbumId == obj.Id);

            foreach (MediaFile media in mediaFiles)
            {
                media.AccessRight = obj.AccessRight;
            }

            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var q = new
            {
                Id = obj.Id.ToString(),
                RelativeTime = getRelativeDateTime(obj.Created),
                obj.Name,
                obj.Type,
                obj.User,
                obj.UserId,
                FileIds = _context.MediaFiles.Where(t => t.AlbumId == obj.Id).Take(5).Select(t => t.FileId).ToList(),
                FilesCount = _context.MediaFiles.Where(t => t.AlbumId == obj.Id).Count() - 5,
                Favorite = obj.Favorite == null ? false : obj.Favorite,
                AccessRight = obj.AccessRight == null ? false : obj.AccessRight,
                obj.Created
            };
            return Ok(q);
        }

        [HttpGet("{albumId}/{fileId}")]
        public async Task<ActionResult> MoveFileInAlbum(int albumId, int fileId)
        {
            var fileObj = await _context.MediaFiles.FindAsync(fileId);
            fileObj.Album = await _context.Album.FindAsync(albumId);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, Message = string.Format("{0} has been moved to {1}", fileObj.Name, fileObj.Album.Name) });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, ex.Message });
            }
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetVideoThumbnail(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
        //        var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

        //        if (file == null)
        //        {
        //            var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
        //            using (var videoThumbnailer = new VideoThumbnailer(fileStream))
        //            //Generate a meaningful thumbnail of the video and
        //            //get a System.Drawing.Bitmap with 100x100 maximum size.
        //            //You are responsible for disposing the bitmap when you are finished with it.
        //            //So it's good practice to have a "using" statement for the retrieved bitmap.
        //            using (var thumbnail = videoThumbnailer.GenerateThumbnail(100))
        //            {
        //                //Reference System.Drawing and use System.Drawing.Imaging namespace for the following line.
        //                MemoryStream ms = new MemoryStream();
        //                thumbnail.Save(ms, ImageFormat.Jpeg);
        //                fileStream.Close();
        //                return File(ms.ToArray(), "image/jpg");
        //            }
        //        }
        //    }
        //    return null;
        //}

        #endregion
        #endregion

        #region Photo related requests here

        [HttpGet("{id}/{width}/{height}")]
        public async Task<IActionResult> GetUserPhoto(string id, int width = 348, int height = 218)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var file = await _context.UserProfiles
                        .Where(u => u.UserId == id)
                        .Select(u => u.Photo)
                        .FirstOrDefaultAsync();

                    if (file == null)
                    {
                        string webRootPath = _env.WebRootPath;
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
                }
                catch (Exception ex)
                {
                    string webRootPath = _env.WebRootPath;
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
                // }
            }
            return NotFound();
        }

        [HttpGet("{id}/{width}/{height}")]
        public async Task<IActionResult> GetUserCompanyPhoto(string id, int width = 348, int height = 218)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var file = await _context.UserProfiles
                    .Where(u => u.UserId == id)
                    .Select(u => u.CompanyLogo)
                    .FirstOrDefaultAsync();

                if (file == null)
                {
                    string webRootPath = _env.WebRootPath;
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

        [HttpGet]
        public async Task<IActionResult> GetLoggedInUserPhoto()
        {
            var user = await _userManager.GetUserAsync(User);
            var userProfile = await _context.UserProfiles.Where(a => a.UserId == user.Id).FirstOrDefaultAsync();
            if (userProfile?.Photo == null)
            {
                string webRootPath = _env.WebRootPath;
                string path = Path.Combine(webRootPath, "img", "no-image.png");
                using (Image imgPhoto = Image.Load(path))
                {
                    ResizeOptions options = new ResizeOptions()
                    {
                        Size = new Size() { Height = 100, Width = 100 },
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

            using (Image imgPhoto = Image.Load(userProfile.Photo))
            {
                imgPhoto.Mutate(x => x
                     .Resize(100, 100)
                 );
                MemoryStream ms = new MemoryStream();
                await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                return File(ms.ToArray(), "image/png");
            }
        }

        [HttpGet("{width}/{height}")]
        public async Task<IActionResult> GetLoggedInUserThumbnail(int width = 45, int height = 45)
        {
            var user = await _userManager.GetUserAsync(User);
            var userProfile = await _context.UserProfiles.Where(a => a.UserId == user.Id).FirstOrDefaultAsync();
            if (userProfile?.Photo == null)
            {
                string webRootPath = _env.WebRootPath;
                string path = Path.Combine(webRootPath, "img", "no-image.png");
                using (Image imgPhoto = Image.Load(path))
                {
                    ResizeOptions options = new ResizeOptions()
                    {
                        Size = new Size() { Height = width, Width = height },
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

            using (Image imgPhoto = Image.Load(userProfile.Photo))
            {
                imgPhoto.Mutate(x => x
                     .Resize(width, height)
                 );
                MemoryStream ms = new MemoryStream();
                await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                return File(ms.ToArray(), "image/png");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> DownloadFile(string id)
        {
            Guid isItGuid = new Guid();
            if (!string.IsNullOrEmpty(id) && Guid.TryParseExact(id, "N", out isItGuid))
            {
                var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

                if (file == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
                    return null;
                }
                var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
                var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

                // The tus protocol does not specify any required metadata.
                // "filetype" is metadata that is specific to this domain and is not required.
                var type = metadata.ContainsKey("filetype")
                          ? metadata["filetype"].GetString(Encoding.UTF8)
                          : "application/octet-stream";
                return File(fileStream, type);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<string> GetCompanyName()
        {
            var user = await _userManager.GetUserAsync(User);
            var userProfileObj = await _context.UserProfiles.Where(a => a.UserId == user.Id).FirstOrDefaultAsync();

            return (string.IsNullOrEmpty(userProfileObj?.CompanyName) ? "Production" : userProfileObj?.CompanyName);
        }

        [HttpPost]
        public async Task<ActionResult> UploadBGImage([FromForm] BackgroundThingsDto fd)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                if (fd.ProjectId == 0)
                {
                    var userProfile = await _context.UserProfiles.Where(a => a.UserId == user.Id).FirstOrDefaultAsync();

                    if (fd.file != null)
                        using (var fileStream = fd.file.OpenReadStream())
                        {
                            byte[] bytes = new byte[fd.file.Length];
                            fileStream.Read(bytes, 0, (int)fd.file.Length);
                            userProfile.BackgroundImage = bytes;
                        }
                    userProfile.BgColor = fd.BackgroundColor;
                    userProfile.BgOpacity = double.Parse(fd.BackgroundOpacity);
                    userProfile.GlassMode = fd.GlassMode;
                    userProfile.CinematicMode = fd.CinematicMode;
                    userProfile.DarkMode = fd.DarkMode;
                }
                else
                {
                    var project = await _context.Project.FindAsync(fd.ProjectId);
                    if (fd.file != null)
                        using (var fileStream = fd.file.OpenReadStream())
                        {
                            byte[] bytes = new byte[fd.file.Length];
                            fileStream.Read(bytes, 0, (int)fd.file.Length);
                            project.BackgroundImage = bytes;
                        }
                    project.BgColor = fd.BackgroundColor;
                    project.BgOpacity = double.Parse(fd.BackgroundOpacity);
                    project.GlassMode = fd.GlassMode;
                    project.CinematicMode = fd.CinematicMode;
                    project.DarkMode = fd.DarkMode;
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpGet("{flag}/{projectId}/{guid}")]
        public async Task<ActionResult> GetBGImage(string flag, int projectId, string guid)
        {
            var user = await _userManager.GetUserAsync(User);
            var userProfile = await _context.UserProfiles.Where(a => a.UserId == user.Id).FirstOrDefaultAsync();
            if (flag == "U")
            {
                if (userProfile.BackgroundImage != null)
                    return new FileContentResult(userProfile.BackgroundImage, MimeTypeMap.GetMimeType("bilal.png"));
            }
            else
            {
                var project = await _context.Project.FindAsync(projectId);//BackgroundImage

                if (project.BackgroundImage != null)
                    return new FileContentResult(project.BackgroundImage, MimeTypeMap.GetMimeType("bilal.png"));
            }

            return NotFound();
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> GetBgColorOpacity(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == 0)
            {
                var userProfile = await _context.UserProfiles.Where(a => a.UserId == user.Id).FirstOrDefaultAsync();
                return Ok(new
                {
                    BackgroundImage = $"url(api/UserProfiles/GetBGImage/U/{id}/{Guid.NewGuid()})",
                    BackgroundOpacity = userProfile.BgOpacity ?? 1,
                    BackgroundColor = userProfile.BgColor ?? "#FFFFFF",
                    ProjectId = id,
                    GlassMode = userProfile.GlassMode,
                    CinematicMode = userProfile.CinematicMode,
                    DarkMode = userProfile.DarkMode,
                });
            }
            else
            {
                var project = await _context.Project.FindAsync(id);
                return Ok(new
                {
                    BackgroundImage = $"url(api/UserProfiles/GetBGImage/P/{id}/{Guid.NewGuid()})",
                    BackgroundOpacity = project.BgOpacity ?? 1,
                    BackgroundColor = project.BgColor ?? "#FFFFFF",
                    ProjectId = id,
                    GlassMode = project.GlassMode,
                    CinematicMode = project.CinematicMode,
                    DarkMode = project.DarkMode
                });
            }
        }

        #endregion
    }
}
