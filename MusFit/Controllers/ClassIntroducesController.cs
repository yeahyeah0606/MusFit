using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MusFit.Models;

namespace MusFit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassIntroducesController : ControllerBase
    {
        private readonly MusFitContext _context;

        public ClassIntroducesController(MusFitContext context)
        {
            _context = context;
        }

        // GET: api/ClassIntroduces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassIntroduce>>> GetClassIntroduces()
        {
            return await _context.ClassIntroduces.ToListAsync();
        }

        // GET: api/ClassIntroduces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassIntroduce>> GetClassIntroduce(int id)
        {
            var classIntroduce = await _context.ClassIntroduces.FindAsync(id);

            if (classIntroduce == null)
            {
                return NotFound();
            }

            return classIntroduce;
        }

        // PUT: api/ClassIntroduces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassIntroduce(int id, ClassIntroduce classIntroduce)
        {
            if (id != classIntroduce.InId)
            {
                return BadRequest();
            }

            _context.Entry(classIntroduce).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassIntroduceExists(id))
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

        // POST: api/ClassIntroduces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassIntroduce>> PostClassIntroduce(ClassIntroduce classIntroduce)
        {
            _context.ClassIntroduces.Add(classIntroduce);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassIntroduce", new { id = classIntroduce.InId }, classIntroduce);
        }

        // DELETE: api/ClassIntroduces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassIntroduce(int id)
        {


            var classIntroduce = await _context.ClassIntroduces.FindAsync(id);
            if (classIntroduce == null)
            {
                return NotFound();
            }

            _context.ClassIntroduces.Remove(classIntroduce);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassIntroduceExists(int id)
        {
            return _context.ClassIntroduces.Any(e => e.InId == id);
        }

    }

}
