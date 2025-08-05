using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elegium.Controllers
{
    [Authorize]
    public class FundingAndFPController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Offer()
        {
            return View();
        }

        public IActionResult FundingRequests()
        {
            return View();
        }
    }
}
