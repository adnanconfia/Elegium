using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elegium.Controllers
{
    [Authorize]
    public class PublicProjectDetailController : Controller
    {
        [Route("[controller]/{id}")]
        public IActionResult Index(string id)
        {
            return View();
        }
    }
}
