using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elegium.Controllers
{
    public class AnnouncementsController : Controller
    {
        public PartialViewResult Index()
        {
            return PartialView();
        }

        public PartialViewResult announcementprofile()
        {
            return PartialView();
        }
    }
}
