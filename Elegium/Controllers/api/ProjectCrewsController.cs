using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.Models.ProjectCrews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.IO;
using Elegium.ExtensionMethods;
using Elegium.Dtos;

namespace Elegium.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectCrewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectCrewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region for User Detail
        [HttpGet]
        public async Task<IActionResult> GetProjectCrewsById([FromQuery] int crewId)
        {
            var crewUser = _context.ProjectCrews.Where(p => p.Id == crewId)
               .Include(a => a.User)
               .FirstOrDefault();

            var userProfile = await _context.UserProfiles
                .Where(u => u.UserId == crewUser.UserId)
                .Select(u => new UserProfileDto()
                {
                    Id = u.Id,
                    ProfileImageAvailable = u.Photo != null ? true : false,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    StreetAddress = u.StreetAddress,
                    Country = u.Country,
                    CountryId = u.CountryId,
                    City = u.City,
                    CityId = u.CityId,
                    PostCode = u.PostCode,
                    CompanyName = u.CompanyName,
                    CompanyStreetAddress = u.CompanyStreetAddress,
                    CompanyCountry = u.CompanyCountry,
                    CompanyCountryId = u.CompanyCountryId,
                    CompanyCity = u.CompanyCity,
                    CompanyCityId = u.CompanyCityId,
                    CompanyPostCode = u.CompanyPostCode
                })
                .FirstOrDefaultAsync();

            List<ProjectCrewListModel> viewModel = new List<ProjectCrewListModel>();
            List<object> data = new List<object>();

            ProjectCrewListModel vm = new ProjectCrewListModel()
            {
                Crew = crewUser,
                CrewUserProfile = userProfile,
                FirstName = crewUser.User.FirstName,
                LastName = crewUser.User.LastName,
                Name = crewUser.User.FirstName + " " + crewUser.User.LastName,
                CrewPostions = await _context.ProjectCrewPositions.Where(a => a.ProjectCrewId == crewUser.Id).Include(a => a.Position).Select(a => a.Position.Name).ToListAsync()


            };

            // rt.position = _context.ProjectCrewPositions.Where(a=>a.ProjectCrewId== user.Id).ToListAsync();

            return Ok(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDto profile)
        {
            var userInDb = await _context.UserProfiles.Where(u => u.Id == profile.Id).FirstOrDefaultAsync();


            if (userInDb == null)
                return BadRequest("User not found");

            userInDb.FirstName = profile.FirstName;
            userInDb.MiddleName = profile.MiddleName;
            userInDb.LastName = profile.LastName;
            userInDb.StreetAddress = profile.StreetAddress;
            userInDb.CountryId = profile.CountryId;
            userInDb.CityId = profile.CityId;
            userInDb.PostCode = profile.PostCode;
            userInDb.CompanyName = profile.CompanyName;
            userInDb.CompanyStreetAddress = profile.CompanyStreetAddress;
            userInDb.CompanyCountryId = profile.CompanyCountryId;
            userInDb.CompanyCityId = profile.CompanyCityId;
            userInDb.CompanyPostCode = profile.CompanyPostCode;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCrewPosition([FromBody] ProjectCrewModel projectCrewModel)
        {
            // int[] crewPositions = null;
            //int crewId = 1;
            try
            {
                var listPosition = await _context.ProjectCrewPositions.Where(a => a.ProjectCrewId == projectCrewModel.ProjectCrew.Id).ToListAsync();
                if (listPosition != null)
                {
                    foreach (var item in listPosition)
                    {
                        _context.ProjectCrewPositions.Remove(item);
                    }

                }



                foreach (var item in projectCrewModel.ProjectCrewPositions)
                {
                    _context.ProjectCrewPositions.Add(new ProjectCrewPosition() { ProjectCrewId = projectCrewModel.ProjectCrew.Id, PositionId = item });
                }


                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<DraftContractFile>>> PostDraftContracts(List<DraftContractFile> documentFiles)
        {
            foreach (var d in documentFiles)
            {
                d.UserFriendlySize = d.Size.GetBytesReadable();
                d.MimeType = MimeMapping.MimeUtility.GetMimeMapping(d.Name);
                FileInfo fi = new FileInfo(d.Name);
                d.Type = fi.Extension.GetFileType();
                d.Extension = fi.Extension;
            }
            await _context.DraftContractFiles.AddRangeAsync(documentFiles);
            await _context.SaveChangesAsync();
            return Ok(documentFiles);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<DraftContractFile>> DeleteDraftContracts(int id)
        {
            var documentFiles = await _context.DraftContractFiles.FindAsync(id);
            if (documentFiles == null)
            {
                return NotFound();
            }

            _context.DraftContractFiles.Remove(documentFiles);
            await _context.SaveChangesAsync();

            return documentFiles;
        }

        // GET: api/DocumentFiles
        [HttpGet("{crewId}/{page}/{size}")]
        public async Task<ActionResult> GetDraftContractsFiles(int crewId, int page, int size)
        {
            var dbList = await (from d in _context.DraftContractFiles.Where(a => a.ProjectCrewId == crewId)
                                select new
                                {
                                    d.ContentType,
                                    d.Extension,
                                    d.FileId,
                                    d.Id,
                                    d.MimeType,
                                    d.Name,
                                    d.Type,
                                    d.UserFriendlySize,
                                    RelativeTime = d.Created.GetRelativeTime(),
                                    UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                    d.Default
                                }).GetPaged(page, size);



            return Ok(dbList);
        }


        [HttpPost]
        public async Task<ActionResult<IEnumerable<DraftContractFile>>> PostContractDocuments(List<ContractDocument> documentFiles)
        {
            foreach (var d in documentFiles)
            {
                d.UserFriendlySize = d.Size.GetBytesReadable();
                d.MimeType = MimeMapping.MimeUtility.GetMimeMapping(d.Name);
                FileInfo fi = new FileInfo(d.Name);
                d.Type = fi.Extension.GetFileType();
                d.Extension = fi.Extension;
            }
            await _context.ContractDocuments.AddRangeAsync(documentFiles);
            await _context.SaveChangesAsync();
            return Ok(documentFiles);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<ContractDocument>> DeleteContractDocument(int id)
        {
            var documentFiles = await _context.ContractDocuments.FindAsync(id);
            if (documentFiles == null)
            {
                return NotFound();
            }

            _context.ContractDocuments.Remove(documentFiles);
            await _context.SaveChangesAsync();

            return documentFiles;
        }

        // GET: api/DocumentFiles
        [HttpGet("{crewId}/{page}/{size}")]
        public async Task<ActionResult> GetContractDocuments(int crewId, int page, int size)
        {
            var dbList = await (from d in _context.ContractDocuments.Where(a => a.ProjectCrewId == crewId)
                                select new
                                {
                                    d.ContentType,
                                    d.Extension,
                                    d.FileId,
                                    d.Id,
                                    d.MimeType,
                                    d.Name,
                                    d.Type,
                                    d.UserFriendlySize,
                                    RelativeTime = d.Created.GetRelativeTime(),
                                    UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                    d.Default
                                }).GetPaged(page, size);



            return Ok(dbList);
        }

        //[HttpPost]
        //public async Task<IActionResult> UpdateCrewPosition([FromBody] int crewId, int[] crewPositions)
        //{


        //    try
        //    {
        //        var listPosition = await _context.ProjectCrewPositions.Where(a => a.ProjectCrewId == crewId).ToListAsync();
        //        if (listPosition != null)
        //        {
        //            foreach (var item in listPosition)
        //            {
        //                _context.ProjectCrewPositions.Remove(item);
        //            }

        //        }



        //        foreach (var item in crewPositions)
        //        {
        //            _context.ProjectCrewPositions.Add(new ProjectCrewPosition() { ProjectCrewId = crewId, PositionId = item });
        //        }


        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {

        //        throw;

        //    }
        //    return Ok();
        //}
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetProjectCrews([FromQuery] int ProjectId)
        {

            var usersList = await _context.ProjectCrews.Where(p => p.ProjectId == ProjectId && p.IsActive)
               .Include(a => a.User)
               .ToListAsync();

            List<ProjectCrewListModel> viewModel = new List<ProjectCrewListModel>();
            List<object> data = new List<object>();
            foreach (ProjectCrew user in usersList)
            {
                ProjectCrewListModel vm = new ProjectCrewListModel()
                {
                    Crew = user,
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    Name = user.User.FirstName + " " + user.User.LastName,
                    CrewPostions = await _context.ProjectCrewPositions.Where(a => a.ProjectCrewId == user.Id).Include(a => a.Position).Select(a => a.Position.Name).ToListAsync(),
                    CrewUserProfile = await _context.UserProfiles
                                        .Where(u => u.UserId == user.UserId)
                                        .Select(u => new UserProfileDto()
                                        {
                                            Id = u.Id,
                                            ProfileImageAvailable = u.Photo != null ? true : false,
                                            FirstName = u.FirstName,
                                            MiddleName = u.MiddleName,
                                            LastName = u.LastName,
                                            StreetAddress = u.StreetAddress,
                                            Country = u.Country,
                                            CountryId = u.CountryId,
                                            City = u.City,
                                            CityId = u.CityId,
                                            PostCode = u.PostCode,
                                            CompanyName = u.CompanyName,
                                            CompanyStreetAddress = u.CompanyStreetAddress,
                                            CompanyCountry = u.CompanyCountry,
                                            CompanyCountryId = u.CompanyCountryId,
                                            CompanyCity = u.CompanyCity,
                                            CompanyCityId = u.CompanyCityId,
                                            CompanyPostCode = u.CompanyPostCode
                                        })
                                        .FirstOrDefaultAsync()

                };
                viewModel.Add(vm);
            }

            // rt.position = _context.ProjectCrewPositions.Where(a=>a.ProjectCrewId== user.Id).ToListAsync();

            return Ok(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjectCrews([FromQuery] int ProjectId)
        {

            var usersList = await _context.ProjectCrews.Where(p => p.ProjectId == ProjectId && p.IsActive)
               .Include(a => a.User)
               .ToListAsync();
            var externaUserList = await _context.ProjectExternalUsers.Where(p => p.ProjectId == ProjectId)
            .Include(a => a.Position)
             .ToListAsync();

            List<ProjectCrewListModel> viewModel = new List<ProjectCrewListModel>();
            List<object> data = new List<object>();
            foreach (ProjectCrew user in usersList)
            {
                //var userGroup = _context.ProjectCrewGroups.Where(a => a.ProjectCrewId == user.Id && a.Group.ProjectId == ProjectId).
                //    Include(a => a.Group).FirstOrDefault();
                var userGroup = _context.ProjectCrewGroups
                    .Where(a => a.ProjectCrewId == user.Id
                       && a.Group.ProjectId == ProjectId)
                    .Include(a => a.Group).FirstOrDefault();

                if (userGroup == null || userGroup.Group.IsInCrewList)
                {
                    ProjectCrewListModel vm = new ProjectCrewListModel()
                    {
                        Crew = user,
                        FirstName = user.User.FirstName,
                        LastName = user.User.LastName,
                        Name = user.User.FirstName + " " + user.User.LastName,
                        CrewPostions = await _context.ProjectCrewPositions.Where(a => a.ProjectCrewId == user.Id).Include(a => a.Position).Select(a => a.Position.Name).ToListAsync(),
                        CrewUserProfile = await _context.UserProfiles
                                        .Where(u => u.UserId == user.UserId)
                                        .Select(u => new UserProfileDto()
                                        {
                                            Id = u.Id,
                                            ProfileImageAvailable = u.Photo != null ? true : false,
                                            FirstName = u.FirstName,
                                            MiddleName = u.MiddleName,
                                            LastName = u.LastName,
                                            StreetAddress = u.StreetAddress,
                                            Country = u.Country,
                                            CountryId = u.CountryId,
                                            City = u.City,
                                            CityId = u.CityId,
                                            PostCode = u.PostCode,
                                            CompanyName = u.CompanyName,
                                            CompanyStreetAddress = u.CompanyStreetAddress,
                                            CompanyCountry = u.CompanyCountry,
                                            CompanyCountryId = u.CompanyCountryId,
                                            CompanyCity = u.CompanyCity,
                                            CompanyCityId = u.CompanyCityId,
                                            CompanyPostCode = u.CompanyPostCode
                                        })
                                        .FirstOrDefaultAsync(),
                        GroupName = userGroup == null ? "" : userGroup.Group.Name,
                        IsExternalUser = false

                    };
                    viewModel.Add(vm);
                }
            }
            foreach (ProjectExternalUser externalUser in externaUserList)
            {
                var exterUserGroup = _context.ExternalUserGroups
                    .Where(a => a.ExternalUserId == externalUser.Id 
                    && a.Group.ProjectId == ProjectId)
                    .Include(a => a.Group).FirstOrDefault();
                if(exterUserGroup == null || exterUserGroup.Group.IsInCrewList )
                {
                    ProjectCrewListModel vm = new ProjectCrewListModel()
                    {
                        externalUser = externalUser,
                        FirstName = externalUser.Name,
                        LastName = "",
                        Name = externalUser.Name,
                        PositionName = externalUser.Position?.Name,
                        CrewUserProfile = null,
                        GroupName = exterUserGroup == null ? "" : exterUserGroup.Group.Name,
                        IsExternalUser = true,
                        defaultImageId = _context.ExternalUserFile.Where(a => a.Default && a.ExternalUserId == externalUser.Id).FirstOrDefault()

                    };
                    viewModel.Add(vm);
                }
               

                // rt.position = _context.ProjectCrewPositions.Where(a=>a.ProjectCrewId== user.Id).ToListAsync();
                
            }


            return Ok(viewModel.OrderBy(a=>a.GroupName));

        }

        [HttpPost]
        public async Task<IActionResult> UpdateProjectCrew([FromBody] ProjectCrew projectCrew)
        {


            try
            {
                _context.ProjectCrews.Update(projectCrew);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateProjectCrew([FromBody] ProjectCrewModel projectCrewModel)
        {
            //Add User
            //  _context.UserProfiles.Add(projectCrewModel.UserProfile);
            try
            {
                var user = new ApplicationUser
                {
                    UserName = projectCrewModel.UserProfile.Email,
                    Email = projectCrewModel.UserProfile.Email,
                    FirstName = projectCrewModel.UserProfile.FirstName,
                    LastName = projectCrewModel.UserProfile.LastName,
                    Company = null,
                    Industry = null,
                    EmailConfirmed = true,
                    Banned = false,
                    Active = true
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    UserProfile profile = new UserProfile()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        CompanyName = user.Company,
                        CompanyTypeId = null,
                        CompanyEmail = user.Email,
                        CompanyStudioEmail = user.Email,
                        UserId = user.Id

                    };

                    _context.UserProfiles.Add(profile);
                    _context.SaveChanges();


                    projectCrewModel.ProjectCrew.UserId = user.Id;
                    _context.ProjectCrews.Add(projectCrewModel.ProjectCrew);
                    await _context.SaveChangesAsync();

                    if (projectCrewModel.ProjectCrewUnits != null)
                    {
                        foreach (var item in projectCrewModel.ProjectCrewUnits)
                        {
                            _context.CrewUnits.Add(new ProjectCrewUnit() { ProjectCrewId = projectCrewModel.ProjectCrew.Id, UnitId = item });
                        }
                    }
                    if (projectCrewModel.ProjectCrewPositions != null)
                    {
                        foreach (var item in projectCrewModel.ProjectCrewPositions)
                        {
                            _context.ProjectCrewPositions.Add(new ProjectCrewPosition() { ProjectCrewId = projectCrewModel.ProjectCrew.Id, PositionId = item });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                else
                {
                    return StatusCode(500, result.Errors.Select(a => a.Description).FirstOrDefault());
                    //return Json(new { success=false, status = "error", message = "error creating customer" });
                    //// return NotFound(new { message = result.Errors[0]. });
                }


            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            // projectCrewModel.ProjectCrew.UserProfileId = projectCrewModel.UserProfile.Id;

            //Add user into crew
            //  _context.ProjectCrews.Add(projectCrewModel.ProjectCrew);

            //Set CrewId to Units
            //foreach (var item in projectCrewModel.ProjectCrewUnits)
            //{

            //    _context.CrewUnits.Add(new ProjectCrewUnit() {ProjectCrewId= projectCrewModel.ProjectCrew.Id,UnitId=item });
            //}
            ////Set CrewId to Position
            //foreach (var item in projectCrewModel.ProjectCrewPositions)
            //{
            //    item.ProjectCrewId = projectCrewModel.ProjectCrew.Id;
            //    _context.ProjectCrewPositions.Add(item);
            //}


            return Ok();
        }

    }
}