using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Projects;
using Microsoft.AspNetCore.Identity;
using Elegium.Models;

namespace Elegium.Controllers.ProjectManagement
{
    public class ProjectManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectManagementController(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: ProjectManagement
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Project.Include(p => p.Currency).Include(p => p.ProductionLanguage).Include(p => p.ProductionType).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<PartialViewResult>_dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["FirstName"] = user.FirstName;
            ViewData["LastName"] = user.LastName;
            return PartialView();
        }
        public PartialViewResult scenesandscripts()
        {
            return PartialView();
        }
        public PartialViewResult cast()
        {
            return PartialView();
        }
        public PartialViewResult actor_details()
        {
            return PartialView();
        }
        public PartialViewResult talent_details()
        {
            return PartialView();
        }
        public PartialViewResult agency_details()
        {
            return PartialView();
        }
        public PartialViewResult character_details()
        {
            return PartialView();
        }
        public PartialViewResult extra_details()
        {
            return PartialView();
        }
        public PartialViewResult scene_details()
        {
            return PartialView();
        }

        public PartialViewResult documents()
        {
            return PartialView();
        }
        public PartialViewResult documentcategory()
        {
            return PartialView();
        }

        public PartialViewResult files()
        {
            return PartialView();
        }

        public PartialViewResult fileProfile()
        {
            return PartialView();
        }

        public PartialViewResult testview()
        {
            return PartialView();
        }

        public PartialViewResult crew()
        {
            return PartialView();
        }
        public PartialViewResult unit()
        {
            return PartialView();
        }
        public PartialViewResult group()
        {
            return PartialView();
        }
        public PartialViewResult userDetail()
        {
            return PartialView();
        }

        public PartialViewResult externalUserDetail()
        {
            return PartialView();
        }

        public PartialViewResult tasks()
        {
            return PartialView();
        }

        public PartialViewResult mytasks()
        {
            return PartialView();
        }

        public PartialViewResult mycreatedtasks()
        {
            return PartialView();
        }

        public PartialViewResult completedtasks()
        {
            return PartialView();
        }

        public PartialViewResult alltasks()
        {
            return PartialView();
        }

        public PartialViewResult taskprofile()
        {
            return PartialView();
        }
    }
}
