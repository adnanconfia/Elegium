using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Projects;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductionTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductionTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionType>>> GetProductionType()
        {
            return await _context.ProductionType.ToListAsync();
        }

        // GET: api/ProductionTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionType>> GetProductionType(int id)
        {
            var productionType = await _context.ProductionType.FindAsync(id);

            if (productionType == null)
            {
                return NotFound();
            }

            return productionType;
        }

        // PUT: api/ProductionTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductionType(int id, ProductionType productionType)
        {
            if (id != productionType.Id)
            {
                return BadRequest();
            }

            _context.Entry(productionType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductionTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProductionType>> PostProductionType(ProductionType productionType)
        {
            _context.ProductionType.Add(productionType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductionType", new { id = productionType.Id }, productionType);
        }

        // DELETE: api/ProductionTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductionType>> DeleteProductionType(int id)
        {
            var productionType = await _context.ProductionType.FindAsync(id);
            if (productionType == null)
            {
                return NotFound();
            }

            _context.ProductionType.Remove(productionType);
            await _context.SaveChangesAsync();

            return productionType;
        }

        private bool ProductionTypeExists(int id)
        {
            return _context.ProductionType.Any(e => e.Id == id);
        }
    }
}
