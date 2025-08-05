using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Elegium.Areas.Identity.Pages.Account;
using Elegium.Data;
using Elegium.Dtos;
using Elegium.Models;
using Elegium.Models.Notifications;
using Elegium.Models.Projects;
using Elegium.ViewModels.ProjectViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = RoleName.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        //private readonly ILogger<RegisterModel> _logger;
        public AdminController(ApplicationDbContext context
            , UserManager<ApplicationUser> userManager
            , IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersSummary()
        {
            var allUsers = _context.Users.Where(a => !a.Trash);
            var totalUsers = await allUsers.CountAsync();
            var activeUsers = await allUsers.Where(a => a.Active && !a.Banned).CountAsync();
            var inActiveUsers = await allUsers.Where(a => !a.Active && !a.Banned).CountAsync();
            var bannedUsers = await allUsers.Where(a => a.Banned).CountAsync();
            var percentageOfActiveUsers = Math.Round(Convert.ToDouble(activeUsers) / Convert.ToDouble(totalUsers) * 100, 2);
            var percentageOfBannedUsers = Math.Round(Convert.ToDouble(bannedUsers) / Convert.ToDouble(totalUsers) * 100);
            var percentageOfInactiveUsers = Math.Round(Convert.ToDouble(inActiveUsers) / Convert.ToDouble(totalUsers) * 100, 2);

            var today = DateTime.UtcNow;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);
            var last = month.AddDays(-1);

            var lastMonthUsers = Convert.ToDouble(await allUsers.Where(a => a.Created >= first && a.Created <= last).CountAsync());
            var currrentMonthUsers = Convert.ToDouble(await allUsers.Where(a => a.Created > month && a.Created <= today).CountAsync());

            double increaseOrDecrese = 0.00f;
            try
            {
                increaseOrDecrese = Math.Round((currrentMonthUsers - lastMonthUsers) / (lastMonthUsers != 0 ? lastMonthUsers : 1) * 100, 2);
            }
            catch (Exception ex)
            {
                increaseOrDecrese = 0;
            }

            var sign = Math.Sign(increaseOrDecrese);

            if (increaseOrDecrese < 0)
                increaseOrDecrese *= -1;

            return Ok(
                new
                {
                    summaryObj = new
                    {
                        totalUsers,
                        activeUsers,
                        inActiveUsers,
                        bannedUsers,
                        percentageOfActiveUsers,
                        percentageOfBannedUsers,
                        percentageOfInactiveUsers,
                        Message = sign == 0 ? "No registrations this month." : sign == 1 ? string.Format(@"{0}% higher than last month ", increaseOrDecrese) : sign < 0 ? string.Format(@"{0}% lower than last month", increaseOrDecrese) : "",
                        increaseOrDecrese
                    }
                });
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string flag)
        {
            var usersList = await (from user in _context.Users
                                   where !user.Trash
                                   && (string.IsNullOrEmpty(flag) || (flag == "A" && user.Active && !user.Banned) || (flag == "B" && user.Banned) || (flag == "I" && !user.Active && !user.Banned))
                                   join userRoles in _context.UserRoles on user.Id equals userRoles.UserId into userRolesList
                                   from userRoles in userRolesList.DefaultIfEmpty()
                                   join role in _context.Roles on userRoles.RoleId equals role.Id into rolesList
                                   from role in rolesList.DefaultIfEmpty()
                                   join userProfile in _context.UserProfiles on user.Id equals userProfile.UserId into userProfileList
                                   from userProfile in userProfileList.DefaultIfEmpty()
                                   join workingPosition in _context.WorkingPositions on userProfile.CompanyPositionId equals workingPosition.Id into workingPositionList
                                   from workingPosition in workingPositionList.DefaultIfEmpty()
                                   select new
                                   {
                                       UserId = user.Id,
                                       user.FirstName,
                                       user.LastName,
                                       Name = user.FirstName + " " + user.LastName,
                                       user.Email,
                                       Created = user.Created.ToString("dd MMM, yyyy"),
                                       user.UserName,
                                       RoleId = role == null ? string.Empty : role.Id,
                                       RoleName = role == null ? string.Empty : role.Name,
                                       PositonName = workingPosition != null ? workingPosition.Name : string.Empty,
                                       UserProfileId = userProfile != null ? userProfile.Id : (int?)null,
                                       FirstTwoCharacters = user.FirstName[0] + user.LastName[0],
                                       user.Active,
                                       user.Banned,
                                       Password = string.Empty,
                                       ConfirmPassword = string.Empty,
                                       user.Company,
                                       IndustryId = string.IsNullOrEmpty(user.Industry) ? (int?)null : int.Parse(user.Industry)
                                   })
                        .ToListAsync();

            return Ok(new { data = usersList });
        }

        [HttpGet("{id}/{gg}")]
        public async Task<IActionResult> GetUserPhoto(string id, string gg)
        {
            var profile = await _context.UserProfiles.FindAsync(int.Parse(id));
            if (profile.Photo == null)
                return new NotFoundResult();
            return File(profile.Photo, "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> BanUnBanUser([FromQuery] string id, [FromQuery] string action)
        {
            var dbuser = await _userManager.FindByIdAsync(id);
            if (dbuser == null)
                return Ok(new { success = false, Msg = "User does not exist!" });
            else
            {
                if (action == "B")
                    dbuser.Banned = !dbuser.Banned;
                else
                    dbuser.Active = !dbuser.Active;
                await _context.SaveChangesAsync();
            }

            var msg = string.Empty;
            if (action == "B" && dbuser.Banned)
            {
                msg = "Banned";
            }
            else if (action == "B" && !dbuser.Banned)
            {
                msg = "Unbanned";
            }
            else if (action == "A" && dbuser.Active)
            {
                msg = "Activated";
            }
            else
            {
                msg = "Deactivated";
            }

            var userObj = await (
                            from user in _context.Users
                            where user.Id == id
                            where !user.Trash
                            join userRoles in _context.UserRoles on user.Id equals userRoles.UserId into userRolesList
                            from userRoles in userRolesList.DefaultIfEmpty()
                            join role in _context.Roles on userRoles.RoleId equals role.Id into rolesList
                            from role in rolesList.DefaultIfEmpty()
                            join userProfile in _context.UserProfiles on user.Id equals userProfile.UserId into userProfileList
                            from userProfile in userProfileList.DefaultIfEmpty()
                            join workingPosition in _context.WorkingPositions on userProfile.CompanyPositionId equals workingPosition.Id into workingPositionList
                            from workingPosition in workingPositionList.DefaultIfEmpty()
                            select new
                            {
                                UserId = user.Id,
                                user.FirstName,
                                user.LastName,
                                Name = user.FirstName + " " + user.LastName,
                                user.Email,
                                Created = user.Created.ToString("dd MMM, yyyy"),
                                user.UserName,
                                RoleId = role.Id,
                                RoleName = role.Name,
                                PositonName = workingPosition != null ? workingPosition.Name : string.Empty,
                                UserProfileId = userProfile != null ? userProfile.Id : (int?)null,
                                FirstTwoCharacters = user.FirstName[0] + user.LastName[0],
                                user.Active,
                                user.Banned,
                                Password = string.Empty,
                                ConfirmPassword = string.Empty,
                                user.Company,
                                IndustryId = string.IsNullOrEmpty(user.Industry) ? (int?)null : int.Parse(user.Industry)
                            })
                        .FirstOrDefaultAsync();
            return Ok(new
            {
                success = true,
                Msg = string.Format(@"{0} has been {1} successfully!", dbuser.FirstName + " " + dbuser.LastName, msg),
                userObj
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] EditUserDto userDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userDto.UserId);
                if (user == null)
                    throw new Exception("User does not exist!");

                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.Company = userDto.Company;
                user.Industry = userDto.IndustryId.ToString();
                user.Banned = userDto.Banned;
                user.Active = userDto.Active;

                if (user.Email != userDto.Email)
                {
                    var userAlreadyExists = await _userManager.FindByEmailAsync(userDto.Email);
                    if (userAlreadyExists != null)
                        throw new Exception("This email already exists! Use a different email address");
                }
                user.Email = userDto.Email;

                if (userDto.ChangePassword)
                {
                    if (!userDto.Password.Equals(userDto.ConfirmPassword))
                        throw new Exception("Passowrd and Confirm Password do not match!");

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passChange = await _userManager.ResetPasswordAsync(user, token, userDto.Password);
                    if (!passChange.Succeeded)
                        throw new Exception("Could not change passsword!");
                }

                var userProfile = await _context.UserProfiles.FindAsync(userDto.UserProfileId);
                if (userProfile != null)
                {

                    userProfile.FirstName = user.FirstName;
                    userProfile.LastName = user.LastName;
                    userProfile.Email = user.Email;
                    userProfile.CompanyName = user.Company;
                    userProfile.CompanyTypeId = int.Parse(user.Industry);
                    userProfile.CompanyEmail = user.Email;
                    userProfile.CompanyStudioEmail = user.Email;
                    userProfile.UserId = user.Id;

                }
                else
                {
                    userProfile = new UserProfile();
                    userProfile.FirstName = user.FirstName;
                    userProfile.LastName = user.LastName;
                    userProfile.Email = user.Email;
                    userProfile.CompanyName = user.Company;
                    userProfile.CompanyTypeId = int.Parse(user.Industry);
                    userProfile.CompanyEmail = user.Email;
                    userProfile.CompanyStudioEmail = user.Email;
                    userProfile.UserId = user.Id;
                    await _context.UserProfiles.AddAsync(userProfile);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var oldRole = roles.ToArray().Length == 0 ? string.Empty : roles.ToArray()[0];
                var newRole = await _context.Roles.FindAsync(userDto.RoleId);

                if (oldRole != newRole.Name)
                {
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    await _userManager.AddToRoleAsync(user, newRole.Name);
                }

                await _context.SaveChangesAsync();
                var userObj = await (from user1 in _context.Users
                                     where user1.Id == user.Id
                                     join userRoles in _context.UserRoles on user.Id equals userRoles.UserId into userRolesList
                                     from userRoles in userRolesList.DefaultIfEmpty()
                                     join role in _context.Roles on userRoles.RoleId equals role.Id into roleList
                                     from role in roleList.DefaultIfEmpty()
                                     join userProfile1 in _context.UserProfiles on user1.Id equals userProfile1.UserId into userProfile1List
                                     from userProfile1 in userProfile1List.DefaultIfEmpty()
                                     join workingPosition in _context.WorkingPositions on userProfile1.CompanyPositionId equals workingPosition.Id into workingPositionList
                                     from workingPosition in workingPositionList.DefaultIfEmpty()
                                     select new
                                     {
                                         UserId = user.Id,
                                         user.FirstName,
                                         user.LastName,
                                         Name = user.FirstName + " " + user.LastName,
                                         user.Email,
                                         Created = user.Created.ToString("dd MMM, yyyy"),
                                         user.UserName,
                                         RoleId = role.Id,
                                         RoleName = role.Name,
                                         PositonName = workingPosition != null ? workingPosition.Name : string.Empty,
                                         UserProfileId = userProfile != null ? userProfile.Id : (int?)null,
                                         FirstTwoCharacters = user.FirstName[0] + user.LastName[0],
                                         user.Active,
                                         user.Banned,
                                         Password = string.Empty,
                                         ConfirmPassword = string.Empty,
                                         user.Company,
                                         IndustryId = string.IsNullOrEmpty(user.Industry) ? (int?)null : int.Parse(user.Industry)
                                     })
                        .FirstOrDefaultAsync();
                return Ok(new
                {
                    success = true,
                    Msg = "User information has been updated successfully!",
                    userObj
                });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, Msg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] EditUserDto userDto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = userDto.Email,
                    Email = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Company = userDto.Company,
                    Industry = userDto.IndustryId.ToString(),
                    EmailConfirmed = true,
                    Banned = userDto.Banned,
                    Active = userDto.Active
                };


                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    var role = await _context.Roles.FindAsync(userDto.RoleId);
                    await _userManager.AddToRoleAsync(user, role.Name); //assign the memeber role

                    UserProfile profile = new UserProfile()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        CompanyName = user.Company,
                        CompanyTypeId = int.Parse(user.Industry),
                        CompanyEmail = user.Email,
                        CompanyStudioEmail = user.Email,
                        UserId = user.Id
                    };

                    _context.UserProfiles.Add(profile);
                    _context.SaveChanges();
                    var userObj = await (
                            from user1 in _context.Users
                            where user1.Id == user.Id
                            where !user1.Trash
                            join userRoles in _context.UserRoles on user1.Id equals userRoles.UserId into userRolesList
                            from userRoles in userRolesList.DefaultIfEmpty()
                            join role1 in _context.Roles on userRoles.RoleId equals role.Id into rolesList
                            from role1 in rolesList.DefaultIfEmpty()
                            join userProfile in _context.UserProfiles on user.Id equals userProfile.UserId into userProfileList
                            from userProfile in userProfileList.DefaultIfEmpty()
                            join workingPosition in _context.WorkingPositions on userProfile.CompanyPositionId equals workingPosition.Id into workingPositionList
                            from workingPosition in workingPositionList.DefaultIfEmpty()
                            select new
                            {
                                UserId = user.Id,
                                user.FirstName,
                                user.LastName,
                                Name = user.FirstName + " " + user.LastName,
                                user.Email,
                                Created = user.Created.ToString("dd MMM, yyyy"),
                                user.UserName,
                                RoleId = role.Id,
                                RoleName = role.Name,
                                PositonName = workingPosition != null ? workingPosition.Name : string.Empty,
                                UserProfileId = userProfile != null ? userProfile.Id : (int?)null,
                                FirstTwoCharacters = user.FirstName[0] + user.LastName[0],
                                user.Active,
                                user.Banned,
                                Password = string.Empty,
                                ConfirmPassword = string.Empty,
                                user.Company,
                                IndustryId = string.IsNullOrEmpty(user.Industry) ? (int?)null : int.Parse(user.Industry)
                            })
                        .FirstOrDefaultAsync();

                    return Ok(new
                    {
                        success = true,
                        Msg = "User has been created!",
                        userObj
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        Msg = result.Errors.FirstOrDefault().Description
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    Msg = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> VerifyUser([FromQuery] string id, [FromQuery] string password)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Ok(new
                {
                    success = false,
                    Msg = "User does not exist!"
                });

            var verify = await _userManager.CheckPasswordAsync(user, password);
            if (verify)
                return Ok(new
                {
                    success = true,
                    Msg = "User password verified"
                });
            else
            {
                return Ok(new
                {
                    success = false,
                    Msg = "Password could not be verified!"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDto deleteUser)
        {
            var user = await _userManager.FindByIdAsync(deleteUser.Id);
            var loggedInUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return Ok(new
                {
                    success = false,
                    Msg = "User does not exist!"
                });

            var verify = await _userManager.CheckPasswordAsync(loggedInUser, deleteUser.Password);
            if (verify)
            {
                try
                {
                    if (user.Id == loggedInUser.Id)
                        throw new Exception("You cannot deleted the logged in user!");

                    //var MediaFiles = _context.MediaFiles.Where(a => a.UserId == user.Id);
                    //var albums = _context.MediaFiles.Where(a => a.UserId == user.Id);
                    //var userProfile = _context.UserProfiles.Where(a => a.UserId == user.Id);
                    //_context.UserRoles.Where(a => a.UserId == user.Id);
                    //_context.UserTokens.Where(a => a.UserId == user.Id);
                    //_context.UserPromotionCategory.Where(a => a.UserId == user.Id);
                    //_context.UserOtherLanguages.Where(a => a.UserId == user.Id);
                    //_context.UserMessages.Where(a => a.FromUserId == user.Id || a.ToUserId == user.Id);
                    //_context.UserEquipment.Where(a => a.UserId == user.Id);
                    //_context.UserAdditionalSkills.Where(a => a.UserId == user.Id);
                    //_context.Project.Where(a => a.UserId == user.Id);

                    //await _userManager.DeleteAsync(user);
                    user.Trash = true;
                    await _context.SaveChangesAsync();
                    return Ok(new
                    {
                        success = true,
                        Msg = "User has been deleted successfully!"
                    });
                }
                catch (Exception ex)
                {
                    return Ok(new
                    {
                        success = false,
                        Msg = ex.Message + (ex.InnerException == null ? "" : " - " + ex.InnerException.Message)
                    });
                }
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    Msg = "You are not verified!"
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModel>>> GetUserProjects([FromQuery] string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);

            List<ProjectViewModel> viewModel = new List<ProjectViewModel>();
            var projects = await _context.Project.Where(p => p.UserId == appUser.Id && !p.Deleted)
                .Include(p => p.ProductionType)
                .Include(p => p.Currency)
                .Include(p => p.ProductionLanguage)
                .ToListAsync();

            foreach (Project project in projects)
            {
                var projectPartnerRequirement = await _context.ProjectPartnerRequirement.Where(p => p.ProjectId == project.Id).FirstOrDefaultAsync();
                var projectFunding = await _context.ProjectFunding.Where(p => p.ProjectId == project.Id).FirstOrDefaultAsync();
                var projectFinancialParticipation = await _context.ProjectFinancialParticipation.Where(p => p.ProjectId == project.Id).FirstOrDefaultAsync();

                ProjectViewModel vm = new ProjectViewModel()
                {
                    Project = project,
                    ProjectPartnerRequirement = projectPartnerRequirement,
                    ProjectPartners = await _context.ProjectPartner.Where(p => p.ProjectId == project.Id).ToListAsync(),
                    ProjectFunding = projectFunding,
                    ProjectFinancialParticipation = projectFinancialParticipation,
                    ProjectVisibilityAreas = await _context.ProjectVisibilityArea.Where(p => p.ProjectId == project.Id).Select(v => v.VisibilityAreaId).ToArrayAsync(),
                    //ProjectPartnerRequirementManagementPhases = await _context.ProjectPartnerRequirementManagementPhase.Where(p => p.ProjectPartnerRequirementId == projectPartnerRequirement.Id).Select(v => v.ProjectManagementPhasesId).ToArrayAsync(),
                    //ProjectFundingManagementPhases = projectFunding != null ? await _context.ProjectFundingManagementPhase.Where(p => p.ProjectFundingId == projectFunding.Id).Select(v => v.ProjectManagementPhasesId).ToArrayAsync() : null,
                    //ProjectFinancialParticipationManagementPhases = projectFinancialParticipation != null ? await _context.ProjectFinancialParticipationManagementPhase.Where(p => p.ProjectFinancialParticipationId == projectFinancialParticipation.Id).Select(v => v.ProjectManagementPhasesId).ToArrayAsync() : null,
                    ProjectOwner = _context.UserProfiles.Include(u => u.CompanyPosition).Include(u => u.CompanyType).Where(u => u.UserId == project.UserId).FirstOrDefault()
                };
                viewModel.Add(vm);
            }

            return viewModel;
        }
    }
}
