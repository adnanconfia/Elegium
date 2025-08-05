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
using Microsoft.AspNetCore.Authorization;

namespace Elegium.Controllers
{
   // [Authorize]
    public class ProfessionalDetailController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        IWebHostEnvironment _env;
        [Route("[controller]/{id}")]
        public async Task<IActionResult> Index(string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && userId == user.Id)
                ViewBag.ShowEditBtn = true;
            else
                ViewBag.ShowEditBtn = false;
            var searchedUser = await _context.UserProfiles.Where(a => a.UserId == userId).FirstOrDefaultAsync();
            if (searchedUser != null)
                ViewBag.Name = searchedUser.FirstName + " " + searchedUser.LastName;
            else
                return RedirectToAction("Index", "Professionals");
            return View();
        }
        public ProfessionalDetailController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
    }
}
