using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Elegium.Models;
using Microsoft.AspNetCore.Identity;

namespace Elegium.Controllers
{
    [Authorize]
    public  class CrewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Unit()
        {
            return View();
        }
    }
}
