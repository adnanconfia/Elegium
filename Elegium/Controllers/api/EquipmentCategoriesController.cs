using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.ViewModels;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EquipmentCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentCategory>>> GetEquipmentCategory()
        {
            return await _context.EquipmentCategory.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentCategoryViewModel>>> GetEquipmentCategoryShort()
        {
            return await _context.EquipmentCategory.Select(e => new EquipmentCategoryViewModel { Id = e.Id, Name = e.Name, EquipmentCategoryType = e.EquipmentCategoryType.Name }).ToListAsync();
        }

        // GET: api/EquipmentCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentCategory>> GetEquipmentCategory(int id)
        {
            var equipmentCategory = await _context.EquipmentCategory.FindAsync(id);

            if (equipmentCategory == null)
            {
                return NotFound();
            }

            return equipmentCategory;
        }

        // PUT: api/EquipmentCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentCategory(int id, EquipmentCategory equipmentCategory)
        {
            if (id != equipmentCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(equipmentCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentCategoryExists(id))
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

        // POST: api/EquipmentCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EquipmentCategory>> PostEquipmentCategory(EquipmentCategory equipmentCategory)
        {
            _context.EquipmentCategory.Add(equipmentCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipmentCategory", new { id = equipmentCategory.Id }, equipmentCategory);
        }

        // DELETE: api/EquipmentCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EquipmentCategory>> DeleteEquipmentCategory(int id)
        {
            var equipmentCategory = await _context.EquipmentCategory.FindAsync(id);
            if (equipmentCategory == null)
            {
                return NotFound();
            }

            _context.EquipmentCategory.Remove(equipmentCategory);
            await _context.SaveChangesAsync();

            return equipmentCategory;
        }

        private bool EquipmentCategoryExists(int id)
        {
            return _context.EquipmentCategory.Any(e => e.Id == id);
        }
    }
}
