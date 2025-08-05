using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.Models.ProjectCrews;
using Elegium.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectExternalUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProjectExternalUsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        #region for external user detail
        //[HttpGet("{id}/{width}/{height}")]
        //public async Task<IActionResult> GetFileThumbnail(string id, int width = 348, int height = 218)
        //{
        //    Guid isItGuid = new Guid();
        //    if (!string.IsNullOrEmpty(id) && Guid.TryParseExact(id, "N", out isItGuid))
        //    {
        //        try
        //        {
        //            var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
        //            var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

        //            if (file == null)
        //            {
        //                HttpContext.Response.StatusCode = 404;
        //                await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
        //                return NotFound();
        //            }
        //            var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
        //            var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

        //            // The tus protocol does not specify any required metadata.
        //            // "filetype" is metadata that is specific to this domain and is not required.
        //            var type = metadata.ContainsKey("filetype")
        //                      ? metadata["filetype"].GetString(Encoding.UTF8)
        //                      : "application/octet-stream";
        //            using (Image imgPhoto = Image.Load(fileStream))
        //            {

        //                ResizeOptions resizeOpt = new ResizeOptions()
        //                {
        //                    // Mode = ResizeMode.Min,
        //                    Size = new Size() { Height = height, Width = width }
        //                };
        //                imgPhoto.Mutate(x => x
        //                     .Resize(resizeOpt)
        //                 );
        //                MemoryStream ms = new MemoryStream();
        //                await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
        //                fileStream.Close();
        //                return File(ms.ToArray(), "image/png");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return NotFound();
        //        }
        //        // }
        //    }
        //    return NotFound();
        //}
        [HttpGet]
        public async Task<IActionResult> GetExternalUserById([FromQuery] int userId)
        {
            //  var crewUser = await _context.ProjectExternalUsers.Where(p => p.Id == userId).Include(a=>a.Department).Include(a=>a.Position).FirstOrDefault()
            //   ;


            var usersList = await _context.ProjectExternalUsers.Where(p => p.Id == userId)
             .Include(a => a.Department).Include(a => a.Position)
             .ToListAsync();


            return Ok(usersList.Count > 0 ? usersList[0] : null);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateExternalUser([FromBody] ProjectExternalUser user)
        {
            //var userInDb = await _context.ProjectExternalUsers.Where(u => u.Id == user.Id).FirstOrDefaultAsync();


            // if (userInDb == null)
            //   return BadRequest("User not found");

            try
            {
                _context.ProjectExternalUsers.Update(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetExternalUserContacts([FromQuery] int userId)
        {
            //  var crewUser = await _context.ProjectExternalUsers.Where(p => p.Id == userId).Include(a=>a.Department).Include(a=>a.Position).FirstOrDefault()
            //   ;


            var usersList = await _context.ExternalUserContact.Where(p => p.ExternalUserId == userId)
             .ToListAsync();


            return Ok(usersList);

        }
        [HttpPost]
        public async Task<IActionResult> CreateExternalUserContact([FromBody] ExternalUserContactDto user)
        {
            ExternalUserContact obj = new ExternalUserContact();
            obj.Email = user.Email;
            obj.PhoneHome = user.PhoneHome;
            obj.PhoneMobile = user.PhoneMobile;
            obj.PhoneOffice = user.PhoneOffice;
            obj.Fax = user.Fax;
            obj.Position = user.Position;
            obj.ExternalUserId = user.ExternalUserId;
            obj.Name = user.Name;
            

            // if (userInDb == null)
            //   return BadRequest("User not found");

            try
            {
                _context.ExternalUserContact.Add(obj);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateExternalUserContact([FromBody] ExternalUserContactDto user)
        {
            //var userInDb = await _context.ProjectExternalUsers.Where(u => u.Id == user.Id).FirstOrDefaultAsync();


            // if (userInDb == null)
            //   return BadRequest("User not found");


            var userInDb = await _context.ExternalUserContact.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            userInDb.Email = user.Email;
            userInDb.PhoneHome = user.PhoneHome;
            userInDb.PhoneMobile = user.PhoneMobile;
            userInDb.PhoneOffice = user.PhoneOffice;
            userInDb.Fax = user.Fax;
            userInDb.Position = user.Position;
            userInDb.Name = user.Name;

            try
            {
                _context.ExternalUserContact.Update(userInDb);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExternalUserContact(int id)
        {
            var externalUser = await _context.ExternalUserContact.FindAsync(id);
            if (externalUser == null)
            {
                return NotFound();
            }

            _context.ExternalUserContact.Remove(externalUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

       
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetProjectExternalUsers([FromQuery] int ProjectId)
        {


            var usersList = await (from user in _context.ProjectExternalUsers
                                   .Include(p => p.Position)
                                   .Where(p => p.ProjectId == ProjectId)
                                   select new
                                   {
                                       user.Id,
                                       user.Name,
                                       user.Email,
                                       user.PhoneHome,
                                       user.PhoneMobile,
                                       user.PhoneOffice,
                                       user.Position,
                                       defaultImageId = _context.ExternalUserFile.Where(a => a.Default && a.ExternalUserId == user.Id).FirstOrDefault()
                                   })
                   .ToListAsync();
            
            return Ok(usersList);
        }


        [HttpPost]
        public async Task<IActionResult> SaveorUpdateExternalUser([FromBody] ProjectExternalUser externalUser)
        {
            //externalUser.ProjectId = 4;
            _context.ProjectExternalUsers.Add(externalUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExternalUser(int id)
        {
            var externalUser = await _context.ProjectExternalUsers.FindAsync(id);
            if (externalUser == null)
            {
                return NotFound();
            }
            var userGroup = _context.ExternalUserGroups.Where(e => e.ExternalUserId == externalUser.Id).FirstOrDefault();
            if(userGroup != null)
                _context.ExternalUserGroups.Remove(userGroup);

            var userUnit = _context.ExternalUserUnits.Where(e => e.ExternalUserId == externalUser.Id).FirstOrDefault();
            if(userUnit != null)
                _context.ExternalUserUnits.Remove(userUnit);

            _context.ProjectExternalUsers.Remove(externalUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
