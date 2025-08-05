using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PromotionCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PromotionCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionCategory>>> GetPromotionCategory()
        {
            return await _context.PromotionCategory.ToListAsync();
        }

        // GET: api/PromotionCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionCategory>> GetPromotionCategory(int id)
        {
            var promotionCategory = await _context.PromotionCategory.FindAsync(id);

            if (promotionCategory == null)
            {
                return NotFound();
            }

            return promotionCategory;
        }

        // PUT: api/PromotionCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromotionCategory(int id, PromotionCategory promotionCategory)
        {
            if (id != promotionCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(promotionCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionCategoryExists(id))
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

        // POST: api/PromotionCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PromotionCategory>> PostPromotionCategory(PromotionCategory promotionCategory)
        {
            _context.PromotionCategory.Add(promotionCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPromotionCategory", new { id = promotionCategory.Id }, promotionCategory);
        }

        // DELETE: api/PromotionCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PromotionCategory>> DeletePromotionCategory(int id)
        {
            var promotionCategory = await _context.PromotionCategory.FindAsync(id);
            if (promotionCategory == null)
            {
                return NotFound();
            }

            _context.PromotionCategory.Remove(promotionCategory);
            await _context.SaveChangesAsync();

            return promotionCategory;
        }

        private bool PromotionCategoryExists(int id)
        {
            return _context.PromotionCategory.Any(e => e.Id == id);
        }
    }
}
