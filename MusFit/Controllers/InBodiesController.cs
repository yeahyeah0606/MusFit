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
    public class InBodiesController : ControllerBase
    {
        private readonly MusFitContext _context;

        public InBodiesController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/InBodies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InBody>>> GetInBodies()
        {
            return await _context.InBodies.ToListAsync();
        }

        [HttpGet("sId/{id}")]
        public async Task<ActionResult<dynamic>> GetInBodyBySId(int id)
        {
            var obj = await _context.InBodies.Where(o => o.SId.Equals(id)).OrderByDescending(o => o.Date).ToListAsync();
            return obj;
        }

        [HttpGet("sId/{id}/{term}")]
        public async Task<ActionResult<dynamic>> GetInBodyByTerm(int id, int term)
        {
            var date = DateTime.Now.AddMonths(-term);
            var obj = from i in _context.InBodies where i.SId == id && i.Date > date orderby i.Date descending select i;

            return await obj.ToListAsync();
        }

        // GET: api/InBodies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InBody>> GetInBody(int id)
        {
            var inBody = await _context.InBodies.FindAsync(id);

            if (inBody == null)
            {
                return NotFound();
            }

            return inBody;
        }

        // PUT: api/InBodies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInBody(int id, InBody inBody)
        {
            if (id != inBody.InBodyId)
            {
                return BadRequest();
            }

            _context.Entry(inBody).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InBodyExists(id))
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

        // POST: api/InBodies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InBody>> PostInBody(InBody inBody)
        {
            _context.InBodies.Add(inBody);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInBody", new { id = inBody.InBodyId }, inBody);
        }

        // DELETE: api/InBodies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInBody(int id)
        {
            var inBody = await _context.InBodies.FindAsync(id);
            if (inBody == null)
            {
                return NotFound();
            }

            _context.InBodies.Remove(inBody);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InBodyExists(int id)
        {
            return _context.InBodies.Any(e => e.InBodyId == id);
        }
    }
}
