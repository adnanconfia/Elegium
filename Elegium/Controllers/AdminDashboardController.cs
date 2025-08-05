using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elegium.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NotificationSettings()
        {
            return View();
        }
    }
}
