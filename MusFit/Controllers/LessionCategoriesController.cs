using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusFit.Models;

namespace MusFit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessionCategoriesController : ControllerBase
    {
        private readonly MusFitContext _context;

        public LessionCategoriesController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/LessionCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessionCategory>>> GetLessionCategories()
        {
            return await _context.LessionCategories.ToListAsync();
        }

        // GET: api/LessionCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LessionCategory>> GetLessionCategory(int id)
        {
            var lessionCategory = await _context.LessionCategories.FindAsync(id);

            if (lessionCategory == null)
            {
                return NotFound();
            }

            return lessionCategory;
        }

        // PUT: api/LessionCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLessionCategory(int id, LessionCategory lessionCategory)
        {
            if (id != lessionCategory.LcId)
            {
                return BadRequest();
            }

            _context.Entry(lessionCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessionCategoryExists(id))
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

        // POST: api/LessionCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LessionCategory>> PostLessionCategory(LessionCategory lessionCategory)
        {
            _context.LessionCategories.Add(lessionCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLessionCategory", new { id = lessionCategory.LcId }, lessionCategory);
        }

        // DELETE: api/LessionCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLessionCategory(int id)
        {
            var lessionCategory = await _context.LessionCategories.FindAsync(id);
            if (lessionCategory == null)
            {
                return NotFound();
            }

            _context.LessionCategories.Remove(lessionCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LessionCategoryExists(int id)
        {
            return _context.LessionCategories.Any(e => e.LcId == id);
        }
    }
}
