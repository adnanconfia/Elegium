using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elegium.Controllers
{
    [Authorize]
    public class ResourcesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Offer()
        {
            return View();
        }

        //[Route("[controller]/[action]/{id}")]
        public IActionResult Detail(int id)
        {
            return View();
        }

        public IActionResult ResourceRequests()
        {
            return View();
        }
    }
}
