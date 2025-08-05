using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.Models.ProjectCrews;
using Elegium.Models.Projects;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkingPositions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectUnit>>> GetUnits(int projectId)
        {
            return await _context.ProjectUnits.Where(a=>a.ProjectId== projectId).ToListAsync();
        }

        
    }
}
