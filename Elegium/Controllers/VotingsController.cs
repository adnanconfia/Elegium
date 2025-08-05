using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elegium.Controllers
{
    public class VotingsController : Controller
    {
        [Authorize(Roles = RoleName.Admin)]
        public PartialViewResult Settings()
        {
            return PartialView();
        }

        public PartialViewResult Nominations()
        {
            return PartialView();
        }
        public PartialViewResult NominationDetail()
        {
            return PartialView();
        }

        public PartialViewResult Voting()
        {
            return PartialView();
        }
        public PartialViewResult VotingDetail()
        {
            return PartialView();
        }
        public PartialViewResult VotingResult()
        {
            return PartialView();
        }
        public PartialViewResult VotingResultDetail()
        {
            return PartialView();
        }

        public PartialViewResult onboarding()
        {
            return PartialView();
        }

        public PartialViewResult ApplyNomination()
        {
            return PartialView();
        }

    }
}
