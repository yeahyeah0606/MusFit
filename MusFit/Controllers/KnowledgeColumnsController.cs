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
    public class KnowledgeColumnsController : ControllerBase
    {
        private readonly MusFitContext _context;

        public KnowledgeColumnsController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/KnowledgeColumns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KnowledgeColumn>>> GetKnowledgeColumns()
        {
            return await _context.KnowledgeColumns.ToListAsync();
        }

        // GET: api/KnowledgeColumns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KnowledgeColumn>> GetKnowledgeColumn(int id)
        {
            var knowledgeColumn = await _context.KnowledgeColumns.FindAsync(id);

            if (knowledgeColumn == null)
            {
                return NotFound();
            }

            return knowledgeColumn;
        }

        // PUT: api/KnowledgeColumns/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKnowledgeColumn(int id, KnowledgeColumn knowledgeColumn)
        {
            if (id != knowledgeColumn.KColumnId)
            {
                return BadRequest();
            }

            _context.Entry(knowledgeColumn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KnowledgeColumnExists(id))
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

        // POST: api/KnowledgeColumns
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KnowledgeColumn>> PostKnowledgeColumn(KnowledgeColumn knowledgeColumn)
        {
            _context.KnowledgeColumns.Add(knowledgeColumn);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKnowledgeColumn", new { id = knowledgeColumn.KColumnId }, knowledgeColumn);
        }

        // DELETE: api/KnowledgeColumns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKnowledgeColumn(int id)
        {
            var knowledgeColumn = await _context.KnowledgeColumns.FindAsync(id);
            if (knowledgeColumn == null)
            {
                return NotFound();
            }

            _context.KnowledgeColumns.Remove(knowledgeColumn);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KnowledgeColumnExists(int id)
        {
            return _context.KnowledgeColumns.Any(e => e.KColumnId == id);
        }
    }
}
